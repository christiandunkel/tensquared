using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * all sound effects are controlled by this script
 */

public class SoundController : MonoBehaviour {

  // singleton
  public static SoundController Instance;

  private void Awake() {
    Instance = this;
  }





  /* 
   * ======================================
   * === GENERAL SOUND PLAYER MANAGMENT ===
   * ======================================
   */

  // movement sound players
  [SerializeField] private AudioSource circleMovementSoundPlayer;
  [SerializeField] private AudioSource rectangleMovementSoundPlayer;
  [SerializeField] private AudioSource grassSoundPlayer;
  // other continuous sound players
  [SerializeField] private AudioSource movingPlatformSoundPlayer;
  [SerializeField] private AudioSource fireSoundPlayer;
  [SerializeField] private AudioSource forceFieldSoundPlayer;
  // one time sound players
  [SerializeField] private AudioSource characterSoundPlayer;
  [SerializeField] private AudioSource shortSoundPlayer;
  [SerializeField] private AudioSource cameraShakeSoundPlayer;
  [SerializeField] private AudioSource disappearingBlockAppearSoundPlayer;
  [SerializeField] private AudioSource disappearingBlockDisappearSoundPlayer;

  public void StopSoundPlayer(string soundPlayer) {

    /*
     * stops a defined sound player from playing
     */

    switch (soundPlayer) {

      /*
       * MOVEMENT SOUND PLAYERS
       */

      case "circleMovementSoundPlayer":
        circleMovementSoundPlayer.Stop(); return;
      case "rectangleMovementSoundPlayer":
        rectangleMovementSoundPlayer.Stop(); return;
      case "grassSoundPlayer":
        grassSoundPlayer.Stop(); return;

      /*
       * OTHER CONTINUOUS SOUND PLAYERS
       */

      case "movingPlatformSoundPlayer":
        movingPlatformSoundPlayer.Stop(); return;
      case "fireSoundPlayer":
        fireSoundPlayer.Stop(); return;
      case "forceFieldSoundPlayer":
        forceFieldSoundPlayer.Stop(); return;

      /*
       * ONE TIME SOUND PLAYERS
       */

      case "characterSoundPlayer":
        characterSoundPlayer.Stop(); return;
      case "shortSoundPlayer":
        shortSoundPlayer.Stop(); return;
      case "cameraShakeSoundPlayer":
        cameraShakeSoundPlayer.Stop(); return;
      case "disappearingBlockAppearSoundPlayer":
        disappearingBlockAppearSoundPlayer.Stop(); return;
      case "disappearingBlockDisappearSoundPlayer":
        disappearingBlockDisappearSoundPlayer.Stop(); return;

    }

    Debug.LogWarning("PlayerController: Sound player " + soundPlayer + " wasn't found.");

  }





  /* 
   * ================================
   * === CONTINUOUS SOUND PLAYERS ===
   * ================================
   */

  private float movingTimer = 0f,
                movingThroughGrassTimer = 0f,
                preventMovementSoundsTimer = 0f, 
                movingPlatformSoundsTimer = 0f, 
                fireSoundTimer = 0f, 
                forceFieldSoundTimer = 0f;

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
     * handle continuous sounds
     */

    movingTimer = movingX && grounded ? 0.2f : movingTimer;




    // general moving sounds
    if (movingTimer > 0f) {
      movingTimer -= Time.fixedDeltaTime;
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
        preventMovementSoundsTimer -= Time.fixedDeltaTime;
      }

    }
    else if (state == "Circle") {
      circleMovementSoundPlayer.Pause();
    }
    else {
      rectangleMovementSoundPlayer.Pause();
    }


    
    // sounds while moving through grass
    if (movingThroughGrassTimer > 0f) {
      movingThroughGrassTimer -= Time.fixedDeltaTime;

      // activate player if still inactive (on game start)
      if (!grassSoundPlayer.isActiveAndEnabled) {
        grassSoundPlayer.gameObject.SetActive(true);
      }
      // unpause, if not playing
      if (!grassSoundPlayer.isPlaying) {
        PlaySound("walkThroughGrassSound");
      }

    }
    else {
      grassSoundPlayer.Stop();
    }



    // sounds of moving platforms
    if (movingPlatformSoundsTimer > 0f) {
      movingPlatformSoundsTimer -= Time.fixedDeltaTime;

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
      fireSoundTimer -= Time.fixedDeltaTime;

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
      forceFieldSoundTimer -= Time.fixedDeltaTime;

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
   * =============================
   * === AUDIO CLIP MANAGEMENT ===
   * =============================
   */

  // character
  [SerializeField] private AudioClip morphSound;
  [SerializeField] private AudioClip landingCircleSound;
  [SerializeField] private AudioClip landingTriangleSound;
  [SerializeField] private AudioClip landingRectangleSound;
  [SerializeField] private AudioClip jumpingTriangleSound;
  [SerializeField] private AudioClip playerDeathSound;
  [SerializeField] private AudioClip walkThroughGrassSound;
  // disappearing block
  [SerializeField] private AudioClip disappearingBlockAppear;
  [SerializeField] private AudioClip disappearingBlockDisappear;
  // water
  [SerializeField] private AudioClip waterSplashSound;
  [SerializeField] private AudioClip waterSplashFloatingBlockSound;
  // spawn point
  [SerializeField] private AudioClip activateSpawnpointSound;
  [SerializeField] private AudioClip respawnAtSpawnpointSound;
  // laser turret
  [SerializeField] private AudioClip laserBulletHit;
  [SerializeField] private AudioClip laserTurretShot;
  // force field
  [SerializeField] private AudioClip enterForceFieldSound;
  [SerializeField] private AudioClip exitForceFieldSound;
  // camera shake
  [SerializeField] private AudioClip earthquake_1_5_secs;
  [SerializeField] private AudioClip earthquake_2_secs;
  [SerializeField] private AudioClip earthquake_2_5_secs_loud;
  [SerializeField] private AudioClip earthquake_3_secs;
  // UI
  [SerializeField] private AudioClip levelCompleteSound;
  // other
  [SerializeField] private AudioClip robotRepairSound;
  [SerializeField] private AudioClip breakingBlockSound;
  [SerializeField] private AudioClip pistonPushSound;

  public float PlaySound(string soundName) {

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

      /*
       * OTHER
       */

      case "robotRepairSound":
        c = robotRepairSound;
        shortSoundPlayer.PlayOneShot(c);
        return c.length;

      case "breakingBlockSound":
        c = breakingBlockSound;
        shortSoundPlayer.PlayOneShot(c);
        return c.length;

      case "pistonPushSound":
        c = pistonPushSound;
        shortSoundPlayer.PlayOneShot(c);
        return c.length;

    }

    Debug.LogWarning("PlayerController: Sound " + soundName + " wasn't found.");
    return 0f;

  }

}
