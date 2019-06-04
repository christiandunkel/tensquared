using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportingBlock : MonoBehaviour {

  public Animator squareTexture1, squareTexture2;

  public float stayTime = 1.5f,
               timeBothAreVisible = .8f;

  private void Awake() {
    startCycle();
  }

  private void startCycle() {
    StopCoroutine(cycle());
    StartCoroutine(cycle());
  }

  IEnumerator cycle() {

    squareTexture1.SetBool("Visible", true);

    yield return new WaitForSeconds(timeBothAreVisible);

    squareTexture2.SetBool("Visible", false);

    yield return new WaitForSeconds(stayTime);

    squareTexture2.SetBool("Visible", true);

    yield return new WaitForSeconds(timeBothAreVisible);

    squareTexture1.SetBool("Visible", false);

    yield return new WaitForSeconds(stayTime);

    startCycle();

  }

}
