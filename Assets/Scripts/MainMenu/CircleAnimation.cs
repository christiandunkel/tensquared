using UnityEngine;

/*
 * powers animation of 'circle rolling into screen' in main menu
 */

public class CircleAnimation : MonoBehaviour {

  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  // timer for animated circle to move into view
  private float timer = 0.0f;
  private int distance = 0;

  // properties for circle rotation
  [SerializeField] private float rotationSpeed = -120f;
  [SerializeField] private float delay = 0f;
  private Vector3 rotationVec = Vector3.zero;

  // circle image
  [SerializeField] private GameObject image = null;





  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  private void Update() {

    if (timer > delay && distance < 200) {

      // additional value to slow down the circle's speed slowly
      // the further he has rolled into the screen
      float reduceSpeed = (float) (distance * 2.1f) / 100;

      transform.Translate(new Vector3(4.3f - reduceSpeed, 0.0f, 0.0f));

      distance++;
      
    }
    else {
      timer += Time.deltaTime;
    }

    rotateCircle();

  }

  private void rotateCircle() {

    /*
     * rotate the child object containing the circle image
     */

    // update circle rotation and keep it in 360 degrees
    float zRotation = (Time.deltaTime * rotationSpeed) % 360;

    rotationVec.z = zRotation;

    image.transform.Rotate(rotationVec);

  }

}
