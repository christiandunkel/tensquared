using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{

  private static int levelsUnlocked = 1;
  private static List<GameObject> LevelButton = new List<GameObject>();

  // element containing all lvl btn gameobjects
  private static GameObject lvlsParent = null;

  public void LoadLevelProgess_()
  {
    LoadLevelProgess();
  }

  public static void LoadLevelProgess()
  {

    Debug.Log("LevelManager: Loaded level progress.");

    // fetch 'lvl btns' container object by tag, if undefined
    if (lvlsParent == null)
    {
      lvlsParent = GameObject.FindGameObjectWithTag("LevelParentContainer");
    }

    // determine current number of unlocked levels
    if (PlayerPrefs.HasKey("lvls_unlocked"))
    {
      levelsUnlocked = PlayerPrefs.GetInt("lvls_unlocked");

      // norm values if too big or too small
      if (levelsUnlocked < 1)
      {
        levelsUnlocked = 1;
      }
      else if (levelsUnlocked > 10)
      {
        levelsUnlocked = 10;
      }

    }

    Debug.Log("LevelManager: " + levelsUnlocked + " level(s) unlocked.");

    // test if lvl button list is empty, and fill it if so
    if (LevelButton.Count == 0)
    {
      // get children (lvl buttons) using transform property
      foreach (Transform child in lvlsParent.transform)
      {
        LevelButton.Add(child.gameObject);
      }
    }

    int counter = 1;
    foreach (GameObject lvl in LevelButton)
    {

      CanvasGroup CG = lvl.GetComponent<CanvasGroup>();

      // disable buttons for levels not yet unlocked
      if (counter > levelsUnlocked)
      {
        CG.alpha = 0.2f; // opacity
        CG.interactable = false;
      }
      // if already unlocked, set visible again
      else
      {
        CG.alpha = 1.0f; // opacity
        CG.interactable = true;
      }

      // set timer values
      foreach (Transform obj in lvl.transform)
      {
        if (obj.name == "Timer")
        {
          string timer = PlayerPrefs.GetString("lvl" + counter + "_timer");
          obj.gameObject.GetComponent<TextMeshProUGUI>().text = timer;
        }
      }

      counter++;
    }

  }

}
