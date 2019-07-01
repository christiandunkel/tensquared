using System.Collections;
using UnityEngine;

/*
 * powers the 'next level' menu at the end of any level
 */

public class LevelEnd : MonoBehaviour {

  // singleton
  public static LevelEnd Instance;

  public int levelID;

  private CanvasGroup CG;
  private CanvasGroup endMenuContainer;
  private CanvasGroup levelCompleteCG;
  private CanvasGroup nextLevelCG;

  private void Awake() {

    Instance = this;
    CG = gameObject.GetComponent<CanvasGroup>();

    levelID = LevelSettings.Instance.levelID;

    // get animators of child elements
    foreach (Transform child in transform) {

      if (child.gameObject.name != "LevelEndMenu") continue;

      endMenuContainer = child.gameObject.GetComponent<CanvasGroup>();

      foreach (Transform child2 in child.gameObject.transform) {

        switch (child2.gameObject.name) {

          case "LevelCompleteAnimation":
            levelCompleteCG = child2.gameObject.GetComponent<CanvasGroup>();
            break;

          case "NextLevelAnimation":
            nextLevelCG = child2.gameObject.GetComponent<CanvasGroup>();
            break;

        }

      }

    }
  }

  public void endLevel() {
    
    LevelTimer.Instance.saveTimer();

    LevelSettings.Instance.setSetting("canMove", false);
    LevelSettings.Instance.setSetting("canJump", false);
    LevelSettings.Instance.setSetting("canMorphToCircle", false);
    LevelSettings.Instance.setSetting("canMorphToTriangle", false);
    LevelSettings.Instance.setSetting("canMorphToRectangle", false);

    PauseMenu.Instance.gameObject.SetActive(false);

    VolumeController.Instance.pauseBackgroundMusic();

    Debug.Log("LevelEnd: Reached end of level.");

    // activate next level
    if (!PlayerPrefs.HasKey("lvls_unlocked")) {
      PlayerPrefs.SetInt("lvls_unlocked", levelID + 1);
    }
    else if (levelID >= PlayerPrefs.GetInt("lvls_unlocked")) {
      PlayerPrefs.SetInt("lvls_unlocked", levelID + 1);
    }

    CG.alpha = 1f;
    CG.interactable = true;

    // play end-screen animation
    StartCoroutine(endSceenAnimation());

    IEnumerator endSceenAnimation() {

      SoundController.Instance.playSound("levelCompleteSound");

      // fade in 'level complete' animation
      for (int i = 0; i < 50; i++) {
        endMenuContainer.alpha = ((float)i) * 2f / 100f;
        yield return new WaitForSeconds(0.01f);
      }
      endMenuContainer.alpha = 1f;
      endMenuContainer.interactable = true;
      yield return new WaitForSeconds(3f);


      // fade out 'level complete' animation
      for (int i = 0; i < 50; i++) {
        levelCompleteCG.alpha = 1 - (((float)i) * 2f / 100f);
        yield return new WaitForSeconds(0.006f);
      }
      levelCompleteCG.alpha = 0f;
      yield return new WaitForSeconds(.2f);


      // fade in 'next level' menu
      for (int i = 0; i < 50; i++) {
        nextLevelCG.alpha = ((float)i) * 2f / 100f;
        yield return new WaitForSeconds(0.006f);
      }
      nextLevelCG.alpha = 1f;
      nextLevelCG.interactable = true;

      StopCoroutine(endSceenAnimation());

    }

  }

  // button that leads player to next level
  public void goToNextLevel() {

    CG.alpha = 0f;
    CG.interactable = false;

    Debug.Log("LevelEnd: Go to next level (" + levelID + "->" + (levelID + 1) + ").");

    SceneTransition.Instance.LoadScene("Level" + (levelID + 1));

  }

  // button that leads back to main menu
  public void goToMainMenu() {

    CG.alpha = 0f;
    CG.interactable = false;

    Debug.Log("LevelEnd: Returned to main menu.");

    PositionAnimator.disabledAnimation = true;
    FadeOnStart.disableDelay = true;

    SceneTransition.Instance.LoadScene("MainMenu");

  }

}
