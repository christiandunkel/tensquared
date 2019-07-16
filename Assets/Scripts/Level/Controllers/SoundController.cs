using UnityEngine;

/*
 * all sound effects are controlled by this script
 */

public class SoundController : MonoBehaviour {

  /* 
   * =================
   * === SINGLETON ===
   * =================
   */

  public static SoundController Instance;

  private void Awake() {
    Instance = this;
  }





  /* 
   * =====================
   * === SOUND PLAYERS ===
   * =====================
   */

  // movement sound players
  [SerializeField] private AudioSource circleMovementSoundPlayer = null;
  [SerializeField] private AudioSource rectangleMovementSoundPlayer = null;
  [SerializeField] private AudioSource grassSoundPlayer = null;

  // other continuous sound players
  [SerializeField] private AudioSource movingPlatformSoundPlayer = null;
  [SerializeField] private AudioSource fireSoundPlayer = null;
  [SerializeField] private AudioSource forceFieldSoundPlayer = null;

  // one time sound players
  [SerializeField] private AudioSource characterSoundPlayer = null;
  [SerializeField] private AudioSource shortSoundPlayer = null;
  [SerializeField] private AudioSource cameraShakeSoundPlayer = null;
  [SerializeField] private AudioSource disappearingBlockAppearSoundPlayer = null;
  [SerializeField] private AudioSource disappearingBlockDisappearSoundPlayer = null;
  [SerializeField] private AudioSource bomberlingSoundPlayer = null;

  // speech sound players
  [SerializeField] private AudioSource speechSoundPlayer = null;




  public void stopSoundPlayer(string soundPlayer) {

    /*
     * stops a defined sound player from playing
     */

    switch (soundPlayer) {

      /* MOVEMENT SOUND PLAYERS */

      case "circleMovementSoundPlayer":
        circleMovementSoundPlayer.Stop();
        break;

      case "rectangleMovementSoundPlayer":
        rectangleMovementSoundPlayer.Stop();
        break;

      case "grassSoundPlayer":
        grassSoundPlayer.Stop();
        break;

      /* OTHER CONTINUOUS SOUND PLAYERS */

      case "movingPlatformSoundPlayer":
        movingPlatformSoundPlayer.Stop();
        break;

      case "fireSoundPlayer":
        fireSoundPlayer.Stop();
        break;

      case "forceFieldSoundPlayer":
        forceFieldSoundPlayer.Stop();
        break;

      /* ONE TIME SOUND PLAYERS */

      case "characterSoundPlayer":
        characterSoundPlayer.Stop();
        break;

      case "shortSoundPlayer":
        shortSoundPlayer.Stop();
        break;

      case "cameraShakeSoundPlayer":
        cameraShakeSoundPlayer.Stop();
        break;

      case "disappearingBlockAppearSoundPlayer":
        disappearingBlockAppearSoundPlayer.Stop();
        break;

      case "disappearingBlockDisappearSoundPlayer":
        disappearingBlockDisappearSoundPlayer.Stop();
        break;

      case "bomberlingSoundPlayer":
        bomberlingSoundPlayer.Stop();
        break;

      /* SPEECH */

      case "speechSoundPlayer":
        speechSoundPlayer.Stop();
        break;

      default:
        Log.Warn($"Sound player {soundPlayer} wasn't found.", this);
        break;

    }

  }





  /* 
   * ================================
   * === CONTINUOUS SOUND PLAYERS ===
   * ================================
   */

  // timers used to determine if a 
  // continuous sound should be playing or not
  private float movingTimer = 0f;
  private float movingThroughGrassTimer = 0f;
  private float preventMovementSoundsTimer = 0f;
  private float movingPlatformSoundsTimer = 0f;
  private float fireSoundTimer = 0f;
  private float forceFieldSoundTimer = 0f;

  public void setTimer(string name, float value) {

    /*
     * sets a timer value later used by handleContinuousSound()
     * in order to know if a continuous sound should be playing
     */

    switch (name) {

      case "movingTimer":
        movingTimer = value;
        break;

      case "movingThroughGrassTimer":
        movingThroughGrassTimer = value;
        break;

      case "preventMovementSoundsTimer":
        preventMovementSoundsTimer = value;
        break;

      case "movingPlatformSoundsTimer":
        movingPlatformSoundsTimer = value;
        break;

      case "fireSoundTimer":
        fireSoundTimer = value;
        break;

      case "forceFieldSoundTimer":
        forceFieldSoundTimer = value;
        break;

    }

  }

