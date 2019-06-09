using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroupFader : MonoBehaviour {

  public static CanvasGroupFader Instance;

  void Awake() {
    Instance = this;
  }

  public CanvasGroup[] uiElements;
  public float fadeinTime = 0.3f,
               fadeoutTime = 0.1f;

  public void FadeIn(GameObject obj)
  {
    CanvasGroup uiElement = obj.GetComponent<CanvasGroup>();
    StartCoroutine(FadeCanvasGroup(uiElement, 0, 1, fadeinTime, false, true, true));
  }

  public void FadeIn(string name)
  {
    CanvasGroup uiElement = GameObject.Find(name).GetComponent<CanvasGroup>();
    StartCoroutine(FadeCanvasGroup(uiElement, 0, 1, fadeinTime, false, true, true));
  }

  public void FadeIn(int id)
  {
    CanvasGroup uiElement = uiElements[id];
    StartCoroutine(FadeCanvasGroup(uiElement, 0, 1, fadeinTime, false, true, true));
  }

  public void FadeOut(GameObject obj)
  {
    CanvasGroup uiElement = obj.GetComponent<CanvasGroup>();
    StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0, fadeoutTime, false, false, false));
  }

  public void FadeOut(string name)
  {
    CanvasGroup uiElement = GameObject.Find(name).GetComponent<CanvasGroup>();
    StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0, fadeoutTime, false, false, false));
  }

  public void FadeOut(int id)
  {
    CanvasGroup uiElement = uiElements[id];
    StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0, fadeoutTime, false, false, false));
  }

  // @ interactableDuring = child elements can be interacted with at start and during animation
  // @ interactableEnd = child elements can be interacted after the animation
  private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime, bool interactableDuring, bool interactableEnd, bool isFadeInAnimation) {

    float timeStartedLerping = Time.time,
          timeSinceStarted = Time.time - timeStartedLerping;

    // child elements interactable or not
    cg.interactable = interactableDuring;

    // start values of canvasgroup scaling
    float scaleX = cg.transform.localScale.x + 0.3f,
          scaleY = cg.transform.localScale.y + 0.3f,
          scaleZ = cg.transform.localScale.z;

    if (isFadeInAnimation) {
      cg.gameObject.SetActive(true);
      cg.transform.localScale += new Vector3(scaleX, scaleY, scaleZ);
    }

    while (true) {

      timeSinceStarted = Time.time - timeStartedLerping;
      float percentageCompleted = timeSinceStarted / lerpTime;

      // opacity
      float currentValue = Mathf.Lerp(start, end, percentageCompleted);
      cg.alpha = currentValue;

      if (isFadeInAnimation) { 
        // scale in
        float transformVal = currentValue / 10 * 3;

        cg.transform.localScale = new Vector3(
          scaleX - transformVal,
          scaleY - transformVal,
          scaleZ
        );
      }

      if (percentageCompleted >= 1) break;

      // runs at speed of update function
      yield return new WaitForEndOfFrame();

    }

    // child elements interactable afterwards
    cg.interactable = interactableEnd;

    // if it faded out, set as inactive
    if (!isFadeInAnimation) {
      cg.gameObject.SetActive(false);
    }

  }

}
