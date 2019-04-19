using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force16x9AspectRatio : MonoBehaviour {


  public int[] startResolution = {1280, 720};
  public int[] aspectRatio = {16, 9};

  /*

  // initialization
  void Start()
  {

    Screen.SetResolution(startResolution[0], startResolution[1], false);

  }

  private float time = 0.0f;
  private float lastTestedTime = 0.0f;
  // wait time in seconds before updating window size
  private float waitBeforeTesting = 0.3f;

  private float lastWidth;
  private float lastHeight;
  private int lastWidthChange = 0;
  private int lastHeightChange = 0;

  private void Update()
  {

    if (!Screen.fullScreen && Camera.main.aspect != 1) {

      time += Time.deltaTime;

      if (lastTestedTime + waitBeforeTesting < time)
      {
        lastTestedTime = time;

        // check what was last changed
        if (lastHeight != Screen.height)
        {
          lastHeightChange = 1;
        }
        if (lastWidth != Screen.width)
        {
          lastWidthChange = 1;
        }

        if (lastWidth == Screen.width && lastHeight == Screen.height)
        {
          updateWindowSize();
          lastWidthChange = 0;
          lastHeightChange = 0;

          lastWidth = Screen.width;
          lastHeight = Screen.height;
        }

      }

    }
  }

  private void updateWindowSize()
  {

    int width = Screen.width,
        height = Screen.height;

    // fit width and height to max resolution of display if they're too big
    if (Screen.currentResolution.width < width)
    {
      width = Screen.currentResolution.width;
    }
    if (Screen.currentResolution.height < height)
    {
      height = Screen.currentResolution.height;
    }

    bool widthIsBigger = width / aspectRatio[0] > height / aspectRatio[1];

    // width is larger in relation to 16:9
    if (lastWidthChange != 0)
    {
      Screen.SetResolution(width, (int)(width * aspectRatio[1] / aspectRatio[0]), false);
    }
    // height is larger in relation to 16:9
    else if (lastHeightChange != 0)
    {
      Screen.SetResolution((int)(height * aspectRatio[0] / aspectRatio[1]), height, false);
    }

  }*/

}
