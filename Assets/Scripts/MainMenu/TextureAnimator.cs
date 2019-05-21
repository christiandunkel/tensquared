using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class TextureAnimator : MonoBehaviour
{

  // public variables
  public int framesPerSec = 30; 
  public string directory; // directory of textures
  public float delay = 0.0f; // delay animation by given seconds
  public bool loop = true; // if false, only play animation once
  public float delayPerLoop = 0.0f; // wait every full loop round for given amount of time
  private float delayPerLoop_temp = 0.0f;

  // internal use
  private SpriteRenderer spriteRenderer = null;
  private Sprite[] sprites;
  private int spriteNum = 0;
  private bool ranOnce = false;

  // Start is called before the first frame update
  void Start()
  {

    // this game object's sprite renderer
    spriteRenderer = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;

    // scan given directory and load images as sprites into memory
    sprites = Resources.LoadAll<Sprite>(directory);
    spriteNum = sprites.Length;

    // delay per loop only valid after first run
    delayPerLoop_temp = 0.0f;

  }

  private float timer = 0.0f;
  private int counter = 0;

  void Update()
  {

    // stop after first runthrough, if loops are disabled
    if (!loop && ranOnce)
    {
      return;
    }
    
    // use normal timer if delay is 0 or less than 0
    if (delay <= 0.0f)
    {
      timer += Time.deltaTime;

      // for beauty's sake, set negative values to 0
      if (delay < 0.0f)
      {
        delay = 0.0f;
      }

    }
    // otherwise reduce delay timer until 0
    else
    {
      delay -= Time.deltaTime;
    }

    if (delayPerLoop_temp > 0.0f)
    {
      delayPerLoop_temp -= Time.deltaTime;
      return;
    }

    // draw every defined frame per second a new image
    if (timer > (1f / framesPerSec))
    {

      // reset to first image, when one cycle is complete
      if (counter >= spriteNum)
      {
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
