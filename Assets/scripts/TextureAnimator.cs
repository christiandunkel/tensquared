using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class TextureAnimator : MonoBehaviour
{

  // public variables
  public int framesPerSec = 20;
  public string directory;

  // internal use
  private SpriteRenderer spriteRenderer;
  private Sprite[] sprites;
  private int spriteNum;
  
  // Start is called before the first frame update
  void Start()
  {

    // this game object's sprite renderer
    spriteRenderer = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;

    // scan given directory and load images as sprites into memory
    sprites = Resources.LoadAll<Sprite>(directory);
    spriteNum = sprites.Length;

    /*

    // define array with size of images
    sprites = new Sprite[info.Length];

    // add files in directory to array
    int count = 0;
    foreach (FileInfo f in info)
    {
      // load image into byte array, then texture
      byte[] fileData = File.ReadAllBytes(f.FullName);
      Texture2D SpriteTexture = new Texture2D(1, 1);
      SpriteTexture.LoadImage(fileData);

      float PixelsPerUnit = 100.0f;
      Debug.Log(SpriteTexture.width);

      sprites[count] = Sprite.Create(SpriteTexture,
        new Rect(0, 0, SpriteTexture.width, SpriteTexture.height),
        new Vector2(0.5f, 0.5f), PixelsPerUnit, 0, SpriteMeshType.Tight);

      count++;
    }//*/

    // print items found
    /* foreach (Sprite sprite in sprites)
    {
      Debug.Log(sprite.name);
    }//*/

  }

  private float timer = 0.0f;
  private int counter = 0;

  void Update()
  {
    
    timer += Time.deltaTime;

    // draw every defined frame per second a new image
    if (timer > (1f / framesPerSec))
    {

      // reset to first image, when one cycle is complete
      if (counter >= spriteNum)
      {
        counter = 0;
      }

      // set image as new sprite in renderer
      spriteRenderer.sprite = sprites[counter] as Sprite;

      // reset timer
      this.timer = 0f;
      counter++;

    }
    
  }

}
