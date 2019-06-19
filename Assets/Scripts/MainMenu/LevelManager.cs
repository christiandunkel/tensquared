using System.Collections.Generic;

using UnityEngine;
using TMPro;

/*
 * manages the accessability of the levels (level buttons9 of the level menu (inside main menu)
 */

public class LevelManager : MonoBehaviour {

  // singleton
  public static LevelManager Instance;

  void Awake() {
    Instance = this;

    Debug.Log("LevelManager: Started.");

    LoadLevelProgess();
  }



  private int levelsUnlocked = 1;
  private List<GameObject> LevelButton = new List<GameObject>();

  // element containing all lvl btn gameobjects
  public GameObject lvlsParent = null;

  public void LoadLevelProgess() {

    Debug.Log("LevelManager: Loaded level progress.");

    // get 'level button' parent container
    lvlsParent = LevelManager.Instance.lvlsParent;

    // determine current number of unlocked levels
    if (PlayerPrefs.HasKey("lvls_unlocked")) {

      levelsUnlocked = PlayerPrefs.GetInt("lvls_unlocked");

      // REMOVE AFTER PRESENTATION
      levelsUnlocked = 3;

      // norm values if too big or too small
      levelsUnlocked = levelsUnlocked < 1 ? 1 : 
                         (levelsUnlocked > 10 ? 10 : levelsUnlocked);
      
    }

    Debug.Log("LevelManager: " + levelsUnlocked + " level(s) unlocked.");

    try { 

      // test if lvl button list is empty, and fill it if so
      if (LevelButton.Count == 0) {
        // get children (lvl buttons) using transform property
        foreach (Transform child in lvlsParent.transform) {
          LevelButton.Add(child.gameObject);
        }
      }

    }
    catch (System.Exception e) {
      Debug.LogWarning("LevelManager: Could not locate level buttons: " + e);
      return;
    } 

    int counter = 1;
    foreach (GameObject lvl in LevelButton) {

      CanvasGroup CG = lvl.GetComponent<CanvasGroup>();

      // disable buttons for levels not yet unlocked
      if (counter > levelsUnlocked) {
        CG.alpha = 0.2f; // opacity
        CG.interactable = false;
      }
      // if already unlocked, set visible again
      else {
        CG.alpha = 1.0f; // opacity
        CG.interactable = true;
      }

      // set timer values
      foreach (Transform obj in lvl.transform) {
        if (obj.name == "Timer") {
          string timer = PlayerPrefs.GetString("lvl" + counter + "_timer");
          obj.gameObject.GetComponent<TextMeshProUGUI>().text = timer;
        }
      }

      counter++;
    }

  }
}