using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* @basic script from 
 * https://unity3d.com/learn/tutorials/topics/2d-game-creation/player-controller-script
 */

public class PlayerController : PhysicsObject
{

  private bool movingX = false;
  private float lastX;
  private bool movingY = false;
  private float lastY;
  public float maxSpeed = 7;
  public float jumpTakeOffSpeed = 7;

  public GameObject circleObject = null;

  /*
   * 1 = circle
   * 2 = triangle
   * 3 = rectangle
   */
  public int state = 1;
  
  private Animator animator;

  // initialization
  void Awake()
  {
    animator = GetComponent<Animator>();

    lastX = transform.position.x;
    lastY = transform.position.y;
  }

  protected override void ComputeVelocity()
  {

    Vector2 move = Vector2.zero;

    move.x = Input.GetAxis("Horizontal");

    // test if player is currently moving on x axis
    movingX = false;
    if (lastX != transform.position.x)
    {
      movingX = true;
    }
    lastX = transform.position.x;

    // test if player is currently moving on Y axis
    movingY = false;
    if (lastY != transform.position.y)
    {
      movingY = true;
    }
    lastY = transform.position.y;

    if (Input.GetButtonDown("Jump") && grounded)
    {
      velocity.y = jumpTakeOffSpeed;
    }

    animator.SetBool("grounded", grounded);
    animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

    targetVelocity = move * maxSpeed;


    // if moving, rotate circle
    if (movingX)
    {
      // rotate circle in the right direction
      rotateCircle(move.x < 0.01f);
    }

  }

  /*
   * called while moving as circle, rotates texture
   */
  private Vector3 rotationVec = new Vector3(0.0f, 0.0f, 0.0f);
  protected void rotateCircle(bool toLeft)
  {

    float zRotation = Time.deltaTime * maxSpeed * (toLeft ? 25.0f : -25f);
    zRotation %= 360;

    rotationVec.z = zRotation;

    circleObject.transform.Rotate(rotationVec);

  }

}
