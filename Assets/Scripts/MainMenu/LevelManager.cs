﻿using System.Collections.Generic;

using UnityEngine;
using TMPro;

/*
 * manages the accessability of the levels (level buttons9 of the level menu (inside main menu)
 */

public class LevelManager : MonoBehaviour {

  /*
   * =================
   * === SINGLETON ===
   * =================
   */

  public static LevelManager Instance;
  
  void Awake() {
    Instance = this;
    initializeManager();
  }





  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  private int levelsUnlocked = 1;
  // max numbers of level unlockable regardless of 'levelsUnlocked'
  private int maxLevelsUnlockable = 4;





  /*
   * ==================
   * === COMPONENTS ===
   * ==================
   */

  private List<GameObject> LevelButton = new List<GameObject>();

  // element containing all lvl btn gameobjects
  [SerializeField] private GameObject lvlsParent = null;





  /*
   * =================
   * === INTERNAL ====
   * =================
   */

  private void initializeManager() {

    /*
     * initializes the level manager
     */

    Log.Print($"Initialised on object '{gameObject.name}'.", this);

    LoadLevelProgess();

  }





  /*
   * =================
   * === EXTERNAL ====
   * =================
   */

  public int getInt(string name) {

    /*
     * returns the value of a given attribute with the name
     */

    switch (name) {

      case "levelsUnlocked":
        return levelsUnlocked;

      case "maxLevelsUnlockable":
        return maxLevelsUnlockable;

    }

    Log.Warn($"Could not find attribute of name {name}.", this);
    return 0;

  }

  public void LoadLevelProgess() {

    /*
     * loads the player's progress into
     * the level buttons in the level menu
     */

    // get 'level button' parent container
    lvlsParent = Instance.lvlsParent;

    // determine current number of unlocked levels
    if (PlayerPrefs.HasKey("lvls_unlocked")) {

      levelsUnlocked = PlayerPrefs.GetInt("lvls_unlocked");

      // REMOVE AFTER PRESENTATION
      levelsUnlocked = levelsUnlocked > maxLevelsUnlockable ? maxLevelsUnlockable : levelsUnlocked;

      // norm values if too big or too small
      levelsUnlocked = levelsUnlocked < 1 ? 1 : 
                         (levelsUnlocked > 10 ? 10 : levelsUnlocked);

    }

    Log.Print($"Loaded progress with {levelsUnlocked} level(s) unlocked.", this);

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
      Log.Warn($"Could not locate level buttons: {e}");
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
          string timer = "";

          if (PlayerPrefs.HasKey("lvl" + counter + "_timer")) {
            timer = PlayerPrefs.GetString("lvl" + counter + "_timer");

            // if timer is in wrong format, write default timer instead
            if (!SaveLoader.timerIsInRightFormat(timer)) {
              string default_timer = SaveLoader.getDefaultTimer();
              timer = default_timer;
              PlayerPrefs.SetString("lvl" + counter + "_timer", default_timer);
            }
          }
          
          obj.gameObject.GetComponent<TextMeshProUGUI>().text = timer;
        }
      }

      counter++;
    }

  }
}