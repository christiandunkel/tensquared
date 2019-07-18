using System.Collections;
using UnityEngine;

/*
 * powers the 'next level' menu at the end of any level
 */

public class LevelEnd : MonoBehaviour {

  /*
   * =================
   * === SINGLETON ===
   * =================
   */

  public static LevelEnd Instance;

  private void Awake() {
    Instance = this;
    initializeScript();
  }





  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  private int levelID;





  /*
   * ==================
   * === COMPONENTS ===
   * ==================
   */

  private CanvasGroup CG;
  private CanvasGroup endMenuContainer;
  private CanvasGroup levelCompleteCG;
  private CanvasGroup nextLevelCG;





  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  private void initializeScript() {

    /*
     * initializes the script and loads the required components
     */

    CG = gameObject.GetComponent<CanvasGroup>();

    levelID = LevelSettings.Instance.getInt(Level.LEVEL_ID);

    // get animators of child elements
    foreach (Transform child in transform) {

      if (child.gameObject.name != "LevelEndMenu") {
        continue;
      }

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





  /*
   * ================
   * === EXTERNAL ===
   * ================
   */

  public void endLevel(string soundName) {

    /*
     * disables all player controls and freezes them in position;
     * loads the 'end level' screen and 'next level' button
     */
    
    LevelTimer.Instance.saveTimer();

    // freeze player
    LevelSettings.Instance.setSetting(Player.CAN_MOVE, false);
    LevelSettings.Instance.setSetting(Player.CAN_JUMP, false);
    LevelSettings.Instance.setSetting(Player.CAN_SELF_DESTRUCT, false);
    LevelSettings.Instance.setSetting(Player.CAN_MORPH_TO_CIRCLE, false);
    LevelSettings.Instance.setSetting(Player.CAN_MORPH_TO_TRIANGLE, false);
    LevelSettings.Instance.setSetting(Player.CAN_MORPH_TO_RECTANGLE, false);

    PauseMenu.Instance.gameObject.SetActive(false);

    VolumeController.Instance.pauseBackgroundMusic();

    Log.Print($"Reached end of level {levelID}.", gameObject);

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

      // play 'level complete' sound
      SoundController.Instance.playSound(soundName);

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

  public void goToNextLevel() {

    /*
     * powers the button that leads player to next level
     */

    // hide level end menu
    CG.alpha = 0f;
    CG.interactable = false;

    Log.Print($"Go to next level, from level {levelID} to level {(levelID + 1)}.", this);

    SceneTransition.Instance.LoadScene("Level" + (levelID + 1));

  }

  public void goToMainMenu() {

    /*
     * powers the button that leads back to main menu
     */

    // hide level end menu
    CG.alpha = 0f;
    CG.interactable = false;

    Log.Print("Go to the main menu.", this);

    // disable start animations of main menu
    PositionAnimator.disabledAnimation = true;
    FadeOnStart.disableDelay();

    SceneTransition.Instance.LoadScene("MainMenu");

  }

}
