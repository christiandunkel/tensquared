using UnityEngine;

/*
 * manages the 'floating block' prefab
 * and how much it sinks into the water on player landing on it
 */

public class FloatingBlock : MonoBehaviour {

  private Rigidbody2D rb2d;
  public ParticleSystem splashParticles;
  private float sinkInTimer = 0f;

  public int type = 1;
  /*
   * type of floating block:
   * 1 : small
   * 2 : normal
   * 3 : big
   */

  void Awake() {

    rb2d = GetComponent<Rigidbody2D>();

  }

  void Update() {
    
    // sink the floating block further into water
    // when player lands on it
    if (sinkInTimer > 0f) {
      sinkInTimer -= Time.deltaTime;

      if (rb2d.mass == 1f) {
        PlayerController player = PlayerController.Instance;
        player.PlaySound("waterSplashSound");
        splashParticles.Play();

        // small floating block
        if (type == 1) {
          switch (player.GetString("state")) {
            case "Circle":
            case "Triangle": rb2d.mass = 50f; break;
            case "Rectangle": rb2d.mass = 150f; break;
          }
        }
        
      }
      
    }
    else if (rb2d.mass > 1f) {
      rb2d.mass = 1f;
    }

  }

  void OnCollisionStay2D(Collision2D col) {

    if (col.gameObject.tag == "Player") {
      sinkInTimer = 1f;
    }

  }

}
