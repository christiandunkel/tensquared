using System.Collections;
using UnityEngine;

/*
 * powers the 'morph indicator' UI on every level
 */

public class MorphIndicator : MonoBehaviour {

  public static MorphIndicator Instance;

  public CanvasGroup icon2, border2, text2,
                     icon3, border3, text3;
  
  private void Awake() {
    Instance = this;
  }

  public void loadMorphIndicators() {

    /*
     * set morphing indicator according to internal player settings
     * (not level settings)
     */

    PlayerController player = PlayerController.Instance;

    if (player.getBool("canMorphToTriangle")) {
      icon2.alpha = 1f;
      border2.alpha = 1f;
      text2.alpha = 1f;
    }
    else {
      icon2.alpha = 0.1f;
      border2.alpha = 0f;
      text2.alpha = 0f;
    }

    if (player.getBool("canMorphToRectangle")) {
      icon3.alpha = 1f;
      border3.alpha = 1f;
      text3.alpha = 1f;
    }
    else {
      icon3.alpha = 0.1f;
      border3.alpha = 0f;
      text3.alpha = 0f;
    }

  }

  public void loadMorphIndicators(bool canMorphToTriangle, bool canMorphToRectangle) {

    /*
     * set the morphing indicator according to the given parameters
     */

    if (canMorphToTriangle) {
      icon2.alpha = 1f;
      border2.alpha = 1f;
      text2.alpha = 1f;
    }
    else {
      icon2.alpha = 0.1f;
      border2.alpha = 0f;
      text2.alpha = 0f;
    }

    if (canMorphToRectangle) {
      icon3.alpha = 1f;
      border3.alpha = 1f;
      text3.alpha = 1f;
    }
    else {
      icon3.alpha = 0.1f;
      border3.alpha = 0f;
      text3.alpha = 0f;
    }

  }

}
