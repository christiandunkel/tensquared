using System.Collections;
using UnityEngine;

/*
 * powers the 'laser turret' prefab
 */

public class LaserTurret : MonoBehaviour {

  /* 
   * ==================
   * === COMPONENTS ===
   * ==================
   */

  [SerializeField] private GameObject turret = null;
  [SerializeField] private GameObject bullet = null;

  // particles shown, when laser turret is disabled
  [SerializeField] private GameObject disabledParticles1 = null;
  private ParticleSystem disabledParticles1_PS;
  [SerializeField] private GameObject disabledParticles2 = null;
  private ParticleSystem disabledParticles2_PS;

  [SerializeField] private ParticleSystem shortParticles = null;

  private SoundController soundController;





  /* 
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  // distance of player to turret, in which turret becomes active
  [SerializeField] private float distanceToPlayerNeededForActivation = 160f;

  [SerializeField] private bool isEnabled = true;

  // shooting timer attributes
  [SerializeField] private GameObject bulletSpawnPosition = null;
  private bool inShootingPosition = true;
  [SerializeField] private float secondsBetweenShots = 3f;
  private float timeUntilNextShot = 0f;





  /* 
   * ================
   * === INTERNAL ===
   * ================
   */

  private void Awake() {

    // get particles systems
    disabledParticles1_PS = disabledParticles1.GetComponent<ParticleSystem>();
    disabledParticles2_PS = disabledParticles2.GetComponent<ParticleSystem>();

    if (secondsBetweenShots < 0.3f) {
      Log.Error($"Shot frequency of '{secondsBetweenShots}' seconds between shots is too low.", gameObject);
    }

    timeUntilNextShot = secondsBetweenShots;

    // delayed start up,
    // wait for other scripts to be ready
    StartCoroutine(delayedAwake());

    IEnumerator delayedAwake() {
      // wait for another loop if scripts aren't ready yet
      while (SoundController.Instance == null) {
        yield return new WaitForSeconds(.1f);
      }
      soundController = SoundController.Instance;
      StopCoroutine(delayedAwake());
    }

  }
  private void Update() {

    if (!isEnabled) {
      // play 'disabled turret' particles
      if (!disabledParticles1.activeSelf) {
        disabledParticles1.SetActive(true);
        disabledParticles2.SetActive(true);
        disabledParticles1_PS.Play();
        disabledParticles2_PS.Play();
      }
      return;
    }
    // don't play 'disabled turret' particles anymore
    else if (disabledParticles1.activeSelf) {
      disabledParticles1.SetActive(false);
      disabledParticles2.SetActive(false);
    }

    // test if player is close enough to turret for it to be active, otherwise return
    Vector2 playerPos = PlayerManager.playerObject.transform.position;
    if (
      Mathf.Pow((playerPos.x - transform.position.x), 2) +
      Mathf.Pow((playerPos.y - transform.position.y), 2)
      >= Mathf.Pow(distanceToPlayerNeededForActivation, 2)
    ) {
      timeUntilNextShot = secondsBetweenShots;
      return;
    }

    // calculate angle between turret and player position
    Vector3 dir = PlayerManager.playerObject.transform.localPosition - turret.transform.position;
    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

    // don't rotate through the turret through the holder texture
    if (angle > -152f && angle < -90f) {
      angle = -152f;
      inShootingPosition = false;
      timeUntilNextShot = secondsBetweenShots;
    }
    else if (angle < -28f && angle >= -90f) {
      angle = -28f;
      inShootingPosition = false;
      timeUntilNextShot = secondsBetweenShots;
    }
    else inShootingPosition = true;

    // make turret face the player
    turret.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    timeUntilNextShot -= Time.fixedDeltaTime;

    if (inShootingPosition) { 
      if (timeUntilNextShot <= 0f) {
        timeUntilNextShot = secondsBetweenShots;
        shootBullet();
      }
    }
    else {
      inShootingPosition = false;
    }

  }

  private void shootBullet() {

    /*
     * shoots a laser bullet in the current direction
     * in which the laser cannon is rotated
     */

    Instantiate(bullet, bulletSpawnPosition.transform.position, turret.transform.rotation);
    soundController.playSound("laserTurretShot");
    shortParticles.Play();

  }





  /* 
   * ================
   * === EXTERNAL ===
   * ================
   */

  public void enable() {

    /*
     * enables the laser turret
     */

    isEnabled = true;

    Log.Print($"Enabled laser turret {gameObject.name}.", gameObject);

  }

  public void disable() {

    /*
     * disables the laser turret
     */

    isEnabled = false;

    Log.Print($"Disabled laser turret {gameObject.name}.", gameObject);

  }

  public static void destroyBullet(GameObject bullet) {

    /*
     * destroys a defined 'laser bullet' object,
     * called by a given laser bullet when hitting something 
     * or flying for too long without hitting any collider
     */

    Destroy(bullet);

  }

}
