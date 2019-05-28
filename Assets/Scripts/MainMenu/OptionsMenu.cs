using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour {

  private static bool optionsOpen = false;

  public void ToggleOptions() {

    if (optionsOpen) {
      CloseOptions();
    }
    else {
      OpenOptions();
    }

  }

  public void OpenOptions() {

    optionsOpen = true;

    CanvasGroupFader.Instance.FadeIn(1);

    CanvasGroupFader.Instance.FadeOut(0);
    CanvasGroupFader.Instance.FadeOut(2);

  }

  public void CloseOptions() {

    optionsOpen = false;

    CanvasGroupFader.Instance.FadeOut(1);

    CanvasGroupFader.Instance.FadeIn(0);

  }

}
