﻿using System.Collections;
using UnityEngine;

/*
 * powers most of the physics calculations for the player to move
 */

public class PhysicsObject : PlayerManager {

  private protected override void OnStart() {
    contactFilter.useTriggers = false;
    contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
    contactFilter.useLayerMask = true;
  }

  private protected override void PhysicsUpdate() {
    targetVelocity = Vector2.zero;
  }

  // called every fixed frame-rate frame (frequency of physics system)
  void FixedUpdate() {

    // if player is set to frozen, don't calculate movement
    if (PlayerController.Instance.isFrozen) return;

    velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
    velocity.x = targetVelocity.x;

    // calculate position after double jump of triangle
    if (!grounded && state == "Triangle") {

      if (inDoubleJump) {

        if (doubleJumpMovement.x == 0f && doubleJumpMovement.y == 0f) {
          setDoubleJumpMovement();
          textureObject.transform.parent.gameObject.GetComponent<Animator>().Play("JumpSquish");
          resetTriangleRotation();
        }

        // reset line renderer
        triangleLineRenderer.SetPositions(new Vector3[2] { transform.position, transform.position });

      }
      else if (velocity.y > 0f) {

        // draw line from player to mouse cursor
        triangleLineRenderer.SetPositions(new Vector3[2] { transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) });
        doubleJumpMovement = Vector2.zero;

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
        resetTriangleRotation();
      }

    }
    else {
      // reset line renderer
      triangleLineRenderer.SetPositions(new Vector3[2] { transform.position, transform.position });
      doubleJumpMovement = Vector2.zero;

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


  private bool playingResetTriangleRotationAnimation = false;
  public void resetTriangleRotation() {

    if (!playingResetTriangleRotationAnimation) {
      playingResetTriangleRotationAnimation = true;
      StartCoroutine(resetTriangleRotationCoroutine());
    }

    IEnumerator resetTriangleRotationCoroutine() {

      yield return new WaitForSeconds(.3f);

      int stepNumber = 15;
      Vector3 startRotation = textureObject.transform.localEulerAngles;

      // leftwards
      if (startRotation.z > 269f) startRotation.z -= 360f;

      float step = startRotation.z / stepNumber;

      for (int i = 0; i < stepNumber; i++) {

        Vector3 newRotation = startRotation;
        newRotation.z = step * (stepNumber - i);
        textureObject.transform.localEulerAngles = newRotation;

        yield return new WaitForFixedUpdate();
      }

      Debug.Log("PhysicsObject: Reset triangle rotation.");

      Vector3 nullRotation = textureObject.transform.localEulerAngles;
      nullRotation.z = 0f;
      textureObject.transform.localEulerAngles = nullRotation;

      playingResetTriangleRotationAnimation = false;
      StopCoroutine(resetTriangleRotationCoroutine());

    }

  }

  

  private void setDoubleJumpMovement() {

    float doubleJumpReducer = .8f,
          angle = getAngleMousePlayer();

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
  // returns angle between null vector and the vector spanning between cursor and player
  private float getAngleMousePlayer() {

    Vector2 mouseCoords = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    Vector2 v1 = Vector2.right,
            v2 = Vector2.zero;

    v2.x = transform.position.x - mouseCoords.x;
    v2.y = transform.position.y - mouseCoords.y;

    // angle between two vectors
    float angleMousePlayer = v1.x * v2.x + v1.y * v2.y;
    angleMousePlayer /= (Mathf.Sqrt(Mathf.Pow(v1.x, 2) + Mathf.Pow(v1.y, 2)) * Mathf.Sqrt(Mathf.Pow(v2.x, 2) + Mathf.Pow(v2.y, 2)));

    angleMousePlayer = Mathf.Rad2Deg * Mathf.Acos(angleMousePlayer);

    return angleMousePlayer;

  }

  // apply movement to rigid body on 2D axes
  private void xMovement(Vector2 move) {Movement(move, false); }
  private void yMovement(Vector2 move) {Movement(move, true); }
  private void Movement(Vector2 move, bool yMovement) {

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