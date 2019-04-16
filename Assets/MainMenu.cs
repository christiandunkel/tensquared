using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;      // to change a scene

public class MainMenu : MonoBehaviour {

  // open the level menu
  public void openLevelMenu() {

    // gets ID of level menu, by taking the current scene's (main menu) index
    // which is defined as 0 in the build settings, and adding 1, which goes to
    // the next scene's (level menu) index, which is defined as 1 in the build settings
    int levelMenuId = SceneManager.GetActiveScene().buildIndex + 1;

    SceneManager.LoadScene(levelMenuId);

  }


  // closes the game
  public void quitGame() {

    Debug.Log("Quit the game!");

    // close the application
    Application.Quit();

  }

}
