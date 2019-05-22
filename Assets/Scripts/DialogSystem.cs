using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogSystem : MonoBehaviour
{

  // singleton

  public static DialogSystem Instance;

  void Awake() {
    Instance = this;
  }

  // settings
  public static bool dialogEnabled = true;

  // attributes
  private static Vector3 dialogBoxPos = new Vector3(0.0f, 0.0f, 0.0f),
                         dialogBoxPosHidden = new Vector3(0.0f, 0.0f, 0.0f);

  // elements of the dialog box
  private static AudioSource audioSource = null;
  private static GameObject dialogBox = null;
  private static GameObject textElement = null;
  private static GameObject panelElement = null;
  private static CanvasGroup closeSymbol = null;
  private static Image iconElement = null;

  // called before the first frame update
  void Start()
  {

    Debug.Log("DialogSystem: Loaded.");

    // get inner elements
    foreach (Transform child in gameObject.transform)
    {

      GameObject obj = child.gameObject;

      dialogIcons = Resources.LoadAll<Sprite>("DialogIcons/");

      if (obj.name == "DialogBox") {

        dialogBox = obj;

        // get local position and hidden position of dialog box
        Vector3 lp = dialogBox.transform.localPosition;
        dialogBoxPos = new Vector3(lp.x, lp.y, lp.z);
        dialogBoxPosHidden = new Vector3(lp.x, 180.0f, lp.z);

        // move the box out the way before first frame (fixes 1-frame flickering bug)
        dialogBox.transform.localPosition = dialogBoxPosHidden;

        dialogBox.GetComponent<CanvasGroup>().alpha = 0.0f;

        foreach (Transform child2 in obj.transform) {

          GameObject obj2 = child2.gameObject;

          switch (obj2.name) {

            case "Background":
              panelElement = obj2;
              break;

            case "Text":
              textElement = obj2;
              break;

            case "CloseSymbol":
              closeSymbol = obj2.GetComponent<CanvasGroup>();
              break;

            case "Icon":
              iconElement = obj2.GetComponent<Image>();
              break;

            default:
              break;

          }

        }
        
      }
      else if (obj.name == "DialogAudioSource") {
        audioSource = obj.GetComponent<AudioSource>();
      }

    }

  }

  void Update() {

    // dialog is disabled, don't continue
    if (!dialogEnabled) {
      return;
    }

    // move the dialog box in or out of the frame
    if (moveDialog != null) {
      MoveDialogBox();
      return;
    }

    

    if (dialogBoxVisible && !typewriterRunning) {

      // close the current dialog window by clicking
      if (Input.GetMouseButtonDown(0)) {
        moveDialog = "up";
      }

    }


    if (audioClipLength > 0.0f)
    {
      audioClipLength -= Time.deltaTime;
    }
    else if (dialogBoxVisible && !typewriterRunning)
    {
      moveDialog = "up";
    }

  }

  // direction in which the dialog box should move
  // @"down" or "up"
  private static string moveDialog = null;
  private static bool dialogBoxVisible = false;

  public static void LoadDialog(string name)
  {

    currentText = "";
    textElement.GetComponent<TextMeshProUGUI>().SetText("");
    dialogBox.GetComponent<CanvasGroup>().alpha = 1.0f;

    switch (name) {

      case "lvl1_hello":
        text = "Hello, my little friend!";
        text += "\nDo you have some time to help me out?";
        audio_path = "lvl1_hello";
        iconElement.sprite = getIcon("laughing");
        break;

      case "lvl1_asleep":
        text = "Is this fellow asleep perhaps?";
        audio_path = "lvl1_asleep";
        iconElement.sprite = getIcon("annoyed");
        break;

      case "lvl1_wake_up":
        text = "Wake up, little friend!\n";
        text += "It's not polite to sleep, when there is a robot in need!";
        audio_path = "lvl1_wake_up";
        iconElement.sprite = getIcon("angry");
        break;

      case "lvl1_move":
        text = "Finally, that's better!\n";
        text += "Now that you're awake, can you walk or roll around?";
        audio_path = "lvl1_move";
        iconElement.sprite = getIcon("laughing");
        break;

      case "lvl1_jump":
        text = "Little friend, are you able to jump over that thing?";
        audio_path = "lvl1_jump";
        iconElement.sprite = getIcon("neutral");
        break;

      case "lvl1_dont_jump_into_water":
        text = "I wouldn't jump into the water, my little friend.";
        text += "\nBecause that isn't water.";
        audio_path = "lvl1_dont_jump_into_water";
        iconElement.sprite = getIcon("neutral");
        break;

      case "lvl1_not_the_smartest_circle":
        text = "You really aren't the smartest circle out there, isn't that right?";
        audio_path = "lvl1_not_the_smartest_circle";
        iconElement.sprite = getIcon("annoyed");
        break;

      case "lvl1_you_dont_learn":
        text = "You don't learn, do you?";
        audio_path = "lvl1_you_dont_learn";
        iconElement.sprite = getIcon("furious");
        break;

      case "lvl1_quick_compared_to_other_circles":
        text = "I have to compliment you! Once you finally woke up, you're actually quite quick on foot, especially in comparison to other circles!";
        audio_path = "lvl1_quick_compared_to_other_circles";
        iconElement.sprite = getIcon("happy");
        break;

      case "lvl1_morph":
        text = "No matter how fast you are, sometimes you just can't overcome an obstacle as a circle.";
        audio_path = "lvl1_morph";
        iconElement.sprite = getIcon("neutral");
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
  private static Sprite[] dialogIcons = null;
  private static Sprite getIcon(string name) {

    switch (name) {

      case "neutral":
        return dialogIcons[0];

      case "happy":
        return dialogIcons[1];

      case "laughing":
        return dialogIcons[2];

      case "surprised":
        return dialogIcons[3];

      case "sleepy":
        return dialogIcons[4];

      case "annoyed":
        return dialogIcons[5];

      case "angry":
        return dialogIcons[6];

      case "furious":
        return dialogIcons[7];

      case "sad":
        return dialogIcons[8];

      default:
        Debug.Log("DialogSystem: Given dialog item " + name + "couldn't be found. Displaying neutral icon.");
        break;

    }

    return dialogIcons[0];

  }


  private static float generalTimer = 0.0f;
  private static float moveTimer = 0.0f;
  private static float scrollTimeDown = 0.6f; // time for dialog box to appear / vanish
  private static float scrollTimeUp = 0.2f; // time for dialog box to appear / vanish
  private static float delayBeforeText = 0.3f; // delay before text appears on dialogbox that is already in position
  
  private static void MoveDialogBox() {

    float divisionValue = scrollTimeUp;

    if (moveDialog == "down") {
      divisionValue = scrollTimeDown;
      textElement.GetComponent<TextMeshProUGUI>().SetText("");
    }

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


  private static float audioClipLength = 0.0f;


  // text is still being written on screen
  private static bool typewriterRunning = false;
  private static string text = ""; // text
  private static string audio_path = ""; // path to audio
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




  private static void PlayVoice() {
    AudioClip clip = Resources.Load("Dialog/" + audio_path, typeof(AudioClip)) as AudioClip;
    audioSource.PlayOneShot(clip);

    // set length of audio clip + some buffer time
    audioClipLength = clip.length + 1.5f;
  }

}