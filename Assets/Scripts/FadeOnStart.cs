using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOnStart : MonoBehaviour
{

  public int fps = 30;
  public float delay = 0.0f;
  public float duration = 1.0f;
  public bool fadeOut = false;
  // true 0 -> 1
  // false 1 -> 0
  private int stepCurr = 0;
  private int stepsTotal;

  void Start()
  {
    
    gameObject.GetComponent<CanvasGroup>().alpha = (!fadeOut ? 0.0f : 1.0f);
    gameObject.GetComponent<CanvasGroup>().interactable = ((delay <= 0.5f) || !fadeOut ? false : true);

    stepsTotal = fps * (int) duration;

  }

  bool run = true;
  float timer = 0.0f;

  void Update()
  {

    if (run)
    {
      FadeElement();
    }
    
  }

  void FadeElement()
  {

    timer += Time.deltaTime;

    if (timer > delay)
    {

      run = false;

      stepCurr += 1;

      if (stepCurr > stepsTotal)
      {
        return;
      }

      if (
        (fadeOut && gameObject.GetComponent<CanvasGroup>().alpha != 0.0f) ||
        (!fadeOut && gameObject.GetComponent<CanvasGroup>().alpha != 1.0f)
      )
      {

        run = true;

        float transparency = (1.0f / stepsTotal) * stepCurr;

        // fix transparency if larger than 1 or smaller than 0
        if (fadeOut && transparency < 0.0f || !fadeOut && transparency > 1.0f)
        {
          transparency = fadeOut ? 0.0f : 1.0f;
        }

        gameObject.GetComponent<CanvasGroup>().alpha = transparency;

      }

      // on fadein, make element interactable again at 82% visibility
      if (!fadeOut && gameObject.GetComponent<CanvasGroup>().alpha >= 0.82f)
      {
        gameObject.GetComponent<CanvasGroup>().interactable = true;
      }

    }

  }

}