  public void handleContinuousSound(string state, bool movingX, bool grounded) {

    /*
     * handle looping sound players
     */

    movingTimer = movingX && grounded ? 0.2f : movingTimer;

    // general moving sounds
    if (movingTimer > 0f) {
      movingTimer -= Time.deltaTime;
      if (preventMovementSoundsTimer <= 0f) {
        if (movingX && grounded) {
          if (state == "Circle" && !circleMovementSoundPlayer.isPlaying) {
            if (!circleMovementSoundPlayer.isActiveAndEnabled) {
              circleMovementSoundPlayer.gameObject.SetActive(true);
            }
            circleMovementSoundPlayer.UnPause();
            rectangleMovementSoundPlayer.Pause();
          }
          else if (state == "Rectangle" && !rectangleMovementSoundPlayer.isPlaying) {
            if (!rectangleMovementSoundPlayer.isActiveAndEnabled) {
              rectangleMovementSoundPlayer.gameObject.SetActive(true);
            }
            circleMovementSoundPlayer.Pause();
            rectangleMovementSoundPlayer.UnPause();
          }
          else if (state == "Triangle") {
            circleMovementSoundPlayer.Pause();
            rectangleMovementSoundPlayer.Pause();
          }
        }
      }
      else {
        preventMovementSoundsTimer -= Time.deltaTime;
      }

    }
    else {
      if (circleMovementSoundPlayer.isPlaying) {
        circleMovementSoundPlayer.Pause();
      }
      if (rectangleMovementSoundPlayer) {
        rectangleMovementSoundPlayer.Pause();
      }
    }


    
    // sounds while moving through grass
    if (movingThroughGrassTimer > 0f) {
      movingThroughGrassTimer -= Time.deltaTime;

      // activate player if still inactive (on game start)
      if (!grassSoundPlayer.isActiveAndEnabled) {
        grassSoundPlayer.gameObject.SetActive(true);
      }
      // unpause, if not playing
      if (!grassSoundPlayer.isPlaying) {
        playSound("walkThroughGrassSound");
      }

    }
    else {
      grassSoundPlayer.Stop();
    }



    // sounds of moving platforms
    if (movingPlatformSoundsTimer > 0f) {
      movingPlatformSoundsTimer -= Time.deltaTime;

      // activate player if still inactive (on game start)
      if (!movingPlatformSoundPlayer.isActiveAndEnabled) {
        movingPlatformSoundPlayer.gameObject.SetActive(true);
      }
      // unpause, if not playing
      if (!movingPlatformSoundPlayer.isPlaying) {
        movingPlatformSoundPlayer.UnPause();
      }

    }
    else {
      movingPlatformSoundPlayer.Pause();
    }



    // sounds of fire
    if (fireSoundTimer > 0f) {
      fireSoundTimer -= Time.deltaTime;

      // activate player if still inactive (on game start)
      if (!fireSoundPlayer.isActiveAndEnabled) {
        fireSoundPlayer.gameObject.SetActive(true);
      }
      // unpause, if not playing
      if (!fireSoundPlayer.isPlaying) {
        fireSoundPlayer.UnPause();
      }

    }
    else {
      fireSoundPlayer.Pause();
    }



    // sounds of force field
    if (forceFieldSoundTimer > 0f) {
      forceFieldSoundTimer -= Time.deltaTime;

      // activate player if still inactive (on game start)
      if (!forceFieldSoundPlayer.isActiveAndEnabled) {
        forceFieldSoundPlayer.gameObject.SetActive(true);
      }
      // unpause, if not playing
      if (!forceFieldSoundPlayer.isPlaying) {
        forceFieldSoundPlayer.UnPause();
      }

    }
    else {
      forceFieldSoundPlayer.Pause();
    }

  }





  /* 
   * ===================
   * === AUDIO CLIPS ===
   * ===================
   */

