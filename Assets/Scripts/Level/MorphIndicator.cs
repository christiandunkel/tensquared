using System.Collections;
using UnityEngine;

/*
 * powers the 'morph indicator' UI on every level
 */

public class MorphIndicator : MonoBehaviour {

  public static MorphIndicator Instance;
  private void Awake() {
    Instance = this;
  }


  [SerializeField] private CanvasGroup icon0 = null, border0 = null, text0 = null; // self-destruct symbol
  [SerializeField] private CanvasGroup icon1 = null, border1 = null, text1 = null; // circle symbol
  [SerializeField] private CanvasGroup icon2 = null, border2 = null, text2 = null; // triangle symbol
  [SerializeField] private CanvasGroup icon3 = null, border3 = null, text3 = null; // rectangle symbol
  [SerializeField] private GameObject stateIndicator = null;
  
  public void loadMorphIndicators() {

    /*
     * set morphing indicator according to internal player settings
     * (not level settings)
     */

    PlayerManager player = PlayerManager.Instance;

    loadMorphIndicators(
      player.getString("state"),
      player.getBool("canSelfDestruct"),
      player.getBool("canMorphToCircle"),
      player.getBool("canMorphToTriangle"),
      player.getBool("canMorphToRectangle")
    );

  }

  public void loadMorphIndicators(string playerState, bool canSelfDestruct, 
    bool canMorphToCircle, bool canMorphToTriangle, bool canMorphToRectangle) {

    /*
     * set the morphing indicator according to the given parameters
     */

    // tests if at least two states can be morphed into,
    // only then display the 'current state' indicator
    if (canMorphToCircle && (canMorphToTriangle || canMorphToRectangle) ||
        (canMorphToTriangle && canMorphToRectangle)) {
      stateIndicator.SetActive(true);
    }
    else {
      stateIndicator.SetActive(false);
    }

    // display state indicator in position of the icon that corresponds the 'current state'
    switch (playerState) {
      case "Circle":
        stateIndicator.transform.localPosition = new Vector3(icon1.transform.localPosition.x, stateIndicator.transform.localPosition.y, stateIndicator.transform.localPosition.z); break;
      case "Triangle":
        stateIndicator.transform.localPosition = new Vector3(icon2.transform.localPosition.x, stateIndicator.transform.localPosition.y, stateIndicator.transform.localPosition.z); break;
      case "Rectangle":
        stateIndicator.transform.localPosition = new Vector3(icon3.transform.localPosition.x, stateIndicator.transform.localPosition.y, stateIndicator.transform.localPosition.z); break;
    }


    if (canSelfDestruct) {
      fadeCanvasGroup(icon0, 1f);
      fadeCanvasGroup(border0, 1f);
      fadeCanvasGroup(text0, 1f);
    }
    else {
      fadeCanvasGroup(icon0, 0.1f);
      fadeCanvasGroup(border0, 0f);
      fadeCanvasGroup(text0, 0f);
    }


    if (canMorphToCircle || playerState == "Circle") {
      fadeCanvasGroup(icon1, 1f);
      fadeCanvasGroup(border1, 1f);
      fadeCanvasGroup(text1, 1f);
    }
    else {
      fadeCanvasGroup(icon1, 0.1f);
      fadeCanvasGroup(border1, 0f);
      fadeCanvasGroup(text1, 0f);
    }

    if (canMorphToTriangle || playerState == "Triangle") {
      fadeCanvasGroup(icon2, 1f);
      fadeCanvasGroup(border2, 1f);
      fadeCanvasGroup(text2, 1f);
    }
    else {
      fadeCanvasGroup(icon2, 0.1f);
      fadeCanvasGroup(border2, 0f);
      fadeCanvasGroup(text2, 0f);
    }

    if (canMorphToRectangle || playerState == "Rectangle") {
      fadeCanvasGroup(icon3, 1f);
      fadeCanvasGroup(border3, 1f);
      fadeCanvasGroup(text3, 1f);
    }
    else {
      fadeCanvasGroup(icon3, 0.1f);
      fadeCanvasGroup(border3, 0f);
      fadeCanvasGroup(text3, 0f);
    }

  }

  private void fadeCanvasGroup(CanvasGroup thisCG, float thisNewValue) {

    /*
     * runs the fading animation of a given canvas group
     */

    // run animation
    StartCoroutine(fadeCanvasGroupCoroutine(thisCG, thisNewValue));

    IEnumerator fadeCanvasGroupCoroutine(CanvasGroup CG, float newValue) {

      float duration = 0.3f;
      int steps = 20;

      // how much to fade in every step
      float fadeBy = Mathf.Abs(CG.alpha - newValue) / steps;
      if (newValue < CG.alpha) {
        fadeBy *= -1;
      }

      for (int i = 0; i < 10; i++) {

        CG.alpha += fadeBy;

        yield return new WaitForSeconds(duration / steps);

      }

      CG.alpha = newValue;

    }

  }

}
