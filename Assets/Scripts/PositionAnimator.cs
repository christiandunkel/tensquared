using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionAnimator : MonoBehaviour
{

  public float globalDelay = 0.0f;
  public int framesPerSecond = 20;
  public Element[] elements = null;
  private List<AnimateObject> animateObjects = null;
  
  void Start()
  {

    // modify all elements
    foreach (Element elem in elements)
    {

      AnimateObject obj = new AnimateObject();

      // assign game object
      obj.obj = elem.obj;

      float startX, startY, startZ,
            endX, endY, endZ;

      // if to animate from some position to current position
      if (elem.toCurrentPos == true)
      {

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

      }
      // other way around, animate from current position to new one
      else
      {

        // get starting position
        startX = elem.obj.transform.localPosition.x;
        startY = elem.obj.transform.localPosition.y;
        startZ = elem.obj.transform.localPosition.z;

        // get ending position
        endX = elem.obj.transform.localPosition.x + elem.moveBy.x;
        endY = elem.obj.transform.localPosition.y + elem.moveBy.y;
        endZ = elem.obj.transform.localPosition.z + elem.moveBy.z;

      }

      // assign start and end position
      obj.startPos = new Vector3(startX, startY, startZ);
      obj.endPos = new Vector3(endX, endY, endZ);

      // calculate steps
      int steps = (int) (elem.duration * framesPerSecond);
      obj.steps = steps;

      // assign translation vector (calculate translation per step)
      obj.translate = new Vector3(
        startX > endX ? (startX - endX) / steps : (endX - startX) / steps,
        startY > endY ? (startY - endY) / steps : (endY - startY) / steps,
        startZ > endZ ? (startZ - endZ) / steps : (endZ - startZ) / steps
      );

      // asign duration
      obj.duration = elem.duration;

      // add global delay to every object
      obj.delay += globalDelay;

      /*Debug.Log(

        elem.obj.name + " will move" + 
        " from (x=" + startX + " y=" + startY + " z=" + startZ + ")" +
        " to (x=" +   endX   + " y=" + endY   + " z=" + endZ + ")" +
        " for the duration of " + obj.duration + "s" + 
        " after a delay of " + obj.delay + "s" +
        " over " + obj.steps + " steps."

      );*/

      animateObjects.Add(obj);

    }

  }

  float timer = 0.0f;
  bool run = true;

  void Update()
  {

    // only run if there's at least one object with more than 0 steps left
    if (run)
    {
      Animate();
    }
    
  }

  void Animate()
  {

    run = false;

    timer += Time.deltaTime;

    if (animateObjects == null)
    {
      Debug.Log("No objects to animate found.");
      return;
    }

    // go through each element and animate them
    foreach (AnimateObject elem in animateObjects)
    {

      if (timer < elem.delay)
      {
        // this element is still in delay phase, try running in next tick 
        run = true;
        continue;
      }

      Debug.Log(timer + " " + elem.delay + " " + elem.steps);

      if (elem.steps <= 0)
      {

        // this element was animated, thus try running in next tick as well
        run = true;

        // move element by amount
        elem.obj.transform.Translate( elem.translate );

        // minus one step
        elem.steps -= 1;

      }

    }

  }

  // class used to assign values via Unity inspector
  [System.Serializable]
  public struct Element
  {

    // attributes
    [SerializeField] public GameObject obj;
    [SerializeField] public float delay;
    [SerializeField] public float duration;
    [SerializeField] public Vector3 moveBy;
    [SerializeField] public bool toCurrentPos; // if true, move from posX and posY to current position
    [SerializeField] public bool loop;

  }

  // class generating objects used by the script
  public class AnimateObject
  {

    public GameObject obj;
    public float delay;
    public float duration;
    public bool loop;

    public Vector3 startPos;
    public Vector3 endPos;
    public Vector3 translate;
    public int steps;

    public AnimateObject() { }

  }

}
