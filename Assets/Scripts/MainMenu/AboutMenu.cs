using UnityEngine;
using UnityEngine.UI;

/*
 * script powers the 'scrolling down/up' animation 
 * when opening the about menu (inside main menu),
 * by moving all elements in the main menu (up and down),
 * instead of the fixed camera
 */

public class AboutMenu : MonoBehaviour {

  /*
   ==================
   === COMPONENTS ===
   ==================
   */

  // all objects that will be moved upwards, when scrolling down to animation menu
  [SerializeField] private GameObject[] obj = null;
  // all buttons that may trigger the about menu appearing / disappearing
  // -> will be disabled while animation is playing
  [SerializeField] private GameObject[] btns = null;





  /*
   ==================
   === ATTRIBUTES ===
   ==================
   */

  // number of objects to animate
  private int objNum;
  // start position of all elements that will be moved
  private Vector2[] startPos = null;
  private float moveValue = 839.0f;
  // state of where to move camera (1 = move down (to about menu), 2 = move up)
  private int move = 0;
  private int lastMove = 2;
  private bool animationPlaying = false;
  // duration of how long the animation is going to take
  private float duration = 1.2f;
  private float timer = 0.0f;





  /*
   ==================
   === INTERNAL ===
   ==================
   */

  private void Start() {

    Log.Print($"Initialised on object '{gameObject.name}'.", this);

    // amount of elements to be animated on triggering the animation
    objNum = obj.Length;

    // get all start positions of the elements
    startPos = new Vector2[objNum];
    for (int i = 0; i < objNum; i++) {
      startPos[i] = obj[i].transform.localPosition;
    }

  }

  private void Update() {

    // cancel, if movement is disabled 
    if (animationPlaying) {
      animateElements();
    }

  }

  private void animateElements() {

    /*
     * if animation is triggered, moves all elements
     * up or down, instead of the fixed camera
     */

    // new run, first, disable all buttons that can trigger the animation
    if (timer == 0f) {
      foreach (GameObject b in btns) {
        b.GetComponent<Button>().interactable = false;
      }
    }

    // time elapsed since start normalized to 0-1
    timer += Time.deltaTime / duration;

    // go through all elements, and update their position relative to time elapsed
    for (int i = 0; i < objNum; i++) {

      Vector2 newPos = new Vector2(startPos[i].x, startPos[i].y + moveValue);

      if (move == 1) {
        obj[i].transform.localPosition = Vector2.Lerp(startPos[i], newPos, timer);
      }
      else if (move == 2) {
        obj[i].transform.localPosition = Vector2.Lerp(newPos, startPos[i], timer);
      }

    }

    // if it's the end of the animation
    if (timer > duration) {

      // reset the animation values
      animationPlaying = false;
      timer = 0f;

      // enable all buttons again
      foreach (GameObject b in btns) {
        b.GetComponent<Button>().interactable = true;
      }

    }

  }





  /*
   ================
   === EXTERNAL ===
   ================
   */

  public void MoveCamera() {

    /*
     * moves camera down- or upwards in main menu
     * to or away from the about menu, depending on the last move 
     */

    // don't do anything, is animation is already playing
    if (animationPlaying) {
      return;
    }

    if (lastMove == 2) {
      move = 1;
      lastMove = 1;
    }
    else {
      move = 2;
      lastMove = 2;
    }

    animationPlaying = true;

  }

  public void MoveCameraUp() {

    /*
     * moves camera upwards to the main menu
     * and away from the about menu
     */

    // don't do anything, is animation is already playing
    if (animationPlaying) {
      return;
    }

    if (lastMove == 1) {
      move = 2;
      lastMove = 2;
      animationPlaying = true;
    }

  }


}
