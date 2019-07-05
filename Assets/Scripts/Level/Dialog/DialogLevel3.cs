using UnityEngine;

/*
 * manages all dialog options for level 3
 */

public class DialogLevel3 : MonoBehaviour {

  public static Dialog getDialog(string name) {

    /*
     * returns a dialog object with the attributes for the name
     */

    Dialog dialog = new Dialog(3);

    switch (name) {

      case "lvl3_robot_humming1":
        dialog.setText(
          "Hmm! HmmHmmmHmmHmmmHmm..."
        );
        dialog.setAudioClip("lvl3_robot_humming1");
        dialog.icon = "happy";
        dialog.isEvil = true;
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

      case "lvl3_EVIL_like_the_melody":
        dialog.setText(
          "The melody in my head won't stop playing...",
          "I like it."
        );
        dialog.setAudioClip("lvl3_EVIL_like_the_melody");
        dialog.icon = "happy";
        dialog.isEvil = true;
        break;

      case "lvl3_neck_nose_ear_specialist":
        dialog.setText(
          "Can you recommend a good ear, nose and throat specialist? " + 
          "I think I have a problem with my ears."
        );
        dialog.setAudioClip("lvl3_neck_nose_ear_specialist");
        dialog.icon = "neutral";
        break;

      case "lvl3_EVIL_beautiful_melody":
        dialog.setText(
          "This melody is beautiful.",
          "I wish you could hear it."
        );
        dialog.setAudioClip("lvl3_EVIL_beautiful_melody");
        dialog.icon = "happy";
        dialog.isEvil = true;
        break;

      case "lvl3_smoking_legs":
        dialog.setText(
          "Hey, little friend. Those new legs you brought me... well, they work, but... they seem to smoke, did you notice that?"
        );
        dialog.setAudioClip("lvl3_smoking_legs");
        dialog.icon = "sad";
        break;

      case "lvl3_EVIL_fruit_juice_shooters":
        dialog.setText(
          "Don't worry. Those laser cannons... I mean, fruit juice shooters are totally harmless."
        );
        dialog.setAudioClip("lvl3_EVIL_fruit_juice_shooters");
        dialog.icon = "happy";
        dialog.isEvil = true;
        break;

      case "lvl3_surprised_about_laser_cannons":
        dialog.setText(
          "Wow, they actually malfunctioned and didn't shoot. Didn't I tell you, they were safe? Just as I expected."
        );
        dialog.setAudioClip("lvl3_surprised_about_laser_cannons");
        dialog.icon = "happy";
        break;

      case "lvl3_EVIL_set_on_fire":
        dialog.setText(
          "It smells like barbecue. Did you set something on fire?"
        );
        dialog.setAudioClip("lvl3_EVIL_set_on_fire");
        dialog.icon = "neutral";
        dialog.isEvil = true;
        break;

      case "lvl3_toxic_gases":
        dialog.setText(
          "There's chlorine in the air. And do I smell a little arsine? " + 
          "Either way, it's no problem to you. You're a circle, " + 
          "and I've never seen a circle die of toxic gases."
        );
        dialog.setAudioClip("lvl3_toxic_gases");
        dialog.icon = "neutral";
        break;

      case "lvl3_melody_is_gone":
        dialog.setText(
          "I think I feel a little better now.",
          "The strange melody is gone."
        );
        dialog.setAudioClip("lvl3_melody_is_gone");
        dialog.icon = "happy";
        break;

      case "lvl3_EVIL_unfortunate":
        dialog.setText(
          "Well, that is truly unfortunate.",
          "And it hurt a little."
        );
        dialog.setAudioClip("lvl3_EVIL_unfortunate");
        dialog.icon = "neutral";
        dialog.isEvil = true;
        break;

      case "lvl3_EVIL_hand_over_your_body":
        dialog.setText(
          "Anyway, that's over. Do you mind handing over your scientifically impossible body to me? " + 
          "There are a few things I would like to experiment with."
        );
        dialog.setAudioClip("lvl3_EVIL_hand_over_your_body");
        dialog.icon = "laughing";
        dialog.isEvil = true;
        break;

    }

    return dialog;

  }


}
