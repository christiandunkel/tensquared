using UnityEngine;

/* 
 * attached to the background layer of a level, powers parallax effect
 * the basic script is from http://answers.unity.com/answers/564891/view.html
 */

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour {

  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  [SerializeField] private float parallaxFactor = 0f;





  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  void Start() {

    Log.Print($"Initialized parallax effect on layer '{gameObject.name}'.", this);

  }





  /*
   * ================
   * === EXTERNAL ===
   * ================
   */

  public void moveBy(float delta) {

    /*
     * moves the layer by the defined factor
     */

    Vector3 newPos = transform.localPosition;
    newPos.x -= delta * parallaxFactor;
    transform.localPosition = newPos;

  }

}
