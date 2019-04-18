using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroupFader : MonoBehaviour
{

  public CanvasGroup[] uiElements;
  public float lerpTime = 0.5f;

  public void FadeIn(int id)
  {
    CanvasGroup uiElement = uiElements[id];
    StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 1, false, true, true));
  }

  public void FadeOut(int id)
  {
    CanvasGroup uiElement = uiElements[id];
    StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0, false, false, false));
  }

  // @ interactableDuring = child elements can be interacted with at start and during animation
  // @ interactableEnd = child elements can be interacted after the animation
  public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, bool interactableDuring, bool interactableEnd, bool isFadeInAnimation)
  {

    float timeStartedLerping = Time.time,
          timeSinceStarted = Time.time - timeStartedLerping,
          percentageCompleted = timeSinceStarted / lerpTime;
    

    // child elements interactable or not
    cg.interactable = interactableDuring;

    if (isFadeInAnimation)
    {
      cg.gameObject.SetActive(true);
    }

    while (true)
    {
      timeSinceStarted = Time.time - timeStartedLerping;
      percentageCompleted = timeSinceStarted / lerpTime;

      float currentValue = Mathf.Lerp(start, end, percentageCompleted);
 
      cg.alpha = currentValue;

      if (percentageCompleted >= 1)
      {
        break;
      }

      // runs at speed of update function
      yield return new WaitForEndOfFrame();
    }

    // child elements interactable afterwards
    cg.interactable = interactableEnd;

    // if it faded out, set as inactive
    if (!isFadeInAnimation) { 
      cg.gameObject.SetActive(false);
    }

    //Debug.Log("Fading done.");

  }

}
