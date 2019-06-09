using UnityEngine;

/*
 * when attached to an game object, it will give it a fade effect on start of the scene
 * defined may be if it's fade-in, fade-out, with delay, duration, fps
 */

[RequireComponent(typeof(CanvasGroup))]
public class FadeOnStart : MonoBehaviour {

  // globally disabling delay of all objects attached to script
  public static bool disableDelay = false;

  // settings for fade effect
  public int fps = 30;
  public float delay = 0.0f,
               duration = 1.0f;
  public bool fadeOut = false; // if set to false, it's a fade-in effect
  
  // amount of steps
  private int stepCurr = 0, stepsTotal;

  void Start() {

    gameObject.GetComponent<CanvasGroup>().alpha = !fadeOut ? 0.0f : 1.0f;
    gameObject.GetComponent<CanvasGroup>().interactable = delay <= 0.5f || !fadeOut ? false : true;

    stepsTotal = fps * (int) duration;

    if (disableDelay) delay = 0.0f;

  }

  private bool fadeIsRunning = true;
  private float timer = 0.0f;

  void Update() {

    // power fade-in over loop
    if (fadeIsRunning) FadeElement();
    
  }

  // power fading effect over time
  void FadeElement() {

    timer += Time.fixedDeltaTime;

    if (timer > delay) {

      fadeIsRunning = false;

      stepCurr += 1;

      if (stepCurr > stepsTotal) return;

      if (
        (fadeOut && gameObject.GetComponent<CanvasGroup>().alpha != 0.0f) ||
        (!fadeOut && gameObject.GetComponent<CanvasGroup>().alpha != 1.0f)
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

}
