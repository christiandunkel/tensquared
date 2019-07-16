using UnityEngine;

/*
 * attached to an game object, gives it a fade effect on loading the scene
 */

[RequireComponent(typeof(CanvasGroup))]
public class FadeOnStart : MonoBehaviour {

  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  // globally disabling delay of all objects attached to script
  private static bool useDelay = true;

  // settings for fade effect
  [SerializeField] private int fps = 30;
  [SerializeField] private float delay = 0.0f;
  [SerializeField] private float duration = 1.0f;
  [SerializeField] private bool fadeOut = false; // if set to false, it's a fade-in effect
  
  // amount of steps
  private int stepCurr = 0, stepsTotal;

  private bool fadeIsRunning = true;
  private float timer = 0.0f;





  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  private void Start() {

    gameObject.GetComponent<CanvasGroup>().alpha = !fadeOut ? 0.0f : 1.0f;
    gameObject.GetComponent<CanvasGroup>().interactable = delay <= 0.5f || !fadeOut ? false : true;

    stepsTotal = fps * (int) duration;

    if (!useDelay) {
      delay = 0f;
    }

  }

  private void Update() {

    // power the 'fade-in' effect via a loop
    if (fadeIsRunning) {
      fadeElement();
    }
    
  }

  private void fadeElement() {

    /*
     * power fading effect over time
     */

    timer += Time.deltaTime;

    if (timer > delay) {

      fadeIsRunning = false;

      stepCurr++;

      if (stepCurr > stepsTotal) {
        return;
      }

      if (
        (fadeOut && gameObject.GetComponent<CanvasGroup>().alpha != 0f) ||
        (!fadeOut && gameObject.GetComponent<CanvasGroup>().alpha != 1f)
      ) {

        fadeIsRunning = true;

        float transparency = (1.0f / stepsTotal) * stepCurr;

        // fix transparency if larger than 1 or smaller than 0
        if (fadeOut && transparency < 0f) transparency = 0f;
        else if (!fadeOut && transparency > 1f) transparency = 1f;

        gameObject.GetComponent<CanvasGroup>().alpha = transparency;

      }

      // on fadein, make element interactable again at 80% visibility
      if (!fadeOut && gameObject.GetComponent<CanvasGroup>().alpha >= 0.8f) {
        gameObject.GetComponent<CanvasGroup>().interactable = true;
      }

    }

  }





  /*
   * ================
   * === EXTERNAL ===
   * ================
   */

  public static void disableDelay() {

    /*
     * disables the set delay for the fade effect
     */

    useDelay = false;

  }

}
