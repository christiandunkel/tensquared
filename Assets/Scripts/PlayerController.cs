using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{

  [System.Serializable]
  public struct Attributes {

    // attributes
    [SerializeField] public string name;
    [SerializeField] public float gravityModifier;
    [SerializeField] public float maxSpeed;
    [SerializeField] public float jumpTakeOffSpeed;
    [SerializeField] public Sprite sprite;
    [SerializeField] public Color particleColor;

  }

  // saves the attributes of each character state
  public Attributes[] characterAttributes = null;



  public float maxSpeed = 14f;
  public float jumpTakeOffSpeed = 14f;

  public AudioSource soundPlayer = null;
  public AudioClip morphSound = null;

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



  private float secondsNotGrounded = 0.0f; // timer for seconds the player hadn't been grounded
  private bool groundedInLastFrame = true;
  protected override void ComputeVelocity()
  {

    LevelSettings settings = LevelSettings.Instance;

    Vector2 move = Vector2.zero;

    // test if player is currently moving
    testForMovement();

    if (!deathAnimationPlaying) {

      // handle movement of character on x and y axis
      if (settings.canMove)
      {

        move.x = Input.GetAxis("Horizontal");

        if (settings.canJump) {

          // jumping
          if (Input.GetButtonDown("Jump") && grounded)
          {
            textureContainer.GetComponent<Animator>().Play("JumpSquish", 0);
            velocity.y = jumpTakeOffSpeed;
          }
          else if (Input.GetButtonDown("Jump") && state == "Triangle" && velocity.y > 0f)
          {
            velocity.y = jumpTakeOffSpeed * 1.2f;
          }

          // landing
          if (!groundedInLastFrame && grounded && secondsNotGrounded > 0.3f)
          {

            textureContainer.GetComponent<Animator>().Play("LandSquish", 0);

            // shake on landing with rectangle
            if (state == "Rectangle")
            {
              CameraShake.Instance.Play(.1f, 18f, 18f);
            }

          }
          groundedInLastFrame = grounded ? true : false;

          // check time sind player was last grounded
          secondsNotGrounded = !grounded ? secondsNotGrounded + Time.deltaTime : 0.0f;

        }

        targetVelocity = move * maxSpeed;

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





      // handle morphing from circle, rectangle, triangle into each other
      if (settings.canMorph) {
        handleMorphing();
      }



      // play death animation and respawn
      if (settings.isDead)
      {
        StartCoroutine(respawn());
      }

    }

  }



  private bool deathAnimationPlaying = false;
  IEnumerator respawn()
  {
    
    deathAnimationPlaying = true;

    LevelSettings settings = LevelSettings.Instance;

    gravityModifier = 0.0f;
    velocity.y = 0.0f;
    settings.canMove = false;
    settings.canMorph = false;
    settings.canJump = false;

    CameraShake.Instance.Play(.2f, 10f, 7f);

    textureObject.GetComponent<SpriteRenderer>().sprite = null;

    playDeathParticles();

    yield return new WaitForSeconds(1.5f);

    // teleport to spawn point
    gameObject.transform.localPosition = settings.playerSpawn;

    SpriteRenderer sr = textureObject.GetComponent<SpriteRenderer>();

    // reset gravity modifier and set sprite visible again
    foreach (Attributes a in characterAttributes)
    {
      if (a.name == state)
      {
        gravityModifier = a.gravityModifier;
        sr.sprite = a.sprite;
        break;
      }
    }

    settings.canMove = true;
    settings.canMorph = true;
    settings.canJump = true;
    settings.isDead = false;

    deathAnimationPlaying = false;

    StopCoroutine(respawn());

  }




  /*
   * changes state of player to other form
   */
  public void handleMorphing()
  {

    if (Input.GetKeyDown("" + 1) && !changingState && state != "Circle")
    {
      newState = "Circle";
      GetComponent<CircleCollider2D>().enabled = true;
      GetComponent<PolygonCollider2D>().enabled = false;
      GetComponent<BoxCollider2D>().enabled = false;
      //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
      //GetComponent<Rigidbody2D>().freezeRotation = true;
      //GetComponent<Rigidbody2D>().rotation = 0f;
      //GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
      ChangeState();
    }

    if (Input.GetKeyDown("" + 2) && !changingState && state != "Triangle")
    {
      newState = "Triangle";
      GetComponent<CircleCollider2D>().enabled = false;
      GetComponent<PolygonCollider2D>().enabled = true;
      GetComponent<BoxCollider2D>().enabled = false;
      //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
      //GetComponent<Rigidbody2D>().freezeRotation = true;
      //GetComponent<Rigidbody2D>().rotation = 0f;
      //GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
      ChangeState();
    }

    if (Input.GetKeyDown("" + 3) && !changingState && state != "Rectangle")
    {
      newState = "Rectangle";
      GetComponent<CircleCollider2D>().enabled = false;
      GetComponent<PolygonCollider2D>().enabled = false;
      GetComponent<BoxCollider2D>().enabled = true;
      //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
      //GetComponent<Rigidbody2D>().freezeRotation = true;
      //GetComponent<Rigidbody2D>().rotation = 0f;
      //GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
      ChangeState();
    }

  }
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

    // from circle to triangle / rectangle
    if (state == "Circle")
    {
      animationArray = new Sprite[newState == "Triangle" ? triangleToCircle.Length : rectToCircle.Length];
      animationArray = assignSpriteArray(animationArray, 
                                         newState == "Triangle" ? triangleToCircle : rectToCircle);
      System.Array.Reverse(animationArray);
    }
    // from triangle to circle / rectangle
    else if (state == "Triangle") 
    {
      animationArray = new Sprite[newState == "Circle" ? triangleToCircle.Length : rectToTriangle.Length];
      animationArray = assignSpriteArray(animationArray, 
                                         newState == "Circle" ? triangleToCircle : rectToTriangle);
      if (newState == "Rectangle") {
        System.Array.Reverse(animationArray);
      }
    }
    // from rectangle to circle / triangle
    else if (state == "Rectangle") 
    {
      animationArray = new Sprite[newState == "Circle" ? rectToCircle.Length : rectToTriangle.Length];
      animationArray = assignSpriteArray(animationArray, 
                                          newState == "Circle" ? rectToCircle : rectToTriangle);
    }

    // play sound
    soundPlayer.PlayOneShot(morphSound);

    // set proper lights
    circleLight.gameObject.SetActive(newState == "Circle" ? true : false);
    triangleLight.gameObject.SetActive(newState == "Triangle" ? true : false);
    rectangleLight.gameObject.SetActive(newState == "Rectangle" ? true : false);

    // set movement variables of the character type
    foreach (Attributes a in characterAttributes)
    {
      if (a.name == newState)
      {
        gravityModifier = a.gravityModifier;
        maxSpeed = a.maxSpeed;
        jumpTakeOffSpeed = a.jumpTakeOffSpeed;
        break;
      }
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
  public GameObject deathParticles = null;
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
  protected void playDeathParticles()
  {

    ParticleSystem.MainModule mainModule = deathParticles.GetComponent<ParticleSystem>().main;
    foreach (Attributes m in characterAttributes) {

      if (m.name == state) {
        mainModule.startColor = m.particleColor;
        break;
      }
      
    }

    deathParticles.SetActive(true);
    deathParticles.GetComponent<ParticleSystem>().Play();
  }

}
