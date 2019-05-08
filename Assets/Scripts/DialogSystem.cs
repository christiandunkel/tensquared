using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class DialogSystem : MonoBehaviour
{

  // singleton

  public static DialogSystem Instance;

  void Awake()
  {
    Instance = this;
  }


  public static bool dialogEnabled = true;


  private static AudioSource audioSource = null;
  private static GameObject dialogBox = null;
  private static Vector3 dialogBoxPos = new Vector3(0.0f, 0.0f, 0.0f);
  private static Vector3 dialogBoxPosHidden = new Vector3(0.0f, 0.0f, 0.0f);
  private static GameObject textElement = null;
  private static GameObject panelElement = null;
  private static CanvasGroup closeSymbol = null;

  // Start is called before the first frame update
  void Start()
  {

    Debug.Log("DialogSystem: Loaded.");

    // get inner elements
    foreach (Transform child in gameObject.transform)
    {
      GameObject obj = child.gameObject;

      if (obj.name == "DialogBox")
      {

        dialogBox = obj;
        dialogBoxPos.x = dialogBox.transform.localPosition.x;
        dialogBoxPos.y = dialogBox.transform.localPosition.y;
        dialogBoxPos.z= dialogBox.transform.localPosition.z;
        dialogBox.GetComponent<CanvasGroup>().alpha = 0.0f;
        dialogBoxPosHidden.x = dialogBox.transform.localPosition.x;
        dialogBoxPosHidden.y = 180.0f;
        dialogBoxPosHidden.z = dialogBox.transform.localPosition.z;

        foreach (Transform child2 in obj.transform)
        {
          GameObject obj2 = child2.gameObject;

          if (obj2.name == "Background")
          {
            panelElement = obj2;
          }
          else if (obj2.name == "Text")
          {
            textElement = obj2;
          }
          else if (obj2.name == "CloseSymbol")
          {
            closeSymbol = obj2.GetComponent<CanvasGroup>();
          }
        }
        
      }
      else if (obj.name == "DialogAudioSource")
      {
        audioSource = obj.GetComponent<AudioSource>();
      }

    }

    LoadDialog("lvl1_greeting");

  }

  void Update()
  {

    if (!dialogEnabled)
    {
      return;
    }

    if (moveDialog != null)
    {
      MoveDialogBox();
    }

    if (dialogBoxVisible && !typewriterRunning && moveDialog == null)
    {
      // close the current dialog window
      if (Input.GetMouseButtonDown(0))
      {
        moveDialog = "up";
      }
    }

  }

  // direction in which the dialog box should move
  // @"down" or "up"
  private static string moveDialog = null;
  private static bool dialogBoxVisible = false;

  public static void LoadDialog(string name)
  {

    currentText = "";
    dialogBox.GetComponent<CanvasGroup>().alpha = 1.0f;

    switch (name)
    {
      case "lvl1_greeting":
        text = "Who are you, my little friend?\nWhat are you doing out here all by yourself?";
        audio = "lvl1_greeting";
        break;
      default:
        Debug.Log("DialogSystem: Could not find dialog \"" + name + "\".");
        return;
    }

    moveDialog = "down";
    // little arrow symbol, indicating that you can close the dialog
    closeSymbol.alpha = 0.0f;
    dialogBoxVisible = true;

  }


  private static float generalTimer = 0.0f;
  private static float moveTimer = 0.0f;
  private static float scrollTimeDown = 0.6f; // time for dialog box to appear / vanish
  private static float scrollTimeUp = 0.2f; // time for dialog box to appear / vanish
  private static float delayBeforeText = 0.3f; // delay before text appears on dialogbox that is already in position

  private static void MoveDialogBox()
  {

    float divisionValue = (moveDialog == "down" ? scrollTimeDown : scrollTimeUp);

    if (moveTimer < 1.0f)
    {
      // from screen position to hidden position
      Vector3 start = dialogBoxPos;
      Vector3 end = dialogBoxPosHidden;

      // from hidden position to screen
      if (moveDialog == "down")
      {
        start = dialogBoxPosHidden;
        end = dialogBoxPos;
        moveTimer += Time.deltaTime / divisionValue;
        dialogBox.GetComponent<CanvasGroup>().alpha = moveTimer;
      }
      else
      {
        moveTimer += Time.deltaTime / divisionValue;
        dialogBox.GetComponent<CanvasGroup>().alpha = 1.0f - moveTimer;
      }

      dialogBox.transform.localPosition = Vector3.Lerp(start, end, moveTimer);
    }

    generalTimer += Time.deltaTime / divisionValue;

    if (
      generalTimer >= 
      (1.0f + delayBeforeText * divisionValue)
    )
    {
      generalTimer = 0.0f;
      moveTimer = 0.0f;
      if (moveDialog == "down")
      {
        typewriterRunning = true;
        DialogSystem.Instance.StartCoroutine(ShowText());
        PlayVoice();
      }
      else
      {
        dialogBoxVisible = false;
      }
      moveDialog = null;

    }

  }




  // text is still being written on screen
  private static bool typewriterRunning = false;
  private static string text = ""; // text
  private static string audio = ""; // path to audio
  private static string currentText = ""; // temporary, current progress of typewriter
  private static float delay = 0.06f; // delay between characters
  private static int currentChar;
  private static Regex punctuation_regex = new Regex(@"[.!?]+");
  private static Match punctuation_match;

  private static IEnumerator ShowText()
  {
    for (int i = 1; i <= text.Length; i++)
    {
      currentText = text.Substring(0, i);
      textElement.GetComponent<TextMeshProUGUI>().SetText(currentText);

      if (i == text.Length)
      {
        DialogSystem.Instance.StopCoroutine(ShowText());
        typewriterRunning = false;
        closeSymbol.alpha = 1.0f;
      }

      // current character, that was just now added
      string thisChar = currentText.Substring(currentText.Length - 1);

      // add bigger delay when there is a dot (from an ellipsis)
      punctuation_match = punctuation_regex.Match(thisChar);

      yield return new WaitForSeconds(punctuation_match.Success ? delay*15 : delay);
    }
  }




  private static void PlayVoice()
  {

    AudioClip clip = Resources.Load("Dialog/" + audio, typeof(AudioClip)) as AudioClip;

    audioSource.PlayOneShot(clip);

  }

}