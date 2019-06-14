using System.Collections;
using UnityEngine;

/*
 * manages most of the player actions except for the physics calculations, which happen in the linked physics object
 * is joint between other a lot of scripts and other scripts / managers
 */

public class PlayerController : PhysicsObject {

  // singleton
  public static PlayerController Instance;
  public static GameObject playerObject;



  /*
   =======================
   === CAMERA CONTROLS ===
   =======================
   */

  public Animator cameraAnimator;

  private float zoomedInCameraTimer = 0f,
                zoomedOutCameraTimer = 0f;



  /*
   ========================
   === PLAYER VARIABLES ===
   ========================
   */

  private bool holdingItem = false;

  private bool canMove = false,
               canMorphToRectangle = false,
               canMorphToTriangle = false,
               canJump = false,

               isDead = false,
               steppedOnPiston = false;

  private bool isChangingState = false;
  private string newState; // state is defined in physics object

  public bool setSpawnpoint = false;

  public bool isFrozen = false,
              frozenInLastFrame = false;
  private float frozenYPos = 0.0f;

  private float secondsNotGrounded = 0f,
                secondsSinceLastJump = 0f, // timer for seconds the player hadn't been grounded
                secondsAsRectangleFalling = 0f;
  private bool groundedInLastFrame = true;

  private float lastX, lastY; // last x and y position
  private bool leftwards = false, // direction on last movement
               movingX = false,
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

  public GameObject heldItemObject,
                    textureContainer, 
                    movementParticles, 
                    deathParticles,
                    doubleJumpParticles;

  public GhostingEffect ghost;



  /*
   =====================
   === SOUND EFFECTS ===
   =====================
   */

  public AudioSource characterSoundPlayer,
                     circleMovementSoundPlayer,
                     rectangleMovementSoundPlayer,
                     grassSoundPlayer,
                     disappearingBlockAppearSoundPlayer,
                     disappearingBlockDisappearSoundPlayer,
                     movingPlatformSoundPlayer,
                     shortSoundPlayer,
                     cameraShakeSoundPlayer;

  public AudioClip morphSound,
                   landingCircleSound,
                   landingTriangleSound,
                   landingRectangleSound,
                   jumpingTriangleSound,
                   playerDeathSound,

                   walkThroughGrassSound,

                   disappearingBlockAppear,
                   disappearingBlockDisappear,
                   movingPlatformSound,

                   waterSplashSound,
                   breakingBlockSound,
                   pistonPushSound,
                   activateSpawnpointSound,
                   respawnAtSpawnpointSound,

                  laserBulletHit,
                  laserTurretShot,

                   earthquake_1_5_secs,
                   earthquake_2_secs,
                   earthquake_2_5_secs_loud,
                   earthquake_3_secs,

                   robotRepairSound,
                   levelCompleteSound;

  private float preventMovementSoundsTimer = 0f,
                movingPlatformSoundsTimer = 0f,
                movingThroughGrassTimer = 0f,
                movingTimer = 0f;

