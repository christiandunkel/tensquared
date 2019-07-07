using System.Collections;
using UnityEngine;

/*
 * powers the 'piston' prefab
 */

public class Piston : MonoBehaviour {

  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  // components
  private Animator animator;
  private static SoundController soundController;

  // attributes
  private bool isMoving = false;
  private const float delayBeforePush = 0.15f;





  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  private void Start() {

    soundController = SoundController.Instance;
    animator = GetComponent<Animator>();

  }

  private void OnTriggerEnter2D(Collider2D col) {

    if (col.gameObject.tag == "Player") {
      Log.Print($"Player stepped on piston '{gameObject.name}'.", this);
      activate();
    }

  }

  private void activate() {

    /*
     * activates the current piston instance
     */

    if (isMoving) {
      return;
    }

    isMoving = true;

    StartCoroutine(openPiston());

    IEnumerator openPiston() {

      // move the piston top up after delay
      yield return new WaitForSeconds(delayBeforePush);
      soundController.playSound("pistonPushSound");
      animator.SetTrigger("PushUp");
      PlayerManager.Instance.setValue("steppedOnPiston", true);

      // reset piston after animation is over
      yield return new WaitForSeconds(1f);
      animator.ResetTrigger("PushUp");
      isMoving = false;

    }

  }





  /*
   * ================
   * === EXTERNAL ===
   * ================
   */

  public static void activatePiston(GameObject piston) {

    /*
     * activates the given piston object
     */

    piston.GetComponent<Piston>().activate();

  }

}
