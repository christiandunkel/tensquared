using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{
  
  public float maxSpeed = 7;
  public float jumpTakeOffSpeed = 7;

  public GameObject textureContainer = null;
  public GameObject textureObject = null;

  // light objects for each of the different textures
  public Light circleLight = null;
  public Light triangleLight = null;
  public Light rectangleLight = null;
  // intensity is set to values given in inspector; used while morphing
  private float circleLightIntensity = 0.0f;
  private float triangleLightIntensity = 0.0f;
  private float rectangleLightIntensity = 0.0f;

  public GhostingEffect ghost;

  private Animator animator;

  // initialization
  void Awake()
  {
    animator = GetComponent<Animator>();

    lastX = transform.position.x;
    lastY = transform.position.y;

    // take light intensity values from Unity inspector
    circleLightIntensity = circleLight.intensity;
    triangleLightIntensity = triangleLight.intensity;
    rectangleLightIntensity = rectangleLight.intensity;

    // the inner texture objects sprite renderer
    SpriteRenderer spriteRenderer = textureObject.GetComponent<SpriteRenderer>();

    // scan given directory and load images as sprites into memory
    rectToCircle = Resources.LoadAll<Sprite>("Morph/Rectangle_to_Circle");
    rectToTriangle = Resources.LoadAll<Sprite>("Morph/Rectangle_to_Triangle");
    triangleToCircle = Resources.LoadAll<Sprite>("Morph/Triangle_to_Circle");

  }




  public void handleJumping()
  {

    // jumping
    if (Input.GetButtonDown("Jump") && grounded) { 
      textureContainer.GetComponent<Animator>().Play("JumpSquish", 0);
      velocity.y = jumpTakeOffSpeed;
    }

    // landing
    if (!groundedInLastFrame && grounded && secondsNotGrounded > 0.3f) { 
      textureContainer.GetComponent<Animator>().Play("LandSquish", 0);
    }
    groundedInLastFrame = grounded ? true : false;

    // check time sind player was last grounded
    secondsNotGrounded = !grounded ? secondsNotGrounded + Time.deltaTime : 0.0f;

  }


  public void handleMorphing()
  {

    if (Input.GetKeyDown("" + 1) && !changingState && state != "Circle") {
      newState = "Circle";
      ChangeState();
    }

    if (Input.GetKeyDown("" + 2) && !changingState && state != "Triangle") {
      newState = "Triangle";
      ChangeState();
    }

    if (Input.GetKeyDown("" + 3) && !changingState && state != "Rectangle") {
      newState = "Rectangle";
      ChangeState();
    }

  }



  private float secondsNotGrounded = 0.0f; // timer for seconds the player hadn't been grounded
  private bool groundedInLastFrame = true;
  protected override void ComputeVelocity()
  {

    LevelSettings settings = LevelSettings.Instance;

    Vector2 move = Vector2.zero;

    // test if player is currently moving
    testForMovement();

    // handle movement of character on x and y axis
    if (settings.canMove)
    {

      move.x = Input.GetAxis("Horizontal");

      if (settings.canJump) {
        handleJumping();
      }

    }

    // if moving, rotate circle in right direction
    if (state == "Circle" && movingX)
    {
      rotateCircle();
    }

    // ghosting effect while moving
    ghost.makeGhost = movingX || movingY ? true : false;

    // ground particles when moving over ground on the x axis
    showMovementParticles(movingX && grounded ? true : false);

    // called when changing state, to animate new texture
    if (changingState)
    {
      animateState();
    }
    
    //animator.SetBool("grounded", grounded);
    //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

    targetVelocity = move * maxSpeed;





    // handle morphing from circle, rectangle, triangle into each other
    if (settings.canMorph) {
      handleMorphing();
    }
    

    
    if (settings.isDead)
    {
      respawn();
    }





  }




  private void respawn()
  {

    LevelSettings settings = LevelSettings.Instance;

    gameObject.transform.localPosition = settings.playerSpawn;

    settings.isDead = false;

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

    // set proper lights
    if (newState == "Circle")
    {
      circleLight.gameObject.SetActive(true);
      rectangleLight.gameObject.SetActive(false);
      triangleLight.gameObject.SetActive(false);
    }
    else if (newState == "Rectangle")
    {
      circleLight.gameObject.SetActive(false);
      rectangleLight.gameObject.SetActive(true);
      triangleLight.gameObject.SetActive(false);
    }
    else
    {
      rectangleLight.gameObject.SetActive(false);
      circleLight.gameObject.SetActive(false);
      triangleLight.gameObject.SetActive(true);
    }

    // set movement variables of each character type
    if (newState == "Circle") {
      gravityModifier = 4f;
      maxSpeed = 18f;
      jumpTakeOffSpeed = 16f;
    }
    else if (newState == "Rectangle") {
      gravityModifier = 15f;
      maxSpeed = 6f;
      jumpTakeOffSpeed = 26f;
    }
    else if (newState == "Triangle") {
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
        circleLight.intensity = 1.0f;
        triangleLight.intensity = 1.0f;
        rectangleLight.intensity = 1.0f;
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
        circleLight.intensity = circleLightIntensity;
        triangleLight.intensity = triangleLightIntensity;
        rectangleLight.intensity = rectangleLightIntensity;
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
    ParticleSystem.MainModule ps_main = movementParticles.GetComponent<ParticleSystem>().main;
    ParticleSystem.VelocityOverLifetimeModule velocity = ps.velocityOverLifetime;

    if (show)
    {
      velocity.x = (leftwards ? 11.0f : -11.0f);
      velocity.y = 7.0f;
      ps_main.startLifetime = 2.7f;
    }
    else
    {
      velocity.x = 0.0f;
      velocity.y = 0.0f;
      ps_main.startLifetime = 0.0f;
    }
  }

}
