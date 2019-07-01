using System.Collections;
using UnityEngine;

/*
 * powers the 'disappearing block double' prefab
 */

public class DisappearingBlockDouble : MonoBehaviour {

  public Animator squareTexture1, squareTexture2;

  public float stayTime = 1.5f,
               timeBothAreVisible = .8f;

  private float soundDist = 175f; // distance of player to gameobject, in which radius' sounds are playing

  private SoundController soundController;

  private void Awake() {

    if (stayTime < .4f) {
      Debug.LogError("DisappearingBlockDouble: Given stayTime " + stayTime + " is too small.");
    }

    if (timeBothAreVisible < .4f) {
      Debug.LogError("DisappearingBlockDouble: Given timeBothAreVisible " + timeBothAreVisible + " is too small.");
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
    if (PlayerController.Instance == null) return 20000f;

    Vector2 v1 = new Vector2(transform.position.x, 
                             transform.position.y),
            v2 = new Vector2(PlayerController.playerObject.transform.position.x, 
                             PlayerController.playerObject.transform.position.y);

    Vector2 richtungsVektor = v1 - v2;

    // distance player to disappearing blocks
    float vektorBetrag = Mathf.Sqrt(Mathf.Pow(richtungsVektor.x, 2) + Mathf.Pow(richtungsVektor.y, 2));

    return vektorBetrag;

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
