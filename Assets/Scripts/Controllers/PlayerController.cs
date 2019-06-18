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
  public static MorphIndicator morphIndicator;



  /*
   =======================
   === CAMERA CONTROLS ===
   =======================
   */

  public Animator cameraAnimator;

  private float zoomedInCameraTimer = 0f,
                zoomedOutCameraTimer = 0f,
                zoomedOutCameraFarTimer = 0f;

  private void handleCameraZoom() {

    /*
     * manages the zoom-in and zoom-out effect
     * of the virtual camera following the player
     */

    // handle camera zooming (inwards)
    if (zoomedInCameraTimer > 0f) {
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
    if (zoomedOutCameraTimer > 0f) {
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

    // handle camera zooming (outwards far)
    if (zoomedOutCameraFarTimer > 0f) {
      zoomedOutCameraFarTimer -= Time.fixedDeltaTime;
      if (!cameraAnimator.GetBool("ZoomedOutFar")) {
        cameraAnimator.SetBool("ZoomedOutFar", true);
        Debug.Log("PlayerController: Entered 'camera zoom out far' area.");
      }
    }
    else if (cameraAnimator.GetBool("ZoomedOutFar")) {
      cameraAnimator.SetBool("ZoomedOutFar", false);
      Debug.Log("PlayerController: Left 'camera zoom out far' area.");
    }

  }



  /*
   ========================
   === PLAYER VARIABLES ===
   ========================
   */

  private bool holdingItem = false;

  private bool canMove = false,
               canMorphToCircle = true,
               canMorphToTriangle = false,
               canMorphToRectangle = false,
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

  public GameObject heldItemObject, textureContainer, 
                    movementParticles, deathParticles, doubleJumpParticles;

  public GhostingEffect ghost;
  public SoundController soundController;



  /*
   ==============
   === SETTER ===
   ==============
   */

  public void SetSetting(string name, bool value) {
    switch (name) {
      case "canMove":             canMove = value; break;
      case "canJump":             canJump = value; break;
      case "canMorphToCircle":    canMorphToCircle = value; break;
      case "canMorphToTriangle":  canMorphToTriangle = value; break;
      case "canMorphToRectangle": canMorphToRectangle = value; break;
      default: Debug.LogWarning("PlayerController: Could not set value as " + name + " is not a valid setting."); break;
    }
    morphIndicator.loadMorphIndicators();
  }

  public void setValue(string name, bool value) {
    switch (name) {
      case "steppedOnPiston":   steppedOnPiston = value; break;
      case "holdingItem":       holdingItem = value; break;
      default: Debug.LogWarning("PlayerController: Could not set value as " + name + " is not a valid value name."); break;
    }
  }



  /*
   ==============
   === GETTER ===
   ==============
   */

  public bool GetBool(string name) {
    switch (name) {
      case "canMorphToCircle": return canMorphToCircle;
      case "canMorphToTriangle": return canMorphToTriangle;
      case "canMorphToRectangle": return canMorphToRectangle;
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



  /*
   ======================
   === INITIALIZATION ===
   ======================
   */

  void Awake() {

    Instance = this;

    morphIndicator = MorphIndicator.Instance;
    morphIndicator.loadMorphIndicators(state, canMorphToCircle, canMorphToTriangle, canMorphToRectangle);

    soundController = SoundController.Instance;

    parentObject = gameObject.transform.parent.gameObject;
    playerObject = gameObject;

    Debug.Log("Player: With parent object '" + parentObject.name + "' initialized.");

    resetAttributes(); // set attributes for start character state
    loadMorphAnimationSprites();
    loadLevelSettingsIntoPlayer();

    lastX = transform.position.x;
    lastY = transform.position.y;

  }

  protected override void UpdateBeforeVelocity() {

    soundController.handleContinuousSound(state, movingX, grounded);
    handleCameraZoom();
    handleHoldingItem();

  }

  protected override void UpdateAfterVelocity() {

    showMovementParticles();

    if (!isDead) {
      rotateCircle(); // rotate circle in the right direction     
      ghost.SetGhosting(movingX || movingY ? true : false); // enable ghosting effect while moving
      handleMorphing();
    }

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

  private void loadMorphAnimationSprites() {

    /*
     * load morph animation sprites into arrays for use in morphing process
     */

    // scan given directory and load morph animation images as sprites into memory
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
          }
        }
        else newArr[i] = arr[i];
      }
      return newArr;
    }

  }

  private void handleHoldingItem() {

    /*
     * handle holding item state
     */

    if (holdingItem && !heldItemObject.activeSelf) {
      heldItemObject.SetActive(true);
    }
    else if (!holdingItem && heldItemObject.activeSelf) {
      heldItemObject.SetActive(false);
    }

  }


  /*
   ================
   === MOVEMENT ===
   ================
   */

  // stop figure rolling away even though there is no movement
  private float rollingFixTimer = 0f,
                rollingFixTimerDefault = 0.05f;
  private void resetDynamicRGB2D() {
    rb2d.freezeRotation = true;
    rb2d.rotation = 0f;
    rb2d.velocity = new Vector2(0f, 0f);
  }

  protected override void ComputeVelocity() {

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
    else if (frozenInLastFrame) {
      frozenInLastFrame = false;
    }

    rollingFixTimer -= Time.deltaTime;
    if (rollingFixTimer <= 0f) {
      rollingFixTimer = rollingFixTimerDefault;
      resetDynamicRGB2D();
    }

    Vector2 move = Vector2.zero;

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
                soundController.PlaySound("jumpingTriangleSound");
              }
            }
            // double jump for triangle
            else if (state == "Triangle" && velocity.y > 0f && !inDoubleJump) {
              inDoubleJump = true;
              velocity.y = jumpTakeOffSpeed * 1.2f;
              secondsSinceLastJump = 0f;
              
              doubleJumpParticles.SetActive(true);
              doubleJumpParticles.GetComponent<ParticleSystem>().Play();

              soundController.StopSoundPlayer("characterSoundPlayer");
              soundController.PlaySound("jumpingTriangleSound");
              
            }

          }
          
          // landing
          if (!groundedInLastFrame && grounded && secondsNotGrounded > 0.2f) {

            textureContainer.GetComponent<Animator>().Play("LandSquish", 0);

            switch (state) {
              case "Rectangle":
                // shake on landing with rectangle
                CameraShake.Instance.Play(.1f, 18f, 18f);
                soundController.PlaySound("landingRectangleSound");
                break;
              case "Triangle": soundController.PlaySound("landingTriangleSound"); break;
              case "Circle": soundController.PlaySound("landingCircleSound"); break;
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

    }

  }

  private void loadLevelSettingsIntoPlayer() {
    LevelSettings settings = LevelSettings.Instance;
    canMove = settings.canMove;
    canJump = settings.canJump;
    canMorphToCircle = settings.canMorphToCircle;
    canMorphToTriangle = settings.canMorphToTriangle;
    canMorphToRectangle = settings.canMorphToRectangle;

    morphIndicator.loadMorphIndicators();
  }

  private void resetAttributes() {
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



  private void testForXMovement(Vector2 move) {

    /*
     * tests if the player is currently moving on the x axis 
     * sets 'movingX' and 'leftwards'
     */

    movingX = false;

    if (isFrozen || state == "Triangle") {
      return;
    }

    if (move.x > 0.02f) {
      movingX = true; leftwards = false;
    }
    else if (move.x < -0.02f) {
      movingX = true; leftwards = true;
    }

  }
  private void testForYMovement() {

    /*
     * tests if the player is currently moving on the y axis 
     * sets 'movingY'
     */

    movingY = false;
    float currentY = transform.position.y;
    if (System.Math.Abs(lastY - currentY) > 0.1f) {
      movingY = true;
    }
    lastY = currentY;

  }


  private Vector3 rotationVec = new Vector3(0.0f, 0.0f, 0.0f);
  private void rotateCircle() {

    /*
     * called while moving as circle in order to rotate circle texture
     */

    if (movingX && state == "Circle") {
      rotationVec.z = (Time.deltaTime * 15 * (leftwards ? 25.0f : -25f)) % 360;
      textureObject.transform.Rotate(rotationVec);
    }
  }




  public void die() {

    /*
     * kill the player and play respawn animation
     */

    // prevent triggering death animation multiple times
    if (!isDead) {
      isDead = true;
      ghost.SetGhosting(false);

      if (setSpawnpoint) {
        isFrozen = true;
      }

      gravityModifier = 0f;
      velocity.y = 0f;

      canMove = false;
      canJump = false;
      canMorphToCircle = false;
      canMorphToTriangle = false;
      canMorphToRectangle = false;
      morphIndicator.loadMorphIndicators(state, false, false, false);

      // make sprite invisible while respawn
      Color color = textureObject.GetComponent<SpriteRenderer>().color;
      color.a = 0f;
      textureObject.GetComponent<SpriteRenderer>().color = color;

      resetDynamicRGB2D();

      // start playing death animation
      StartCoroutine(respawn());
    }

    IEnumerator respawn() {

      CameraShake.Instance.Play(.2f, 10f, 7f);

      playDeathParticles();
      soundController.PlaySound("playerDeathSound");

      yield return new WaitForSeconds(1.5f);

      // teleport to spawn point
      Vector2 ps = LevelSettings.Instance.playerSpawn;
      gameObject.transform.localPosition = ps;
      frozenYPos = ps.y;

      // reset sprite to circle
      state = "Circle";
      Attributes a = getAttributes();
      textureObject.GetComponent<SpriteRenderer>().sprite = a.sprite;

      // make sprite visible again
      Color color = textureObject.GetComponent<SpriteRenderer>().color;
      color.a = 1f;
      textureObject.GetComponent<SpriteRenderer>().color = color;

      // handle spawn point animation (being pushed out of the tube)
      if (setSpawnpoint) {

        PlayerController.Instance.soundController.PlaySound("respawnAtSpawnpointSound");
        yield return new WaitForSeconds(.6f);

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

      // reset attributes to circle once more
      state = "Circle";
      resetAttributes();

      isDead = false;

      loadLevelSettingsIntoPlayer(); // reset internal settings for player, replace with level settings

      StopCoroutine(respawn());

    }

  }

  

  private void setCollider(string state) {
    GetComponent<CircleCollider2D>().enabled = (state == "Circle" ? true : false);
    GetComponent<PolygonCollider2D>().enabled = (state == "Triangle" ? true : false);
    GetComponent<BoxCollider2D>().enabled = (state == "Rectangle" ? true : false);
  }


  private Sprite[] rectToCircle, rectToTriangle, triangleToCircle; // sprite arrays containing morphing graphics

  private void handleMorphing() {

    /*
     * changes player's state to another one 
     */

    bool initiateMorphing = false;

    if (!isChangingState) { 
      if (canMorphToCircle && Input.GetKeyDown(KeyCode.Alpha1) && state != "Circle") {
        newState = "Circle";
        initiateMorphing = true;
      }
      else if (canMorphToTriangle && Input.GetKeyDown(KeyCode.Alpha2)  && state != "Triangle") {
        newState = "Triangle";
        initiateMorphing = true;
        ScriptedEventsManager.Instance.LoadEvent(2, "morph_to_triangle");
      }
      else if (canMorphToRectangle && Input.GetKeyDown(KeyCode.Alpha3) && state != "Rectangle") {
        newState = "Rectangle";
        initiateMorphing = true;
      }
    }



    // the final array which will be animated
    Sprite[] animationArray = null;

    if (initiateMorphing) {

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
      }

      // play morphing sound
      soundController.PlaySound("morphSound");

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

      // set new state of character
      state = newState;
      setCollider(state);
      resetDynamicRGB2D();
      isChangingState = true;

      morphIndicator.loadMorphIndicators(state, canMorphToCircle, canMorphToTriangle, canMorphToRectangle);

      // start morphing animation
      StopCoroutine(animateState());
      StartCoroutine(animateState());

    }

    IEnumerator animateState() {

      // reset rotation
      textureObject.transform.eulerAngles = Vector3.zero;

      float animationDuration = 0.16f;

      for (int i = 0; i < animationArray.Length; i++) {

        yield return new WaitForSeconds(animationDuration / animationArray.Length);
        textureObject.GetComponent<SpriteRenderer>().sprite = animationArray[i] as Sprite;

      }

      isChangingState = false;

      StopCoroutine(animateState());

    }

  }

  private void showMovementParticles() {

    /*
     * update state of "movement particles" every frame
     * (ground particles when moving over ground on the x axis)
     */

    bool showParticles = false;

    // don't show particles if not moving
    if (movingX && grounded) {
      showParticles = true;
    }

    ParticleSystem ps = movementParticles.GetComponent<ParticleSystem>();
    ParticleSystem.MainModule ps_main = ps.main;
    ParticleSystem.VelocityOverLifetimeModule ps_velocity = ps.velocityOverLifetime;

    if (showParticles) {
      ps_velocity.x = (leftwards ? 11f : -11f);
      ps_velocity.y = 7f;
      ps_main.startLifetime = 2.7f;
    }
    else {
      ps_velocity.x = 0f;
      ps_velocity.y = 0f;
      ps_main.startLifetime = 0f;
    }

  }

  private void playDeathParticles() {

    /*
     * play exploding particles on death
     */

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
        soundController.PlaySound("waterSplashSound");
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
        soundController.PlaySound("pistonPushSound");
        break;

      case "NoMorphForceField":
        Debug.Log("PlayerController: Entered a 'no morph force field'.");
        soundController.PlaySound("enterForceFieldSound");
        canMorphToCircle = false;
        canMorphToTriangle = false;
        canMorphToRectangle = false;
        morphIndicator.loadMorphIndicators(state, false, false, false);
        break;

    }

  }

  public void OnTriggerExit2D(Collider2D col) {

    switch (col.gameObject.tag) {

      case "NoMorphForceField":
        Debug.Log("PlayerController: Left a 'no morph force field'.");
        loadLevelSettingsIntoPlayer();
        soundController.PlaySound("exitForceFieldSound");
        morphIndicator.loadMorphIndicators(state, canMorphToCircle, canMorphToTriangle, canMorphToRectangle);
        break;

    }

  }

  public void OnTriggerStay2D(Collider2D col) {

    switch (col.gameObject.tag) {

      /* Camera Zooming */

      case "ZoomInCamera":
        zoomedInCameraTimer = 0.5f;
        break;

      case "ZoomOutCamera":
        zoomedOutCameraTimer = 0.5f;
        break;

      case "ZoomOutCameraFar":
        zoomedOutCameraFarTimer = 0.5f;
        break;

      /* Sounds */

      case "Grass":
        if (movingX && grounded) {
          soundController.setTimer("movingThroughGrassTimer", 0.2f);
        }
        break;

      case "PreventMovementSounds":
        soundController.setTimer("preventMovementSoundsTimer", 0.2f);
        break;

      case "MovingPlatformSounds":
        soundController.setTimer("movingPlatformSoundsTimer", 0.2f);
        break;

      case "FireSounds":
        soundController.setTimer("fireSoundTimer", 0.2f);
        break;

      case "NoMorphForceField":
        soundController.setTimer("forceFieldSoundTimer", 0.2f);
        break;

    }

  }

  public void OnCollisionStay2D(Collision2D col) {
    
    switch (col.gameObject.tag) {

      case "MovingPlatform":
        // make the player move along the moving platform if standing on top
        transform.position += col.gameObject.GetComponent<MovingPlatform>().movePlayerBy;
        break;

    }

  }

}
