using UnityEngine;

/*
 * manages all dialog options for level 3
 */

public class DialogLevel3 : MonoBehaviour {

  public static Dialog getDialog(string name) {

    /*
     * returns a dialog object with the attributes for the name
     */

    Dialog dialog = new Dialog();

    switch (name) {

      case "lvl3_name":
        dialog.setText(
          "Example",
          "Text"
        );
        dialog.setAudioClip("lvl3_audio_path");
        dialog.icon = "neutral";
        break;

    }

    return dialog;

  }


}
