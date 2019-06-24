using UnityEngine;

/*
 * powers the 'piston' prefab
 */

public class Piston : MonoBehaviour {

  /*
   * =================
   * === SINGLETON ===
   * =================
   */

  public static Piston Instance;

  void Awake() {
    Instance = this;
  }





  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  // components
  private Animator animator;

  // attributes
  public bool pistonIsPlaying = false;
  private const float delayBeforePush = 0.2f;
  private float timer = 0f;





  /*
   * ================
   * === EXTERNAL ===
   * ================
   */

  public static void activate(GameObject piston) {

    /*
     * activates the given piston object
     */

    piston.GetComponent<Piston>().activate();

  }

  public void activate() {

    /*
     * activates the current piston instance
     */

    if (timer <= 0f) {
      pistonIsPlaying = true;
      timer = delayBeforePush;
    }

  }





  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  private void Start() {
    animator = GetComponent<Animator>();
  }

  private void Update() {

    if (pistonIsPlaying && timer <= 0f) {
      pistonIsPlaying = false;
      animator.SetTrigger("PushUp");
      PlayerManager.Instance.setValue("steppedOnPiston", true);
    }
    else {
      timer -= Time.deltaTime;
    }

  }
}