  // plays a sound and returns the audio clip length as a float
  public float PlaySound(string soundName) {

    AudioClip c;

    switch (soundName) {

      case "morphSound":                 c = morphSound; characterSoundPlayer.PlayOneShot(c); return c.length;
      case "landingCircleSound":         c = landingCircleSound; characterSoundPlayer.PlayOneShot(c); return c.length;
      case "landingTriangleSound":       c = landingTriangleSound; characterSoundPlayer.PlayOneShot(c); return c.length;
      case "landingRectangleSound":      c = landingRectangleSound; characterSoundPlayer.PlayOneShot(c); return c.length;
      case "jumpingTriangleSound":       c = jumpingTriangleSound; characterSoundPlayer.PlayOneShot(c); return c.length;
      case "playerDeathSound":           c = playerDeathSound; characterSoundPlayer.PlayOneShot(c); return c.length;

      case "walkThroughGrassSound":      c = walkThroughGrassSound; grassSoundPlayer.PlayOneShot(c); return c.length;

      case "disappearingBlockAppear":    c = disappearingBlockAppear; disappearingBlockAppearSoundPlayer.PlayOneShot(c); return c.length;
      case "disappearingBlockDisappear": c = disappearingBlockDisappear; disappearingBlockDisappearSoundPlayer.PlayOneShot(c); return c.length;
      case "movingPlatformSound":        c = movingPlatformSound; movingPlatformSoundPlayer.PlayOneShot(c); return c.length;
       
      case "waterSplashSound":           c = waterSplashSound; shortSoundPlayer.PlayOneShot(c); return c.length;
      case "breakingBlockSound":         c = breakingBlockSound; shortSoundPlayer.PlayOneShot(c); return c.length;
      case "pistonPushSound":            c = pistonPushSound; shortSoundPlayer.PlayOneShot(c); return c.length;
      case "activateSpawnpointSound":    c = activateSpawnpointSound; shortSoundPlayer.PlayOneShot(c); return c.length;
      case "respawnAtSpawnpointSound":   c = respawnAtSpawnpointSound; shortSoundPlayer.PlayOneShot(c); return c.length;

      case "laserBulletHit":             c = laserBulletHit; shortSoundPlayer.PlayOneShot(c); return c.length;
      case "laserTurretShot":            c = laserTurretShot; shortSoundPlayer.PlayOneShot(c); return c.length;

      case "earthquake_1_5_secs":        c = earthquake_1_5_secs; cameraShakeSoundPlayer.PlayOneShot(c); return c.length;
      case "earthquake_2_secs":          c = earthquake_2_secs; cameraShakeSoundPlayer.PlayOneShot(c); return c.length;
      case "earthquake_2_5_secs_loud":   c = earthquake_2_5_secs_loud; cameraShakeSoundPlayer.PlayOneShot(c); return c.length;
      case "earthquake_3_secs":          c = earthquake_3_secs; cameraShakeSoundPlayer.PlayOneShot(c); return c.length;
        
      case "robotRepairSound":           c = robotRepairSound; shortSoundPlayer.PlayOneShot(c); return c.length;
      case "levelCompleteSound":         c = levelCompleteSound; shortSoundPlayer.PlayOneShot(c); return c.length;

      default: Debug.LogWarning("PlayerController: Sound " + soundName + " wasn't found."); break;

    }

    return 0f;

  }

  public void StopSoundPlayer(string soundPlayer) {

    switch (soundPlayer) {

      case "characterSoundPlayer": characterSoundPlayer.Stop(); break;
      case "circleMovementSoundPlayer": circleMovementSoundPlayer.Stop(); break;
      case "rectangleMovementSoundPlayer": rectangleMovementSoundPlayer.Stop(); break;
      case "grassSoundPlayer": grassSoundPlayer.Stop(); break;
      case "disappearingBlockAppearSoundPlayer": disappearingBlockAppearSoundPlayer.Stop(); break;
      case "disappearingBlockDisappearSoundPlayer": disappearingBlockDisappearSoundPlayer.Stop(); break;
      case "movingPlatformSoundPlayer": movingPlatformSoundPlayer.Stop(); break;
      case "shortSoundPlayer": shortSoundPlayer.Stop(); break;
      case "cameraShakeSoundPlayer": cameraShakeSoundPlayer.Stop(); break;
      default: break;

    }

  }



  /*
   ==============
   === SETTER ===
   ==============
   */

  public void SetSetting(string name, bool value) {
    switch (name) {
      case "canMove":             canMove = value; break;
      case "canJump":             canJump = value; break;
      case "canMorphToRectangle": canMorphToRectangle = value; break;
      case "canMorphToTriangle":  canMorphToTriangle = value; break;
      default: Debug.LogWarning("PlayerController: Could not set value as " + name + " is not a valid setting."); break;
    }
  }

  public void setValue(string name, bool val) {
    switch (name) {
      case "steppedOnPiston":   steppedOnPiston = val; break;
      case "holdingItem":       holdingItem = val; break;
      default: Debug.LogWarning("PlayerController: Could not set value as " + name + " is not a valid value name."); break;
    }
  }



  /*
   ==============
   === GETTER ===
   ==============
   */

  public bool getBool(string name) {
    switch (name) {
      case "isDead": return isDead;
      case "holdingItem": return holdingItem;
      default: Debug.LogWarning("PlayerController: Boolean of the name " + name + " couldn't be found."); break;
    }
    return false;
  }

