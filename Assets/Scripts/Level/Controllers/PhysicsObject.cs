using System.Collections;
using UnityEngine;

/*
 * powers most of the physics calculations for the player to move
 */

public class PhysicsObject : PlayerManager {

  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  private protected override void OnStart() {
    contactFilter.useTriggers = false;
    contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
    contactFilter.useLayerMask = true;
  }

  private protected override void PhysicsUpdate() {
    targetVelocity = Vector2.zero;
  }

  private protected override void OnFixedUpdate() {

    /*
     * manage the physics of the player;
     * method is called every fixed frame-rate frame
     * (with the frequency of the physics system)
     */

    // if player is set to frozen, don't calculate movement
    if (isFrozen || isDead) {
      return;
    }

    // reset double jump movement when player is dead or morphing
    if (isDead) {
      doubleJumpMovement = Vector2.zero;
      return;
    }


    velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
    velocity.x = targetVelocity.x;

    // calculate position after double jump of triangle
    if (!grounded && state == "Triangle") {

      if (inDoubleJump) {

        // initialize double jump if no double jump movement value is assigned yet
        if (!doubleJumpMovementIsAssigned) {

          doubleJumpMovementIsAssigned = true;

          calculateDoubleJumpMovement();
          textureObject.transform.parent.gameObject.GetComponent<Animator>().Play("JumpSquish");
          resetTriangleRotation(1);

        }

        // reset line renderer
        triangleLineRenderer.SetPositions(new Vector3[2] { transform.position, transform.position });

      }
      else if (velocity.y > 0f) {

        // draw line from player to mouse cursor
        triangleLineRenderer.SetPositions(new Vector3[2] { transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) });

        // reset double jump movement
        doubleJumpMovement = Vector2.zero;
        doubleJumpMovementIsAssigned = false;

        // calculate angle in which to rotate the triangle
        float angle = getAngleMousePlayer();

        // leftwards : rightwards
        Vector3 newRotation = textureObject.transform.localEulerAngles;
        newRotation.z = angle < 90f ? 90f - angle : -(angle - 90f);
        textureObject.transform.localEulerAngles = newRotation;

      }
      else {
        // reset line renderer
        triangleLineRenderer.SetPositions(new Vector3[2] { transform.position, transform.position });
        resetTriangleRotation(2);
      }

    }
    else {

      // reset line renderer
      triangleLineRenderer.SetPositions(new Vector3[2] { transform.position, transform.position });

      // reset double jump movement
      doubleJumpMovement = Vector2.zero;
      doubleJumpMovementIsAssigned = false;
      playingResetTriangleRotationAnimation = false;

      // reset rotation of triangle
      if (textureObject.transform.localEulerAngles.z > 357f && textureObject.transform.localEulerAngles.z > 3f) {
        Vector3 newRotation = textureObject.transform.localEulerAngles;
        newRotation.z = 0f;
        textureObject.transform.localEulerAngles = newRotation;
      }

    }

    grounded = false;

    // change in position
    Vector2 deltaPosition = velocity * Time.deltaTime;

