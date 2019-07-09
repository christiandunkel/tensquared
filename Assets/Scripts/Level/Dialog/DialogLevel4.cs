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

      case "lvl4_where_have_you_gone":
        dialog.setText(
          "Where have you gone, little circle? Why do you run away? " + 
          "I just want to do a few experiments on your scientifically unexplainable body!"
        );
        dialog.setAudioClip("lvl4_where_have_you_gone");
        dialog.icon = "sad";
        dialog.isEvil = true;
        break;

      case "lvl4_four_limbed_animals":
        dialog.setText(
          "Those houses, no, factories... Some weird animals scurry around in there. " + 
          "They have four limbs and love pouring liquids on other liquids. Strange, are they not?"
        );
        dialog.setAudioClip("lvl4_four_limbed_animals");
        dialog.icon = "neutral";
        dialog.isEvil = true;
        break;

      case "lvl4_inspired_me":
        dialog.setText(
          "But, they have inspired me... " + 
          "That is why I threw them out, and now, I use these houses myself... for science projects."
        );
        dialog.setAudioClip("lvl4_inspired_me");
        dialog.icon = "happy";
        dialog.isEvil = true;
        break;

      case "lvl4_new_friends":
        dialog.setText(
          "Since you rolled away, little circle, I made new friends. " + 
          "I have tinkered around a little and created them from the best metal available. " + 
          "I don't need you anymore."
        );
        dialog.setAudioClip("lvl4_new_friends");
        dialog.icon = "laughing";
        dialog.isEvil = true;
        break;

      case "lvl4_hear_explosion":
        dialog.setText(
          "Did I hear an explosion somewhere?",
          "Little circle, did you do something to my friends?"
        );
        dialog.setAudioClip("lvl4_hear_explosion");
        dialog.icon = "surprised";
        dialog.isEvil = true;
        break;

      case "lvl4_jealous":
        dialog.setText(
          "Please don't be jealous and go around hurting my friends! " + 
          "If you want to be my friend so badly, " + 
          "just come here and I will forgive you for everything!"
        );
        dialog.setAudioClip("lvl4_jealous");
        dialog.icon = "angry";
        dialog.isEvil = true;
        break;

      case "lvl4_tea_and_cookies":
        dialog.setText(
          "I have some tea and cookies over here. " + 
          "Do you want them? They are very tasty! Come and get some!"
        );
        dialog.setAudioClip("lvl4_tea_and_cookies");
        dialog.icon = "happy";
        dialog.isEvil = true;
        break;

      case "lvl4_am_i_not_trustworthy":
        dialog.setText(
          "I'm sad that you don't trust me, little circle. " + 
          "Am I not trustworthy? I don't understand."
        );
        dialog.setAudioClip("lvl4_am_i_not_trustworthy");
        dialog.icon = "sad";
        dialog.isEvil = true;
        break;

      case "lvl4_you_are_near":
        dialog.setText(
          "I sometimes feel like I have a strange connection with you, little circle. " + 
          "For example, right now, I can feel that you are near."
        );
        dialog.setAudioClip("lvl4_you_are_near");
        dialog.icon = "neutral";
        dialog.isEvil = true;
        break;

      /*
       * End sequence (evil) 
       */

      case "lvl4_end_evil_1":
        dialog.setText(
          "There you are, little circle! " + 
          "I have waited for you, and you have come! " + 
          "Does that mean, you finally want to support me and science?"
        );
        dialog.setAudioClip("lvl4_end_evil_1");
        dialog.icon = "happy";
        dialog.isEvil = true;
        break;

      case "lvl4_end_evil_2":
        dialog.setText(
          "Let's get you over here."
        );
        dialog.setAudioClip("lvl4_end_evil_2");
        dialog.icon = "laughing";
        dialog.isEvil = true;
        break;

      case "lvl4_end_evil_3":
        dialog.setText(
          "Your body is truly very interesting, little circle."
        );
        dialog.setAudioClip("lvl4_end_evil_3");
        dialog.icon = "happy";
        dialog.isEvil = true;
        break;

      case "lvl4_end_evil_4":
        dialog.setText(
          "What is that? Oh, that's strange..."
        );
        dialog.setAudioClip("lvl4_end_evil_4");
        dialog.icon = "surprised";
        dialog.isEvil = true;
        break;

      case "lvl4_end_evil_5":
        dialog.setText(
          "What is going on? Truly peculiar."
        );
        dialog.setAudioClip("lvl4_end_evil_5");
        dialog.icon = "neutral";
        dialog.isEvil = true;
        break;

      case "lvl4_end_evil_6":
        dialog.setText(
          "Oh no, I was wrong, this isn't good! " + 
          "Make it stop! Stop! Stop! Stop! Stop! Stop... stop... stop... stop..."
        );
        dialog.setAudioClip("lvl4_end_evil_6");
        dialog.icon = "angry";
        dialog.isEvil = true;
        break;

      /*
       * End sequence (normal) 
       */

      case "lvl4_end_normal_1":
        dialog.setText(
          "Where am I? What happened?"
        );
        dialog.setAudioClip("lvl4_end_normal_1");
        dialog.icon = "surprised";
        break;

      case "lvl4_end_normal_2":
        dialog.setText(
          "I feel strange...",
          "I had a dream about a circle.",
          "Can robots dream?"
        );
        dialog.setAudioClip("lvl4_end_normal_2");
        dialog.icon = "neutral";
        break;

      case "lvl4_end_normal_3":
        dialog.setText(
          "Was this not a dream?",
          "Little friend, are you there?",
          "Are you somewhere?"
        );
        dialog.setAudioClip("lvl4_end_normal_3");
        dialog.icon = "neutral";
        break;

      case "lvl4_end_normal_4":
        dialog.setText(
          "Oh, you are down there. " + 
          "Circular as ever, and certainly real... " + 
          "This is strange. You now feel like a part of me."
        );
        dialog.setAudioClip("lvl4_end_normal_4");
        dialog.icon = "surprised";
        break;

      case "lvl4_end_normal_5":
        dialog.setText(
          "Little friend, do you want to tell me what happened over some tea and cookies?"
        );
        dialog.setAudioClip("lvl4_end_normal_5");
        dialog.icon = "happy";
        break;

    }

    return dialog;

  }


}
