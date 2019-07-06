using System.Collections;
using UnityEngine;

/*
 * manages most of the player actions except for the physics calculations, which happen in the linked physics object
 * is joint between other a lot of scripts and other scripts / managers
 */

public class PlayerController : PhysicsObject {

  /*
   ======================
   === INITIALISATION ===
   ======================
   */

  private protected override void OnAwake() {

    /*
     * called on Awake() by PlayerManager
     */

    // set proper states in morph indicator UI
    morphIndicator.loadMorphIndicators(state, canSelfDestruct, canMorphToCircle, canMorphToTriangle, canMorphToRectangle);

    // set attributes for start character state
    resetAttributes();
    loadLevelSettingsIntoPlayer();

    lastX = transform.position.x;
    lastY = transform.position.y;

    Log.Print($"Initialised on object '{gameObject.name}'.", this);

  }

  private protected override void UpdateBeforeVelocity() {

    /*
     * called by PlayerManager before velocity is calculated
     */

    if (!isDead && canSelfDestruct && Input.GetKeyDown(KeyCode.Alpha0)) {
      die();
    }

    soundController.handleContinuousSound(state, movingX, grounded);
    handleCameraZoom();
    handleHoldingItem();

  }

  private protected override void UpdateAfterVelocity() {

    /*
     * called by PlayerManager after velocity is updated
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
     * handle the 'holding item' state
     */

    if (holdingItem && !heldItemObject.activeSelf) {
      heldItemObject.SetActive(true);
    }
    else if (!holdingItem && heldItemObject.activeSelf) {
      heldItemObject.SetActive(false);
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
      else if (canMorphToTriangle && Input.GetKeyDown(KeyCode.Alpha2) && state != "Triangle") {
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

      morphIndicator.loadMorphIndicators(state, canSelfDestruct, canMorphToCircle, canMorphToTriangle, canMorphToRectangle);

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
        Log.Print("Entered 'camera zoom in' area.", cameraAnimator.gameObject);
      }
    }
    else if (cameraAnimator.GetBool("ZoomedIn")) {
      cameraAnimator.SetBool("ZoomedIn", false);
      Log.Print("Left 'camera zoom in' area.", cameraAnimator.gameObject);
    }

    // handle camera zooming (outwards)
    if (zoomedOutCameraTimer > 0f) {
      zoomedOutCameraTimer -= Time.fixedDeltaTime;
      if (!cameraAnimator.GetBool("ZoomedOut")) {
        cameraAnimator.SetBool("ZoomedOut", true);
        Log.Print("Entered 'camera zoom out' area.", cameraAnimator.gameObject);
      }
    }
    else if (cameraAnimator.GetBool("ZoomedOut")) {
      cameraAnimator.SetBool("ZoomedOut", false);
      Log.Print("Left 'camera zoom out' area.", cameraAnimator.gameObject);
    }

    // handle camera zooming (outwards far)
    if (zoomedOutCameraFarTimer > 0f) {
      zoomedOutCameraFarTimer -= Time.fixedDeltaTime;
      if (!cameraAnimator.GetBool("ZoomedOutFar")) {
        cameraAnimator.SetBool("ZoomedOutFar", true);
        Log.Print("Entered 'camera zoom out far' area.", cameraAnimator.gameObject);
      }
    }
    else if (cameraAnimator.GetBool("ZoomedOutFar")) {
      cameraAnimator.SetBool("ZoomedOutFar", false);
      Log.Print("Left 'camera zoom out far' area.", cameraAnimator.gameObject);
    }

    // handle camera zooming (outwards cinematic)
    if (zoomedOutCameraCinematicTimer > 0f) {
      zoomedOutCameraCinematicTimer -= Time.fixedDeltaTime;
      if (!cameraAnimator.GetBool("ZoomedOutCinematic")) {
        cameraAnimator.SetBool("ZoomedOutCinematic", true);
        Log.Print("Entered 'camera zoom out cinematic' area.", cameraAnimator.gameObject);
      }
    }
    else if (cameraAnimator.GetBool("ZoomedOutCinematic")) {
      cameraAnimator.SetBool("ZoomedOutCinematic", false);
      Log.Print("Left 'camera zoom out cinematic' area.", cameraAnimator.gameObject);
    }

    // handle camera zooming (downwards)
    if (zoomedDownCameraTimer > 0f) {
      zoomedDownCameraTimer -= Time.fixedDeltaTime;
      if (!cameraAnimator.GetBool("ZoomedDown")) {
        cameraAnimator.SetBool("ZoomedDown", true);
        Log.Print("Entered 'camera zoom down' area.", cameraAnimator.gameObject);
      }
    }
    else if (cameraAnimator.GetBool("ZoomedDown")) {
      cameraAnimator.SetBool("ZoomedDown", false);
      Log.Print("Left 'camera zoom down' area.", cameraAnimator.gameObject);
    }

  }





  /*
   ================
   === MOVEMENT ===
   ================
   */

  // stop figure rolling away even though there is no movement
  private float rollingFixTimer = 0f;
  private float rollingFixTimerDefault = 0.05f;

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

