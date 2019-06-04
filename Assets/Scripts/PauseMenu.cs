using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

  // singleton
  public static PauseMenu Instance;
  void Awake()
  {
    Instance = this;
  }

  public bool isPaused;

  public GameObject pauseMenuUI;

  void Start() {
    // set game as non-paused on start
    CanvasGroup canvasGroup = pauseMenuUI.GetComponent<CanvasGroup>();
    canvasGroup.interactable = false;
    canvasGroup.alpha = 0.0f;
    Time.timeScale = 1.0f;
    isPaused = false;

    Debug.Log("PauseMenu: Loaded.");
  }

  void Update()
  {

    if (!fadingNow && Input.GetKeyDown(KeyCode.Escape))
    {
      
      if (isPaused)
      {
        ResumeGame();
      }
      else
      {
        PauseGame();
      }
    }

    if (fadingNow)
    {
      Fade();
    }

  }

  public void LoadMainMenu()
  {

    Debug.Log("PauseMenu: Returned to main menu.");

    Time.timeScale = 1.0f;
    isPaused = false;

    // menu parts are already in place
    PositionAnimator.disabledAnimation = true;
    FadeOnStart.disableDelay = true;

    SceneTransition.Instance.LoadScene("MainMenu");

  }

  public void ResumeGame()
  {

    // resume game speed
    Time.timeScale = 1.0f;

    // vanish pause menu
    FadeOut();

    isPaused = false;
    Debug.Log("PauseMenu: Closed.");
  }

  void PauseGame()
  {
    // display pause menu
    FadeIn();

    isPaused = true;
    Debug.Log("PauseMenu: Opened.");
  }



  private float fadeinTime = 0.2f;
  private float fadeoutTime = 0.05f;

  private bool fadingNow = false;
  private bool fadein = false;
  private float fadeTimer = 0.0f;
  private int fadeCount = 1;

  private void FadeIn() { fadingNow = true; fadein = true; }
  private void FadeOut() { fadingNow = true; fadein = false; }

  private void Fade()
  {

    fadeTimer += Time.deltaTime;

    if (fadeTimer > (1 / (60.0f / (fadein ? fadeinTime : fadeoutTime) ) ) )
    {

      fadeTimer = 0.0f;

      CanvasGroup canvasGroup = pauseMenuUI.GetComponent<CanvasGroup>();

      if (fadein)
      {
        canvasGroup.alpha = (float)fadeCount / 10.0f;
      }
      else
      {
        canvasGroup.alpha = 1.0f - ((float)fadeCount / 10.0f);
      }

      if (fadeCount >= 10)
      {

        fadeCount = 0;
        fadingNow = false;

        if (fadein)
        {
          canvasGroup.interactable = true;
          canvasGroup.alpha = 1.0f;
          // freeze game
          Time.timeScale = 0.0f;
        }
        else
        {
          canvasGroup.interactable = false;
          canvasGroup.alpha = 0.0f;
        }

      }

      fadeCount++;

    }

  }

}
