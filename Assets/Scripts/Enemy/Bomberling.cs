using System.Collections;
using UnityEngine;

/*
 * powers the 'Bomberling'-enemy prefab
 */

public class Bomberling : MonoBehaviour {

  // internal objects
  private GameObject textureObject;

  // external objects
  private GameObject playerObject;

  // settings and attributes
  public float radiusOfActivation = 150f;
  public float movementSpeed = 4f;
  private bool isDead = false;
  private bool isRunning = false;
  private bool runningLeftwards = false;

  void Awake() {

    // get player object
    playerObject = PlayerManager.Instance.gameObject;
  
    // get child elements
    foreach (Transform child in gameObject.transform) {
      switch (child.gameObject.name) {
        case "Texture":
          textureObject = child.gameObject;
          break;
      }
    }

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

  }

  IEnumerator run() {

    /*
     * move bomberling until he collides with something and dies
     */

    while (!isDead && isRunning) {
      yield return new WaitForSeconds(.05f);
      Vector3 moveBy = Vector3.zero;
      moveBy.x = movementSpeed;
      if (runningLeftwards) moveBy.x *= -1;
      transform.position += moveBy;
    }

    StopCoroutine(run());

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
      yield return new WaitForSeconds(.2f);
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
    }

    // don't do anything, if collision is from non-player collider
    // that touches the player from below, only activate on hitting walls
    if (col.gameObject.transform.position.y > transform.position.y) {

      // TODO: proper detection for colliders to the left and right

      selfDestruct();

    }

  }

}
