using System.Collections.Generic;
using UnityEngine;

/* 
 * attached to the background layer of a level, powers parallax effect
 * the basic script is from http://answers.unity.com/answers/564891/view.html
 */

[ExecuteInEditMode]
public class ParallaxBackground : MonoBehaviour {

  /*
   * ==================
   * === COMPONENTS ===
   * ==================
   */

  public ParallaxCamera parallaxCamera;
  List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();





  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  private void Start() {

    if (parallaxCamera == null) {
      parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();
    }
    else {
      parallaxCamera.onCameraTranslate += moveLayers;
    }

    setLayers();

    Log.Print($"Initialized parallax effect on background '{gameObject.name}'.", this);

  }

  private void setLayers() {

    /*
     * get the current parallax layers
     */

    parallaxLayers.Clear();

    for (int i = 0; i < transform.childCount; i++) {

      ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

      if (layer != null) {
        parallaxLayers.Add(layer);
      }

    }

  }

  private void moveLayers(float delta) {

    /*
     * moves all layers relative to player's current position
     */

    foreach (ParallaxLayer layer in parallaxLayers) {
      layer.moveBy(delta);
    }

  }

}
