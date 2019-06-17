using System.Collections;

using UnityEngine;

public class AntiMorphForceField : MonoBehaviour {

  /*
   * powers the wobbling effect of the anti morph force field
   * when a player enters
   */

  private float wobbleTimer = 0f,
                wobbleDuration = .3f;
  private bool wobbleActive = false;
  private Vector3 originalScale = Vector3.zero;

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

  private void wobbleForceField() {

    StopCoroutine(wobble());
    StartCoroutine(wobble());

    IEnumerator wobble() {

      // first wobble
      Vector3 newScale = originalScale;
      newScale.x += (newScale.x / 30f);
      newScale.y += (newScale.x / 30f);

      int stepsExpand = 8;
      int stepsRetract = 16;
      int steps = stepsExpand + stepsRetract;

      //  3 5th of the time
      float waitFor = ((wobbleDuration / 5) * 3) / steps;
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

      // second wobble
      newScale = originalScale;
      newScale.x += (newScale.x / 50f);
      newScale.y += (newScale.x / 50f);

      stepsExpand = 8;
      stepsRetract = 16;
      steps = stepsExpand + stepsRetract;

      //  2 5th of the time
      waitFor = ((wobbleDuration / 5) * 2) / steps;
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

      // third wobble

      transform.localScale = originalScale;

      StopCoroutine(wobble());

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

}
