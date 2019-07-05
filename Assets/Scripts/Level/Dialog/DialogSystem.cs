using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DigitalRuby.SimpleLUT;

/*
 * powers the dialog boxes and roboter voice in the levels
 */

public class DialogSystem : MonoBehaviour {

  /*
   * =================
   * === SINGLETON ===
   * =================
   */

  public static DialogSystem Instance;

  private void Awake() {
    Instance = this;
    dialogQueue = new ArrayList();
  }





  /*
   * ==================
   * === COMPONENTS ===
   * ==================
   */

  // array containg all roboter icons as sprites
  private static Sprite[] dialogIcons = null;
  private static Sprite[] dialogIconsEvil = null;

  // robot emoticon
  private static Image iconElement = null;

  private static Animator animator = null;
  private static AudioSource audioSource = null;

  // elements of the dialog box
  private static GameObject dialogBox = null;
  private static TextMeshProUGUI textElement = null;

  // background of dialogue box
  private static GameObject panelElement = null;
  private static Image panelElementImage = null;

  // elements for red jittering effect on the evil robot voice
  private static GameObject jitterContainer = null;
  private static GameObject[] jitterElements = null;
  private static Vector2 jitterElementStartPos = Vector2.zero;
  private static int jitterElementAmount = 4;
  private static SimpleLUT colorGrading;
  private static Color originalTint;
  private static Color evilRedTint = new Color(169f / 255f, 37f / 255f, 33f / 255f);

  // audio visualization
  private static LineRenderer[] voiceLineRenderers;





  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  // list of dialogs to be played one after the other
  private static ArrayList dialogQueue;
  private static Dialog currentDialogPlaying;

  // states of dialog box
  private static bool dialogBoxVisible = false;
  private static bool dialogBoxWasVisibleInLastFrame = false;

  private static float speechVolumePercentage = 0f;







  /*
   * ======================
   * === INITIALISATION ===
   * ======================
   */

