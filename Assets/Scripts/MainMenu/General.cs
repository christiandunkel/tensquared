using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * includes some general usage functions
 */

public class General : MonoBehaviour {

  /*
   * ================
   * === EXTERNAL ===
   * ================
   */

  public void enableButton(Button btn) {

    /*
     * enables given button
     */

    Log.Print($"Enabled button {btn.gameObject.name}.");
    btn.interactable = true;

  }

  public void disableButton(Button btn) {

    /*
     * disables given button
     */

    Log.Print($"Disabled button {btn.gameObject.name}.");
    btn.interactable = false;

  }

  public void _loadSceneByName(string name) {

    /*
     * loads a scene with a certain name
     */

    Log.Print($"Loaded scene {name}.");
    SceneManager.LoadScene(name);

  }

  public void _quitGame() {

    /*
     * closes the game
     */

    // close the application
    Application.Quit();

    // for debug purposes in Unity editor
    Log.Print("Quit the game.");

  }

}
