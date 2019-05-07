using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* @basic script from 
 * https://unity3d.com/learn/tutorials/topics/2d-game-creation/player-controller-script
 */

public class PlayerController : PhysicsObject
{
  
  public float maxSpeed = 7;
  public float jumpTakeOffSpeed = 7;

  public GameObject circleObject = null;

  /*
   * "circle"
   * "triangle"
   * "rectangle"
   */
  public string state = "circle";
  
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

    // test if player is currently moving
    testForMovement();

    if (Input.GetButtonDown("Jump") && grounded)
    {
      velocity.y = jumpTakeOffSpeed;
    }

    animator.SetBool("grounded", grounded);
    animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

    targetVelocity = move * maxSpeed;


    // if moving, rotate circle
    if (state == "circle" && movingX)
    {
      // rotate circle in the right direction
      rotateCircle();
    }

    if (movingX && grounded)
    {
      showMovementParticles(true);
    }
    else
    {
      showMovementParticles(false);
    }

  }

  /*
   * tests if the player is currently moving
   * sets movingX, movingY, upwards and leftwards
   */
  private bool leftwards = false; // direction on last movement
  private bool movingX = false;
  private float lastX;
  private bool upwards = false; // direction on last movement
  private bool movingY = false;
  private float lastY;
  protected void testForMovement()
  {

    // test if player is currently moving on x axis
    movingX = false;
    float currX = transform.position.x; // current
    if (System.Math.Abs(lastX - currX) > 0.1f)
    {
      movingX = true;
      leftwards = (lastX > currX ? true : false);
    }
    lastX = transform.position.x;

    // test if player is currently moving on Y axis
    movingY = false;
    float currY = transform.position.y; // current
    if (System.Math.Abs(lastY - currY) > 0.1f)
    {
      movingY = true;
      upwards = (lastY < currY ? true : false);
    }
    lastY = transform.position.y;

  }


  /*
   * called while moving as circle, rotates texture
   */
  private Vector3 rotationVec = new Vector3(0.0f, 0.0f, 0.0f);
  protected void rotateCircle()
  {

    float zRotation = Time.deltaTime * maxSpeed * (leftwards ? 25.0f : -25f);
    zRotation %= 360;

    rotationVec.z = zRotation;

    circleObject.transform.Rotate(rotationVec);

  }

  /*
  * called every frame
  * update state of showing movement particles
  */
  public GameObject movementParticles = null;
  protected void showMovementParticles(bool show)
  {

    ParticleSystem ps = movementParticles.GetComponent<ParticleSystem>();
    ParticleSystem.VelocityOverLifetimeModule velocity = ps.velocityOverLifetime;

    if (show)
    {
      velocity.x = (leftwards ? 11.0f : -11.0f);
      velocity.y = 7.0f;
      ps.startLifetime = 5.0f;
    }
    else
    {
      velocity.x = 0.0f;
      velocity.y = 0.0f;
      ps.startLifetime = 0.0f;
    }
  }

}
