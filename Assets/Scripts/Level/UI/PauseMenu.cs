using UnityEngine;

/*
 * powers the pause menu; manages pausing and resuming the game
 */

public class PauseMenu : MonoBehaviour {

  /*
   * =================
   * === SINGLETON ===
   * =================
   */
  public static PauseMenu Instance;

  private void Awake() {
    Instance = this;
  }





  /*
   * ==================
   * === COMPONENTS ===
   * ==================
   */

  public GameObject pauseMenuUI;





  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  private bool currentlyPaused;
  [SerializeField] private bool allowOpeningPauseMenu = true;

  // attributes for fading effect of pause menu
  private float fadeTimer = 0.0f;
  private float fadeinTime = 0.2f;
  private float fadeoutTime = 0.05f;

  private bool fadingNow = false;
  private bool fadein = false;

  private int fadeCount = 0;





  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  private void Start() {
    // set game as non-paused on start (hidden pause menu)
    CanvasGroup canvasGroup = pauseMenuUI.GetComponent<CanvasGroup>();
    canvasGroup.interactable = false;
    canvasGroup.alpha = 0.0f;
    Time.timeScale = 1.0f;
    currentlyPaused = false;

    Log.Print("Initialized.");
  }

  private void Update() {

    if (allowOpeningPauseMenu && !fadingNow && Input.GetKeyDown(KeyCode.Escape)) {
      
      if (currentlyPaused) {
        ResumeGame();
      }
      else {
        PauseGame();
      }

    }

    if (fadingNow) {
      Fade();
    }

  }

  private void FadeIn() {

    /*
     * sets the attributes that fade in the pause menu
     */

    fadingNow = true;
    fadein = true;

  }

  private void FadeOut() {

    /*
     * sets the attributes that fade out the pause menu
     */

    fadingNow = true;
    fadein = false;

  }

  private void Fade() {

    /*
     * powers the fade in or out effect of the pause menu
     */

    fadeTimer += Time.deltaTime;

    if (fadeTimer > (1 / (60f / (fadein ? fadeinTime : fadeoutTime)))) {

      fadeTimer = 0f;

      CanvasGroup canvasGroup = pauseMenuUI.GetComponent<CanvasGroup>();

      if (fadein) canvasGroup.alpha = (float)fadeCount / 10.0f;
      else canvasGroup.alpha = 1.0f - ((float)fadeCount / 10.0f);

      if (fadeCount >= 10) {

        fadeCount = 0;
        fadingNow = false;

        if (fadein) {
          canvasGroup.interactable = true;
          canvasGroup.alpha = 1f;

          // freeze game
          Time.timeScale = 0f;
        }
        else {
          canvasGroup.interactable = false;
          canvasGroup.alpha = 0f;
        }

      }

      fadeCount++;

    }

  }





  /*
   * ================
   * === EXTERNAL ===
   * ================
   */

  public void LoadMainMenu() {

    /*
     * powers the 'main menu' button in the pause menu
     */

    ResumeGame();

    Log.Print("Returned to main menu.", this);

    Time.timeScale = 1.0f; // resume game-speed
    currentlyPaused = false;

    // menu parts of main menu should be in place when returning to menu
    // -> therefore, deactivate animations
    PositionAnimator.disabledAnimation = true;
    FadeOnStart.disableDelay();

    SceneTransition.Instance.LoadScene("MainMenu");

  }

  public void ResumeGame() {

    /*
     * powers the 'resume game' button in the pause menu
     */

    // resume game-speed
    Time.timeScale = 1.0f; 

    // hide pause menu
    FadeOut();
    currentlyPaused = false;

    Log.Print("Closed the pause menu.", this);

  }

  public void PauseGame() {

    /*
     * pauses the game
     */

    // display pause menu
    FadeIn();
    currentlyPaused = true;

    Log.Print("Opened the pause menu.", this);

  }

  public bool isPaused() {

    /*
     * returns true, if the game is currently paused
     */

    return currentlyPaused;

  }

}
