using System.Collections.Generic;
using UnityEngine;

/*
 * smoothly animates given objects to a new position in world space
 */

public class PositionAnimator : MonoBehaviour
{

  public float globalDelay = 0.0f;
  public int framesPerSecond = 20;
  public Element[] elements = null;
  private List<AnimateObject> animateObjects = new List<AnimateObject>();
  
  // immediately loads end positions all elements at their end positions
  public static bool disabledAnimation = false;

  private float timer = 0.0f;
  private bool run = true;

  private void Start() {

    if (disabledAnimation) return;

    // modify all elements
    foreach (Element elem in elements) {

      AnimateObject obj = new AnimateObject();

      // assign game object
      obj.obj = elem.obj;

      float startX, startY, startZ,
            endX, endY, endZ;

      // get starting position
      startX = elem.obj.transform.localPosition.x + elem.moveBy.x;
      startY = elem.obj.transform.localPosition.y + elem.moveBy.y;
      startZ = elem.obj.transform.localPosition.z + elem.moveBy.z;

      // get ending position
      endX = elem.obj.transform.localPosition.x;
      endY = elem.obj.transform.localPosition.y;
      endZ = elem.obj.transform.localPosition.z;

      // set new position of object
      elem.obj.transform.localPosition = new Vector3(startX, startY, startZ);

      // assign start and end position
      obj.startPos = new Vector3(startX, startY, startZ);
      obj.endPos = new Vector3(endX, endY, endZ);

      // calculate steps
      int steps = (int) (elem.duration * framesPerSecond);
      obj.steps = steps;
      obj.stepsTotal = steps;

      // assign translation vector (calculate translation per step)
      obj.translate = new Vector3(
        startX > endX ? (startX - endX) / steps : (endX - startX) / steps,
        startY > endY ? (startY - endY) / steps : (endY - startY) / steps,
        startZ > endZ ? (startZ - endZ) / steps : (endZ - startZ) / steps
      );
      /* Debug.Log("Translate vektor for " + obj.obj.name + ": " + obj.translate); //*/

      // assign duration
      obj.duration = elem.duration;

      // add global delay to every object
      obj.delay = elem.delay + globalDelay;

      // assign movement variables
      obj.leftToRight = elem.leftToRight;
      obj.downwards = elem.downwards;

      animateObjects.Add(obj);

    }

  }

  private void Update() {

    // only run if there's at least one object with more than 0 steps left
    if (run) Animate();
    
  }

  private void Animate() {

    run = false;

    timer += Time.deltaTime;

    if (animateObjects == null) {
      Debug.Log("No objects to animate found.");
      return;
    }

    // go through each element and animate them
    foreach (AnimateObject obj in animateObjects) {

      // this element is still in delay phase, try in next tick again
      if (timer < obj.delay) {
        run = true;
        continue;
      }

      if (obj.steps > 0) {

        // this element was animated, thus try running in next tick as well
        run = true;

        float translateX = obj.translate.x,
              translateY = obj.translate.y,
              translateZ = obj.translate.z;

        // smooth animations with more than 50 steps
        if (obj.stepsTotal > 70) { 

          // make first ten steps faster
          if ((obj.stepsTotal - obj.steps) < 31)  {
            translateX += (obj.translate.z * obj.steps * 9.5f / 100);
            translateY += (obj.translate.z * obj.steps * 9.5f / 100);
            translateZ += (obj.translate.z * obj.steps * 9.5f / 100);
          }

          // make last ten steps slower
          if (obj.steps < 31) {
            int temp = obj.steps + 1 - 30;
            
            translateX -= (obj.translate.z * temp * 9.5f / 100);
            translateY -= (obj.translate.z * temp * 9.5f / 100);
            translateZ -= (obj.translate.z * temp * 9.5f / 100);
          }

        }

        // move element by amount
        obj.obj.transform.localPosition += new Vector3(obj.leftToRight ? translateX : -translateX, 0.0f, 0.0f);
        obj.obj.transform.localPosition += new Vector3(0.0f, !obj.downwards ? translateY : -translateY, 0.0f);
        obj.obj.transform.localPosition += new Vector3(0.0f, 0.0f, translateZ);

        // minus one step
        obj.steps -= 1;

      }

    }

  }

  // class used to assign values via Unity inspector
  [System.Serializable]
  public struct Element {

    // attributes
    [SerializeField] public GameObject obj;
    [SerializeField] public float delay;
    [SerializeField] public float duration;
    [SerializeField] public Vector3 moveBy;
    
    // directions in which the object will move
    [SerializeField] public bool downwards;
    [SerializeField] public bool leftToRight;

  }

  // class generating objects used by the script
  public class AnimateObject {

    public GameObject obj;
    public float delay;
    public float duration;

    public Vector3 startPos;
    public Vector3 endPos;
    public Vector3 translate;
    public int steps;
    public int stepsTotal;

    public bool downwards;
    public bool leftToRight;

    public AnimateObject() {}

  }

}
