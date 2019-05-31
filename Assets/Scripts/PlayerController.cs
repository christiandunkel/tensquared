using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : PhysicsObject
{

  // singleton
  public static PlayerController Instance;



  /*
   =======================
   === CAMERA CONTROLS ===
   =======================
   */

  public Animator VirtualCameraAnimator;

  private float zoomedInCameraTimer = 0.0f;



  /*
   ========================
   === PLAYER VARIABLES ===
   ========================
   */

  private bool canMove = false,
               canMorph = false,
               canJump = false,

               isDead = false,
               steppedOnPiston = false;

  private bool isChangingState = false;
  private string state = "Circle",
                 newState = "";

  public bool setSpawnpoint = false;

  public bool isFrozen = false;
  private bool frozenInLastFrame = false;
  private float frozenYPos = 0.0f;

  private bool inDoubleJump = false; // is true, if player executed double jump and is still in air

  private float secondsNotGrounded = 0.0f; // timer for seconds the player hadn't been grounded
  private bool groundedInLastFrame = true;

  private float lastX, lastY; // last x and y position
  private bool leftwards = false, // direction on last movement
               movingX = false,
               upwards = false, // direction on last movement
               movingY = false;



  /*
   =========================
   === PLAYER ATTRIBUTES ===
   =========================
   */

  private float maxSpeed,
                jumpTakeOffSpeed;

  [System.Serializable]
  public struct Attributes {
    [SerializeField] public string name;
    [SerializeField] public float gravityModifier;
    [SerializeField] public float maxSpeed;
    [SerializeField] public float jumpTakeOffSpeed;
    [SerializeField] public Sprite sprite;
    [SerializeField] public Color particleColor;
  }

  // saves the attributes of each character state
  public Attributes[] characterAttributes = new Attributes[3];



  /*
   ======================
   === PLAYER OBJECTS ===
   ======================
   */

  private GameObject parentObject;

  public AudioSource soundPlayer;
  public AudioClip morphSound,
                   landingRectangleSound,
                   waterSplashSound,
                   walkThroughGrassSound,
                   pistonPushSound;

  public GameObject textureContainer;
  public GameObject textureObject;

  public GhostingEffect ghost;

  public GameObject movementParticles = null;
  public GameObject deathParticles = null;



  /*
   ==============
   === LIGHTS ===
   ==============
   */

  // lights for each of the different textures
  public Light circleLight, triangleLight, rectangleLight;
  // intensity is set in inspector; value is used while morphing
  private float circleLightIntensity = 0.0f,
                triangleLightIntensity = 0.0f,
                rectangleLightIntensity = 0.0f;



  /*
   ==============
   === SETTER ===
   ==============
   */

  public void SetSetting(string name, bool value) {
    switch (name) {
      case "canMove":   canMove = value; break;
      case "canJump":   canJump = value; break;
      case "canMorph":  canMorph = value; break;
      default: break;
    }
  }

  public void setValue(string name, bool val) {
    switch (name) {
      case "steppedOnPiston":   steppedOnPiston = val; break;
      default: break;
    }
  }



  /*
   ==============
   === GETTER ===
   ==============
   */

  public bool getBool(string name) {
    switch (name) {
      case "isDead":
        return isDead;
      default:
        Debug.Log("PlayerController: Boolean of the name " + name + " couldn't be found.");
        break;
    }
    return false;
  }

  private Attributes getAttributes() {
    return getAttributes(state);
  }

  private Attributes getAttributes(string stateName) {

    // look for given name
    foreach (Attributes a_temp in characterAttributes) {
      if (a_temp.name == stateName) {
        return a_temp;
      }
    }

    // if fails return attributes for current state
    Attributes a = new Attributes();
    foreach (Attributes a_temp in characterAttributes) {
      if (a_temp.name == state) {
        a = a_temp;
        break;
      }
    }
    return a;

  }



  /*
   ======================
   === INITIALIZATION ===
   ======================
   */

  void Awake() {

    parentObject = gameObject.transform.parent.gameObject;
    Instance = this;

    Debug.Log("Player: With parent object '" + parentObject.name + "' initialized.");

    // set attributes for start character state
    Attributes a = getAttributes();
    maxSpeed = a.maxSpeed;
    jumpTakeOffSpeed = a.jumpTakeOffSpeed;
    gravityModifier = a.gravityModifier;

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
  private bool loadSettings_Toggle = true;
  void loadSettings()
  {
    loadSettings_Toggle = false;

    LevelSettings settings = LevelSettings.Instance;
    canMove = settings.canMove;
    canJump = settings.canJump;
    canMorph = settings.canMorph;
  }



  /*
   ================
   === MOVEMENT ===
   ================
   */

  // stop figure rolling away even though there is no movement
  private float rollingFixTimer = 0.0f;
  private float rollingFixTimerDefault = 0.3f;
  private void stopRollingFix() {
    GetComponent<Rigidbody2D>().freezeRotation = true;
    GetComponent<Rigidbody2D>().rotation = 0f;
    GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
  }
  
  protected override void ComputeVelocity() {

    if (isFrozen) {
      if (!frozenInLastFrame) {
        ghost.SetGhosting(false);
        frozenInLastFrame = true;
        frozenYPos = gameObject.transform.position.y;
      }
      // hold object on the same y position
      Vector3 frozenPos = gameObject.transform.position;
      frozenPos.y = frozenYPos;
      gameObject.transform.position = frozenPos;
      return;
    }
    else if (frozenInLastFrame) {
      frozenInLastFrame = false;
    }

    // handle camera zooming
    if (zoomedInCameraTimer > 0.0f) {
      zoomedInCameraTimer -= Time.deltaTime;
      if (!VirtualCameraAnimator.GetBool("ZoomedIn")) {
        VirtualCameraAnimator.SetBool("ZoomedIn", true);
      }
    }
    else if (VirtualCameraAnimator.GetBool("ZoomedIn")) {
      VirtualCameraAnimator.SetBool("ZoomedIn", false);
    }

    if (loadSettings_Toggle) {
      loadSettings();
    }

    Vector2 move = Vector2.zero;

    rollingFixTimer -= Time.deltaTime;
    if (rollingFixTimer <= 0.0f) {
      rollingFixTimer = rollingFixTimerDefault;
      stopRollingFix();
    }

    if (grounded) {
      inDoubleJump = false;
    }

    // test if player is currently moving
    testForMovement();

    if (!isDead) {

      // handle movement of character on x and y axis
      if (canMove) {

        move.x = Input.GetAxis("Horizontal");
        if (move.x > 0.0f) {
          rollingFixTimer = rollingFixTimerDefault;
        }

        if (canJump) {

          // jumping
          if (Input.GetButtonDown("Jump")) {

            if (grounded) {
              textureContainer.GetComponent<Animator>().Play("JumpSquish", 0);
              velocity.y = jumpTakeOffSpeed;
            }
            // double jump for triangle
            else if (state == "Triangle" && velocity.y > 0f && !inDoubleJump) {
              inDoubleJump = true;
              velocity.y = jumpTakeOffSpeed * 1.2f;
            }

          }
          
          // landing
          if (!groundedInLastFrame && grounded && secondsNotGrounded > 0.2f) {

            textureContainer.GetComponent<Animator>().Play("LandSquish", 0);

            // shake on landing with rectangle
            if (state == "Rectangle")
            {
              CameraShake.Instance.Play(.1f, 18f, 18f);
              soundPlayer.PlayOneShot(landingRectangleSound);
            }

          }
          groundedInLastFrame = grounded ? true : false;

          // check time sind player was last grounded
          secondsNotGrounded = !grounded ? secondsNotGrounded + Time.deltaTime : 0.0f;

        }

        if (steppedOnPiston) {
          steppedOnPiston = false;
          velocity.y = jumpTakeOffSpeed * 2f;
        }

        targetVelocity = move * maxSpeed;

      }

      // if moving on x axis
      if (movingX) {

        // rotate circle in right direction
        if (state == "Circle") {
          rotateCircle();
        }

      }

      // ground particles when moving over ground on the x axis
      showMovementParticles(movingX && grounded ? true : false);
      
      // enable ghosting effect while moving
      ghost.SetGhosting(movingX || movingY ? true : false);

      // called when changing state, to animate new texture
      if (isChangingState) {
        animateState();
      }

      // handle morphing from circle, rectangle, triangle into each other
      if (canMorph) {
        handleMorphing();
      }

      updateMovementSounds();

    }

  }


  
  IEnumerator respawn() {

    if (setSpawnpoint) {
      isFrozen = true;
    }
    
    isDead = true;
    gravityModifier = 0.0f;
    velocity.y = 0.0f;

    canMove = false;
    canMorph = false;
    canJump = false;

    GetComponent<Rigidbody2D>().freezeRotation = true;
    GetComponent<Rigidbody2D>().rotation = 0f;
    GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);

    CameraShake.Instance.Play(.2f, 10f, 7f);

    textureObject.GetComponent<SpriteRenderer>().sprite = null;

    playDeathParticles();

    yield return new WaitForSeconds(1.5f);

    // teleport to spawn point
    Vector2 ps = LevelSettings.Instance.playerSpawn;
    gameObject.transform.localPosition = ps;
    frozenYPos = ps.y;

    SpriteRenderer sr = textureObject.GetComponent<SpriteRenderer>();

    // set sprite visible again
    Attributes a = getAttributes();
    sr.sprite = a.sprite;

    // handle spawn point animation
    if (setSpawnpoint) {
      yield return new WaitForSeconds(.2f);
      // come out of spawn point tube
      float spawnPointmoveCharBy = 2.65f / 50f;
      for (int i = 0; i < 50; i++)
      {
        gameObject.transform.position += new Vector3(spawnPointmoveCharBy, 0f, 0f);
        yield return new WaitForSeconds(0.03f);
      }
      isFrozen = false;
    }

    // reset gravity modifier
    gravityModifier = a.gravityModifier;

    isDead = false;

    // reset internal settings for player with level settings
    LevelSettings sett = LevelSettings.Instance;
    canMove = sett.canMove;
    canMorph = sett.canMorph;
    canJump = sett.canJump;

    // reset attributes to current state
    Attributes resA = getAttributes();
    gravityModifier = resA.gravityModifier;
    maxSpeed = resA.maxSpeed;
    jumpTakeOffSpeed = resA.jumpTakeOffSpeed;
    textureObject.GetComponent<SpriteRenderer>().sprite = resA.sprite;

    StopCoroutine(respawn());

  }




  /*
   * changes state of player to other form
   */
  public void handleMorphing()
  {

    if (Input.GetKeyDown("" + 1) && !isChangingState && state != "Circle") {
      newState = "Circle";
      GetComponent<CircleCollider2D>().enabled = true;
      GetComponent<PolygonCollider2D>().enabled = false;
      GetComponent<BoxCollider2D>().enabled = false;
      //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
      GetComponent<Rigidbody2D>().freezeRotation = true;
      GetComponent<Rigidbody2D>().rotation = 0f;
      GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
      ChangeState();
    }

    if (Input.GetKeyDown("" + 2) && !isChangingState && state != "Triangle") {
      ScriptedEventsManager.Instance.LoadEvent(1, "morph_to_triangle");

      newState = "Triangle";
      GetComponent<CircleCollider2D>().enabled = false;
      GetComponent<PolygonCollider2D>().enabled = true;
      GetComponent<BoxCollider2D>().enabled = false;
      //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
      GetComponent<Rigidbody2D>().freezeRotation = true;
      GetComponent<Rigidbody2D>().rotation = 0f;
      GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
      ChangeState();
    }

    if (Input.GetKeyDown("" + 3) && !isChangingState && state != "Rectangle") {
      newState = "Rectangle";
      GetComponent<CircleCollider2D>().enabled = false;
      GetComponent<PolygonCollider2D>().enabled = false;
      GetComponent<BoxCollider2D>().enabled = true;
      //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
      GetComponent<Rigidbody2D>().freezeRotation = true;
      GetComponent<Rigidbody2D>().rotation = 0f;
      GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
      ChangeState();
    }

  }
  // the morphing graphics
  private Sprite[] rectToCircle;
  private Sprite[] rectToTriangle;
  private Sprite[] triangleToCircle;
  // the final array which will be animated
  private static Sprite[] animationArray;
  protected void ChangeState() {

    switch (state) {
      // from circle to triangle / rectangle
      case "Circle":
        assignAnimationArray(newState == "Triangle" ? triangleToCircle : rectToCircle, true);
        break;

      // from triangle to circle / rectangle
      case "Triangle":
        assignAnimationArray(newState == "Circle" ? triangleToCircle : rectToTriangle, newState == "Rectangle");
        break;

      // from rectangle to circle / triangle
      case "Rectangle":
        assignAnimationArray(newState == "Circle" ? rectToCircle : rectToTriangle, false);
        break;

      default:
        break;

    }

    // play sound
    soundPlayer.PlayOneShot(morphSound);

    // set proper lights
    circleLight.gameObject.SetActive(newState == "Circle" ? true : false);
    triangleLight.gameObject.SetActive(newState == "Triangle" ? true : false);
    rectangleLight.gameObject.SetActive(newState == "Rectangle" ? true : false);

    // set movement variables of the character type
    Attributes a = getAttributes(newState);
    gravityModifier = a.gravityModifier;
    maxSpeed = a.maxSpeed;
    jumpTakeOffSpeed = a.jumpTakeOffSpeed;

    // reset frame counter for state-change animation
    frameCounter = 0;

    isChangingState = true;

  }


  // create and reutrn new array with values of given array
  private static void assignAnimationArray(Sprite[] a, bool reverse) {

    animationArray = new Sprite[a.Length];

    int counter = reverse ? a.Length - 1 : 0;

    foreach (Sprite sprite in a) {
      animationArray[counter] = sprite;
      counter = reverse ? counter - 1 : counter + 1;
    }

  }





  private float animationDuration = 0.16f;
  private int frameCounter = 0;
  private float stateChangeTimer = 0.0f;
  private void animateState() {

    stateChangeTimer += Time.deltaTime;

    if (stateChangeTimer > animationDuration / animationArray.Length) {
      stateChangeTimer = 0.0f;

      if (frameCounter >= 1) {

        // reset rotation
        Vector3 ea = textureObject.transform.eulerAngles;
        ea.z = 0.0f;
        textureObject.transform.eulerAngles = ea;

        circleLight.intensity = 1.0f;
        triangleLight.intensity = 1.0f;
        rectangleLight.intensity = 1.0f;
      }

      textureObject.GetComponent<SpriteRenderer>().sprite = animationArray[frameCounter] as Sprite;
      
      // last image -> reset
      if (frameCounter >= animationArray.Length - 1)
      {
        stateChangeTimer = 0.0f;
        frameCounter = 0;
        isChangingState = false;
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
  protected void testForMovement() {

    // test if player is currently moving on x axis
    movingX = false;
    float currX = transform.position.x; // current
    if (System.Math.Abs(lastX - currX) > 0.1f) {
      movingX = true;
      leftwards = (lastX > currX ? true : false);
    }
    lastX = transform.position.x;

    // test if player is currently moving on Y axis
    movingY = false;
    float currY = transform.position.y; // current
    if (System.Math.Abs(lastY - currY) > 0.1f) {
      movingY = true;
      upwards = (lastY < currY ? true : false);
    }
    lastY = transform.position.y;

  }





  /*
   * called while moving as circle, rotates texture
   */
  private Vector3 rotationVec = new Vector3(0.0f, 0.0f, 0.0f);
  protected void rotateCircle() {
    rotationVec.z = (Time.deltaTime * maxSpeed * (leftwards ? 25.0f : -25f)) % 360;
    textureObject.transform.Rotate(rotationVec);
  }






  /*
  * called every frame
  * update state of "showing movement particles"
  */
  protected void showMovementParticles(bool show) {

    ParticleSystem ps = movementParticles.GetComponent<ParticleSystem>();
    ParticleSystem.MainModule ps_main = ps.main;
    ParticleSystem.VelocityOverLifetimeModule ps_velocity = ps.velocityOverLifetime;

    if (show) {
      ps_velocity.x = (leftwards ? 11.0f : -11.0f);
      ps_velocity.y = 7.0f;
      ps_main.startLifetime = 2.7f;
    }
    else {
      ps_velocity.x = 0.0f;
      ps_velocity.y = 0.0f;
      ps_main.startLifetime = 0.0f;
    }
  }

  protected void playDeathParticles() {

    ParticleSystem.MainModule mainModule = deathParticles.GetComponent<ParticleSystem>().main;

    // set particle color for death particles
    Attributes a = getAttributes();
    mainModule.startColor = a.particleColor;

    deathParticles.SetActive(true);
    deathParticles.GetComponent<ParticleSystem>().Play();

  }



  /*
   ===============================
   === TRIGGERS AND COLLISIONS ===
   ===============================
   */

  public void OnCollisionEnter2D(Collision2D col) {

    string colObjName = col.gameObject.name;

    switch (colObjName)
    {

      case "KillZone":
        Debug.Log("Player died by entering a kill zone.");
        StartCoroutine(respawn());
        break;

      default:
        break;

    }

  }


  private float grassWalkTimer = 0.0f;
  private void updateMovementSounds() {
    grassWalkTimer -= Time.deltaTime;
  }

  public void OnTriggerEnter2D(Collider2D col) {

    switch (col.gameObject.tag) {

      case "Grass":
        if (grassWalkTimer <= 0.0f) {
          grassWalkTimer = 0.24f;
          soundPlayer.PlayOneShot(walkThroughGrassSound);
        }
        break;

      case "Water":
        Debug.Log("PlayerController: Player died by entering water.");
        soundPlayer.PlayOneShot(waterSplashSound);
        StartCoroutine(respawn());
        ScriptedEventsManager.Instance.LoadEvent(1, "water_death");
        break;

      case "MovingPlatform":
        //gameObject.transform.parent = col.gameObject.transform;
        break;

      case "Piston":
        Debug.Log("PlayerController: Stepped on a piston.");
        Piston.Instance.GoUp(col.gameObject);
        soundPlayer.PlayOneShot(pistonPushSound);
        break;

      default:
        break;

    }

  }

  public void OnTriggerExit2D(Collider2D col) {

    switch (col.gameObject.tag) {

      case "MovingPlatform":
        //gameObject.transform.parent = parentObject.transform;
        break;

      default:
        break;

    }

  }

  public void OnTriggerStay2D(Collider2D col) {

    switch (col.gameObject.name) {

      case "ZoomInCamera":
        zoomedInCameraTimer = 0.5f;
        break;

      default:
        break;

    }

  }


}
