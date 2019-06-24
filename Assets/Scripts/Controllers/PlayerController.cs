using System.Collections;
using UnityEngine;

/*
 * manages most of the player actions except for the physics calculations, which happen in the linked physics object
 * is joint between other a lot of scripts and other scripts / managers
 */

public class PlayerController : PhysicsObject {



  /*
   =======================
   === CAMERA CONTROLS ===
   =======================
   */

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
   ======================
   === INITIALIZATION ===
   ======================
   */

  private protected override void OnAwake() {

    morphIndicator.loadMorphIndicators(state, canMorphToCircle, canMorphToTriangle, canMorphToRectangle);

    resetAttributes(); // set attributes for start character state
    loadLevelSettingsIntoPlayer();

    lastX = transform.position.x;
    lastY = transform.position.y;

    Debug.Log("PlayerController: Initialized.");

  }

  private protected override void UpdateBeforeVelocity() {

    /*
     * called before velocity is calculate
     */

    if (!isDead && Input.GetKeyDown(KeyCode.Alpha0)) {
      die();
    }

    soundController.handleContinuousSound(state, movingX, grounded);
    handleCameraZoom();
    handleHoldingItem();

  }

  private protected override void UpdateAfterVelocity() {

    /*
     * called after velocity is updated
     */

    showMovementParticles();

    if (!isDead) {
      rotateCircle(); // rotate circle in the right direction     
      ghostingEffect.set(movingX || movingY ? true : false); // enable ghosting effect while moving

      // only allow morph a little time after start up, 
      // to give any level settings that still need to be initialized correctly time
      // (for example if set through starting frequence in scripted event manager)
      if (Time.time > .6f) {
        handleMorphing();
      }
      
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

    /*
     * reset attributes of dynamic rigid body
     */

    rb2d.freezeRotation = true;
    rb2d.rotation = 0f;
    rb2d.velocity = new Vector2(0f, 0f);
  }

  private protected override void ComputeVelocity() {

    /*
     * calculate velocity for player under consideration
     * of his current state, settings and attributes
     */

    // handle frozen state
    if (isFrozen) {
      if (!frozenInLastFrame) {
        ghostingEffect.disable();
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
                soundController.playSound("jumpingTriangleSound");
              }
            }
            // double jump for triangle
            else if (state == "Triangle" && velocity.y > 0f && !inDoubleJump) {

              inDoubleJump = true;
              velocity.y = jumpTakeOffSpeed * 1.2f;
              secondsSinceLastJump = 0f;
              
              doubleJumpParticles.SetActive(true);
              doubleJumpParticles.GetComponent<ParticleSystem>().Play();

              soundController.stopSoundPlayer("characterSoundPlayer");
              soundController.playSound("jumpingTriangleSound");
              
            }

          }
          
          // landing
          if (!groundedInLastFrame && grounded && secondsNotGrounded > 0.2f) {

            textureContainer.GetComponent<Animator>().Play("LandSquish", 0);

            switch (state) {
              case "Rectangle":
                // shake on landing with rectangle
                CameraShake.Instance.Play(.1f, 18f, 18f);
                soundController.playSound("landingRectangleSound");
                break;
              case "Triangle":
                soundController.playSound("landingTriangleSound");
                break;
              case "Circle":
                soundController.playSound("landingCircleSound");
                break;
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

  protected void loadLevelSettingsIntoPlayer() {

    /*
     * load the level settings from the 'level settings' script
     * and replace the values of the internal variables with them
     */

    LevelSettings settings = LevelSettings.Instance;
    canMove = settings.canMove;
    canJump = settings.canJump;
    canMorphToCircle = settings.canMorphToCircle;
    canMorphToTriangle = settings.canMorphToTriangle;
    canMorphToRectangle = settings.canMorphToRectangle;

    // reload 'morph indicator' UI in case morph settings are different now
    morphIndicator.loadMorphIndicators();

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
      ghostingEffect.disable();

      if (hasSpawnpointSet) {
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
      soundController.playSound("playerDeathSound");

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
      if (hasSpawnpointSet) {

        soundController.playSound("respawnAtSpawnpointSound");
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

    /*
     * set the right 2D collider depending
     * on the player's current state
     */

    GetComponent<CircleCollider2D>().enabled = (state == "Circle" ? true : false);
    GetComponent<PolygonCollider2D>().enabled = (state == "Triangle" ? true : false);
    GetComponent<BoxCollider2D>().enabled = (state == "Rectangle" ? true : false);
  }


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
        scriptedEvents.LoadEvent(2, "morph_to_triangle");
      }
      else if (canMorphToRectangle && Input.GetKeyDown(KeyCode.Alpha3) && state != "Rectangle") {
        newState = "Rectangle";
        initiateMorphing = true;
        scriptedEvents.LoadEvent(2, "rectangle_morph_praises");
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
      soundController.playSound("morphSound");

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

  void OnTriggerEnter2D(Collider2D col) {

    switch (col.gameObject.tag) {

      case "Water":
        Debug.Log("PlayerController: Player died by entering water.");
        soundController.playSound("waterSplashSound");
        die();
        scriptedEvents.LoadEvent(1, "water_death");
        break;

      case "KillZone":
        Debug.Log("PlayerController: Player died by entering a kill zone.");
        die();
        break;

      case "Piston":
        Debug.Log("PlayerController: Stepped on a piston.");
        Piston.Instance.GoUp(col.gameObject);
        soundController.playSound("pistonPushSound");
        break;

      case "NoMorphForceField":
        Debug.Log("PlayerController: Entered a 'no morph force field'.");
        soundController.playSound("enterForceFieldSound");
        canMorphToCircle = false;
        canMorphToTriangle = false;
        canMorphToRectangle = false;
        morphIndicator.loadMorphIndicators(state, false, false, false);
        scriptedEvents.LoadEvent(2, "morph_force_field");
        break;

    }

  }

  void OnTriggerExit2D(Collider2D col) {

    switch (col.gameObject.tag) {

      case "NoMorphForceField":
        Debug.Log("PlayerController: Left a 'no morph force field'.");
        loadLevelSettingsIntoPlayer();
        soundController.playSound("exitForceFieldSound");
        morphIndicator.loadMorphIndicators(state, canMorphToCircle, canMorphToTriangle, canMorphToRectangle);
        break;

    }

  }

  void OnTriggerStay2D(Collider2D col) {

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

  void OnCollisionStay2D(Collision2D col) {

    switch (col.gameObject.tag) {

      case "MovingPlatform":
        // make the player move along the moving platform if standing on top
        transform.position += col.gameObject.GetComponent<MovingPlatform>().movePlayerBy;
        break;

    }

  }

}
