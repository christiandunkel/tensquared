using UnityEngine;

/*
 * script is powering a hover effect over a button by setting an animator boolean
 * the actual hover effect may be defined in the animator of the game object
 */

[RequireComponent(typeof(Animator))]
public class ButtonHoverEffect : MonoBehaviour {

  /*
   ==================
   === ATTRIBUTES ===
   ==================
   */

  private Animator animator = null;
  private bool isHovering = false;





  /*
   ================
   === INTERNAL ===
   ================
   */

  private void Start() {

    Log.Print($"Defined a hover effect on button '{gameObject.name}'.", this);

    // get animator component of button element
    animator = gameObject.GetComponent<Animator>();

  }

  private void Update() {
    animator.SetBool("Hovering", isHovering);
  }





  /*
   ================
   === EXTERNAL ===
   ================
   */

  public void hoverEnter() {

    /*
     * sets the "Hovering" boolean in the 
     * button's animator to true
     */

    isHovering = true;

  }

  public void hoverExit() {

    /*
     * sets the "Hovering" boolean in the 
     * button's animator to false
     */

    isHovering = false;

  }

}
