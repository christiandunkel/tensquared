using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

  // singleton
  public static SceneTransition Instance;
  void Awake()
  {
    Instance = this;
  }

  public Animator animator;

  public void LoadScene(string name)
  {
    StartCoroutine(PlaySceneTransition(name));
  }

  IEnumerator PlaySceneTransition(string name)
  {

    animator.SetTrigger("End");
    yield return new WaitForSeconds(1.5f);
    SceneManager.LoadScene(name);

  }

}
