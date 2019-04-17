using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenuAnimation : MonoBehaviour {

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

    // move circle into view on main menu
    if (moveIn < 80) {
      // addition speed boost at beginning
      transform.Translate(new Vector3(1.5f, 0.0f, 0.0f));
    }

    if (moveIn < 150) {
      // addition speed boost for main distance
      transform.Translate(new Vector3(1.5f, 0.0f, 0.0f));
    }

    if (moveIn < 200) {
      // normal speed
      transform.Translate(new Vector3(1.0f, 0.0f, 0.0f));
      moveIn++;
    }

  }

}