  // character
  [SerializeField] private AudioClip morphSound = null;
  [SerializeField] private AudioClip landingCircleSound = null;
  [SerializeField] private AudioClip landingTriangleSound = null;
  [SerializeField] private AudioClip landingRectangleSound = null;
  [SerializeField] private AudioClip jumpingTriangleSound = null;
  [SerializeField] private AudioClip playerDeathSound = null;
  [SerializeField] private AudioClip walkThroughGrassSound = null;

  // disappearing block
  [SerializeField] private AudioClip disappearingBlockAppear = null;
  [SerializeField] private AudioClip disappearingBlockDisappear = null;

  // water
  [SerializeField] private AudioClip waterSplashSound = null;
  [SerializeField] private AudioClip waterSplashFloatingBlockSound = null;

  // toxic bubbles
  [SerializeField] private AudioClip bubbleIndicatorSound = null;
  [SerializeField] private AudioClip bubbleShootOutSound = null;

  // spawn point
  [SerializeField] private AudioClip activateSpawnpointSound = null;
  [SerializeField] private AudioClip respawnAtSpawnpointSound = null;

  // laser turret
  [SerializeField] private AudioClip laserBulletHit = null;
  [SerializeField] private AudioClip laserTurretShot = null;

  // bomberling
  [SerializeField] private AudioClip bomberlingScreamSound = null;
  [SerializeField] private AudioClip bomberlingShortScreamSound = null;
  [SerializeField] private AudioClip bomberlingExplodeSound = null;

  // force field
  [SerializeField] private AudioClip enterForceFieldSound = null;
  [SerializeField] private AudioClip exitForceFieldSound = null;

  // camera shake
  [SerializeField] private AudioClip earthquake_1_5_secs = null;
  [SerializeField] private AudioClip earthquake_2_secs = null;
  [SerializeField] private AudioClip earthquake_2_5_secs_loud = null;
  [SerializeField] private AudioClip earthquake_3_secs = null;

  // UI
  [SerializeField] private AudioClip levelCompleteSound = null;
  [SerializeField] private AudioClip levelCompleteSoundEvil = null;

  // robot
  [SerializeField] private AudioClip robotRepairSound = null;
  [SerializeField] private AudioClip robotScreamSound = null;
  [SerializeField] private AudioClip robotElectricDefect = null;
  [SerializeField] private AudioClip robotPartsFallOff = null;

  // other
  [SerializeField] private AudioClip breakingBlockSound = null;
  [SerializeField] private AudioClip pistonPushSound = null;
  [SerializeField] private AudioClip closingDoorSound = null;

