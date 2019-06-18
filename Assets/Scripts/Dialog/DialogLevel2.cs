using UnityEngine;

public class DialogLevel2 : MonoBehaviour {

  public static Dialog getDialog(string name) {

    Dialog dialog = new Dialog();

    switch (name) {

      case "lvl2_no_legs_over_here":
        dialog.setText(
          "Legs? Over here? No legs? No, that's not good.",
          "Oh, maybe there.No ? What about down there?"
        );
        dialog.setAudioClip("lvl2_no_legs_over_here");
        dialog.icon = "annoyed";
        break;

      case "lvl2_aaaahh":
        dialog.setText(
          "Ahhhhhh..."
        );
        dialog.setAudioClip("lvl2_aaaahh");
        dialog.icon = "neutral";
        break;

      case "lvl2_you_are_here_as_well":
        dialog.setText(
          "Oh, you are here as well, little friend?",
          "That's truly an interesting coincidence, don't you think?"
        );
        dialog.setAudioClip("lvl2_you_are_here_as_well");
        dialog.icon = "happy";
        break;

      case "lvl2_do_you_want_to_get_out_of_here":
        dialog.setText(
          "Do you want to get out of here, little friend?",
          "You are a smart circle, you should be able to think of a solution!"
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

      case "lvl2_send_blueprint_for_science":
        dialog.setText(
          "You truly have to sent me a blueprint of how to make one of you...",
          "So I can tinker around a little... for science."
        );
        dialog.setAudioClip("lvl2_send_blueprint_for_science");
        dialog.icon = "happy";
        break;

      case "lvl2_you_are_out_bring_me_legs":
        dialog.setText(
          "Now that you out there, do you mind looking around for a robot leg or two?",
          "I have misplaced mine.Even bringing me a pair of second-hand goods will do...",
          "Although I will feel very sad inside..."
        );
        dialog.setAudioClip("lvl2_you_are_out_bring_me_legs");
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
          "That's great! You can even increase your mass! Wonderful!",
          "Although, according to my calculations...",
          "It's scientifically impossible... probably."
        );
        dialog.setAudioClip("lvl2_rectangle_great");
        dialog.icon = "surprised";
        break;

      case "lvl2_if_i_have_to_be_honest":
        dialog.setText(
          "If you are worrying, don't! The new look fits you quite well.", 
          "If I have to be honest, you looked a little scrawny before."
        );
        dialog.setAudioClip("lvl2_if_i_have_to_be_honest");
        dialog.icon = "happy";
        break;

      case "lvl2_breakable_block":
        dialog.setText(
          "This metallic thing looks quite brittle, don't you think?",
          "Maybe letting something heavy fall on top of it from a certain distance will break it...",
          "Well, that's lucky."
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

      case "lvl2_force_fields_everywhere":
        dialog.setText(
          "You can find those blue blobs all over the place, like litter, destroying the landscape.",
          "At least, they sound nice. And sometimes that's all that counts, don't you think?"
        );
        dialog.setAudioClip("lvl2_breakable_block");
        dialog.icon = "neutral";
        break;

      case "lvl2_thank_you_my_little_friend":
        dialog.setText(
          "Thank you very much, my little friend! You're a life-safer!",
          "I will certainly repay you!"
        );
        dialog.setAudioClip("lvl2_thank_you_my_little_friend");
        dialog.icon = "happy";
        break;

      case "lvl2_tea_and_cookies":
        dialog.setText(
          "Why don't you visit me in the future for some tea and cookies?",
          "But don't forget to bring your scientifically impossible body with you!"
        );
        dialog.setAudioClip("lvl2_tea_and_cookies");
        dialog.icon = "laughing";
        break;

      case "lvl2_where_did_you_pick_up_these_legs":
        dialog.setText(
          "Why don't you visit me in the future for some tea and cookies?",
          "But don't forget to bring your scientifically impossible body with you!"
        );
        dialog.setAudioClip("lvl2_where_did_you_pick_up_these_legs");
        dialog.icon = "surprised";
        break;

    }

    return dialog;

  }


}
