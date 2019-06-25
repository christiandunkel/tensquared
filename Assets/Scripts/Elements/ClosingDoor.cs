using System.Collections;
using UnityEngine;

/*
 * powers the 'closing door left' and 'closing door right' prefab
 */

public class ClosingDoor : MonoBehaviour {

  /*
   * ===============
   * === OBJECTS ===
   * ===============
   */

  private GameObject playerObject;

  [SerializeField] private GameObject doorObject = null;
  private SpriteRenderer doorObjectSR = null;
  private BoxCollider2D doorObjectCollider = null;
  [SerializeField] private GameObject doorPosMarker = null;





  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  [SerializeField] public bool isLeftDoor = true;
  private bool isMoving = false;
  private bool isOpen = false;
  private bool isClosed = true;

  // max position to which the door opens
  private Vector2 topPositionDoor = Vector2.zero;

  private Vector2 doorSize = Vector2.zero;
  private Vector3 doorPos = Vector3.zero;
  // distance from bottom to doorPosMarker 
  // -> distance animation needs to move the door
  private float distanceToMove = 0f;
  // in how many steps the animation should happen
  private int animationSteps = 15;
  private float timeBetweenEachStep = 0.015f;

  



  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  private void Awake() {

    playerObject = PlayerManager.Instance.gameObject;
    topPositionDoor = doorPosMarker.transform.position;
    doorObjectSR = doorObject.GetComponent<SpriteRenderer>();
    doorObjectCollider = doorObject.GetComponent<BoxCollider2D>();
    doorSize = doorObjectSR.size;
    doorPos = doorObject.transform.position;

    // distance by which to move the door up when its opening
    distanceToMove = (topPositionDoor.y - doorPos.y) / doorObject.transform.localScale.y;

    // warning if door position marker is placed wrongly
    if (topPositionDoor.y < doorPos.y) {
      Debug.LogWarning("ClosingDoor: The door opening marker 'TopPositionDoor' has to be placed higher than the door bottom.");
    }

  }

  private void Update() {

    if (isMoving) return;

    // difference in position on x axis
    // from player object to this instance of a door
    float xDistanceToPlayer = playerObject.transform.position.x - transform.position.x;

    // door is open, when player is on the left of it
    if (isLeftDoor) {

      // if open and player is to the right, close the door
      if (isOpen && xDistanceToPlayer < -18f) {
        closeDoor();
      }
      // if closed and player is to the left, open the door
      else if (isClosed && xDistanceToPlayer > 18f) {
        openDoor();
      }

    }
    // door is open, when player is on the right of it
    else {

      // if open and player is to the left, close the door
      if (isOpen && xDistanceToPlayer > 18f) {
        closeDoor();
      }
      // if closed and player is to the right, open the door
      else if (isClosed && xDistanceToPlayer < -18f) {
        openDoor();
      }

    }

  }

  private void openDoor() {

    // set new attributes
    isMoving = true;
    isOpen = true;
    isClosed = false;

    // precautionary reset door size/position values at start
    doorObjectSR.size = doorSize;
    doorObjectCollider.size = doorSize;
    doorObject.transform.position = doorPos;

    // start animation
    StartCoroutine(animate());

    IEnumerator animate() {

      // increase the doors height by this
      Vector2 addSize = Vector2.zero;
      addSize.y = distanceToMove / animationSteps;

      // move the door up by this
      Vector3 addPosition = addSize;
      addPosition.y *= doorObject.transform.localScale.y / 2;

      for (int i = 0; i < animationSteps; i++) {
        // grow sprite and collider of door
        doorObjectSR.size += addSize;
        doorObjectCollider.size += addSize;
        // as the door grows from its center, move it up by half
        doorObject.transform.position += addPosition;
        yield return new WaitForSeconds(timeBetweenEachStep);
      }

      isMoving = false;

    }

  }

  private void closeDoor() {

    // set new attributes
    isMoving = true;
    isOpen = false;
    isClosed = true;

    // start animation
    StartCoroutine(animate());

    IEnumerator animate() {

      // increase the doors height by this
      Vector2 subtractSize = Vector2.zero;
      subtractSize.y = distanceToMove / animationSteps;

      // move the door up by this
      Vector3 subtractPosition  = subtractSize;
      subtractPosition.y *= doorObject.transform.localScale.y / 2;

      for (int i = 0; i < animationSteps; i++) {
        // grow sprite and collider of door
        doorObjectSR.size -= subtractSize;
        doorObjectCollider.size -= subtractSize;
        // as the door grows from its center, move it up by half
        doorObject.transform.position -= subtractPosition;
        yield return new WaitForSeconds(timeBetweenEachStep);
      }

      // reset door size/position values at end
      doorObjectSR.size = doorSize;
      doorObjectCollider.size = doorSize;
      doorObject.transform.position = doorPos;

      isMoving = false;

    }
  }

}
