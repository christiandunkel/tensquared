using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour {

  public static LevelTimer Instance;

  public GameObject timer;
  private bool timerIsActive = false;
  private bool timerLockedIn = false;
  private float currentTimer = 0f; 
  private string currentTimerString = "00:00:000";

  void Awake() {

    Instance = this;

    // if the next level relative to this one is already unlocked
    // activate the timer for this level
    if (PlayerPrefs.HasKey("lvls_unlocked") &&
        PlayerPrefs.GetInt("lvls_unlocked") > LevelSettings.Instance.levelID) {
      timerIsActive = true;
      GetComponent<CanvasGroup>().alpha = 1f;
    }

  }

  void Update() {

    if (!timerIsActive || timerLockedIn || PauseMenu.Instance.isPaused) return;

    // increase timer and display new number
    currentTimer += Time.fixedDeltaTime;

    // max timer value 95:99:999
    if (currentTimer > 9599999f) currentTimer = 9599999f;

    timer.GetComponent<TextMeshProUGUI>().text = convertToTimerFormat();

  }

  private string convertToTimerFormat() {

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

    int seconds = 0;

    timer = System.Text.RegularExpressions.Regex.Replace(timer, @":", "");
    seconds = System.Int32.Parse(timer);

    return seconds;

  }

  // save timer value in player prefs at the end of an level
  public void saveTimer() {

    if (!timerIsActive) {
      return;
    }

    timerLockedIn = true;

    int lvlID = LevelSettings.Instance.levelID;

    // get timer that's already saved in player prefs
    int oldTimer = convertStringTimerToInt(PlayerPrefs.GetString("lvl" + lvlID + "_timer"));

    // check if timer isn't 0 (default value), and new timer is faster/smaller than old one
    // if so, replace old timer with new timer (currentTimer)
    if (oldTimer == 0 || (int)(currentTimer * 1000f) < oldTimer) {
      Debug.Log("LevelTimer: Saved timer " + currentTimerString + " for level " + lvlID + ".");
      PlayerPrefs.SetString("lvl" + lvlID + "_timer", currentTimerString);
    }

  }

}
