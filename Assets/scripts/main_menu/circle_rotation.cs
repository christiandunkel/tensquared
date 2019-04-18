using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class circle_rotation : MonoBehaviour {

  // properties for circle rotation
  public float rotationSpeed = 3.0f;
  private float zRotation = 0.0f;
  private Vector3 rotationVec = new Vector3(0.0f, 0.0f, 0.0f);

  // called once per frame
  void Update() {

    // update circle rotation on main menu screen
    // and keep it in 360 degrees
    zRotation = Time.deltaTime * rotationSpeed;
    zRotation %= 360;

    rotationVec.z = zRotation;

    transform.Rotate(rotationVec);

  }
}
