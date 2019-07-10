using System.IO;
using UnityEngine;

/*
 * provides a variety of utility methods
 */

public static class Util {

  /*
   * ========================
   * === DISTANCE ON AXIS ===
   * ========================
   */




    /* X Axis */

  public static float distanceOnAxisX(GameObject obj1, GameObject obj2, bool useLocalPosition = false) {

    /*
     * calculate the distance between two objects on the x axis
     */

    if (useLocalPosition) {
      return distance(obj1.transform.localPosition.x, obj2.transform.localPosition.x);
    }
    else {
      return distance(obj1.transform.position.x, obj2.transform.position.x);
    }

  }

  public static float distanceOnAxisX(Vector3 vec, GameObject obj, bool useLocalPosition = false) {

    /*
     * calculate the distance between a vector and an object on the x axis
     */

    if (useLocalPosition) {
      return distance(vec.x, obj.transform.localPosition.x);
    }
    else {
      return distance(vec.x, obj.transform.position.x);
    }

  }

  public static float distanceOnAxisX(GameObject obj, Vector3 vec, bool useLocalPosition = false) {

    /*
     * calculate the distance between a vector and an object on the x axis
     */

    return distanceOnAxisX(vec, obj, useLocalPosition);

  }




  /* Y Axis */

  public static float distanceOnAxisY(GameObject obj1, GameObject obj2, bool useLocalPosition = false) {

    /*
     * calculate the distance between two objects on the y axis
     */

    if (useLocalPosition) {
      return distance(obj1.transform.localPosition.y, obj2.transform.localPosition.y);
    }
    else {
      return distance(obj1.transform.position.y, obj2.transform.position.y);
    }

  }

  public static float distanceOnAxisY(Vector3 vec, GameObject obj, bool useLocalPosition = false) {

    /*
     * calculate the distance between a vector and an object on the y axis
     */

    if (useLocalPosition) {
      return distance(vec.y, obj.transform.localPosition.y);
    }
    else {
      return distance(vec.y, obj.transform.position.y);
    }

  }

  public static float distanceOnAxisY(GameObject obj, Vector3 vec, bool useLocalPosition = false) {

    /*
     * calculate the distance between a vector and an object on the y axis
     */

    return distanceOnAxisY(vec, obj, useLocalPosition);

  }




  /* Z Axis */

  public static float distanceOnAxisZ(GameObject obj1, GameObject obj2, bool useLocalPosition = false) {

    /*
     * calculate the distance between two objects on the z axis
     */

    if (useLocalPosition) {
      return distance(obj1.transform.localPosition.z, obj2.transform.localPosition.z);
    }
    else {
      return distance(obj1.transform.position.z, obj2.transform.position.z);
    }

  }

  public static float distanceOnAxisZ(Vector3 vec, GameObject obj, bool useLocalPosition = false) {

    /*
     * calculate the distance between a vector and an object on the z axis
     */

    if (useLocalPosition) {
      return distance(vec.z, obj.transform.localPosition.z);
    }
    else {
      return distance(vec.z, obj.transform.position.z);
    }

  }

  public static float distanceOnAxisZ(GameObject obj, Vector3 vec, bool useLocalPosition = false) {

    /*
     * calculate the distance between a vector and an object on the z axis
     */

    return distanceOnAxisZ(vec, obj, useLocalPosition);

  }

  public static float distanceOnAxisXY(GameObject obj1, GameObject obj2, bool useLocalPosition = false) {

    /*
     * calculate the distance between two objects on the x and y axis
     */

    if (useLocalPosition) {
      return distance((Vector2)obj1.transform.localPosition, (Vector2)obj2.transform.localPosition);
    }
    else {
      return distance((Vector2)obj1.transform.position, (Vector2)obj2.transform.position);
    }

  }





  /*
   * ======================
   * === DISTANCE TOTAL ===
   * ======================
   */

  public static float distance(GameObject obj1, GameObject obj2, bool useLocalPosition = false) {

    /*
     * calculate the distance between two objects
     */

    if (useLocalPosition) {
      return (obj1.transform.localPosition - obj2.transform.localPosition).magnitude;
    }
    else {
      return (obj1.transform.position - obj2.transform.position).magnitude;
    }

  }

  public static float distance(Transform transform1, Transform transform2, bool useLocalPosition = false) {

    /*
     * calculate the distance between two transforms
     */

    if (useLocalPosition) {
      return (transform1.localPosition - transform2.localPosition).magnitude;
    }
    else {
      return (transform1.position - transform2.position).magnitude;
    }

  }

  public static float distance(Vector3 vec1, Vector3 vec2) {

    /*
     * calculate the distance between two vector3's
     */

    return (vec1 - vec2).magnitude;

  }

  public static float distance(Vector2 vec1, Vector2 vec2) {

    /*
     * calculate the distance between two vector2's
     */

    return (vec1 - vec2).magnitude;

  }

  public static float distance(float num1, float num2) {

    /*
     * calculates the difference between two floats
     */

    return Mathf.Abs(num1 - num2);

  }

  public static float distance(int num1, float num2) {

    /*
     * calculates the difference between a float and an integer
     */

    return Mathf.Abs(num1 - num2);

  }

  public static float distance(float num1, int num2) {

    /*
     * calculates the difference between a float and an integer
     */

    return Mathf.Abs(num1 - num2);

  }

  public static float distance(int num1, int num2) {

    /*
     * calculates the difference between two integers
     */

    return Mathf.Abs(num1 - num2);

  }

}
