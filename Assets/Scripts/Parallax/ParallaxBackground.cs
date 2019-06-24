using System.Collections.Generic;
using UnityEngine;

/* 
 * attached to the background layer of a level, powers parallax effect
 * the basic script is from http://answers.unity.com/answers/564891/view.html
 */

[ExecuteInEditMode]
public class ParallaxBackground : MonoBehaviour {

  public ParallaxCamera parallaxCamera;
  List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();

  private void Start() {

    if (parallaxCamera == null) parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();
    if (parallaxCamera != null) parallaxCamera.onCameraTranslate += Move;
      
    SetLayers();

  }

  private void SetLayers() {

    parallaxLayers.Clear();

    for (int i = 0; i < transform.childCount; i++) {

      ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

      if (layer != null) parallaxLayers.Add(layer);

    }

  }

  private void Move(float delta) {

    foreach (ParallaxLayer layer in parallaxLayers) layer.Move(delta);

  }

}
