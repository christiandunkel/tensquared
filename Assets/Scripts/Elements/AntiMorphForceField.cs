using System.Collections;
using UnityEngine;

/*
 * powers the wobbling effect of the anti morph force field
 * when a player enters
 */

public class AntiMorphForceField : MonoBehaviour {

  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  private float wobbleTimer = 0f;
  private float wobbleDuration = .3f;
  private bool wobbleActive = false;
  private Vector3 originalScale = Vector3.zero;





  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  void Awake() {
    originalScale = transform.localScale;
  }

  void Update() {
    
    if (wobbleTimer > 0f) {
      wobbleTimer -= Time.fixedDeltaTime;

      if (!wobbleActive) {
        wobbleActive = true;
        wobbleForceField();
      }
      
    }
    else {
      wobbleActive = false;
    }

  }

  void OnTriggerEnter2D(Collider2D col) {

    if (col.gameObject.tag == "Player") {
      wobbleTimer = 1.0f;
    }

  }

  void OnTriggerExit2D(Collider2D col) {

    if (col.gameObject.tag == "Player") {
      wobbleTimer = 1.0f;
    }

  }

  private void wobbleForceField() {

    /*
     * plays a small wobbling animation of the force field
     */

    StopCoroutine(wobble());
    StartCoroutine(wobble());

    IEnumerator wobble() {

      /* first wobble */

      // calculate a new scale for the increase of force field
      Vector3 newScale = originalScale;
      newScale.x += (newScale.x / 30f);
      newScale.y += (newScale.x / 30f);

      // total steps used when expanding and rectracting 
      // the force field (scale) while playing the wobbling effect
      int stepsExpand = 8;
      int stepsRetract = 16;
      int steps = stepsExpand + stepsRetract;

      float waitFor = ((wobbleDuration / 5) * 3) / steps; // 3 5th of the time
      float growBy = Mathf.Abs(newScale.x - originalScale.x) / stepsExpand;
      float shrinkBy = Mathf.Abs(newScale.x - originalScale.x) / stepsRetract;

      for (int i = 0; i < steps; i++) {

        if (i < stepsExpand) {
          transform.localScale += new Vector3(growBy, growBy, 0f);
        }
        else {
          transform.localScale -= new Vector3(shrinkBy, shrinkBy, 0f);
        }
        
        yield return new WaitForSeconds(waitFor);
      }

      /* second wobble */

      newScale = originalScale;
      newScale.x += (newScale.x / 50f);
      newScale.y += (newScale.x / 50f);

      stepsExpand = 8;
      stepsRetract = 16;
      steps = stepsExpand + stepsRetract;

      waitFor = ((wobbleDuration / 5) * 2) / steps; // 2 5th of the time
      growBy = Mathf.Abs(newScale.x - originalScale.x) / stepsExpand;
      shrinkBy = Mathf.Abs(newScale.x - originalScale.x) / stepsRetract;

      for (int i = 0; i < steps; i++) {

        if (i < stepsExpand) {
          transform.localScale += new Vector3(growBy, growBy, 0f);
        }
        else {
          transform.localScale -= new Vector3(shrinkBy, shrinkBy, 0f);
        }

        yield return new WaitForSeconds(waitFor);
      }



      // return to original scale at the end
      transform.localScale = originalScale;

      StopCoroutine(wobble());

    }

  }

}
