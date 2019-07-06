using UnityEngine;

/*
 * simple script to manage the accessability of the options menu (inside main menu)
 */

public class OptionsMenu : MonoBehaviour {

  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  private static bool optionsOpen = false;





  /*
   * ================
   * === EXTERNAL ===
   * ================
   */

  public void ToggleOptions() {

    /*
     * toggles the options menu
     */

    if (optionsOpen) {
      CloseOptions();
    }
    else {
      OpenOptions();
    }

  }

  public void OpenOptions() {

    /*
     * opens the options menu
     */

    optionsOpen = true;

    CanvasGroupFader.Instance.FadeIn(1);
    CanvasGroupFader.Instance.FadeOut(0);
    CanvasGroupFader.Instance.FadeOut(2);

  }

  public void CloseOptions() {

    /*
     * closes the options menu
     */

    optionsOpen = false;

    CanvasGroupFader.Instance.FadeOut(1);
    CanvasGroupFader.Instance.FadeIn(0);

  }

}