  public float GetFloat(string name) {
    switch (name) {
      case "secondsNotGrounded": return secondsNotGrounded;
      case "secondsAsRectangleFalling": return secondsAsRectangleFalling;
      default: Debug.LogWarning("PlayerController: Float of the name " + name + " couldn't be found."); break;
    }
    return 0f;
  }

  public string GetString(string name) {
    switch (name) {
      case "state": return state;
      default: Debug.LogWarning("PlayerController: String of the name " + name + " couldn't be found."); break;
    }
    return null;
  }

  public GameObject GetObject(string name) {
    switch (name) {
      case "textureObject": return textureObject;
      default: Debug.LogWarning("PlayerController: GameObject of the name " + name + " couldn't be found."); break;
    }
    return null;
  }

  private Attributes getAttributes() {
    return getAttributes(state);
  }

  private Attributes getAttributes(string stateName) {

    // look for given name
    foreach (Attributes a_temp in characterAttributes) {
      if (a_temp.name == stateName) return a_temp;
    }

    // if fails return attributes for current state
    Attributes a = new Attributes();
    foreach (Attributes a_temp in characterAttributes) {
      if (a_temp.name == state) a = a_temp; break;
    }
    return a;

  }



  /*
   ======================
   === INITIALIZATION ===
   ======================
   */

  void Awake() {
    Instance = this;

    parentObject = gameObject.transform.parent.gameObject;
    playerObject = gameObject;

    Debug.Log("Player: With parent object '" + parentObject.name + "' initialized.");

    // set attributes for start character state
    resetAttributesOfState();

    lastX = transform.position.x;
    lastY = transform.position.y;
    
    // scan given directory and load images as sprites into memory
    rectToCircle = AddFiles(Resources.LoadAll<Sprite>("Morph/Rectangle_to_Circle"), "Rectangle", "Circle");
    rectToTriangle = AddFiles(Resources.LoadAll<Sprite>("Morph/Rectangle_to_Triangle"), "Rectangle", "Triangle");
    triangleToCircle = AddFiles(Resources.LoadAll<Sprite>("Morph/Triangle_to_Circle"), "Triangle", "Circle");


    Sprite[] AddFiles(Sprite[] arr, string fileFront, string fileEnd) {
      Sprite[] newArr = AddFileAtFront(arr, fileFront);
      return AddFileAtEnd(newArr, fileEnd);
    }
    Sprite[] AddFileAtFront(Sprite[] arr, string file) {
      Sprite[] newArr = new Sprite[arr.Length + 1];
      for (int i = 0; i < newArr.Length; i++) {
        if (i == 0) {
          switch (file) {
            case "Circle": newArr[i] = Resources.Load<Sprite>("Morph/Final/circle_with_light"); break;
            case "Triangle": newArr[i] = Resources.Load<Sprite>("Morph/Final/triangle_with_light"); break;
            case "Rectangle": newArr[i] = Resources.Load<Sprite>("Morph/Final/rectangle_with_light"); break;
            default: break;
          }
        }
        else newArr[i] = arr[i - 1];
      }
      return newArr;
    }

    Sprite[] AddFileAtEnd(Sprite[] arr, string file) {
      Sprite[] newArr = new Sprite[arr.Length + 1];
      for (int i = 0; i < newArr.Length; i++) {
        if (i == newArr.Length - 1) {
          switch (file) {
            case "Circle": newArr[i] = Resources.Load<Sprite>("Morph/Final/circle_with_light"); break;
            case "Triangle": newArr[i] = Resources.Load<Sprite>("Morph/Final/triangle_with_light"); break;
            case "Rectangle": newArr[i] = Resources.Load<Sprite>("Morph/Final/rectangle_with_light"); break;
            default: break;
          }
        }
        else newArr[i] = arr[i];
      }
      return newArr;
    }

  }

  void Start() {

    loadLevelSettingsIntoPlayer();

  }



  /*
   ================
   === MOVEMENT ===
   ================
   */

  // stop figure rolling away even though there is no movement
  private float rollingFixTimer = 0.0f,
                rollingFixTimerDefault = 0.05f;
  private void stopRollingFix() {
    GetComponent<Rigidbody2D>().freezeRotation = true;
    GetComponent<Rigidbody2D>().rotation = 0f;
    GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
  }