  private void Start() {

    // reset dialog system on start/restart of level
    dialogBoxVisible = false;
    dialogQueue.Clear();


    Debug.Log("DialogSystem: Loaded.");


    // volume level of speech sound players as set via the volume controller
    // as percentage
    speechVolumePercentage = VolumeController.Instance.getVolume("speech") * 100;


    // load audio visualisation objects
    GameObject[] lr_objs = GameObject.FindGameObjectsWithTag("VoiceLineRenderer");
    voiceLineRenderers = new LineRenderer[lr_objs.Length];
    int counter = 0;
    foreach (GameObject obj in lr_objs) {
      voiceLineRenderers[counter] = obj.GetComponent<LineRenderer>();
      counter++;
    }
    

    // load icons into sprite array
    dialogIcons = Resources.LoadAll<Sprite>("DialogIcons/");
    dialogIconsEvil = Resources.LoadAll<Sprite>("DialogIconsEvil/");

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

            case "BackgroundJitterEffect":
              jitterContainer = obj2;
              jitterElements = new GameObject[jitterElementAmount];
              // get single elements inside jitter container,
              // that will later be animated when evil robot voice is playing
              int i = 0;
              foreach (Transform child3 in jitterContainer.transform) {
                jitterElements[i] = child3.gameObject;

                // get start position of jitter elements, 
                // which is the same for all of them
                if (i == 0) {
                  jitterElementStartPos = child3.gameObject.transform.localPosition;
                }

                i++;
              }

              // get element on main camera for color grading
              GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
              colorGrading = mainCamera.GetComponent<SimpleLUT>();
              originalTint = colorGrading.TintColor;
              break;

            case "Background":
              panelElement = obj2;
              panelElementImage = panelElement.GetComponent<Image>();
              break;

            case "Text":
              textElement = obj2.GetComponent<TextMeshProUGUI>();
              break;

            case "Icon":
              iconElement = obj2.GetComponent<Image>();
              break;

          }

        }
        
      }
      else if (obj.name == "DialogAudioSource") {
        audioSource = obj.GetComponent<AudioSource>();
      }

    }

  }





  /*
   * ================
   * === INTERNAL ===
   * ================
   */
   private float colorGradingTimer = 0f;

  private void Update() {

    // if dialogue is in queue and none playing, play it
    if (!dialogBoxVisible && dialogQueue.Count > 0) {
      loadDialogFromQueue();
    }

    if (dialogBoxVisible) {

      if (!dialogBoxWasVisibleInLastFrame) {
        colorGradingTimer = 0f;
      }

      // visualiaze voice line renderers
      if (voiceLineRenderers.Length > 0) {
        visualizeVoice();
      }

      // display red-tinted flicker effect on dialog box
      if (currentDialogPlaying.isEvil) {
        visualizeEvilVoice();

        if (colorGradingTimer <= 1f) {
          colorGradingTimer += (Time.fixedDeltaTime / 4f);
        }

        if (colorGrading != null) {
          // adjust color grading on evil voice
          colorGrading.TintColor = Color.Lerp(colorGrading.TintColor, evilRedTint, colorGradingTimer);
        }
      }

      dialogBoxWasVisibleInLastFrame = true;

    }
    // reset dialogue box after dialogue is over
    else {

      if (dialogBoxWasVisibleInLastFrame) {

        // reset position of elements of jitter effect
        for (int i = 0; i < jitterElementAmount; i++) {
          jitterElements[i].transform.localPosition = jitterElementStartPos;
        }

        dialogBoxWasVisibleInLastFrame = false;

        colorGradingTimer = 0f;

      }

      if (colorGrading != null) {

        if (colorGradingTimer <= 1f) {
          colorGradingTimer += (Time.fixedDeltaTime / 4f);
        }

        // reset color grading back to normal after evil voice dialogue is over
        colorGrading.TintColor = Color.Lerp(colorGrading.TintColor, originalTint, colorGradingTimer);

      }

    }

  }

  private static void loadDialogFromQueue() {

    /*
     * load first dialog in queue into memory
     */

    // get dialog name
    string name = dialogQueue[0].ToString();
    dialogQueue.RemoveAt(0);

    // filter prefix of name
    Regex regex = new Regex(@"^lvl[0-9]{1,2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    Match match = regex.Match(name);

    Dialog dialog = new Dialog();

    // get dialog from class file
    switch (match.Value) {

      case "lvl1":
        dialog = DialogLevel1.getDialog(name);
        break;

      case "lvl2":
        dialog = DialogLevel2.getDialog(name);
        break;

      case "lvl3":
        dialog = DialogLevel3.getDialog(name);
        break;

      case "lvl4":
        dialog = DialogLevel4.getDialog(name);
        break;

    }

    if (dialog.text == "") {
      Debug.LogWarning("DialogSystem: Could not find dialog '" + name + "'.");
      return;
    }

    // dialog is currently playing, break it
    if (dialogBoxVisible) {
      audioSource.Stop();
      Instance.StopCoroutine(playDialog());
    }

    currentDialogPlaying = dialog;
    Instance.StartCoroutine(playDialog());



    IEnumerator playDialog() {

      /*
       * plays dialog saved in 'currentDialogPlaying':
       * fades the dialogue into the screen
       * writes text, plays voice, then fades out
       */

      dialogBoxVisible = true;

      // reset dialog
      textElement.SetText("");

      // load robot image
      iconElement.sprite = getDialogIcon(currentDialogPlaying.icon, currentDialogPlaying.isEvil);

      // play animation
      animator.SetBool("ShowDialog", true);
      yield return new WaitForSeconds(0.2f);

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
      yield return new WaitForSeconds(0.2f);

      // reset dialog
      textElement.SetText("");
      dialogBoxVisible = false;
      Instance.StopCoroutine(playDialog());

    }

  }

  private static Sprite getDialogIcon(string iconName, bool evilVoice) {

    /*
     * returns the roboter icon with given name
     * loads the icon from array by key
     */

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
      }
      Debug.LogWarning(
        "DialogSystem: Given dialog item " + iconName + " couldn't be found. Displaying neutral icon."
      );
      return 0;
    }

    // return sprite at index
    return evilVoice ? dialogIconsEvil[getIndex()] : dialogIcons[getIndex()];

  }

  private static void visualizeVoice() {

    /*
     * takes all defined line renderers and applies the sound frequency data values,
     * extracted from the dialog audio clip, to the line as points
     */

    int dataPoints = 64;
    float[] samples = new float[dataPoints];

    // get sine data values from audio clip playing in player
    audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);

    // as the sample data is affected by the volume set in the volume controller
    // -> need to correct values with current speech volume
    for (int i = 0; i < dataPoints; i++) {
      samples[i] *= (100f / speechVolumePercentage);
    }

    // go through all line renderers found in scene
    foreach (LineRenderer lr in voiceLineRenderers) {

      float posX = lr.transform.position.x;
      float posY = lr.transform.position.y;

      // temporary variable in which to save points later applied to line renderer
      Vector3[] points = new Vector3[dataPoints];

      // go through all audio values and convert them to points on the line renderer
      for (int i = 0; i < dataPoints; i++) {
        points[i] = Vector3.zero;
        points[i].x = posX + (i / 1.3f);
        points[i].y = posY + (samples[i] * 50);

        // hard max-height of each point = 2f over y position of line renderer
        if (Mathf.Abs(posY - points[i].y) > 2f) {
          points[i].y = posY + 2f;
        }
      }

      // apply converted audio values to line renderer
      lr.SetPositions(points);

    }

  }

  private static void visualizeEvilVoice() {

    /*
     * applies the highest sound frequency data values from the voice
     * as a red tint to the dialog background
     */

    int dataPoints = 64;
    float[] samples = new float[dataPoints];

    // get sine data values from audio clip playing in player
    audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);

    // as the sample data is affected by the volume set in the volume controller
    // -> need to correct values with current speech volume
    for (int i = 0; i < dataPoints; i++) {
      samples[i] *= (100f / speechVolumePercentage);
    }

    // find highest frequency value in array
    float max = samples[0];

    for (int i = 1; i < dataPoints; i++) {
      if (samples[i] > max) {
        max = samples[i];
      }
    }

    // increase value to be significantly high enough to matter
    max *= 5000;
    // if audio was too loud and exceeds 255 RGB value, limit it
    if (max > 255f) {
      max = 255f;
    }

    // normalize color to be between 0 and 1
    float normalizedColor = max / 255f;
    // limit the color value to 0.6
    if (normalizedColor > 0.6f) {
      normalizedColor = 0.6f;
    }

    // get previous normalized color
    float previousNormalizedColor = 1f - panelElementImage.color.g;
    // calculate mid point between the two colors to smoothen the transition
    float smoothNormalizedColor = (normalizedColor + previousNormalizedColor) / 2;

    Color redTint = new Color(1f, 1f - smoothNormalizedColor, 1f - smoothNormalizedColor);

    panelElementImage.color = redTint;

    // animate all 4 jitter elements depending on the audio sin curve
    float offsetJitterElement = smoothNormalizedColor * 20f;
    for (int i = 0; i < jitterElementAmount; i++) {
      Vector2 newPos = jitterElementStartPos;
      // animate the 4 elements in the 4 different diagonal directions
      newPos.x += (i == 1 || i == 2) ? offsetJitterElement : -offsetJitterElement;
      newPos.y += (i == 1 || i == 3) ? offsetJitterElement : -offsetJitterElement;
      jitterElements[i].transform.localPosition = newPos;
    }

  }





  /*
   * ================
   * === EXTERNAL ===
   * ================
   */

  public static void loadDialog(string name) {

    /*
     * add a given dialogue to the queue;
     * it will be played the earliest 
     * where no other dialog is playing
     */

    if (!dialogQueue.Contains(name)) {
      dialogQueue.Add(name);
    }

  }

  public static LineRenderer[] getLineRenderers() {

    /*
     * returns an array of all line renderers
     */

    return voiceLineRenderers;

  }

  public static LineRenderer getLineRenderer(int id) {

    /*
     * returns the line renderer with the given id
     */

    if (id >= voiceLineRenderers.Length) {
      Debug.LogWarning("DialogSystem: Voice line renderer array (Length: " + voiceLineRenderers.Length + ")" +
                       " does not contain an id = " + id + ".");
      return null;
    }

    return voiceLineRenderers[id];

  }

  public static LineRenderer getLineRenderer(string name) {

    /*
     * returns the line renderer with the given object name
     */

    foreach (LineRenderer lr in voiceLineRenderers) {

      if (lr.gameObject.name == name) {
        return lr;
      }

    }

    Debug.LogWarning("DialogSystem: No voice line renderer with the name '" + name + "' found.");
    return null;

  }

}