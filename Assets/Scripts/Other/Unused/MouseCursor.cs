using UnityEngine;

/*
 * powers an animated mouse cursor
 */

public class MouseCursor : MonoBehaviour {

  /*
   * ==================
   * === COMPONENTS ===
   * ==================
   */

  private Animator animator;
  [SerializeField] private SpriteRenderer cursorImage = null;
  [SerializeField] private ParticleSystem clickEffect = null;
  [SerializeField] private ParticleSystem trailEffect = null;





  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  [SerializeField] private bool playParticleEffects = true;

  private Vector2 mousePos;
  private Vector2 lastMousePos;

  private float trailTimer = 0.0f;
  private float timeBetweenTrailParticles = 0.1f;





  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  private void Start() {

    Log.Print($"Initialised animated mouse cursor '{gameObject.name}'.", this);

    Cursor.visible = false;
    animator = gameObject.GetComponent<Animator>();
    trailTimer = timeBetweenTrailParticles;

  }

  private void Update() {

    bool pauseMenuExists = PauseMenu.Instance != null;

    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    gameObject.transform.position = mousePos;

    if (pauseMenuExists) {

      if (PauseMenu.Instance.isPaused()) {
        Cursor.visible = true;
        // make image transparent
        cursorImage.color = new Color(1f, 1f, 1f, 0f);
      }
      else {
        // deactivate cursor once more, if visible through bug
        if (Cursor.visible) {
          Cursor.visible = false;
        }
        if (cursorImage.color.a < 1f) {
          // make image visible 
          cursorImage.color = new Color(1f,1f,1f,1f);
        }
      }

    }
    else if (Cursor.visible) {
      Cursor.visible = false;
    }

    // play different animation on holding mouse button
    if (Input.GetMouseButton(0)) animator.SetBool("IsClicking", true);
    else animator.SetBool("IsClicking", false);

    if (playParticleEffects) {
      playParticles();
    }

    lastMousePos = mousePos;

  }

  private void playParticles() {

    /*
     * plays particle effects when moving the mouse cursor,
     * as well as, when clicking on the screen
     */

    // spawn in particle effect on click
    if (Input.GetMouseButtonDown(0)) {
      GameObject ce = Instantiate(clickEffect.gameObject, gameObject.transform.position, Quaternion.identity);
      ce.transform.SetParent(gameObject.transform.parent.gameObject.transform);
      Destroy(ce, 1.5f);
    }

    // spawn trail particles behind cursor, if it's moving
    if (lastMousePos.x != mousePos.x || lastMousePos.y != mousePos.y) {

      if (trailTimer <= 0f) {
        trailTimer = timeBetweenTrailParticles;
        GameObject te = Instantiate(trailEffect.gameObject, gameObject.transform.position, Quaternion.identity);
        te.transform.SetParent(gameObject.transform.parent.gameObject.transform);
        Destroy(te, .8f);
      }
      else {
        trailTimer -= Time.deltaTime;
      }

    }

  }

}
