using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class circle_animation : MonoBehaviour {

  private float zRotation = 0.0f;
  public float rotationSpeed = 3.0f;

  // timer for animated circle to move into view
  private int moveIn = 0;

  // called once per frame
  void Update() {

    // update circle rotation on main menu screen
    // and keep it in 360 degrees
    zRotation = Time.time * rotationSpeed;

    //GetComponent<Image>().transform.Rotate(new Vector3(0.0f, 0.0f, zRotation));
    //Debug.Log(GetComponent<Image>().sprite.associatedAlphaSplitTexture);

    if (moveIn < 200) {
      float reduceSpeed = (float) (moveIn * 2.1) / 100;
      transform.Translate(new Vector3(4.3f - reduceSpeed, 0.0f, 0.0f));
      moveIn++;
    }

  }

}
