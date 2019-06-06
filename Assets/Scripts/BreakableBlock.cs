using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour {

  private Sprite[] breakingSprites;
  private GameObject breakParticles;

  void Awake() {

    foreach (Transform child in transform.parent.gameObject.transform) {
      switch (child.gameObject.name) {
        case "BreakParticles": breakParticles = child.gameObject; break;
        default: break;
      }
    }

    breakingSprites = Resources.LoadAll<Sprite>("BreakableBlock");

  }

  private void OnTriggerEnter2D(Collider2D col) {
    
    if (
      col.gameObject.tag == "Player" && 
      PlayerController.Instance.GetFloat("secondsNotGrounded") > 1f &&
      PlayerController.Instance.GetString("state") == "Rectangle"
    ) {
      StartCoroutine(breakBlock());
    }

  }

  private IEnumerator breakBlock() {

    GetComponent<Animator>().SetTrigger("BreakBlock");

    yield return new WaitForSeconds(.2f);

    PlayerController.Instance.PlaySound("breakingBlockSound");

    for (int i = 0; i < breakingSprites.Length; i++) {
      GetComponent<SpriteRenderer>().sprite = breakingSprites[i];

      if (i == breakingSprites.Length - 1) {
        breakParticles.SetActive(true);
        breakParticles.GetComponent<ParticleSystem>().Play();
      }

      yield return new WaitForSeconds(.1f);
    }

    gameObject.SetActive(false);

    StopCoroutine(breakBlock());

  }

}
