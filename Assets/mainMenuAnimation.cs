using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenuAnimation : MonoBehaviour {

  private float zRotation = 5.0f;

    // called once per frame
    void Update() {

      // update circle rotation on main menu screen
      zRotation += 6;

      // keep it in 360 degrees
      zRotation %= 360;

      gameObject.transform.eulerAngles = new Vector3(
        gameObject.transform.eulerAngles.x,
        gameObject.transform.eulerAngles.y,
        -zRotation
      );    
    
    }

}
