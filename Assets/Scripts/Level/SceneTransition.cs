using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * powers animated transition effect between different scenes
 */

public class SceneTransition : MonoBehaviour {

  /*
   * =================
   * === SINGLETON ===
   * =================
   */
  public static SceneTransition Instance;

  private void Awake() {
    Instance = this;
  }





  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  public Animator animator;
  private bool sceneTransitionPlaying = false;





  /*
   * ================
   * === EXTERNAL ===
   * ================
   */

  public void LoadScene(string name) {

    if (!sceneTransitionPlaying) {
      sceneTransitionPlaying = true;
      StartCoroutine(PlaySceneTransition());
    }

    IEnumerator PlaySceneTransition() {

      animator.SetTrigger("End");
      yield return new WaitForSeconds(1.5f);
      SceneManager.LoadScene(name);

    }

  }

}