  private int timerBecauseUnityIsBeingStupid = 0;
  protected override void ComputeVelocity() {

    // stop movement audio sources which are on loop and start playing when loading the scene
    if (timerBecauseUnityIsBeingStupid < 500) {
      timerBecauseUnityIsBeingStupid++;
      circleMovementSoundPlayer.Pause();
      rectangleMovementSoundPlayer.Pause();
      movingPlatformSoundPlayer.Pause();
    }
    
    movingTimer = movingX && grounded ? 0.2f : movingTimer;

    // general moving sounds
    if (movingTimer > 0f) {
      movingTimer -= Time.fixedDeltaTime;
      if (preventMovementSoundsTimer <= 0f) {
        if (movingX && grounded) {
          if (state == "Circle" && !circleMovementSoundPlayer.isPlaying) {
            circleMovementSoundPlayer.UnPause();
            rectangleMovementSoundPlayer.Pause();
          }
          else if (state == "Rectangle" && !rectangleMovementSoundPlayer.isPlaying) {
            circleMovementSoundPlayer.Pause();
            rectangleMovementSoundPlayer.UnPause();
          }
          else if (state == "Triangle") {
            circleMovementSoundPlayer.Pause();
            rectangleMovementSoundPlayer.Pause();
          }
        }
      }
      else preventMovementSoundsTimer -= Time.fixedDeltaTime;


    }
    else if (state == "Circle") circleMovementSoundPlayer.Pause();
    else rectangleMovementSoundPlayer.Pause();

    // sounds while moving through grass
    if (movingThroughGrassTimer > 0f) {
      movingThroughGrassTimer -= Time.fixedDeltaTime;
      if (!grassSoundPlayer.isPlaying) PlaySound("walkThroughGrassSound");
    }
    else grassSoundPlayer.Stop();

    // sounds of moving platforms
    if (movingPlatformSoundsTimer > 0f) {
      movingPlatformSoundsTimer -= Time.fixedDeltaTime;
      if (!movingPlatformSoundPlayer.isPlaying) movingPlatformSoundPlayer.UnPause();
    }
    else movingPlatformSoundPlayer.Pause();

    // handle camera zooming (inwards)
    if (zoomedInCameraTimer > 0.0f) {
      zoomedInCameraTimer -= Time.fixedDeltaTime;
      if (!cameraAnimator.GetBool("ZoomedIn")) {
        cameraAnimator.SetBool("ZoomedIn", true);
        Debug.Log("PlayerController: Entered 'camera zoom in' area.");
      }
    }
    else if (cameraAnimator.GetBool("ZoomedIn")) {
      cameraAnimator.SetBool("ZoomedIn", false);
      Debug.Log("PlayerController: Left 'camera zoom in' area.");
    }

    // handle camera zooming (outwards)
    if (zoomedOutCameraTimer > 0.0f) {
      zoomedOutCameraTimer -= Time.fixedDeltaTime;
      if (!cameraAnimator.GetBool("ZoomedOut")) {
        cameraAnimator.SetBool("ZoomedOut", true);
        Debug.Log("PlayerController: Entered 'camera zoom out' area.");
      }
    }
    else if (cameraAnimator.GetBool("ZoomedOut")) {
      cameraAnimator.SetBool("ZoomedOut", false);
      Debug.Log("PlayerController: Left 'camera zoom out' area.");
    }

    // handle holding item state
    if (holdingItem && !heldItemObject.activeSelf) heldItemObject.SetActive(true);
    else if (!holdingItem && heldItemObject.activeSelf) heldItemObject.SetActive(false);

    // handle frozen state
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
    else if (frozenInLastFrame) frozenInLastFrame = false;



    Vector2 move = Vector2.zero;

    rollingFixTimer -= Time.deltaTime;
    if (rollingFixTimer <= 0.0f) {
      rollingFixTimer = rollingFixTimerDefault;
      stopRollingFix();
    }

    if (grounded) inDoubleJump = false;

    // test if player is currently moving
    testForYMovement();
    
    if (!isDead) {

      if (!canMove) movingX = false;
      // handle movement of character on x and y axis
      else {

        move.x = Input.GetAxis("Horizontal");

        // test for movement on x axis
        testForXMovement(move);

        if (move.x > 0.0f) rollingFixTimer = rollingFixTimerDefault;

        if (canJump) {

          secondsSinceLastJump += Time.fixedDeltaTime;

          // jumping
          if (Input.GetKeyDown(KeyCode.Space)) {

            if (secondsNotGrounded < 0.13f && secondsSinceLastJump >= 0.4f) {
              textureContainer.GetComponent<Animator>().Play("JumpSquish", 0);
              velocity.y = jumpTakeOffSpeed;
              secondsSinceLastJump = 0f;

              if (state == "Triangle") {
                PlaySound("jumpingTriangleSound");
              }
            }
            // double jump for triangle
            else if (state == "Triangle" && velocity.y > 0f && !inDoubleJump) {
              inDoubleJump = true;
              velocity.y = jumpTakeOffSpeed * 1.2f;
              secondsSinceLastJump = 0f;
              
              doubleJumpParticles.SetActive(true);
              doubleJumpParticles.GetComponent<ParticleSystem>().Play();

              StopSoundPlayer("characterSoundPlayer");
              PlaySound("jumpingTriangleSound");
              
            }

          }
          
          // landing
          if (!groundedInLastFrame && grounded && secondsNotGrounded > 0.2f) {

            textureContainer.GetComponent<Animator>().Play("LandSquish", 0);

            switch (state) {
              case "Rectangle":
                // shake on landing with rectangle
                CameraShake.Instance.Play(.1f, 18f, 18f);
                PlaySound("landingRectangleSound");
                break;
              case "Triangle": PlaySound("landingTriangleSound"); break;
              case "Circle": PlaySound("landingCircleSound"); break;
              default: break;
            }

          }
          groundedInLastFrame = grounded ? true : false;

          // check time sind player was last grounded
          secondsNotGrounded = !grounded ? secondsNotGrounded + Time.deltaTime : 0f;
          secondsAsRectangleFalling = !grounded && state == "Rectangle" ? secondsAsRectangleFalling + Time.deltaTime : 0f;

        }

        if (steppedOnPiston) {
          steppedOnPiston = false;
          velocity.y = jumpTakeOffSpeed * 2f;
        }

        targetVelocity = move * maxSpeed;

      }
      
      if (movingX && state == "Circle") rotateCircle(); // rotate circle in the right direction     
      showMovementParticles(movingX && grounded ? true : false);  // ground particles when moving over ground on the x axis
      ghost.SetGhosting(movingX || movingY ? true : false); // enable ghosting effect while moving

      handleMorphing();
      if (isChangingState) animateState(); // called when changing state, to animate new texture

    }

  }

