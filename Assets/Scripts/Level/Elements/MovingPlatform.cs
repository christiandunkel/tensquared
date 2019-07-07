using UnityEngine;

/*
 * powers the 'moving platform' prefab
 */

public class MovingPlatform : MonoBehaviour {

  /* 
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  // save movement distance of each tick 
  // to move the player standing on top along
  public Vector3 movePlayerBy = Vector3.zero;

  // the platform moves from leftPos to rightPos, 
  // starting at startPos, with nextPos being the next pos to move to
  [SerializeField] private Transform leftPos = null;
  [SerializeField] private Transform rightPos = null;
  [SerializeField] private Transform startPos = null;
  private Vector3 nextPos;

  // last position of platform on y axis
  private float lastYPos;

  // how fast the platform moves
  public float speed;





  /* 
   * ================
   * === INTERNAL ===
   * ================
   */

  private void Start() {

    // to calculate offset
    // (pos' are placed at borders in unity to where platform should move,
    // but they actually represent the center position to where the platform will move,
    // thus there's the need to offset them)
    float offset = gameObject.GetComponent<BoxCollider2D>().bounds.size.x * 0.5f;

    Vector3 lp = leftPos.localPosition;
    leftPos.localPosition = new Vector3(lp.x + offset, lp.y, lp.z);

    Vector3 rp = rightPos.localPosition;
    rightPos.localPosition = new Vector3(rp.x - offset, rp.y, rp.z);

    gameObject.transform.localPosition = startPos.localPosition;
    nextPos = startPos.localPosition == leftPos.localPosition ? rightPos.localPosition : leftPos.localPosition;

  }

  // Update is called once per frame
  private void Update()  {

    // if position to which to move to was reached,
    // switch to a new position to move to
    if (gameObject.transform.localPosition == leftPos.localPosition) {
      nextPos =  rightPos.localPosition;
    }
    else if (gameObject.transform.localPosition == rightPos.localPosition) {
      nextPos = leftPos.localPosition;
    }

    // calculate the next position relative to time and speed
    Vector3 tempPos = Vector3.MoveTowards(gameObject.transform.localPosition, nextPos, speed * Time.deltaTime);

    // calculate the position difference between old and new position
    movePlayerBy.x = Mathf.Abs(gameObject.transform.localPosition.x - tempPos.x);
    movePlayerBy.y = Mathf.Abs(gameObject.transform.localPosition.y - tempPos.y);

    // add x and y offset since Unity position calculations seem to be a bit wonky
    float offset = 0.03f;

    // if moving leftwards, the moveBy value on x axis has to be negative
    if (nextPos == leftPos.localPosition) {
      movePlayerBy.x *= -1;
      movePlayerBy.x -= offset;
    }
    else {
      movePlayerBy.x += offset;
    }
    // if moving downwards, the moving value on y axis needs to be negated
    if (tempPos.y < lastYPos) movePlayerBy.x *= -1;

    // set platform to new position
    gameObject.transform.localPosition = tempPos;

    lastYPos = tempPos.y;

  }

}
