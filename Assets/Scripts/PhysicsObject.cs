﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {

  public float minGroundNormalY = .65f;

  // scale gravity with this value
  public float gravityModifier = 4f;

  protected Vector2 velocity, targetVelocity, groundNormal;
  protected bool grounded;

  protected ContactFilter2D contactFilter;
  protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
  protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);


  protected const float minMoveDistance = 0.001f,
                        shellRadius = 0.01f;

  // reference to the 2D rigid body connected to the object
  protected Rigidbody2D rb2d;

  void OnEnable() {
    rb2d = GetComponent<Rigidbody2D>();
  }

  void Start() {
    contactFilter.useTriggers = false;
    contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
    contactFilter.useLayerMask = true;
  }

  void Update() {
    targetVelocity = Vector2.zero;
    ComputeVelocity();
  }

  protected virtual void ComputeVelocity() {}

  // has frequency of physics system; called every fixed frame-rate frame
  void FixedUpdate() {

    // if player is set to frozen, don't calculate movement
    if (PlayerController.Instance.isFrozen) return;

    velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
    velocity.x = targetVelocity.x;

    grounded = false;

    // change in position
    Vector2 deltaPosition = velocity * Time.deltaTime;

    xMovement((new Vector2(groundNormal.y, -groundNormal.x)) * deltaPosition.x);

    yMovement(Vector2.up * deltaPosition.y);

  }

  private void xMovement(Vector2 move) {Movement(move, false); }
  private void yMovement(Vector2 move) {Movement(move, true); }
  private void Movement(Vector2 move, bool yMovement) {

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
    rb2d.position = rb2d.position + move.normalized * distance;

  }

}
