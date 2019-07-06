using UnityEngine;

/*
 * loads sprites from ressources and plays them as an animation on a game object
 */

public class TextureAnimator : MonoBehaviour {

  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  [SerializeField] private int framesPerSec = 30;

  // directory containing the sprites
  [SerializeField] private string directory = "";

  // delay animation start by given seconds
  [SerializeField] private float delay = 0f;

  // if false, only play animation once
  [SerializeField] private bool loop = true;

  // wait every full loop round for given amount of time
  [SerializeField] private float delayPerLoop = 0.0f; 
  private float delayPerLoop_temp = 0.0f;

  // internal use
  private SpriteRenderer spriteRenderer = null;
  private Sprite[] sprites;
  private int spriteNum = 0;
  private bool ranOnce = false;

  private float timer = 0.0f;
  private int counter = 0;





  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  private void Start() {

    // this game object's sprite renderer
    spriteRenderer = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;

    // scan given directory and load images as sprites into memory
    sprites = Resources.LoadAll<Sprite>(directory);
    spriteNum = sprites.Length;

    // delay per loop only valid after first run
    delayPerLoop_temp = 0.0f;

  }

  private void Update() {

    // stop after first runthrough, if loops are disabled
    if (!loop && ranOnce) {
      return;
    }
    
    // use normal timer if delay is 0 or less than 0
    if (delay <= 0.0f) {

      timer += Time.deltaTime;

      // for beauty's sake, set negative values to 0
      if (delay < 0f) {
        delay = 0f;
      }

    }
    // otherwise reduce delay timer until 0
    else {
      delay -= Time.deltaTime;
    }

    if (delayPerLoop_temp > 0.0f) {
      delayPerLoop_temp -= Time.deltaTime;
      return;
    }

    // draw every defined frame per second a new image
    if (timer > 1f / framesPerSec) {

      // reset to first image, when one cycle is complete
      if (counter >= spriteNum) {
        counter = 0;
        ranOnce = true;
        delayPerLoop_temp = delayPerLoop;
      }

      // set image as new sprite in renderer
      spriteRenderer.sprite = sprites[counter] as Sprite;

      // reset timer
      this.timer = 0f;
      counter++;

    }
    
  }

}