    // apply normal player movement (left, right, jump)
    xMovement((new Vector2(groundNormal.y, -groundNormal.x)) * deltaPosition.x);
    yMovement(Vector2.up * deltaPosition.y);

  }





  /*
   * ===================
   * === DOUBLE JUMP ===
   * ===================
   */

  private bool playingResetTriangleRotationAnimation = false;
  private void resetTriangleRotation(int u) {

    /*
     * reset texture's rotation back to normal
     * (when it was changed for double jump on triangle)
     */

    if (!playingResetTriangleRotationAnimation) {
      playingResetTriangleRotationAnimation = true;
      Log.Print($"Start 'reset triangle rotation' process on {(u == 1 ? "double jump" : "landing")}.", this);
      StopCoroutine(resetTriangleRotationCoroutine());
      StartCoroutine(resetTriangleRotationCoroutine());
    }

    IEnumerator resetTriangleRotationCoroutine() {

      // wait for a short while, before resetting rotation
      yield return new WaitForSeconds(.3f);

      // get current rotation of triangle
      Vector3 startRotation = textureObject.transform.localEulerAngles;

      // if rotated leftwards
      if (startRotation.z > 269f) {
        startRotation.z -= 360f;
      }

      int stepNumber = 15;
      float step = startRotation.z / stepNumber;

      // smoothly rotate triangle back to original rotation
      for (int i = 0; i < stepNumber; i++) {

        Vector3 newRotation = startRotation;
        newRotation.z = step * (stepNumber - i);
        textureObject.transform.localEulerAngles = newRotation;

        yield return new WaitForFixedUpdate();

      }

      Log.Print("Finished resetting triangle rotation.", this);

      // if for loop goes a bit over 0f, make a final reset
      if (textureObject.transform.localEulerAngles.z != 0f) {
        Vector3 nullRotation = textureObject.transform.localEulerAngles;
        nullRotation.z = 0f;
        textureObject.transform.localEulerAngles = nullRotation;
      }
      
      StopCoroutine(resetTriangleRotationCoroutine());

    }

  }
  
  private void calculateDoubleJumpMovement() {

    /*
     * calculates the velocity for double jump
     * and saves it inside vector2 'doubleJumpMovement'
     */

    // don't give double jump velocity, if dead or morphing
    if (isDead || isChangingState) {
      doubleJumpMovement = Vector2.zero;
      return;
    }

    float doubleJumpReducer = .8f;
    float angle = getAngleMousePlayer();

    // leftwards
    if (angle < 90f) {

      doubleJumpMovement.x = -((90f - angle) / (50f * doubleJumpReducer));

      // less upwards movement the closer the cursor is to the vertical vector standing on player
      doubleJumpMovement.y = -((90f - angle) / (100f * doubleJumpReducer));

    }
    // rightwards
    else if (angle > 90f) {

      doubleJumpMovement.x = (angle - 90f) / (50f * doubleJumpReducer);

      // less upwards movement the closer the cursor is to the vertical vector standing on player
      doubleJumpMovement.y = -((angle - 90f) / (100f * doubleJumpReducer));

    }
    else {
      doubleJumpMovement.x = 0f;
    }

  }

  private float getAngleMousePlayer() {

    /*
     *  returns angle between null vector and 
     *  the vector spanning between cursor and player
     */

    Vector2 mouseCoords = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    Vector2 v1 = Vector2.right;
    Vector2 v2 = Vector2.zero;

    v2.x = transform.position.x - mouseCoords.x;
    v2.y = transform.position.y - mouseCoords.y;

    // angle between two vectors
    float angleMousePlayer = v1.x * v2.x + v1.y * v2.y;
    angleMousePlayer /= (Mathf.Sqrt(Mathf.Pow(v1.x, 2) + Mathf.Pow(v1.y, 2)) * Mathf.Sqrt(Mathf.Pow(v2.x, 2) + Mathf.Pow(v2.y, 2)));
    angleMousePlayer = Mathf.Rad2Deg * Mathf.Acos(angleMousePlayer);

    return angleMousePlayer;

  }

  private void OnCollisionEnter2D(Collision2D col) {

    // reset double jump velocity when hitting a wall / collider
    doubleJumpMovement = Vector2.zero;

  }





  /*
   * ================
   * === MOVEMENT ===
   * ================
   */

  private void xMovement(Vector2 move) {

    /*
     * apply velocity to rigid body on x axis
     */

    Movement(move, false);

  }

  private void yMovement(Vector2 move) {

    /*
     * apply velocity to rigid body on y axis
     */

    Movement(move, true);

  }

  private void Movement(Vector2 move, bool yMovement) {

    /*
     * apply velocity to rigid body on 2D axes
     */

    move.x += !yMovement ? doubleJumpMovement.x : 0f;
    move.y += yMovement ? doubleJumpMovement.y : 0f;

    float distance = move.magnitude; // betrag vektor

    if (distance > minMoveDistance) {

      int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
      hitBufferList.Clear();

      for (int i = 0; i < count; i++) hitBufferList.Add(hitBuffer[i]);

      for (int i = 0; i < hitBufferList.Count; i++) {

        Vector2 currentNormal = hitBufferList[i].normal;

        if (currentNormal.y > minGroundNormalY) {
          grounded = true;
          if (yMovement) {
            groundNormal = currentNormal;
            currentNormal.x = 0;
          }
        }

        float projection = Vector2.Dot(velocity, currentNormal);

        if (projection < 0) velocity = velocity - projection * currentNormal;

        float modifiedDistance = hitBufferList[i].distance - shellRadius;
        distance = modifiedDistance < distance ? modifiedDistance : distance;

      }

    }

    // calculate final position of rigid body
    Vector2 pos = rb2d.position + move.normalized * distance;
    rb2d.position = pos;

  }

}
