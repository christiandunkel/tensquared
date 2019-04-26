using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

  public int LevelsUnlocked = 1;
  public List<GameObject> LevelButton;

  void Start()
  {
    // get children (lvl buttons) using transform property
    foreach (Transform child in transform)
    {
      LevelButton.Add(child.gameObject);
    }

    int counter = 1;
    foreach (GameObject lvl in LevelButton)
    {

      // disable buttons for levels not yet unlocked
      if (counter > LevelsUnlocked)
      {
        CanvasGroup CG = lvl.GetComponent<CanvasGroup>();
        CG.alpha = 0.2f; // opacity
        CG.interactable = false;
      }

      counter++;
    }

  }

}
