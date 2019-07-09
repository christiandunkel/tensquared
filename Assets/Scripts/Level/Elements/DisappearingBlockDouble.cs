using System.Collections;
using UnityEngine;

/*
 * powers the 'disappearing block double' prefab
 */

public class DisappearingBlockDouble : MonoBehaviour {

  /* 
   * ==================
   * === COMPONENTS ===
   * ==================
   */

  [SerializeField] private Animator squareTexture1 = null;
  [SerializeField] private Animator squareTexture2 = null;

  private SoundController soundController;





  /* 
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  [SerializeField] private float stayTime = 1.5f;
  [SerializeField] private float timeBothAreVisible = .8f;

  // distance of player to gameobject, in which sounds should be playing
  private float soundDist = 175f;





  /* 
   * ================
   * === INTERNAL ===
   * ================
   */

  private void Awake() {

    if (stayTime < .4f) {
      Log.Error($"Variable 'stayTime' ({stayTime}) is too small.");
    }

    if (timeBothAreVisible < .4f) {
      Log.Error($"Variable 'timeBothAreVisible' ({timeBothAreVisible}) is too small.");
    }

    // delay; start up scripted events once other scripts are ready
    StartCoroutine(delayedAwake());

    IEnumerator delayedAwake() {
      // wait for another loop if scripts aren't ready yet
      while (SoundController.Instance == null) {
        yield return new WaitForSeconds(.1f);
      }
      soundController = SoundController.Instance;
      startCycle();
      StopCoroutine(delayedAwake());
    }

  }

  private float distanceToPlayer() {

    /*
     * calculate the distance between a player and the block
     */

    // return high value if player isn't yet initialized
    if (PlayerManager.Instance == null) {
      return 20000f;
    }

    return Util.distanceOnAxisXY(PlayerManager.playerObject, gameObject);

  }

  private void startCycle() {

    /*
     * animate appearance and disappearance of the blocks
     */

    // start animation of disappearing blocks
    StopCoroutine(cycle());
    StartCoroutine(cycle());

    IEnumerator cycle() {

      squareTexture1.SetBool("Visible", true);
      if (distanceToPlayer() < soundDist) {
        soundController.playSound("disappearingBlockAppear");
      }

      yield return new WaitForSeconds(timeBothAreVisible);

      squareTexture2.SetBool("Visible", false);
      if (distanceToPlayer() < soundDist) {
        soundController.playSound("disappearingBlockDisappear");
      }

      yield return new WaitForSeconds(stayTime);

      squareTexture2.SetBool("Visible", true);
      if (distanceToPlayer() < soundDist) {
        soundController.playSound("disappearingBlockAppear");
      }

      yield return new WaitForSeconds(timeBothAreVisible);

      squareTexture1.SetBool("Visible", false);
      if (distanceToPlayer() < soundDist) {
        soundController.playSound("disappearingBlockDisappear");
      }

      yield return new WaitForSeconds(stayTime);
      startCycle();

    }
  }

}
