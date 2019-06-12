using UnityEngine;

/*
 * powers the 'moving platform' prefab
 */

public class MovingPlatform : MonoBehaviour {

  // save current and last positions
  // in order to move the player standing on top along
  public float thisX = 0f, lastX = 0f,
               thisY = 0f, lastY = 0f;

  public Transform leftPos, rightPos,
                   startPos;
  public float speed;

  private Vector3 nextPos;

  void Start() {

    // to calculate offset
    // (pos' are placed at borders in unity to where platform should move,
    // but they actually represent the center position to where the platform will move,
    // thus you need to offset them)
    float offset = gameObject.GetComponent<BoxCollider2D>().bounds.size.x * 0.5f;

    Vector3 lp = leftPos.localPosition;
    leftPos.localPosition = new Vector3(lp.x + offset, lp.y, lp.z);

    Vector3 rp = rightPos.localPosition;
    rightPos.localPosition = new Vector3(rp.x - offset, rp.y, rp.z);

    gameObject.transform.localPosition = startPos.localPosition;
    nextPos = startPos.localPosition == leftPos.localPosition ? rightPos.localPosition : leftPos.localPosition;

  }

  // Update is called once per frame
  void Update()  {

    if (gameObject.transform.localPosition == leftPos.localPosition) {
      nextPos =  rightPos.localPosition;
    }
    else if (gameObject.transform.localPosition == rightPos.localPosition) {
      nextPos = leftPos.localPosition;
    }

    gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, nextPos, speed * Time.deltaTime);

    lastX = thisX;
    thisX = transform.localPosition.x;
    lastY = thisY;
    thisY = transform.localPosition.y;

  }

}
