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

      case "lvl3_robot_humming1":
        dialog.setText(
          "Hmm! HmmHmmmHmmHmmmHmm..."
        );
        dialog.setAudioClip("lvl3_robot_humming1");
        dialog.icon = "happy";
        break;

      case "lvl3_robot_humming2":
        dialog.setText(
          "HmmHmmmHmHmmmHmm..."
        );
        dialog.setAudioClip("lvl3_robot_humming2");
        dialog.icon = "happy";
        break;

      case "lvl3_robot_humming3":
        dialog.setText(
          "H-H-HHH-HmHmmm-HmmHm-Hm..."
        );
        dialog.setAudioClip("lvl3_robot_humming3");
        dialog.icon = "happy";
        break;

    }

    return dialog;

  }


}
