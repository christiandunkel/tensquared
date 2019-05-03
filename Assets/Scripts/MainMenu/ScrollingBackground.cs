using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{

  public bool useLocalTransform = true;

  public float scrollPercentage = 0.3f;
  private float scrollBy; // calculated with width and scrollPercentage
  private float startPosX;
  private float counter;

  void Start()
  {

    RectTransform rt = (RectTransform)transform;
    float width = rt.rect.width;

    if (useLocalTransform)
    {
      startPosX = transform.localPosition.x;
    }
    else
    {
      startPosX = transform.position.x;
    }
    
    scrollBy = width / 100 * scrollPercentage;
    counter = 0.0f;

    /*Debug.Log("Object " + gameObject.name + " will move from x=" + startPosX + " over " + width + " points.");//*/

  }

  
  // Update is called once per frame
  void Update()
  {
    // current x position of element
    counter += 1.0f;


    if (useLocalTransform) { 

      // move object in relation to parent
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
    else
    {

      // move object
      transform.position = new Vector3(
        transform.position.x - scrollBy,
        transform.position.y,
        transform.position.z
      );

      // reset counter on 100 %
      if ((counter * scrollPercentage) >= 100.0f)
      {
        counter = 0.0f;
        // reset position of element back to beginning
        transform.position = new Vector3(
          startPosX,
          transform.position.y,
          transform.position.z
        );
      }

    }

  }

}
