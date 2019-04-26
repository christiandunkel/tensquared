using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;      // to change a scene

public class Buttons : MonoBehaviour {

  // open scene by id
  public void OpenScene(int id) {

    SceneManager.LoadScene(id);

  }

  // open scene by name
  public void OpenScene(string name)
  {

    SceneManager.LoadScene(name);

  }

  // closes the game
  public void QuitGame() {

    // close the application
    Application.Quit();

    // for debug purposes in Unity editor
    Debug.Log("Quit the game!");

  }

}
