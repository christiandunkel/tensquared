using UnityEngine;

/*
 * powers the pause menu; manages pausing and resuming the game
 */

public class PauseMenu : MonoBehaviour {

  // singleton
  public static PauseMenu Instance;
  private void Awake() {
    Instance = this;
  }

  public bool isPaused;

  public bool allowOpeningPauseMenu = true;

  public GameObject pauseMenuUI;

  private void Start() {
    // set game as non-paused on start (hidden pause menu)
    CanvasGroup canvasGroup = pauseMenuUI.GetComponent<CanvasGroup>();
    canvasGroup.interactable = false;
    canvasGroup.alpha = 0.0f;
    Time.timeScale = 1.0f;
    isPaused = false;

    Debug.Log("PauseMenu: Loaded.");
  }

  private void Update() {

    if (allowOpeningPauseMenu && !fadingNow && Input.GetKeyDown(KeyCode.Escape)) {
      
      if (isPaused) ResumeGame();
      else PauseGame();

    }

    if (fadingNow) Fade();

  }

  public void LoadMainMenu() {

    ResumeGame();

    Debug.Log("PauseMenu: Returned to main menu.");

    Time.timeScale = 1.0f; // resume game-speed
    isPaused = false;

    // menu parts of main menu should be in place when returning to menu
    // -> therefore, deactivate animations
    PositionAnimator.disabledAnimation = true;
    FadeOnStart.disableDelay();

    SceneTransition.Instance.LoadScene("MainMenu");

  }

  public void ResumeGame() {
    Time.timeScale = 1.0f; // resume game-speed
    FadeOut(); // hide pause menu
    isPaused = false;
    Debug.Log("PauseMenu: Closed.");
  }

  void PauseGame() {
    FadeIn(); // display pause menu
    isPaused = true;
    Debug.Log("PauseMenu: Opened.");
  }



  private float fadeTimer = 0.0f,
                fadeinTime = 0.2f,
                fadeoutTime = 0.05f;

  private bool fadingNow = false,
               fadein = false;

  private int fadeCount = 0;

  private void FadeIn() {
    fadingNow = true;
    fadein = true;
  }
  private void FadeOut() {
    fadingNow = true;
    fadein = false;
  }

  private void Fade() {

    fadeTimer += Time.deltaTime;

    if (fadeTimer > (1 / (60f / (fadein ? fadeinTime : fadeoutTime) ) ) ) {

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
          Time.timeScale = 0f; // freeze game
        }
        else {
          canvasGroup.interactable = false;
          canvasGroup.alpha = 0f;
        }

      }

      fadeCount++;

    }

  }

}
