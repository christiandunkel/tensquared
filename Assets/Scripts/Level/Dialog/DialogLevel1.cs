using UnityEngine;

/*
 * manages all dialog options for level 1
 */

public class DialogLevel1 : MonoBehaviour {

  public static Dialog getDialog(string name) {

    /*
     * returns a dialog object with the attributes for the name
     */

    Dialog dialog = new Dialog(1);

    switch (name) {
      
      case "lvl1_hello":
        dialog.setText(
          "Hello, my little friend!",
          "Do you have some time to help me out?"
        );
        dialog.setAudioClip("lvl1_hello");
        dialog.icon = "laughing";
        break;
      
      case "lvl1_asleep":
        dialog.setText(
          "Are you asleep perhaps?"
        );
        dialog.setAudioClip("lvl1_asleep");
        dialog.icon = "annoyed";
        break;

      case "lvl1_move":
        dialog.setText(
          "Good, now that you're awake...",
          "Can you roll over to give me a hand?"
        );
        dialog.setAudioClip("lvl1_move");
        dialog.icon = "laughing";
        break;

      case "lvl1_jump":
        dialog.setText(
          "Can you jump over those blocks?"
        );
        dialog.setAudioClip("lvl1_jump");
        dialog.icon = "neutral";
        break;

      case "lvl1_dont_jump_into_water":
        dialog.setText(
          "I wouldn't jump into that lake!",
          "That isn't water, and it is deadly."
        );
        dialog.setAudioClip("lvl1_dont_jump_into_water");
        dialog.icon = "neutral";
        break;

      case "lvl1_not_the_smartest_circle":
        dialog.setText(
          "You really aren't the smartest circle out there."
        );
        dialog.setAudioClip("lvl1_not_the_smartest_circle");
        dialog.icon = "annoyed";
        break;

      case "lvl1_its_me":
        dialog.setText(
          "Hey, little friend! It's me!",
          "It's great that you are finally here!"
        );
        dialog.setAudioClip("lvl1_its_me");
        dialog.icon = "happy";
        break;

      case "lvl1_arms_are_further_ahead":
        dialog.setText(
          "Now, I would appreciate it very much if you could bring me my arms! They are a little further ahead!"
        );
        dialog.setAudioClip("lvl1_arms_are_further_ahead");
        dialog.icon = "happy";
        break;

      case "lvl1_quick_compared_to_other_circles":
        dialog.setText(
          "You're doing great! You're actually quite quick on foot, especially when compared to other circles!"
        );
        dialog.setAudioClip("lvl1_quick_compared_to_other_circles");
        dialog.icon = "happy";
        break;

      case "lvl1_pick_up_arms":
        dialog.setText(
          "That's them! Those are my arms!",
          "Could you pick them up for me?"
        );
        dialog.setAudioClip("lvl1_pick_up_arms"); 
        dialog.icon = "surprised";
        break;

      case "lvl1_bring_arms_back":
        dialog.setText(
          "Now please bring them back to me!"
        );
        dialog.setAudioClip("lvl1_bring_arms_back");
        dialog.icon = "happy";
        break;

      case "lvl1_thank_you":
        dialog.setText(
          "Thank you!",
          "Finally I can move this rusty old body of mine!"
        );
        dialog.setAudioClip("lvl1_thank_you");
        dialog.icon = "laughing";
        break;

      case "lvl1_where_did_i_leave_my_legs":
        dialog.setText(
          "But now... where did I leave my legs again?"
        );
        dialog.setAudioClip("lvl1_where_did_i_leave_my_legs");
        dialog.icon = "sad";
        break;

      case "lvl1_im_off":
        dialog.setText(
          "Well, I'm off looking for them.",
          "Bye bye, little friend!"
        );
        dialog.setAudioClip("lvl1_im_off");
        dialog.icon = "neutral";
        break;

    }

    return dialog;

  }


}
