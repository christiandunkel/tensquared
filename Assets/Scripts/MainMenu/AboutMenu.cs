using UnityEngine;
using UnityEngine.UI;

/*
 * script powers the animation of "scrolling down", when about menu (inside main menu) is opened,
 * as well as the "scrolling up" animation, when the about menu is closed
 */

public class AboutMenu : MonoBehaviour {

  // all objects that will be moved upwards, when scrolling down to animation menu
  public GameObject[] obj = null;

  // all buttons that may trigger the about menu appearing / disappearing
  // buttons will be disabled while animation is playing
  public GameObject[] btns = null;

  private Vector2[] startPos = null;
  private float moveValue = 839.0f;

  private void Start() {

    startPos = new Vector2[obj.Length];

    int counter = 0;
    foreach (GameObject o in obj) {
      startPos[counter] = new Vector2(o.transform.localPosition.x, o.transform.localPosition.y);
      counter++;
    }

  }


  private int move = 0, // 0 = disabled
                        // 1 = move down (to about menu)
                        // 2 = move up (back to main menu)
              lastMove = 2;

  private float duration = 1.2f,
                timer = 0.0f;

  private void Update() {

    // cancel, if movement is disabled 
    if (move <= 0) return;

    // new run, disable button
    if (timer == 0.0f) {
      foreach (GameObject b in btns) {
        b.GetComponent<Button>().interactable = false;
      }
    }

    timer += Time.deltaTime / duration;

    int counter = 0;
    foreach (GameObject o in obj) {

      Vector2 posA = new Vector2(startPos[counter].x, startPos[counter].y + moveValue);

      if (move == 1) {
        o.transform.localPosition = Vector2.Lerp(startPos[counter], posA, timer);
      }
      else if (move == 2) {
        o.transform.localPosition = Vector2.Lerp(posA, startPos[counter], timer);
      }

      counter++;

    }

    // end of animation
    if (timer > duration) {

      move = 0;
      timer = 0;

      // enable all buttons again
      foreach (GameObject b in btns) {
        b.GetComponent<Button>().interactable = true;
      }

    }

  }

  public void MoveCamera() {

    if (lastMove == 2) {
      move = 1;
      lastMove = 1;
    }
    else {
      move = 2;
      lastMove = 2;
    }

  }

  public void MoveCameraUp() {

    if (lastMove == 1) {
      move = 2;
      lastMove = 2;
    }

  }


}