  private void loadLevelSettingsIntoPlayer() {
    LevelSettings settings = LevelSettings.Instance;
    canMove = settings.canMove;
    canJump = settings.canJump;
    canMorphToRectangle = settings.canMorphToRectangle;
    canMorphToTriangle = settings.canMorphToTriangle;
  }

  private void resetAttributesOfState() {
    Attributes resA = getAttributes();
    gravityModifier = resA.gravityModifier;
    maxSpeed = resA.maxSpeed;
    jumpTakeOffSpeed = resA.jumpTakeOffSpeed;
    textureObject.GetComponent<SpriteRenderer>().sprite = resA.sprite;

    // particle color for movement particles
    ParticleSystem.MainModule mainModule = movementParticles.GetComponent<ParticleSystem>().main;
    mainModule.startColor = resA.particleColor;

    // particle color for death particles
    mainModule = deathParticles.GetComponent<ParticleSystem>().main;
    mainModule.startColor = resA.particleColor;
  }


  /*
   * tests if the player is currently moving
   * sets movingX, movingY and leftwards
   */
  private void testForXMovement(Vector2 move) {

    if (isFrozen) {
      movingX = false;
      return;
    }

    if (move.x > 0.02f) {
      movingX = true; leftwards = false;
    }
    else if (move.x < -0.02f) {
      movingX = true; leftwards = true;
    }
    else movingX = false;

  }
  private void testForYMovement() {

    // test if player is currently moving on Y axis
    movingY = false;
    float currY = transform.position.y; // current
    if (System.Math.Abs(lastY - currY) > 0.1f) {
      movingY = true;
    }
    lastY = transform.position.y;

  }