  public float playSound(string soundName) {

    /*
     * plays a sound with the related sound player
     * returns the audio clip length as float
     */

    AudioClip c;

    switch (soundName) {

      /*
       * CHARACTER
       */

      case "morphSound":
        c = morphSound;
        characterSoundPlayer.PlayOneShot(c);
        return c.length;

      case "landingCircleSound":
        c = landingCircleSound;
        characterSoundPlayer.PlayOneShot(c);
        return c.length;

      case "landingTriangleSound":
        c = landingTriangleSound;
        characterSoundPlayer.PlayOneShot(c);
        return c.length;

      case "landingRectangleSound":
        c = landingRectangleSound;
        characterSoundPlayer.PlayOneShot(c);
        return c.length;

      case "jumpingTriangleSound":
        c = jumpingTriangleSound;
        characterSoundPlayer.PlayOneShot(c);
        return c.length;

      case "playerDeathSound":
        c = playerDeathSound;
        characterSoundPlayer.PlayOneShot(c);
        return c.length;

      case "walkThroughGrassSound":
        c = walkThroughGrassSound;
        grassSoundPlayer.PlayOneShot(c);
        return c.length;

      /*
       * DISAPPEARING BLOCK
       */

      case "disappearingBlockAppear":
        c = disappearingBlockAppear;
        disappearingBlockAppearSoundPlayer.PlayOneShot(c);
        return c.length;

      case "disappearingBlockDisappear":
        c = disappearingBlockDisappear;
        disappearingBlockDisappearSoundPlayer.PlayOneShot(c);
        return c.length;

      /*
       * WATER
       */

      case "waterSplashSound":
        c = waterSplashSound;
        shortSoundPlayer.PlayOneShot(c);
        return c.length;

      case "waterSplashFloatingBlockSound":
        c = waterSplashFloatingBlockSound;
        shortSoundPlayer.PlayOneShot(c);
        return c.length;

      /*
       * TOXIC BUBBLES
       */

      case "bubbleIndicatorSound":
        c = bubbleIndicatorSound;
        shortSoundPlayer.PlayOneShot(c);
        return c.length;

      case "bubbleShootOutSound":
        c = bubbleShootOutSound;
        shortSoundPlayer.PlayOneShot(c);
        return c.length;

      /*
       * SPAWN POINT
       */

      case "activateSpawnpointSound":
        c = activateSpawnpointSound;
        shortSoundPlayer.PlayOneShot(c);
        return c.length;

      case "respawnAtSpawnpointSound":
        c = respawnAtSpawnpointSound;
        shortSoundPlayer.PlayOneShot(c);
        return c.length;

      /*
       * LASER TURRET
       */

      case "laserBulletHit":
        c = laserBulletHit;
        shortSoundPlayer.PlayOneShot(c);
        return c.length;

      case "laserTurretShot":
        c = laserTurretShot;
        shortSoundPlayer.PlayOneShot(c);
        return c.length;

      /*
       * BOMBERLING
       */

      case "bomberlingScreamSound":
        c = bomberlingScreamSound;
        bomberlingSoundPlayer.PlayOneShot(c);
        return c.length;

      case "bomberlingShortScreamSound":
        c = bomberlingShortScreamSound;
        bomberlingSoundPlayer.PlayOneShot(c);
        return c.length;

      case "bomberlingExplodeSound":
        c = bomberlingExplodeSound;
        bomberlingSoundPlayer.PlayOneShot(c);
        return c.length;

      /*
       * FORCE FIELD
       */

      case "enterForceFieldSound":
        c = enterForceFieldSound;
        shortSoundPlayer.PlayOneShot(c);
        return c.length;

      case "exitForceFieldSound":
        c = exitForceFieldSound;
        shortSoundPlayer.PlayOneShot(c);
        return c.length;

      /*
       * CAMERA SHAKE / EARTHQUAKE
       */
      
      case "earthquake_1_5_secs":
        c = earthquake_1_5_secs;
        cameraShakeSoundPlayer.PlayOneShot(c);
        return c.length;

      case "earthquake_2_secs":
        c = earthquake_2_secs;
        cameraShakeSoundPlayer.PlayOneShot(c);
        return c.length;

      case "earthquake_2_5_secs_loud":
        c = earthquake_2_5_secs_loud;
        cameraShakeSoundPlayer.PlayOneShot(c);
        return c.length;

      case "earthquake_3_secs":
        c = earthquake_3_secs;
        cameraShakeSoundPlayer.PlayOneShot(c);
        return c.length;

      /*
       * UI
       */

      case "levelCompleteSound":
        c = levelCompleteSound;
        shortSoundPlayer.PlayOneShot(c);
        return c.length;

      case "levelCompleteSoundEvil":
        c = levelCompleteSoundEvil;
        shortSoundPlayer.PlayOneShot(c);
        return c.length;

      /*
       * ROBOT
       */

      case "robotRepairSound":
        c = robotRepairSound;
        shortSoundPlayer.PlayOneShot(c);
        return c.length;

      case "robotScreamSound":
        c = robotScreamSound;
        speechSoundPlayer.PlayOneShot(c);
        return c.length;

      case "robotElectricDefect":
        c = robotElectricDefect;
        speechSoundPlayer.PlayOneShot(c);
        return c.length;

      case "robotPartsFallOff":
        c = robotPartsFallOff;
        speechSoundPlayer.PlayOneShot(c);
        return c.length;

      /*
       * OTHER
       */

      case "breakingBlockSound":
        c = breakingBlockSound;
        shortSoundPlayer.PlayOneShot(c);
        return c.length;

      case "pistonPushSound":
        c = pistonPushSound;
        shortSoundPlayer.PlayOneShot(c);
        return c.length;

      case "closingDoorSound":
        c = closingDoorSound;
        shortSoundPlayer.PlayOneShot(c);
        return c.length;

    }

    Log.Warn($"Sound '{soundName}' wasn't found.", this);
    return 0f;

  }

}
