using UnityEngine;

public struct Dialog {

  /*
   * defines the structure of a dialog
   */

  public string text;
  public int textLength;
  public AudioClip audioClip;
  public float audioClipLength;
  public string icon;

  public void setText(params string[] text_) {

    text = "";
    for (int i = 0; i < text_.Length; i++) {
      text += text_[i];

      // add new line
      if (i  < text_.Length - 1) {
        text += "\n";
      }
    }

    textLength = text.Length;

  }

  public void setAudioClip(string path) {

    audioClip = Resources.Load("Dialog/" + path, typeof(AudioClip)) as AudioClip;
    audioClipLength = audioClip.length;

  }

}