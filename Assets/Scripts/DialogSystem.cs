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

  // read-only states of dialog box
  private static bool dialogBoxVisible = false;
  private static bool typewriterRunning = false;

  // array containg all roboter icons as sprites
  private static Sprite[] dialogIcons = null;

  // elements of the dialog box
  private static Animator animator = null;
  private static AudioSource audioSource = null;
  private static GameObject dialogBox = null;
  private static GameObject textElement = null;
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
            case "Background":  panelElement = obj2; break;
            case "Text":        textElement = obj2; break;
            case "Icon":        iconElement = obj2.GetComponent<Image>(); break;
            default: break;
          }

        }
        
      }
      else if (obj.name == "DialogAudioSource") {
        audioSource = obj.GetComponent<AudioSource>();
      }

    }

  }




  public static void LoadDialog(string name) {

    DialogSystem.Instance.StopCoroutine(PlayDialog());
    string icon = "";

    switch (name) {

      case "lvl1_hello":
        text = "Hello, my little friend!";
        text += "\nDo you have some time to help me out?";
        audio_path = "lvl1_hello";
        icon = "laughing";
        break;

      case "lvl1_asleep":
        text = "Is this fellow asleep perhaps?";
        audio_path = "lvl1_asleep";
        icon = "annoyed";
        break;

      case "lvl1_wake_up":
        text = "Wake up, little friend!\n";
        text += "It's not polite to sleep, when there is a robot in need!";
        audio_path = "lvl1_wake_up";
        icon = "angry";
        break;

      case "lvl1_move":
        text = "Finally, that's better!\n";
        text += "Now that you're awake, can you walk or roll around?";
        audio_path = "lvl1_move";
        icon = "laughing";
        break;

      case "lvl1_jump":
        text = "Little friend, are you able to jump over that thing?";
        audio_path = "lvl1_jump";
        icon = "neutral";
        break;

      case "lvl1_dont_jump_into_water":
        text = "I wouldn't jump into the water if I were you.";
        text += "\nBecause that isn't water, and it is deadly.";
        audio_path = "lvl1_dont_jump_into_water";
        icon = "neutral";
        break;

      case "lvl1_not_the_smartest_circle":
        text = "You really aren't the smartest circle out there, isn't that right?";
        audio_path = "lvl1_not_the_smartest_circle";
        icon = "annoyed";
        break;

      case "lvl1_you_dont_learn":
        text = "You don't learn, do you?";
        audio_path = "lvl1_you_dont_learn";
        icon = "furious";
        break;

      case "lvl1_quick_compared_to_other_circles":
        text = "I have to compliment you! Once you finally woke up, you're actually quite quick on foot, especially in comparison to other circles!";
        audio_path = "lvl1_quick_compared_to_other_circles";
        icon = "happy";
        break;

      case "lvl1_morph":
        text = "No matter how fast you are, sometimes you just can't overcome an obstacle as a circle.";
        audio_path = "lvl1_morph";
        icon = "neutral";
        break;

      default:
        Debug.Log("DialogSystem: Could not find dialog \"" + name + "\".");
        return;

    }

    iconElement.sprite = getIcon(icon);

    DialogSystem.Instance.StartCoroutine(PlayDialog());

  }



  private static Sprite getIcon(string name) {

    switch (name) {
      case "neutral":   return dialogIcons[0];
      case "happy":     return dialogIcons[1];
      case "laughing":  return dialogIcons[2];
      case "surprised": return dialogIcons[3];
      case "sleepy":    return dialogIcons[4];
      case "annoyed":   return dialogIcons[5];
      case "angry":     return dialogIcons[6];
      case "furious":   return dialogIcons[7];
      case "sad":       return dialogIcons[8];

      default:
        Debug.Log("DialogSystem: Given dialog item " + name + "couldn't be found. Displaying neutral icon.");
        break;
    }

    return dialogIcons[0];

  }

  
  private static float audioClipLength = 0.0f;

  // text is still being written on screen
  private static string text = "",
          audio_path = "", // path to audio
          currentText = ""; // temporary, current progress of typewriter
  private static float delayBetweenChars; // calculated by audio clip and text length

  private static IEnumerator PlayDialog() {

    dialogBoxVisible = true;
    animator.SetBool("ShowDialog", true);
    yield return new WaitForSeconds(0.5f);

    currentText = "";
    textElement.GetComponent<TextMeshProUGUI>().SetText("");

    // play roboter voice
    AudioClip clip = Resources.Load("Dialog/" + audio_path, typeof(AudioClip)) as AudioClip;
    audioSource.PlayOneShot(clip);

    // set length of audio clip + some buffer time
    audioClipLength = clip.length + 1.5f;

    // delay in seconds after each character before next one is written
    delayBetweenChars = audioClipLength / text.Length;

    for (int i = 1; i <= text.Length; i++) {

      currentText = text.Substring(0, i);
      textElement.GetComponent<TextMeshProUGUI>().SetText(currentText);

      if (i == text.Length) {
        typewriterRunning = false;
      }

      yield return new WaitForSeconds(delayBetweenChars);

    }
    
    yield return new WaitForSeconds(0.4f);
    animator.SetBool("ShowDialog", false);

    yield return new WaitForSeconds(0.3f);
    textElement.GetComponent<TextMeshProUGUI>().SetText("");
    dialogBoxVisible = false;
    DialogSystem.Instance.StopCoroutine(PlayDialog());

  }

}