  /*
   * called while moving as circle, rotates texture
   */
  private Vector3 rotationVec = new Vector3(0.0f, 0.0f, 0.0f);
  protected void rotateCircle() {
    rotationVec.z = (Time.deltaTime * 15 * (leftwards ? 25.0f : -25f)) % 360;
    textureObject.transform.Rotate(rotationVec);
  }




  // kill the player and make him respawn
  public void die() {

    // prevent triggering death animation multiple times
    if (!isDead) {
      isDead = true;

      if (setSpawnpoint) {
        isFrozen = true;
      }

      gravityModifier = 0f;
      velocity.y = 0f;

      canMove = false;
      canJump = false;
      canMorphToRectangle = false;
      canMorphToTriangle = false;

      GetComponent<Rigidbody2D>().freezeRotation = true;
      GetComponent<Rigidbody2D>().rotation = 0f;
      GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);

      // start playing death animation
      StartCoroutine(respawn());
    }

    IEnumerator respawn() {

      CameraShake.Instance.Play(.2f, 10f, 7f);

      textureObject.GetComponent<SpriteRenderer>().sprite = null;

      playDeathParticles();
      PlaySound("playerDeathSound");

      yield return new WaitForSeconds(1.5f);

      // teleport to spawn point
      Vector2 ps = LevelSettings.Instance.playerSpawn;
      gameObject.transform.localPosition = ps;
      frozenYPos = ps.y;

      SpriteRenderer sr = textureObject.GetComponent<SpriteRenderer>();

      // set sprite visible again
      Attributes a = getAttributes();
      sr.sprite = a.sprite;

      // handle spawn point animation (being pushed out of the tube)
      if (setSpawnpoint) {

        PlayerController.Instance.PlaySound("respawnAtSpawnpointSound");
        yield return new WaitForSeconds(.8f);

        // move the metallic arm holding the player out of the spawn point
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        foreach (GameObject sp in spawnPoints) {
          SpawnPoint sp_script = sp.GetComponent<SpawnPoint>();
          if (sp_script != null) sp_script.animateHoldingArm();
        }

        // move the player out of the spawn point
        int steps = 50;
        float spawnPointmoveCharBy = 19f / steps;
        for (int i = 0; i < steps; i++) {
          gameObject.transform.position += new Vector3(spawnPointmoveCharBy, 0f, 0f);
          yield return new WaitForSeconds(0.03f);
        }
        isFrozen = false;
      }

      // reset gravity modifier
      gravityModifier = a.gravityModifier;

      isDead = false;

      loadLevelSettingsIntoPlayer(); // reset internal settings for player, replace with level settings
      resetAttributesOfState(); // reset attributes to current state
      StopCoroutine(respawn());

    }

  }

  




  // changes state of player to other form
  public void handleMorphing() {

    if (!(canMorphToTriangle || canMorphToRectangle)) {
      return;
    }

    void setCollider(int id) {
      GetComponent<CircleCollider2D>().enabled = (id == 1 ? true : false);
      GetComponent<PolygonCollider2D>().enabled = (id == 2 ? true : false);
      GetComponent<BoxCollider2D>().enabled = (id == 3 ? true : false);
    }

    if (Input.GetKeyDown("" + 1) && !isChangingState && state != "Circle") {
      newState = "Circle";
      setCollider(1);
      GetComponent<Rigidbody2D>().freezeRotation = true;
      GetComponent<Rigidbody2D>().rotation = 0f;
      GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
      ChangeState();
    }

    if (canMorphToTriangle && Input.GetKeyDown("" + 2) && !isChangingState && state != "Triangle") {
      ScriptedEventsManager.Instance.LoadEvent(1, "morph_to_triangle");

      newState = "Triangle";
      setCollider(2);
      GetComponent<Rigidbody2D>().freezeRotation = true;
      GetComponent<Rigidbody2D>().rotation = 0f;
      GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
      ChangeState();
    }

    if (canMorphToRectangle && Input.GetKeyDown("" + 3) && !isChangingState && state != "Rectangle") {
      newState = "Rectangle";
      setCollider(3);
      GetComponent<Rigidbody2D>().freezeRotation = true;
      GetComponent<Rigidbody2D>().rotation = 0f;
      GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
      ChangeState();
    }

  }
  
  private Sprite[] rectToCircle, rectToTriangle, triangleToCircle, // sprite arrays containing morphing graphics
                   animationArray; // the final array which will be animated

  protected void ChangeState() {

    // create and reutrn new array with values of given array
    void assignAnimationArray(Sprite[] spriteArray, bool reverse) {

      animationArray = new Sprite[spriteArray.Length];

      int counter = reverse ? spriteArray.Length - 1 : 0;

      foreach (Sprite sprite in spriteArray) {
        animationArray[counter] = sprite;
        counter = reverse ? counter - 1 : counter + 1;
      }

    }

    switch (state) {
      // circle --> triangle/rectangle
      case "Circle": assignAnimationArray(newState == "Triangle" ? triangleToCircle : rectToCircle, true); break;
      // triangle --> circle/rectangle
      case "Triangle": assignAnimationArray(newState == "Circle" ? triangleToCircle : rectToTriangle, newState == "Rectangle"); break;
      // rectangle --> circle/triangle
      case "Rectangle": assignAnimationArray(newState == "Circle" ? rectToCircle : rectToTriangle, false); break;
      default: break;
    }

    // play sound
    PlaySound("morphSound");

    // set movement variables of the character type
    Attributes a = getAttributes(newState);
    gravityModifier = a.gravityModifier;
    maxSpeed = a.maxSpeed;
    jumpTakeOffSpeed = a.jumpTakeOffSpeed;

    // particle color for movement particles
    ParticleSystem.MainModule mainModule = movementParticles.GetComponent<ParticleSystem>().main;
    mainModule.startColor = a.particleColor;

    // particle color for death particles
    mainModule = deathParticles.GetComponent<ParticleSystem>().main;
    mainModule.startColor = a.particleColor;

    // reset frame counter for state-change animation
    frameCounter = 0;

    isChangingState = true;

  }





  private float animationDuration = 0.16f;
  private int frameCounter = 0;
  private float stateChangeTimer = 0f;
  private void animateState() {

    stateChangeTimer += Time.deltaTime;

    if (stateChangeTimer > animationDuration / animationArray.Length) {
      stateChangeTimer = 0f;

      if (frameCounter >= 1) {

        // reset rotation
        Vector3 ea = textureObject.transform.eulerAngles;
        ea.z = 0f;
        textureObject.transform.eulerAngles = ea;

      }

      textureObject.GetComponent<SpriteRenderer>().sprite = animationArray[frameCounter] as Sprite;
      
      // last image -> reset
      if (frameCounter >= animationArray.Length - 1) {
        stateChangeTimer = 0f;
        frameCounter = 0;
        isChangingState = false;
        state = newState;
      }

      frameCounter++;

    }

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

  public void OnTriggerEnter2D(Collider2D col) {

    switch (col.gameObject.tag) {

      case "Water":
        Debug.Log("PlayerController: Player died by entering water.");
        PlaySound("waterSplashSound");
        die();
        ScriptedEventsManager.Instance.LoadEvent(1, "water_death");
        break;

      case "KillZone":
        Debug.Log("PlayerController: Player died by entering a kill zone.");
        die();
        break;

      case "Piston":
        Debug.Log("PlayerController: Stepped on a piston.");
        Piston.Instance.GoUp(col.gameObject);
        PlaySound("pistonPushSound");
        break;

      default:
        break;

    }

  }

  public void OnTriggerStay2D(Collider2D col) {

    switch (col.gameObject.tag) {

      case "ZoomInCamera":
        zoomedInCameraTimer = 0.5f;
        break;

      case "ZoomOutCamera":
        zoomedOutCameraTimer = 0.5f;
        break;

      case "PreventMovementSounds":
        preventMovementSoundsTimer = 0.2f;
        break;

      case "Grass":
        if (movingX && grounded) movingThroughGrassTimer = 0.2f;
        break;

      case "MovingPlatformSounds":
        movingPlatformSoundsTimer = 0.2f;
        break;

      default:
        break;

    }

  }

  public void OnCollisionStay2D(Collision2D col) {
    
    switch (col.gameObject.tag) {

      case "MovingPlatform":
        // make the player move along the moving platform if standing on top
        transform.position += col.gameObject.GetComponent<MovingPlatform>().movePlayerBy;
        break;

      default:
        break;

    }

  }

}
