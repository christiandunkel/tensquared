using System.Collections;
using UnityEngine;

/*
 * powers the 'corrupted icon' effect on the dialogue system
 * by generating a Perlin Noise texture
 * HAS TO BE PLACED ON 3D-OBJECT -> QUAD
 */

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class NoiseGenerator : MonoBehaviour {

  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  [SerializeField] private int width = 128;
  [SerializeField] private int height = 128;

  // the higher it's set, the bigger the pixel's of the noise will be
  [SerializeField] private float scale = 10f;

  // offset values inside noise texture
  // (by constantly randomizing them, creates animated noise effect)
  private float offsetX = 100f;
  private float offsetY = 100f;





  /*
   * ==================
   * === COMPONENTS ===
   * ==================
   */

  Renderer textureRenderer = null;




  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  private void Start() {

    Log.Print($"Initialised 'corrupted icon' noise on object '{gameObject.name}'.", this);

    textureRenderer = GetComponent<Renderer>();

    // start the noise generation animation
    StopCoroutine(updateNoiseTexture());
    StartCoroutine(updateNoiseTexture());

  }

  private IEnumerator updateNoiseTexture() {

    /*
     * periodically generates a random noise texture
     * to be displayed on the renderer
     */

    while (true) { 

      // create randomized offset
      offsetX = Random.Range(0f, 99999f);
      offsetY = Random.Range(0f, 99999f);

      // change Quad texture
      textureRenderer.material.mainTexture = generateNoiseTexture();

      yield return new WaitForSeconds(.1f);

    }

  }

  private Texture2D generateNoiseTexture() {

    /*
     * generates a texture containing a Perlin Noise pattern
     */

    Texture2D texture = new Texture2D(width, height);

    // loop through each pixel of texture and 
    // generate Perlin Noise map for texture
    for (int x = 0; x < width; x++) {
      for (int y = 0; y < height; y++) {

        // calculate color using a Perlin Noise function
        Color color = calculateColor(x, y);

        // set current pixel to color
        texture.SetPixel(x, y, color);

      }
    }

    // apply the color data
    texture.Apply();

    return texture;

  }

  private Color calculateColor(int x, int y) {

    /*
     * generates a pseudo-random color for the
     * given pixel coordinates using a Perlin Noise function
     */

    // normalize pixel coordinates to 0-1
    // as Perlin Noise repeats at whole numbers
    float xNormalized = (float)x / width * scale + offsetX;
    float yNormalized = (float)y / width * scale + offsetY;

    // calculate and return color
    float colorValue = Mathf.PerlinNoise(xNormalized, yNormalized);

    // make the minimum allowed color a light gray instead of white
    if (colorValue > 0.5f) {
      colorValue /= 2f;
    }

    return new Color(colorValue, colorValue, colorValue);

  }

}
