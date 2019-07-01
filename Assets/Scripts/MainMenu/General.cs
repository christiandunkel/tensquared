using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * includes some general usage functions
 */

public class General : MonoBehaviour {

  public void disableButton(Button btn) {
    Log.Print($"Disabled button {btn.name}.");
    btn.interactable = false;
  }

  public void enableButton(Button btn) {
    Log.Print($"Enabled button {btn.name}.");
    btn.interactable = true;
  }

  public void _loadSceneByName(string name) {
    Log.Print($"Loaded scene {name}.");
    SceneManager.LoadScene(name);
  }

  public void _quitGame() {

    // close the application
    Application.Quit();

    // for debug purposes in Unity editor
    Log.Print("Quit the game.");

  }

}
