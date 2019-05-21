using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[RequireComponent(typeof(SpriteRenderer))]
public class SpriteDropShadow : MonoBehaviour
{

  public Vector3 shadowOffset = new Vector3(0.3f, -0.3f, 0f);
  private Material shadowMaterial = null;

  private GameObject shadow;

  private void Awake() {
    shadowMaterial = Resources.Load<Material>("Materials/ShadowMaterial");
  }

  void Start()  {

    shadow = new GameObject("Shadow");
    shadow.transform.parent = transform;

    shadow.transform.localPosition = shadowOffset;
    shadow.transform.localRotation = Quaternion.identity;

    SpriteRenderer sr_original = GetComponent<SpriteRenderer>();
    SpriteRenderer sr_shadow = shadow.AddComponent<SpriteRenderer>();

    sr_shadow.sprite = sr_original.sprite;
    sr_shadow.material = shadowMaterial;

    sr_shadow.flipX = sr_original.flipX;
    sr_shadow.flipY = sr_original.flipY;

    sr_shadow.sortingLayerName = sr_original.sortingLayerName;
    sr_shadow.sortingOrder = sr_original.sortingOrder - 1;

    sr_shadow.transform.localScale = new Vector3(1f, 1f, 1f);

  }

  private void LateUpdate() {
    shadow.transform.localPosition = shadowOffset;
  }

}
