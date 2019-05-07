using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{

  private int levelsUnlocked = 1;
  private List<GameObject> LevelButton = new List<GameObject>();

  // element containing all lvl btn gameobjects
  public GameObject lvlsParent = null;

  public void LoadLevelProgess()
  {

    Debug.Log("LevelManager: Loaded level progress.");

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

    // get children (lvl buttons) using transform property
    foreach (Transform child in lvlsParent.transform)
    {
      LevelButton.Add(child.gameObject);
    }

    int counter = 1;
    foreach (GameObject lvl in LevelButton)
    {

      // disable buttons for levels not yet unlocked
      if (counter > levelsUnlocked)
      {
        CanvasGroup CG = lvl.GetComponent<CanvasGroup>();
        CG.alpha = 0.2f; // opacity
        CG.interactable = false;
      }
      // if already unlocked, set timer value
      else
      {
        foreach (Transform obj in lvl.transform)
        {
          if (obj.name == "Timer")
          {
            string timer = PlayerPrefs.GetString("lvl" + counter + "_timer");
            obj.gameObject.GetComponent<TextMeshProUGUI>().text = timer;
          }
        }
      }

      counter++;
    }

  }

}
