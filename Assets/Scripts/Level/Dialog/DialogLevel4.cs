using UnityEngine;

/*
 * manages all dialog options for level 4
 */

public class DialogLevel4 : MonoBehaviour {

  public static Dialog getDialog(string name) {

    /*
     * returns a dialog object with the attributes for the name
     */

    Dialog dialog = new Dialog(4);

    switch (name) {

      case "lvl4_name":
        dialog.setText(
          "Example",
          "Text"
        );
        dialog.setAudioClip("lvl4_audio_path");
        dialog.icon = "neutral";
        break;

    }

    return dialog;

  }


}
