using UnityEngine;

/*
 * script is powering a hover effect over a button by setting an animator boolean
 * the actual hover effect may be defined in the animator of the game object
 */

[RequireComponent(typeof(Animator))]
public class ButtonHover : MonoBehaviour {

  private Animator animator = null;

  private void Start() {
    animator = gameObject.GetComponent<Animator>();
  }

  private bool isHovering = false;

  public void Update() {
    animator.SetBool("Hovering", isHovering);
  }

  public void hoverEnter() {
    isHovering = true;
  }

  public void hoverExit() {
    isHovering = false;
  }

}
