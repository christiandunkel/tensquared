using UnityEngine;

/*
 * generates a black, transparent version of the sprite 
 * displayed behind the attached game object as a shadow
 */


[RequireComponent(typeof(SpriteRenderer))]
public class SpriteDropShadow : MonoBehaviour {

  /*
   ==================
   === ATTRIBUTES ===
   ==================
   */

  [SerializeField] private Vector3 shadowOffset = new Vector3(0.3f, -0.3f, 0f);
  private Material shadowMaterial = null;

  private GameObject shadow;





  /*
   ================
   === INTERNAL ===
   ================
   */

  private void Awake() {

    // load 'shadow shader' material from ressources
    shadowMaterial = Resources.Load<Material>("Materials/ShadowMaterial");

  }

  private void Start()  {

    // create the shadow sprite object as child of the original object
    shadow = new GameObject("Shadow");
    shadow.transform.parent = transform;

    // position the shadow object according to the defined offset
    shadow.transform.localPosition = shadowOffset;
    shadow.transform.localRotation = Quaternion.identity;

    // get sprite renderer components
    SpriteRenderer sr_original = GetComponent<SpriteRenderer>();
    SpriteRenderer sr_shadow = shadow.AddComponent<SpriteRenderer>();

    // set shadow's sprite to original's sprite and apply 'shadow shader' material
    sr_shadow.sprite = sr_original.sprite;
    sr_shadow.material = shadowMaterial;

    // apply the 'flip' settings of the original sprite to the shadow
    sr_shadow.flipX = sr_original.flipX;
    sr_shadow.flipY = sr_original.flipY;

    // set shadow sprite in the same layer by -1 in order behind original sprite
    sr_shadow.sortingLayerName = sr_original.sortingLayerName;
    sr_shadow.sortingOrder = sr_original.sortingOrder - 1;

    // reset scale of shadow object, which it might have inherited from original object
    sr_shadow.transform.localScale = Vector3.one;

  }

  private void LateUpdate() {

    // update the shadows position to always be on game object
    shadow.transform.localPosition = shadowOffset;

  }

}
