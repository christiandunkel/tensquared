using System.Collections;
using UnityEngine;

/*
 * powers the 'breakable block' prefab
 */

public class BreakableBlock : MonoBehaviour {

  /*
   * ==================
   * === COMPONENTS ===
   * ==================
   */

  [SerializeField] private GameObject breakParticles = null;





  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  private Sprite[] breakingSprites;





  /*
   * ==================
   * === INTERNAL ===
   * ==================
   */

  private void Awake() {

    // load sprites powering the animation of the block breaking
    breakingSprites = Resources.LoadAll<Sprite>("BreakableBlock");

  }

  private void OnTriggerEnter2D(Collider2D col) {
    
    // if player falls on top of the block as rectangle from a certain height
    if (
      col.gameObject.tag == "Player" && 
      PlayerManager.Instance.getFloat("secondsAsRectangleFalling") > .52f
    ) {
      StartCoroutine(breakBlock());
    }

  }

  private IEnumerator breakBlock() {

    /*
     * plays an animation of the block breaking,
     * with it finally disappearing at the end
     */

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
