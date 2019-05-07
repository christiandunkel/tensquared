using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class General : MonoBehaviour
{

  public void disableButton(Button btn)
  {
    btn.interactable = false;
  }

  public void enableButton(Button btn)
  {
    btn.interactable = true;
  }

  public void _loadSceneByName(string name)
  {

    SceneManager.LoadScene(name);

  }

  public void _quitGame()
  {

    // close the application
    Application.Quit();

    // for debug purposes in Unity editor
    Debug.Log("Quit the game!");

  }

}
