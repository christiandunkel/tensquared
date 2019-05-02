using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleAnimation : MonoBehaviour {

  // timer for animated circle to move into view
  private float timer = 0.0f;
  private int distance = 0;

  // circle image
  public GameObject image = null;

  // properties for circle rotation
  public float rotationSpeed = -120.0f;
  public float delay = 0.0f;
  private Vector3 rotationVec = new Vector3(0.0f, 0.0f, 0.0f);

  void Update() {

    if ((timer > delay) && (distance < 200))
    {

      float reduceSpeed = (float) (distance * 2.1f) / 100;

      transform.Translate(new Vector3(4.3f - reduceSpeed, 0.0f, 0.0f));

      distance += 1;
      
    }
    else
    {
      timer += Time.deltaTime;
    }

    RotateCircle();

  }

  // rotate the child containing the circle image
  void RotateCircle()
  {

    // update circle rotation on main menu screen
    // and keep it in 360 degrees
    float zRotation = Time.deltaTime * rotationSpeed;
    zRotation %= 360;

    rotationVec.z = zRotation;

    image.transform.Rotate(rotationVec);

  }

}
