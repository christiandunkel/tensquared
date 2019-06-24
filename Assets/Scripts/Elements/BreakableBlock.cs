using System.Collections;
using UnityEngine;

/*
 * powers the 'breakable block' prefab
 */

public class BreakableBlock : MonoBehaviour {

  private Sprite[] breakingSprites;
  public GameObject breakParticles;

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
      PlayerManager.Instance.getFloat("secondsAsRectangleFalling") > .52f &&
      PlayerManager.Instance.getString("state") == "Rectangle"
    ) {
      StartCoroutine(breakBlock());
    }

  }

  private IEnumerator breakBlock() {

    GetComponent<Animator>().SetTrigger("BreakBlock");

    yield return new WaitForSeconds(.2f);

    SoundController.Instance.playSound("breakingBlockSound");

    for (int i = 0; i < breakingSprites.Length; i++) {
      GetComponent<SpriteRenderer>().sprite = breakingSprites[i];

      // spawn particles after a few ticks
      if (i == 3) {
        breakParticles.SetActive(true);
        breakParticles.GetComponent<ParticleSystem>().Play();
      }

      yield return new WaitForSeconds(.1f);
    }

    gameObject.SetActive(false);

    StopCoroutine(breakBlock());

  }

}
