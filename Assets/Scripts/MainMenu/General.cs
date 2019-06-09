using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * includes some general usage functions
 */

public class General : MonoBehaviour {

  public void disableButton(Button btn) {
    Debug.Log("General: Disabled button " + btn.name + ".");
    btn.interactable = false;
  }

  public void enableButton(Button btn) {
    Debug.Log("General: Enabled button " + btn.name + ".");
    btn.interactable = true;
  }

  public void _loadSceneByName(string name) {
    Debug.Log("General: Loaded scene " + name + ".");
    SceneManager.LoadScene(name);
  }

  public void _quitGame() {

    // close the application
    Application.Quit();

    // for debug purposes in Unity editor
    Debug.Log("General: Quit the game.");

  }

}
