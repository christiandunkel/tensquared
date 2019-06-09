using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* @basic script from 
 * https://answers.unity.com/questions/551808/parallax-scrolling-using-orthographic-camera.html 
 */

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour {

  public float parallaxFactor;

  public void Move(float delta) {
    Vector3 newPos = transform.localPosition;
    newPos.x -= delta * parallaxFactor;
    transform.localPosition = newPos;
  }

}
