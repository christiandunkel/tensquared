using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force16x9AspectRatio : MonoBehaviour
{
  // initialization
  void Start()
  {

    // desired aspect ratio
    float targetRatio = 16.0f / 9.0f;

    // game window's current aspect ratio
    float windowRatio = (float)Screen.width / (float)Screen.height;

    // current viewport height should be scaled by this amount
    float scaleHeight = windowRatio / targetRatio;

    // camera component to modify
    Camera camera = GetComponent<Camera>();

    // if scaled height is less than current height, add letterbox
    if (scaleHeight < 1.0f)
    {
      Rect rect = camera.rect;

      rect.width = 1.0f;
      rect.height = scaleHeight;
      rect.x = 0;
      rect.y = (1.0f - scaleHeight) / 2.0f;

      camera.rect = rect;
    }
    else // add pillarbox
    {
      float scalewidth = 1.0f / scaleHeight;

      Rect rect = camera.rect;

      rect.width = scalewidth;
      rect.height = 1.0f;
      rect.x = (1.0f - scalewidth) / 2.0f;
      rect.y = 0;

      camera.rect = rect;
    }
  }
  
}
