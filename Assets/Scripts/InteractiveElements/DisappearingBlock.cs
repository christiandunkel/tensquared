using System.Collections;
using UnityEngine;

/*
 * powers the 'disappearing block' prefab
 */

public class DisappearingBlock : MonoBehaviour {

  public Animator squareTexture1, squareTexture2;

  public float stayTime = 1.5f,
               timeBothAreVisible = .8f;

  private float soundDist = 25f; // distance of player to gameobject, in which radius' sounds are playing

  private void Awake() {
    startCycle();
  }

  private void startCycle() {
    StopCoroutine(cycle());
    StartCoroutine(cycle());
  }

  private float distanceToPlayer() {

    // return high value if player isn't yet initialized
    if (PlayerController.Instance == null) return 2000f;

    Vector2 v1 = new Vector2(transform.position.x, 
                             transform.position.y),
            v2 = new Vector2(PlayerController.Instance.gameObject.transform.position.x, 
                             PlayerController.Instance.gameObject.transform.position.y);

    Vector2 richtungsVektor = v1 - v2;

    // distance player to disappearing blocks
    float vektorBetrag = Mathf.Sqrt(Mathf.Pow(richtungsVektor.x, 2) + Mathf.Pow(richtungsVektor.y, 2));

    return vektorBetrag;

  }

  IEnumerator cycle() {

    squareTexture1.SetBool("Visible", true);
    if (distanceToPlayer() < soundDist) PlayerController.Instance.PlaySound("disappearingBlockAppear");

    yield return new WaitForSeconds(timeBothAreVisible);

    squareTexture2.SetBool("Visible", false);
    if (distanceToPlayer() < soundDist) PlayerController.Instance.PlaySound("disappearingBlockDisappear");

    yield return new WaitForSeconds(stayTime);

    squareTexture2.SetBool("Visible", true);
    if (distanceToPlayer() < soundDist) PlayerController.Instance.PlaySound("disappearingBlockAppear");

    yield return new WaitForSeconds(timeBothAreVisible);

    squareTexture1.SetBool("Visible", false);
    if (distanceToPlayer() < soundDist) PlayerController.Instance.PlaySound("disappearingBlockDisappear");

    yield return new WaitForSeconds(stayTime);
    startCycle();

  }

}
