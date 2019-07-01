using UnityEngine;

/* 
 * attached to the background layer of a level, powers parallax effect
 * the basic script is from http://answers.unity.com/answers/564891/view.html
 */

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour {

  public float parallaxFactor;

  public void Move(float delta) {
    Vector3 newPos = transform.localPosition;
    newPos.x -= delta * parallaxFactor;
    transform.localPosition = newPos;
  }

}
