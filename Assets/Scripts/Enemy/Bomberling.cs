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
  private Rigidbody2D rb2d;
  private BoxCollider2D boxCollider;
  private float boxColliderSizeHalf;

  // settings and attributes
  public float radiusOfActivation = 150f;
  public float movementSpeed = 2.5f;
  private bool isDead = false;
  private bool isRunning = false;
  private bool runningLeftwards = false;

  void Awake() {

    // get player object
    playerObject = PlayerManager.Instance.gameObject;

    // get components
    rb2d = GetComponent<Rigidbody2D>();
    boxCollider = GetComponent<BoxCollider2D>();
    float boxColliderOffset = 3f;
    boxColliderSizeHalf = boxCollider.size.x / 2f + boxColliderOffset;

  }

  void Update() {

    if (isDead || isRunning) return;

    // activate the bomberling if the player is close enough
    // and player is roughly on the same height (y value)
    if (distanceToPlayer() <= radiusOfActivation &&
        Mathf.Abs(playerObject.gameObject.transform.position.y - transform.position.y) < 15f) {
      startRunning();
    }

  }

  float distanceToPlayer() {

    /*
     * calculates the distance between this instance of a bomberling and the player
     */

    return ((Vector2)transform.position - (Vector2)playerObject.transform.position).magnitude;

  }

  void startRunning() {

    /*
     * Bomberling starts running into the direction where the player is
     */

    textureObject.GetComponent<Animator>().SetTrigger("StartRunning");
    // TODO: running Animation in Unity

    // TODO: play short scream sound

    isRunning = true;
    runningLeftwards = transform.position.x > playerObject.transform.position.x ? true : false;

    StartCoroutine(run());

    IEnumerator run() {

      /*
       * move bomberling until he collides with something and dies
       */

      while (!isDead && isRunning) {
        yield return new WaitForSeconds(.05f);
        Vector2 moveBy = Vector3.zero;
        moveBy.x = movementSpeed;
        if (runningLeftwards) moveBy.x *= -1;
        rb2d.velocity += moveBy;
      }

      StopCoroutine(run());

    }

  }

  void selfDestruct() {

    /*
     * manages the process of the bomberling exploding and dying
     */

    // don't trigger this twice
    if (isDead) return;

    isDead = true;
    isRunning = false;
    StartCoroutine(deathProcess());

    IEnumerator deathProcess() {

      // stop walking animation and make sprite invisible
      textureObject.GetComponent<Animator>().SetTrigger("StopRunning");
      rb2d.velocity = Vector2.zero;
      rb2d.freezeRotation = true;
      rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
      CameraShake.Instance.Play(.45f, 18f, 18f);
      dyingParticles.SetActive(true);
      dyingParticles.GetComponent<ParticleSystem>().Play();
      yield return new WaitForSeconds(.6f);
      CameraShake.Instance.Play(.2f, 50f, 50f);
      deathParticles.SetActive(true);
      deathParticles.GetComponent<ParticleSystem>().Play();
      yield return new WaitForSeconds(.1f);
      textureObject.GetComponent<SpriteRenderer>().sprite = null;

      StopCoroutine(deathProcess());

    }

  }

  void OnCollisionEnter2D(Collision2D col) {

    if (isDead) return;
    
    // kill player on touch
    if (col.gameObject.tag == "Player") {
      PlayerManager.Instance.die();
      // destroy itself
      selfDestruct();
      return;
    }

  }

  void OnCollisionStay2D(Collision2D col) {

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
