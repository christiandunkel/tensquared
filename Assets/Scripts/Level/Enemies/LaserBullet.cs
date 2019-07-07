using System.Collections;
using UnityEngine;

/*
 * powers the 'laser bullet' prefab,
 * instantiated by 'laser turret' prefab when shooting
 */

[RequireComponent(typeof(Rigidbody2D))]
public class LaserBullet : MonoBehaviour {

  /* 
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  private float secondsBeforeVanishing = 3f;
  private float speed = 260f;
  private bool hasHit = false;
  private Rigidbody2D rb2d;

  // particle effects
  public GameObject explodeParticles;





  /* 
   * ================
   * === INTERNAL ===
   * ================
   */

  private void Start() {
    rb2d = GetComponent<Rigidbody2D>();
    rb2d.velocity = transform.right * speed;
  }

  private void Update() {
    // timer before the bullet vanishes, 
    // if it doesn't hit any collider
    secondsBeforeVanishing -= Time.fixedDeltaTime;
  }

  private void OnCollisionEnter2D(Collision2D col) {
    
    if (!hasHit || secondsBeforeVanishing <= 0f) {
      hasHit = true;

      // if the collider hit by the bullet was from the player, kill them
      if (col.gameObject.tag == "Player") {
        PlayerManager.Instance.die();
      }

      StartCoroutine(destroyBullet());
    }

  }

  private IEnumerator destroyBullet() {

    /*
     * plays a sound and particle effect on hitting something,
     * as well as destroys the bullet
     */

    rb2d.velocity = Vector2.zero;

    GetComponent<SpriteRenderer>().sprite = null;
    GetComponent<CapsuleCollider2D>().isTrigger = true;

    SoundController.Instance.playSound("laserBulletHit");

    explodeParticles.SetActive(true);
    explodeParticles.GetComponent<ParticleSystem>().Play(true);

    yield return new WaitForSeconds(.5f);

    // remove the bullet
    LaserTurret.destroyBullet(gameObject);

  }

}
