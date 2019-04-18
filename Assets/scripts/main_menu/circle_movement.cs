using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class circle_movement : MonoBehaviour {

  // timer for animated circle to move into view
  private int moveIn = 0;
  
  // called once per frame
  void Update() {

    if (moveIn < 200) {
      float reduceSpeed = (float) (moveIn * 2.1) / 100;
      transform.Translate(new Vector3(4.3f - reduceSpeed, 0.0f, 0.0f));
      moveIn++;
    }

  }
  //*/
}
