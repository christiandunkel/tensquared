using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

  public Transform leftPos, rightPos;
  public Transform startPos;
  public float speed;

  private Vector3 nextPos;
  private 

  // Start is called before the first frame update
  void Start()
  {

    // to calculate offset
    // (pos' are placed at borders in unity to where platform should move,
    // but they actually represent the center position to where the platform will move,
    // thus you need to offset them)
    float offset = gameObject.GetComponent<BoxCollider2D>().bounds.size.x * 3;
    Debug.Log("offset " + offset);

    leftPos.localPosition = new Vector3(
      leftPos.localPosition.x + offset,
      leftPos.localPosition.y,
      leftPos.localPosition.z
    );

    rightPos.localPosition = new Vector3(
      rightPos.localPosition.x - offset,
      rightPos.localPosition.y,
      rightPos.localPosition.z
    );

    Debug.Log("left " + leftPos.localPosition);
    Debug.Log("right " + rightPos.localPosition);

    gameObject.transform.localPosition = startPos.localPosition;
    nextPos = startPos.localPosition == leftPos.localPosition ? rightPos.localPosition : leftPos.localPosition;

  }

  // Update is called once per frame
  void Update()
  {

    if (gameObject.transform.localPosition == leftPos.localPosition) {
      nextPos = rightPos.localPosition;
    }
    else if (gameObject.transform.localPosition == rightPos.localPosition) {
      nextPos = leftPos.localPosition;
    }

    gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, nextPos, speed * Time.deltaTime);
  }

}
