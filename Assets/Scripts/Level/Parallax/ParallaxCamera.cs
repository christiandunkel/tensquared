﻿using UnityEngine;

/* 
 * attached to the camera of a level, powers parallax effect
 * the basic script is from http://answers.unity.com/answers/564891/view.html
 */

[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour {

  public delegate void ParallaxCameraDelegate(float deltaMovement);
  public ParallaxCameraDelegate onCameraTranslate;
  private float oldPosition;

  private void Start() {
    oldPosition = transform.position.x;
  }

  private void Update() {
    if (transform.position.x != oldPosition) {
      if (onCameraTranslate != null) {
        float delta = oldPosition - transform.position.x;
        onCameraTranslate(delta);
      }
      oldPosition = transform.position.x;
    }
  }

}