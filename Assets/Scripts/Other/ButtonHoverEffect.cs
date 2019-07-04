using UnityEngine;

/*
 * script is powering a hover sound and a hover effect over a button by setting an animator boolean
 * the actual hover effect may be defined in the animator of the game object
 * 
 * button needs animation boolean "Hovering"
 */

[RequireComponent(typeof(Animator))]
public class ButtonHoverEffect : MonoBehaviour {

  /*
   ==================
   === ATTRIBUTES ===
   ==================
   */

  [SerializeField] private AudioSource soundPlayer = null;
  [SerializeField] private AudioClip hoverSound = null;
  private Animator animator = null;
  private bool isHovering = false;
  private bool hoverEffectEnabled = false;

  // later checked to determine, if parent and grandparent object are visible
  private CanvasGroup grandParentCG;
  private CanvasGroup parentCG;

  // timer that counts down until the next time a sound can be played
  private float soundDelayTimer = 0f;



  /*
   ================
   === INTERNAL ===
   ================
   */

  private void Start() {

    Log.Print($"Defined a hover effect on button '{gameObject.name}'.", this);

    // get animator component of button element
    animator = gameObject.GetComponent<Animator>();

    // get canvas group component of parent, to later check if parent is visible
    parentCG = transform.parent.gameObject.GetComponent<CanvasGroup>();
    grandParentCG = parentCG.gameObject.transform.parent.GetComponent<CanvasGroup>();

  }

  private void Update() {

    // check if parent element is visible CanvasGroup
    if (parentCG != null) {
      hoverEffectEnabled = parentCG.interactable && parentCG.alpha > 0.9f;
    }
    if (grandParentCG != null) {
      // if parent CG is okay, then grandParentCG's attributes have to be okay as well, otherwise -> false
      hoverEffectEnabled = hoverEffectEnabled && grandParentCG.interactable && grandParentCG.alpha > 0.9f;
    }

    if (hoverEffectEnabled) {
      animator.SetBool("Hovering", isHovering);
    }

    if (soundDelayTimer > 0f) {
      soundDelayTimer -= Time.fixedDeltaTime;
    }
    
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
     
    // play sound effect on hover
    if (
      hoverEffectEnabled && 
      !animator.GetBool("Hovering") && !isHovering &&
      soundDelayTimer <= 0f
    ) {
      soundPlayer.PlayOneShot(hoverSound);
      soundDelayTimer = 0.5f;
    }

    if (hoverEffectEnabled) {
      isHovering = true;
    }
    
  }

  public void hoverExit() {

    /*
     * sets the "Hovering" boolean in the 
     * button's animator to false
     */

    isHovering = false;

  }

}
