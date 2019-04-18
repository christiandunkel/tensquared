using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sidescrolling_animation : MonoBehaviour
{
  public float scrollPercentage = 0.3f;
  private float scrollBy; // calculated with width and scrollPercentage
  private float startPosX;
  private float width;
  private float counter;

  void Start()
  {
    RectTransform rt = (RectTransform)transform;
    width = rt.rect.width;

    startPosX = transform.localPosition.x;
    scrollBy = width / 100 * scrollPercentage;
    counter = 0.0f;

  }

  
  // Update is called once per frame
  void Update()
  {
    // current x position of element
    counter += 1.0f;

    // move ground in relation to parent
    transform.localPosition = new Vector3(
      transform.localPosition.x - scrollBy, 
      transform.localPosition.y, 
      transform.localPosition.z
    );

    // reset counter on 100 %
    if ((counter * scrollPercentage) >= 100.0f)
    {
      counter = 0.0f;
      // reset position of element back to beginning
      transform.localPosition = new Vector3(
        startPosX,
        transform.localPosition.y,
        transform.localPosition.z
      );
    }

  }

}
