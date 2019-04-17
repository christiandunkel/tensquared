using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circle_animation : MonoBehaviour {

  private float zRotation = 5.0f;

  // timer for animated circle to move into view
  private int moveIn = 0;

  // initialization
  void Start() {

  }

  // called once per frame
  void Update() {

    // update circle rotation on main menu screen
    // and keep it in 360 degrees
    zRotation += 6;
    zRotation %= 360;
  /*
    gameObject.transform.eulerAngles = new Vector3(

      gameObject.transform.eulerAngles.x,
      gameObject.transform.eulerAngles.y,
      -zRotation

    );*/

    if (moveIn < 200) {
      float reduceSpeed = (float) (moveIn * 2.1) / 100;
      transform.Translate(new Vector3(4.3f - reduceSpeed, 0.0f, 0.0f));
      moveIn++;
    }

  }

}
