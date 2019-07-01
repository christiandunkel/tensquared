using System.Collections;
using UnityEngine;

/*
 * powers the 'disappearing block single' prefab
 */

public class DisappearingBlockSingle : MonoBehaviour {

  public Animator squareTexture;
  private SoundController soundController;

  public float stayTime = 1.5f,
               hiddenTime = 1f;

  private float soundDist = 175f; // distance of player to gameobject, in which radius' sounds are playing

  private void Awake() {

    if (stayTime < .4f) {
      Debug.LogError("DisappearingBlockSingle: Given stayTime " + stayTime + " is too small.");
    }

    if (hiddenTime < .4f) {
      Debug.LogError("DisappearingBlockSingle: Given hiddenTime " + hiddenTime + " is too small.");
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

    // start animation of disappearing blocks
    StopCoroutine(cycle());
    StartCoroutine(cycle());

    IEnumerator cycle() {

      squareTexture.SetBool("Visible", true);
      if (distanceToPlayer() < soundDist) {
        soundController.playSound("disappearingBlockAppear");
      }

      yield return new WaitForSeconds(stayTime);

      squareTexture.SetBool("Visible", false);
      if (distanceToPlayer() < soundDist) {
        soundController.playSound("disappearingBlockDisappear");
      }

      yield return new WaitForSeconds(hiddenTime);

      startCycle();

    }
  }

}
