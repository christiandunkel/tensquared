using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * powers the dialog boxes and roboter voice in the levels
 */

public class DialogSystem : MonoBehaviour {

  // singleton
  public static DialogSystem Instance;

  void Awake() {
    Instance = this;
    dialogQueue = new ArrayList();
  }

  // list of dialogs to be played one after the other
  private static ArrayList dialogQueue;
  private static Dialog currentDialogPlaying;

  // read-only states of dialog box
  private static bool dialogBoxVisible = false;

  // array containg all roboter icons as sprites
  private static Sprite[] dialogIcons = null;

  // elements of the dialog box
  private static Animator animator = null;
  private static AudioSource audioSource = null;
  private static GameObject dialogBox = null;
  private static TextMeshProUGUI textElement = null;
  private static GameObject panelElement = null;
  private static Image iconElement = null;



  void Start() {

    Debug.Log("DialogSystem: Loaded.");

    // load icons into sprite array
    dialogIcons = Resources.LoadAll<Sprite>("DialogIcons/");

    // get inner elements
    foreach (Transform child in gameObject.transform) {

      GameObject obj = child.gameObject;

      if (obj.name == "DialogBox") {

        dialogBox = obj;
        animator = dialogBox.GetComponent<Animator>();

        foreach (Transform child2 in obj.transform) {

          GameObject obj2 = child2.gameObject;

          // get parts of dialog system
          switch (obj2.name) {

            case "Background":
              panelElement = obj2;
              break;

            case "Text":
              textElement = obj2.GetComponent<TextMeshProUGUI>();
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

    // if dialogue is in queue and none playing, play it
    if (!dialogBoxVisible && dialogQueue.Count > 0) {
      loadDialogFromQueue();
    }

  }

  // adds the dialog to queue 
  // -> will be played the earliest where no other dialog is playing
  public static void LoadDialog(string name) {
    if (!dialogQueue.Contains(name)) {
      dialogQueue.Add(name);
    }
  }

  // load first dialog in queue into memory
  public static void loadDialogFromQueue() {

    // get dialog name
    string name = dialogQueue[0].ToString();
    dialogQueue.RemoveAt(0);

    // filter prefix of name
    Regex regex = new Regex(@"^lvl[0-9]{1,2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    Match match = regex.Match(name);

    Dialog dialog = new Dialog();

    switch (match.Value) {

      case "lvl1":
        dialog = DialogLevel1.getDialog(name);
        break;

      case "lvl2":
        //DialogLevel2.getDialog(name);
        break;

    }

    if (dialog.text == "") {
      Debug.LogWarning("DialogSystem: Could not find dialog '" + name + "'.");
      return;
    }

    // dialog is currently playing, break it
    if (dialogBoxVisible) {
      audioSource.Stop();
      DialogSystem.Instance.StopCoroutine(playDialog());
    }

    currentDialogPlaying = dialog;
    DialogSystem.Instance.StartCoroutine(playDialog());

  }

  // load an icon from array by key
  private static Sprite getDialogIcon(string iconName) {

    // get index of icon in sprite array by key
    int getIndex() {
      switch (iconName) {
        case "neutral":   return 0;
        case "happy":     return 1;
        case "laughing":  return 2;
        case "surprised": return 3;
        case "sleepy":    return 4;
        case "annoyed":   return 5;
        case "angry":     return 6;
        case "furious":   return 7;
        case "sad":       return 8;
        default:
          Debug.Log(
            "DialogSystem: Given dialog item " + iconName +
            " couldn't be found. Displaying neutral icon."
          );
          break;
      }
      return 0;
    }

    return dialogIcons[getIndex()];

  }

  // play dialog saved in 'currentDialogPlaying'
  private static IEnumerator playDialog() {

    // load robot image
    iconElement.sprite = getDialogIcon(currentDialogPlaying.icon);

    // info on visibility of dialog
    bool dialogBoxWasVisibleOnStart = dialogBoxVisible;
    dialogBoxVisible = true;

    // reset dialog
    textElement.SetText("");

    // play animation
    animator.SetBool("ShowDialog", true);
    yield return new WaitForSeconds(dialogBoxWasVisibleOnStart ? 0.15f : 0.5f);

    // play roboter voice
    audioSource.PlayOneShot(currentDialogPlaying.audioClip);

    // animate text
    string currentText = "";
    for (int i = 1; i <= currentDialogPlaying.textLength; i++) {
      currentText = currentDialogPlaying.text.Substring(0, i);
      textElement.SetText(currentText + "_");
      yield return new WaitForSeconds(.01f);
    }
    textElement.SetText(currentText); // remove '_' at the end

    // wait for complete audio clip length + some padding
    // (minus the time already waited while writing the dialog)
    yield return new WaitForSeconds(.4f + currentDialogPlaying.audioClipLength - (currentDialogPlaying.textLength * .01f));

    // fade out dialog
    animator.SetBool("ShowDialog", false);
    yield return new WaitForSeconds(0.3f);

    // reset dialog
    textElement.SetText("");
    dialogBoxVisible = false;
    DialogSystem.Instance.StopCoroutine(playDialog());

  }

}