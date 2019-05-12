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

  public GameObject textureContainer = null;
  public GameObject textureObject = null;
  public GhostingEffect ghost;

  private Animator animator;

  // initialization
  void Awake()
  {
    animator = GetComponent<Animator>();

    lastX = transform.position.x;
    lastY = transform.position.y;

    // the inner texture objects sprite renderer
    SpriteRenderer spriteRenderer = textureObject.GetComponent<SpriteRenderer>();

    // scan given directory and load images as sprites into memory
    rectToCircle = Resources.LoadAll<Sprite>("Morph/Rectangle_to_Circle");
    rectToTriangle = Resources.LoadAll<Sprite>("Morph/Rectangle_to_Triangle");
    triangleToCircle = Resources.LoadAll<Sprite>("Morph/Triangle_to_Circle");

  }




  private bool groundedInLastFrame = true;
  protected override void ComputeVelocity()
  {

    Vector2 move = Vector2.zero;

    move.x = Input.GetAxis("Horizontal");

    // test if player is currently moving
    testForMovement();
    
    // jumping
    if (Input.GetButtonDown("Jump") && grounded)
    {
      textureContainer.GetComponent<Animator>().Play("JumpSquish", 0);
      velocity.y = jumpTakeOffSpeed;
    }

    // landing
    if (!groundedInLastFrame && grounded)
    {
      textureContainer.GetComponent<Animator>().Play("LandSquish", 0);
    }
    groundedInLastFrame = grounded ? true : false;

    if (Input.GetKeyDown("" + 1) && !changingState && state != "Circle")
    {
      newState = "Circle";
      ChangeState();
    }

    if (Input.GetKeyDown("" + 2) && !changingState && state != "Triangle")
    {
      newState = "Triangle";
      ChangeState();
    }

    if (Input.GetKeyDown("" + 3) && !changingState && state != "Rectangle")
    {
      newState = "Rectangle";
      ChangeState();
    }

    //animator.SetBool("grounded", grounded);
    //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

    targetVelocity = move * maxSpeed;


    // if moving, rotate circle
    if (state == "Circle" && movingX)
    {
      // rotate circle in the right direction
      rotateCircle();
    }

    // ghosting effect while moving
    if (movingX || movingY)
    {
      ghost.makeGhost = true;
    }
    else
    {
      ghost.makeGhost = false;
    }

    // ground particles when moving over it
    if (movingX && grounded)
    {
      showMovementParticles(true);
    }
    else
    {
      showMovementParticles(false);
    }

    // called when changing state, to animate new texture
    if (changingState)
    {
      animateState();
    }

  }





  /*
   * changes state of player to other form
   */
  private bool changingState = false;
  private string state = "Circle";
  private string newState = "";
  // the morphing graphics
  private Sprite[] rectToCircle;
  private Sprite[] rectToTriangle;
  private Sprite[] triangleToCircle;
  // the final array which will be animated
  private Sprite[] animationArray;
  protected void ChangeState()
  {

    if (state == "Circle")
    {
      if (newState == "Triangle")
      {
        animationArray = new Sprite[triangleToCircle.Length];
        animationArray = assignSpriteArray(animationArray, triangleToCircle);
        System.Array.Reverse(animationArray);
      }
      else if (newState == "Rectangle")
      {
        animationArray = new Sprite[rectToCircle.Length];
        animationArray = assignSpriteArray(animationArray, rectToCircle);
        System.Array.Reverse(animationArray);
      }
    }
    else if (state == "Triangle")
    {
      if (newState == "Circle")
      {
        animationArray = new Sprite[triangleToCircle.Length];
        animationArray = assignSpriteArray(animationArray, triangleToCircle);
      }
      else if (newState == "Rectangle")
      {
        animationArray = new Sprite[rectToTriangle.Length];
        animationArray = assignSpriteArray(animationArray, rectToTriangle);
        System.Array.Reverse(animationArray);
      }
    }
    else if (state == "Rectangle")
    {
      if (newState == "Circle")
      {
        animationArray = new Sprite[rectToCircle.Length];
        animationArray = assignSpriteArray(animationArray, rectToCircle);
      }
      else if (newState == "Triangle")
      {
        animationArray = new Sprite[rectToTriangle.Length];
        animationArray = assignSpriteArray(animationArray, rectToTriangle);
      }
    }

    // set movement variables
    if (newState == "Circle")
    {
      gravityModifier = 4f;
      maxSpeed = 14f;
      jumpTakeOffSpeed = 14f;
    }
    else if (newState == "Rectangle")
    {
      gravityModifier = 15f;
      maxSpeed = 6f;
      jumpTakeOffSpeed = 15f;
    }
    else if (newState == "Triangle")
    {
      gravityModifier = 2f;
      maxSpeed = 0f;
      jumpTakeOffSpeed = 20f;
    }

    // reset frame counter for state-change animation
    frameCounter = 0;

    changingState = true;

  }


  // assigns values of array b to array a, 
  private static Sprite[] assignSpriteArray(Sprite[] a, Sprite[] b)
  {
    int counter = 0;
    foreach (Sprite sprite in b)
    {
      a[counter] = sprite;
      counter++;
    }
    return a;
  }





  private float animationDuration = 0.16f;
  private int frameCounter = 0;
  private float stateChangeTimer = 0.0f;
  private void animateState()
  {

    stateChangeTimer += Time.deltaTime;

    if (stateChangeTimer > animationDuration / animationArray.Length)
    {
      stateChangeTimer = 0.0f;

      if (frameCounter >= 1)
      {
        textureObject.transform.eulerAngles = new Vector3(
          textureObject.transform.eulerAngles.x,
          textureObject.transform.eulerAngles.y,
          0.0f
        );
      }

      textureObject.GetComponent<SpriteRenderer>().sprite = 
        animationArray[frameCounter] as Sprite;
      
      // last image -> reset
      if (frameCounter >= animationArray.Length - 1)
      {
        stateChangeTimer = 0.0f;
        frameCounter = 0;
        changingState = false;
        state = newState;
      }

      frameCounter++;

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

    textureObject.transform.Rotate(rotationVec);

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
      ps.startLifetime = 2.7f;
    }
    else
    {
      velocity.x = 0.0f;
      velocity.y = 0.0f;
      ps.startLifetime = 0.0f;
    }
  }

}
