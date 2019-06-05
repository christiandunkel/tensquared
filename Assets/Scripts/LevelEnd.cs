using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour {

  public static LevelEnd Instance;

  public int levelID = 1;

  private CanvasGroup CG;

  void Awake() {
    Instance = this;
    CG = gameObject.GetComponent<CanvasGroup>();
  }

  public void endLevel() {

    Debug.Log("LevelEnd: Reached end of level.");

    LevelSettings.Instance.SetSetting("canMove", false);
    LevelSettings.Instance.SetSetting("canMorph", false);

    // activate next level
    if (!PlayerPrefs.HasKey("lvls_unlocked")) PlayerPrefs.SetInt("lvls_unlocked", levelID + 1);
    else if (!(levelID < PlayerPrefs.GetInt("lvls_unlocked"))) PlayerPrefs.SetInt("lvls_unlocked", levelID + 1);

    CG.alpha = 0;
    CG.interactable = true;

  }

  public void goToNextLevel() {

    Debug.Log("LevelEnd: Go to next level (" + levelID + "->" + (levelID + 1) + ").");

    SceneTransition.Instance.LoadScene("Level" + (levelID + 1));

  }

  public void goToMainMenu() {

    Debug.Log("LevelEnd: Returned to main menu.");

    SceneTransition.Instance.LoadScene("MainMenu");

  }

}