      movingX = false;
      movingY = false;

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

    // vector containing movement values
    Vector2 move = Vector2.zero;

    // apply movement from x axis input
    if (!isDead && canMove) {
      move.x = Input.GetAxis("Horizontal");
    }

    // test if player is currently moving
    testForMovement(move);

    if (grounded) {
      inDoubleJump = false;
    }

    if (!isDead) {

      // handle movement of character on x and y axis
      if (canMove) {

        if (move.x > 0.0f) {
          rollingFixTimer = rollingFixTimerDefault;
        }

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
                CameraShake.Instance.play(.2f, 17f, 12f);
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

        } /* end of canJump if */

        if (steppedOnPiston) {
          steppedOnPiston = false;
          velocity.y = jumpTakeOffSpeed * 2f;
        }

        targetVelocity = move * maxSpeed;

      } /* end of canMove if */

    }

  }

  private void testForMovement(Vector2 move) {

    /*
     * tests if the player is currently moving on the x or y axis 
     * sets 'movingX', 'movingY' and 'leftwards'
     */

    if (!canMove || state == "Triangle") {
      movingX = false;
    }

    if (isFrozen) {
      movingX = false;
      movingY = false;
    }
    else {
      testForXMovement();
      testForYMovement();
    }


    // test for movement on x axis
    void testForXMovement() {

      movingX = false;

      if (move.x > 0.02f) {
        movingX = true;
        leftwards = false;
      }
      else if (move.x < -0.02f) {
        movingX = true;
        leftwards = true;
      }

    }

    // test for movement on y axis
    void testForYMovement() {

      float currentY = transform.position.y;
      movingY = Mathf.Abs(lastY - currentY) > 0.1f ? true : false;
      lastY = currentY;

    }

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





  /*
   * =================
   * === PARTICLES ===
   * =================
   */

  private void showMovementParticles() {

    /*
     * update state of "movement particles" every frame
     * (ground particles when moving over ground on the x axis)
     */

    ParticleSystem ps = movementParticles.GetComponent<ParticleSystem>();
    ParticleSystem.MainModule ps_main = ps.main;
    ParticleSystem.VelocityOverLifetimeModule ps_velocity = ps.velocityOverLifetime;

    if (movingX && grounded) {
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
   ================
   === TRIGGERS ===
   ================
   */

  private void OnTriggerEnter2D(Collider2D col) {

    switch (col.gameObject.tag) {

      case "Water":
        Log.Print($"Player died by falling in water '{col.gameObject.name}'.", col.gameObject);
        soundController.playSound("waterSplashSound");
        scriptedEvents.LoadEvent(1, "water_death");
        die();
        break;

      case "KillZone":
        Log.Print($"Player died by entering kill zone '{col.gameObject.name}'.", col.gameObject);
        die();
        break;

      case "NoMorphForceField":
        Log.Print($"Entered anti morph force field '{col.gameObject.name}'.", col.gameObject);
        soundController.playSound("enterForceFieldSound");
        canMorphToCircle = false;
        canMorphToTriangle = false;
        canMorphToRectangle = false;
        morphIndicator.loadMorphIndicators(state, true, false, false, false);
        scriptedEvents.LoadEvent(2, "morph_force_field");
        break;

    }

  }

  private void OnTriggerExit2D(Collider2D col) {

    switch (col.gameObject.tag) {

      case "NoMorphForceField":
        Log.Print($"Left anti morph force field '{col.gameObject.name}'.", col.gameObject);
        soundController.playSound("exitForceFieldSound");
        loadLevelSettingsIntoPlayer();
        morphIndicator.loadMorphIndicators(state, canSelfDestruct, canMorphToCircle, canMorphToTriangle, canMorphToRectangle);
        break;

    }

  }

  private void OnTriggerStay2D(Collider2D col) {

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

      case "ZoomOutCameraCinematic":
        zoomedOutCameraCinematicTimer = 0.5f;
        break;

      case "ZoomDownCamera":
        zoomedDownCameraTimer = 0.5f;
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

  private void OnCollisionStay2D(Collision2D col) {

    switch (col.gameObject.tag) {

      case "MovingPlatform":
        // make the player move along the moving platform if standing on top
        transform.position += col.gameObject.GetComponent<MovingPlatform>().movePlayerBy;
        break;

    }

  }





  /*
   * ================
   * === EXTERNAL ===
   * ================
   */

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
      morphIndicator.loadMorphIndicators(state, false, false, false, false);

      // make sprite invisible while respawn
      Color color = textureObject.GetComponent<SpriteRenderer>().color;
      color.a = 0f;
      textureObject.GetComponent<SpriteRenderer>().color = color;

      resetDynamicRGB2D();

      // start playing death animation
      StartCoroutine(respawn());
    }

    IEnumerator respawn() {

      CameraShake.Instance.play(.2f, 13f, 8f);

      playDeathParticles();
      soundController.playSound("playerDeathSound");

      yield return new WaitForSeconds(1.5f);

      // teleport to spawn point
      Vector2 ps = LevelSettings.Instance.getVector2("playerSpawn");
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

}
