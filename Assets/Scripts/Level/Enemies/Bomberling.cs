using System.Collections;
using UnityEngine;

/*
 * powers the 'Bomberling'-enemy prefab,
 * which is a small robot detecting nearby players,
 * running towards them and exploding
 */

public class Bomberling : MonoBehaviour {

  /* 
   * ==================
   * === COMPONENTS ===
   * ==================
   */

  // external objects
  private GameObject playerObject;

  // internal objects
  [SerializeField] private GameObject textureObject = null;
  [SerializeField] private GameObject dyingParticles = null;
  [SerializeField] private GameObject deathParticles = null;

  // components
  private SoundController soundController;
  private Rigidbody2D rb2d;
  private BoxCollider2D boxCollider;
  private float boxColliderSizeHalf;





  /* 
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  // settings and attributes
  [SerializeField] private float radiusOfActivation = 120f;
  [SerializeField] private float movementSpeed = 25f;
  [SerializeField] private float movementSpeedIncrease = 1.5f;
  private bool isDead = false;
  private bool isRunning = false;
  private bool runningLeftwards = false;





  /* 
   * ================
   * === INTERNAL ===
   * ================
   */

  private void Awake() {

    // get player object
    playerObject = PlayerManager.Instance.gameObject;

    // get components
    soundController = SoundController.Instance;
    rb2d = GetComponent<Rigidbody2D>();
    boxCollider = GetComponent<BoxCollider2D>();
    float boxColliderOffset = 3f;
    boxColliderSizeHalf = boxCollider.size.x / 2f + boxColliderOffset;

    if (radiusOfActivation < 0f) {
      Log.Warn($"Radius of activation is '{radiusOfActivation}', but can not be negative.", gameObject);
    }

    if (movementSpeed < 0f) {
      Log.Warn($"Movement speed is '{movementSpeed}', but can not be negative.", gameObject);
    }

    if (movementSpeedIncrease < 0f) {
      Log.Warn($"Movement speed increase is '{movementSpeedIncrease}', but can not be negative.", gameObject);
    }

  }

  private void Update() {

    if (isDead || isRunning) {
      return;
    }

    // sometimes bomberling slowly slide to one side without activation,
    // if placed a little above the ground on game start
    if (!isRunning) {
      rb2d.velocity = Vector2.zero;
    }

    // activate the bomberling if the player is close enough
    // and player is roughly on the same height (y value)
    if (
      distanceToPlayer() <= radiusOfActivation &&
      Util.distanceOnAxisY(playerObject, gameObject) < 15f
    ) {
      startRunning();
    }

  }

  private void startRunning() {

    /*
     * Bomberling starts running into the direction where the player is
     */
    
    if (runningLeftwards) {
      textureObject.GetComponent<Animator>().SetTrigger("StandUpLeft");
    }
    else {
      textureObject.GetComponent<Animator>().SetTrigger("StandUpRight");
    }

    soundController.playSound("bomberlingScreamSound");

    isRunning = true;
    runningLeftwards = transform.position.x > playerObject.transform.position.x ? true : false;

    StartCoroutine(run());
    StartCoroutine(playSound());

    IEnumerator run() {

      /*
       * move bomberling until he collides with something and dies
       */
      yield return new WaitForSeconds(.5f);

      long counter = 0;
      while (!isDead && isRunning) {
        yield return new WaitForSeconds(.05f);

        Vector2 moveBy = Vector2.zero;

        // start movement in first loop
        if (counter == 0) {
          moveBy.x = movementSpeed;
          if (runningLeftwards) {
            moveBy.x *= -1;
          }
          rb2d.velocity = moveBy;
        }
        // additive movement in every loop
        else {
          moveBy.x += movementSpeedIncrease;
          if (runningLeftwards) {
            moveBy.x *= -1;
          }
          rb2d.velocity += moveBy;
        }
        counter++;
      }

      StopCoroutine(run());

    }

    IEnumerator playSound() {

      /*
       * play continuous bomberling scream sounds
       */

      yield return new WaitForSeconds(.5f);

      while (!isDead && isRunning) {

        if (distanceToPlayer() < 150f) {
          soundController.playSound("bomberlingShortScreamSound");
        }
        
        yield return new WaitForSeconds(.5f);
      }

      StopCoroutine(playSound());

    }

  }

  private float distanceToPlayer() {

    /*
     * returns the distance of the turret and the player on the x and y axis
     */

    return Util.distanceOnAxisXY(playerObject, gameObject);

  }

  private void selfDestruct() {

    /*
     * manages the process of the bomberling exploding and dying
     */

    // don't trigger this twice
    if (isDead) {
      return;
    }

    isDead = true;
    isRunning = false;
    StartCoroutine(deathProcess());

    IEnumerator deathProcess() {

      ScriptedEventsManager.Instance.LoadEvent(4, "bomberling_explosion");

      Log.Print($"Bomberling {gameObject.name} self-destructed.", this);

      // stop walking animation and make sprite invisible
      rb2d.velocity = Vector2.zero;
      rb2d.freezeRotation = true;
      rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
      CameraShake.Instance.play(.45f, 18f, 18f);
      dyingParticles.SetActive(true);
      dyingParticles.GetComponent<ParticleSystem>().Play();
      yield return new WaitForSeconds(.3f);

      if (distanceToPlayer() < 150f) {
        soundController.playSound("bomberlingExplodeSound");
        CameraShake.Instance.play(.2f, 50f, 50f);
      }

      textureObject.GetComponent<Animator>().SetTrigger("Hidden");
      deathParticles.SetActive(true);
      deathParticles.GetComponent<ParticleSystem>().Play();
      yield return new WaitForSeconds(.1f);
      textureObject.GetComponent<SpriteRenderer>().sprite = null;

      // disable all collision components of bomberling
      GetComponent<BoxCollider2D>().enabled = false;
      Destroy(GetComponent<Rigidbody2D>());

      // kill player if in radius of explosion
      if (distanceToPlayer() < 30f) {
        PlayerManager.Instance.die();
      }

      // make texture disappear, even if still in animation cycle
      textureObject.SetActive(false);

      StopCoroutine(deathProcess());

    }

  }

  private void OnCollisionEnter2D(Collision2D col) {

    if (isDead) {
      return;
    }
    
    // kill player on touch
    if (col.gameObject.tag == "Player") {

      // kill player
      PlayerManager.Instance.die();

      // destroy itself
      selfDestruct();
      return;

    }

  }

  private void OnCollisionStay2D(Collision2D col) {

    if (isDead) return;

    // get right and left border point of collider
    Vector2 leftColliderPos = transform.position;
    leftColliderPos.x -= boxColliderSizeHalf;
    Vector2 rightColliderPos = transform.position;
    rightColliderPos.x += boxColliderSizeHalf;

    // check if bomberling ran into (inside) a collider
    if (col.collider.bounds.Contains(leftColliderPos) ||
        col.collider.bounds.Contains(rightColliderPos)) {

      selfDestruct();

    }

  }

}
