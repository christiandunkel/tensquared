using System.Collections;
using UnityEngine;

/*
 * powers the 'Bomberling'-enemy prefab
 */

public class Bomberling : MonoBehaviour {

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

  // settings and attributes
  [SerializeField] private float radiusOfActivation = 120f;
  [SerializeField] private float movementSpeed = 25f;
  [SerializeField] private float movementSpeedIncrease = 1.5f;
  private bool isDead = false;
  private bool isRunning = false;
  private bool runningLeftwards = false;

  private void Awake() {

    // get player object
    playerObject = PlayerManager.Instance.gameObject;

    // get components
    soundController = SoundController.Instance;
    rb2d = GetComponent<Rigidbody2D>();
    boxCollider = GetComponent<BoxCollider2D>();
    float boxColliderOffset = 3f;
    boxColliderSizeHalf = boxCollider.size.x / 2f + boxColliderOffset;

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
    if (distanceToPlayer() <= radiusOfActivation &&
        Mathf.Abs(playerObject.gameObject.transform.position.y - gameObject.transform.position.y) < 15f) {

      startRunning();

    }

  }

  private float distanceToPlayer() {

    /*
     * calculates the distance between this instance of a bomberling and the player
     */

    float distance = ((Vector2)gameObject.transform.position - (Vector2)playerObject.transform.position).magnitude;

    return distance;

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

      Log.Print($"Bomberling {gameObject.name} self-destructed.", gameObject);

      // stop walking animation and make sprite invisible
      rb2d.velocity = Vector2.zero;
      rb2d.freezeRotation = true;
      rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
      CameraShake.Instance.play(.45f, 18f, 18f);
      dyingParticles.SetActive(true);
      dyingParticles.GetComponent<ParticleSystem>().Play();
      yield return new WaitForSeconds(.6f);

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

      StopCoroutine(deathProcess());

    }

  }

  private void OnCollisionEnter2D(Collision2D col) {

    if (isDead) return;
    
    // kill player on touch
    if (col.gameObject.tag == "Player") {
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
