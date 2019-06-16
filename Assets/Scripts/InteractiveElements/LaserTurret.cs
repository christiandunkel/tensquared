using UnityEngine;

/*
 * powers the 'laser turret' prefab
 */

public class LaserTurret : MonoBehaviour {

  public GameObject turret, bullet;
  public ParticleSystem shortParticles;

  // distance of player to turret, in which turret becomes active
  public float distanceToPlayerNeededForActivation = 120f;

  // shooting timer attributes
  public GameObject bulletSpawnPosition;
  private bool inShootingPosition = true;
  public float secondsBetweenShots = 3f;
  private float timeUntilNextShot = 0f;

  void Awake() {
    
    if (secondsBetweenShots < 0.3f) {
      Debug.LogError("LaserTurret: Shot frequency of " + secondsBetweenShots  + 
                     " seconds between shots is too low.");
    }

    timeUntilNextShot = secondsBetweenShots;

  }

  private void Update() {

    // test if player is close enough to turret for it to be active, otherwise return
    Vector2 playerPos = PlayerController.playerObject.transform.position;
    if (
      Mathf.Pow((playerPos.x - transform.position.x), 2) +
      Mathf.Pow((playerPos.y - transform.position.y), 2)
      >= Mathf.Pow(distanceToPlayerNeededForActivation, 2)
    ) {
      timeUntilNextShot = secondsBetweenShots;
      return;
    }

    // calculate angle between turret and player position
    Vector3 dir = PlayerController.playerObject.transform.localPosition - turret.transform.position;
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
    Instantiate(bullet, bulletSpawnPosition.transform.position, turret.transform.rotation);
    PlayerController.Instance.PlaySound("laserTurretShot");
    shortParticles.Play();
  }

  // external method to destroy a bullet object,
  // (using it inside laserBullet.cs has some problematic quirks)
  public static void destroyBullet(GameObject bullet) {
    Destroy(bullet);
  }

}
