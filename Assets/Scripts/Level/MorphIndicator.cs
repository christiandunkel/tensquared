using System.Collections;
using UnityEngine;

/*
 * powers the 'morph indicator' UI on every level
 */

public class MorphIndicator : MonoBehaviour {

  public static MorphIndicator Instance;

  public CanvasGroup icon1, border1, text1,
                     icon2, border2, text2,
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

    loadMorphIndicators(
      player.GetString("state"),
      player.GetBool("canMorphToCircle"),
      player.GetBool("canMorphToTriangle"),
      player.GetBool("canMorphToRectangle")
    );

  }

  public void loadMorphIndicators(string playerState, bool canMorphToCircle, bool canMorphToTriangle, bool canMorphToRectangle) {

    /*
     * set the morphing indicator according to the given parameters
     */

    if (canMorphToCircle || playerState == "Circle") {
      icon1.alpha = 1f;
      border1.alpha = 1f;
      text1.alpha = 1f;
    }
    else {
      icon1.alpha = 0.1f;
      border1.alpha = 0f;
      text1.alpha = 0f;
    }

    if (canMorphToTriangle || playerState == "Triangle") {
      icon2.alpha = 1f;
      border2.alpha = 1f;
      text2.alpha = 1f;
    }
    else {
      icon2.alpha = 0.1f;
      border2.alpha = 0f;
      text2.alpha = 0f;
    }

    if (canMorphToRectangle || playerState == "Rectangle") {
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
