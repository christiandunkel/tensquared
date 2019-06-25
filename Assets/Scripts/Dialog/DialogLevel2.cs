using UnityEngine;

/*
 * manages all dialog options for level 2
 */

public class DialogLevel2 : MonoBehaviour {

  public static Dialog getDialog(string name) {

    /*
     * returns a dialog object with the attributes for the name
     */

    Dialog dialog = new Dialog();

    switch (name) {

      case "lvl2_no_legs_over_here":
        dialog.setText(
          "Legs? Over here? No legs? That's not good.",
          "Oh, maybe there. No? What about down there?"
        );
        dialog.setAudioClip("lvl2_no_legs_over_here");
        dialog.icon = "annoyed";
        break;

      case "lvl2_you_are_here_as_well":
        dialog.setText(
          "Oh, you are here as well, little friend?",
          "That's truly an interesting coincidence...",
          "Don't you think?"
        );
        dialog.setAudioClip("lvl2_you_are_here_as_well");
        dialog.icon = "happy";
        break;

      case "lvl2_do_you_want_to_get_out_of_here":
        dialog.setText(
          "Do you want to get out of here, little friend?",
          "You are a smart circle! You should be able to think of a solution!"
        );
        dialog.setAudioClip("lvl2_do_you_want_to_get_out_of_here");
        dialog.icon = "laughing";
        break;

      case "lvl2_full_of_surprises":
        dialog.setText(
          "You are full of surprises, little friend.",
          "You can actually completely reform your body.",
          "What are you made of, can you tell me?"
        );
        dialog.setAudioClip("lvl2_full_of_surprises");
        dialog.icon = "surprised";
        break;
        
      case "lvl2_you_are_out":
        dialog.setText(
          "Now that you're out there...",
          "Do you mind looking around for a robot leg or two?",
          "I have misplaced mine."
        );
        dialog.setAudioClip("lvl2_you_are_out");
        dialog.icon = "neutral";
        break;

      case "lvl2_can_you_morph_into_other_forms":
        dialog.setText(
          "Little friend, I'm quite curious...",
          "Can you morph into other forms as well?"
        );
        dialog.setAudioClip("lvl2_can_you_morph_into_other_forms");
        dialog.icon = "happy";
        break;
        
      case "lvl2_rectangle_great":
        dialog.setText(
          "That's great! You can even increase your mass! Wonderful!" + 
          "Although, according to my calculations... It's scientifically impossible... probably."
        );
        dialog.setAudioClip("lvl2_rectangle_great");
        dialog.icon = "surprised";
        break;

      case "lvl2_breakable_block":
        dialog.setText(
          "This metallic thing looks quite brittle, don't you think?" + 
          "Maybe letting something heavy fall on top of it from a certain distance will break it."
        );
        dialog.setAudioClip("lvl2_breakable_block");
        dialog.icon = "neutral";
        break;

      case "lvl2_smash_right":
        dialog.setText(
          "Great, you smashed right through that.",
          "I am in awe."
        );
        dialog.setAudioClip("lvl2_smash_right");
        dialog.icon = "happy";
        break;

      case "lvl2_second_hand_legs":
        dialog.setText(
          "Did you find any legs yet?",
          "You could even bring me second-hand goods, although I would feel very sad inside."
        );
        dialog.setAudioClip("lvl2_second_hand_legs");
        dialog.icon = "sad";
        break;

      case "lvl2_found_legs":
        dialog.setText(
          "Is that a pair of legs? Very good!"
        );
        dialog.setAudioClip("lvl2_found_legs");
        dialog.icon = "happy";
        break;

      case "lvl2_force_fields_everywhere":
        dialog.setText(
          "You can find those blue blobs all over the place, like litter, destroying the landscape." + 
          "At least, they sound nice. And sometimes that's all that counts, don't you think?"
        );
        dialog.setAudioClip("lvl2_force_fields_everywhere");
        dialog.icon = "neutral";
        break;

      case "lvl2_thank_you_my_little_friend":
        dialog.setText(
          "Thank you very much, my little friend! You're a life-safer! I will certainly repay you!"
        );
        dialog.setAudioClip("lvl2_thank_you_my_little_friend");
        dialog.icon = "happy";
        break;

      case "lvl2_where_did_you_pick_up_these_legs":
        dialog.setText(
          "But tell me my little friend, where did you pick up these legs? They make me feel a little strange..."
        );
        dialog.setAudioClip("lvl2_where_did_you_pick_up_these_legs");
        dialog.icon = "surprised";
        break;

    }

    return dialog;

  }


}
