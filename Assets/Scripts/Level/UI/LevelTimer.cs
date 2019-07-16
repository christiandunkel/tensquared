using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour {

  /*
   * =================
   * === SINGLETON ===
   * =================
   */

  public static LevelTimer Instance;

  private void Awake() {
    Instance = this;
    initialize();
  }





  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  [SerializeField] private GameObject timer = null;
  private bool timerIsActive = false;
  private bool timerLockedIn = false;
  private float currentTimer = 0f; 
  private string currentTimerString = "00:00:000";





  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  private void initialize() {

    /*
     * initializes the script
     */

    // if the next level relative to this one is already unlocked
    // activate the timer for this level
    if (PlayerPrefs.HasKey("lvls_unlocked") &&
        PlayerPrefs.GetInt("lvls_unlocked") > LevelSettings.Instance.getInt("levelID")) {
      timerIsActive = true;
      GetComponent<CanvasGroup>().alpha = 1f;
    }

  }

  private void Update() {

    if (!timerIsActive || timerLockedIn || PauseMenu.Instance.isPaused()) {
      return;
    }

    // increase timer and display new number
    currentTimer += Time.deltaTime;

    // max timer value 95:99:999
    if (currentTimer > 9599999f) {
      currentTimer = 9599999f;
    }

    timer.GetComponent<TextMeshProUGUI>().text = convertToTimerFormat();

  }

  private string convertToTimerFormat() {

    /*
     * converts the current timer value (seconds) to a string in the timer format
     */

    int temp = (int) (currentTimer * 1000f);

    string newTimerValue = temp.ToString();

    // pad left side with zeros
    newTimerValue = newTimerValue.PadLeft(7, '0');

    // insert colons into timer value
    newTimerValue = newTimerValue.Insert(2, ":");
    newTimerValue = newTimerValue.Insert(5, ":");

    currentTimerString = newTimerValue;

    return newTimerValue;

  }

  private int convertStringTimerToInt(string timer) {

    /*
     * converts the timer (string) to seconds (int)
     */

    int seconds = 0;

    timer = System.Text.RegularExpressions.Regex.Replace(timer, @":", "");
    seconds = System.Int32.Parse(timer);

    return seconds;

  }





  /*
   * ================
   * === EXTERNAL ===
   * ================
   */

  public void saveTimer() {

    /*
     * saves the timer value in player prefs
     */

    if (!timerIsActive) {
      return;
    }

    timerLockedIn = true;

    int lvlID = LevelSettings.Instance.getInt("levelID");

    // get timer that's already saved in player prefs
    int oldTimer = 99999999;
    if (PlayerPrefs.HasKey($"lvl{lvlID}_timer")) {
      oldTimer = convertStringTimerToInt(PlayerPrefs.GetString($"lvl{lvlID}_timer"));
    }

    // check if timer isn't 0 (default value), and new timer is faster/smaller than old one
    if (oldTimer == 0 || (int)(currentTimer * 1000f) < oldTimer) {

      Log.Print($"Saved timer {currentTimerString} for level {lvlID}.", this);

      // save the new timer in the player prefs 
      PlayerPrefs.SetString($"lvl{lvlID}_timer", currentTimerString);

    }

  }

}
