using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * powers all scripted events in the different levels
 */

public class ScriptedEventsManager : MonoBehaviour {

  public static ScriptedEventsManager Instance;

  public Animator virtualCameraAnimator;
  public bool playStartFrequence = true,
              playEvents = true;

  private List<string> eventsAlreadyRun = new List<string>();

  private void Awake() {

    Instance = this;

    Debug.Log("ScriptedEventManager: Initialised.");

    // block start frequence if events or start frequence are disabled
    if (!playStartFrequence || !playEvents) {
      virtualCameraAnimator.SetTrigger("StartFrequenceOver");
      return;
    }

    // delay; start up scripted events once other scripts are ready
    StartCoroutine(delayedAwake());

    IEnumerator delayedAwake() {

      // wait for another loop if scripts aren't ready yet
      while (
        PlayerManager.Instance == null ||
        DialogSystem.Instance == null ||
        TooltipManager.Instance == null ||
        SoundController.Instance == null
      ) {
        yield return new WaitForSeconds(.1f);
      }

      Debug.Log("ScriptedEventManager: Successfully loaded as all required scripts are loaded.");

      // start frequence of each level
      switch (LevelSettings.Instance.levelID) {
        case 1: StartCoroutine(StartFrequenceLvl1()); break;
        case 2: StartCoroutine(StartFrequenceLvl2()); break;
        case 3: StartCoroutine(StartFrequenceLvl3()); break;
        case 4: StartCoroutine(StartFrequenceLvl4()); break;
      }

      StopCoroutine(delayedAwake());

    }

  }

  

  public void LoadEvent(int lvl, string name) {

    // only play events of current level
    if (lvl != LevelSettings.Instance.levelID || !playEvents) return;

    // don't play an event twice
    if (eventsAlreadyRun.Contains(lvl + "_" + name)) return;

    switch (lvl) {
      case 1:
        LoadLevel1Event(); break;
      case 2:
        LoadLevel2Event(); break;
      case 3:
        LoadLevel3Event(); break;
      case 4:
        LoadLevel4Event(); break;
    }

    // add event to lists of already run events
    eventsAlreadyRun.Add(lvl + "_" + name);

    void LoadLevel1Event() {
      switch (name) {
        case "hide_move_tooltip":
          TooltipManager.hideTooltip("Move"); break;
        case "jump_tooltip":
          StartCoroutine(Lvl1_JumpTooltip()); break;
        case "hide_jump_tooltip":
          TooltipManager.hideTooltip("Jump"); break;
        case "dialog_about_water":
          DialogSystem.loadDialog("lvl1_dont_jump_into_water"); break;
        case "dialog_about_water_death":
          DialogSystem.loadDialog("lvl1_not_the_smartest_circle"); break;
        case "dialog_you_are_quick":
          DialogSystem.loadDialog("lvl1_quick_compared_to_other_circles"); break;
        case "robot_appear_scene":
          StartCoroutine(Lvl1_RobotAppearScene()); break;
        case "dialog_pick_up_arms":
          StartCoroutine(Lvl1_PickUpArms()); break;
        case "dialog_bring_arms_back":
          StartCoroutine(Lvl1_BringArmsBack()); break;
        case "robot_get_arms_scene":
          StartCoroutine(Lvl1_RobotGetArmsScene()); break;
      }
    }

    void LoadLevel2Event() {
      switch (name) {
        case "morph_to_triangle":
          StartCoroutine(Lvl2_FirstMorphToTriangle()); break;
        case "bring_arms_back":
          DialogSystem.loadDialog("lvl2_you_are_out"); break;
        case "can_you_morph_into_other_forms":
          StartCoroutine(LVL2_CanYouMorphIntoOtherForms()); break;
        case "rectangle_morph_praises":
          StartCoroutine(LVL2_RectangleMorphPraises()); break;
        case "breakable_block":
          DialogSystem.loadDialog("lvl2_breakable_block"); break;
        case "smash_right":
          DialogSystem.loadDialog("lvl2_smash_right"); break;
        case "second_hand_legs":
          DialogSystem.loadDialog("lvl2_second_hand_legs"); break;
        case "pick_up_legs":
          StartCoroutine(LVL2_PickUpLegs()); break;
        case "morph_force_field":
          StartCoroutine(LVL2_ForceField()); break;
        case "receive_arms":
          StartCoroutine(LVL2_ReceiveArms()); break;
      }
    }

    void LoadLevel3Event() {
      switch (name) {
        case "robot_humming2":
          DialogSystem.loadDialog("lvl3_robot_humming2"); break;
        case "robot_humming3":
          DialogSystem.loadDialog("lvl3_robot_humming3"); break;
      }
    }

    void LoadLevel4Event() {
      switch (name) {
        case "":
          break;
      }
    }

  }





  /* ===============
   * === LEVEL 4 ===
   * ===============
   */


  private IEnumerator StartFrequenceLvl4() {
    yield return new WaitForSeconds(1f);
    StopCoroutine(StartFrequenceLvl4());
  }





  /* ===============
   * === LEVEL 3 ===
   * ===============
   */


  private IEnumerator StartFrequenceLvl3() {
    yield return new WaitForSeconds(5f);
    DialogSystem.loadDialog("lvl3_robot_humming1");
    yield return new WaitForSeconds(5f);
    StopCoroutine(StartFrequenceLvl3());
  }




    /* ===============
     * === LEVEL 2 ===
     * ===============
     */


  private IEnumerator StartFrequenceLvl2() {
    LevelSettings.Instance.setSetting("canMorphToCircle", true);
    LevelSettings.Instance.setSetting("canMorphToTriangle", false);
    LevelSettings.Instance.setSetting("canMorphToRectangle", false);
    
    // disabled 2nd voice linerenderer for standing robot,
    // until the robot stands up at the end of the level
    LineRenderer robotStandUpVoiceLineRenderer = DialogSystem.getLineRenderer("RobotStandUpVoiceLineRenderer");
    robotStandUpVoiceLineRenderer.gameObject.SetActive(false);

    yield return new WaitForSeconds(2f);
    DialogSystem.loadDialog("lvl2_no_legs_over_here");
    yield return new WaitForSeconds(13.5f);

    GameObject robotObject = GameObject.Find("RobotFallingDown");
    GameObject robotObjectTexture = GameObject.Find("RobotFallingDownTexture");

    robotObjectTexture.GetComponent<Animator>().SetBool("FallDown", true);
    yield return new WaitForSeconds(.6f);
    SoundController.Instance.playSound("robotScreamSound");
    yield return new WaitForSeconds(2.3f);
    // robot lands on ground
    GameObject.Find("RobotLandingParticles").GetComponent<ParticleSystem>().Play();
    CameraShake.Instance.play(.4f, 34f, 34f);

    yield return new WaitForSeconds(1.5f);
    robotObjectTexture.GetComponent<Animator>().SetBool("LookRight", true);
    yield return new WaitForSeconds(1f);
    DialogSystem.loadDialog("lvl2_you_are_here_as_well");
    yield return new WaitForSeconds(9f);
    robotObjectTexture.GetComponent<Animator>().SetBool("LookRight", false);
    DialogSystem.loadDialog("lvl2_do_you_want_to_get_out_of_here");
    yield return new WaitForSeconds(12f);

    virtualCameraAnimator.SetTrigger("StartFrequenceOver");
    yield return new WaitForSeconds(.3f);
    LevelSettings.Instance.setSetting("canMorphToTriangle", true);
    TooltipManager.showTooltip("MorphTriangle");
    StopCoroutine(StartFrequenceLvl2());
  }
  private IEnumerator Lvl2_FirstMorphToTriangle() {
    TooltipManager.hideTooltip("MorphTriangle");
    yield return new WaitForSeconds(.9f);
    TooltipManager.showTooltip("DoubleJumpTriangle");
    yield return new WaitForSeconds(3f);
    TooltipManager.hideTooltip("DoubleJumpTriangle");
    yield return new WaitForSeconds(.5f);
    DialogSystem.loadDialog("lvl2_full_of_surprises");
    StopCoroutine(Lvl2_FirstMorphToTriangle());
  }
  private IEnumerator LVL2_CanYouMorphIntoOtherForms() {

    GameObject robotObject = GameObject.Find("RobotFallingDown");
    GameObject robotObjectTexture = GameObject.Find("RobotFallingDownTexture");

    robotObjectTexture.GetComponent<Animator>().SetBool("FallDown", false);
    yield return new WaitForSeconds(.2f);
    robotObjectTexture.GetComponent<Animator>().SetBool("FallDown", true);
    yield return new WaitForSeconds(.2f);
    robotObject.transform.position = new Vector2(611f, 189.2f);
    yield return new WaitForSeconds(.4f);
    SoundController.Instance.playSound("robotScreamSound");
    yield return new WaitForSeconds(2.3f);
    // robot lands on ground
    GameObject.Find("RobotLandingParticles").GetComponent<ParticleSystem>().Play();
    CameraShake.Instance.play(.4f, 34f, 34f);

    yield return new WaitForSeconds(1.65f);
    DialogSystem.loadDialog("lvl2_can_you_morph_into_other_forms");

    yield return new WaitForSeconds(6.5f);

    LevelSettings.Instance.setSetting("canMorphToRectangle", true);
    TooltipManager.showTooltip("MorphRectangle");
    StopCoroutine(LVL2_CanYouMorphIntoOtherForms());

  }
  private IEnumerator LVL2_RectangleMorphPraises() {
    TooltipManager.hideTooltip("MorphRectangle");
    yield return new WaitForSeconds(.5f);
    DialogSystem.loadDialog("lvl2_rectangle_great");
    GameObject.Find("OnlyProceedAsRectangle").SetActive(false);
    StopCoroutine(LVL2_RectangleMorphPraises());
  }
  private IEnumerator LVL2_ForceField() {
    yield return new WaitForSeconds(.8f);
    DialogSystem.loadDialog("lvl2_force_fields_everywhere");
    StopCoroutine(LVL2_ForceField());
  }

  private GameObject robotLegSmokeParticles = null;
  private GameObject robotLegSmokeParticles2 = null;

  private IEnumerator LVL2_PickUpLegs() {
    GameObject.Find("RobotLegs").SetActive(false);
    PlayerManager.Instance.setValue("holdingItem", true);
    yield return new WaitForSeconds(.3f);
    DialogSystem.loadDialog("lvl2_found_legs");

    robotLegSmokeParticles = GameObject.Find("RobotLegSmokeParticles");
    robotLegSmokeParticles2 = GameObject.Find("RobotLegSmokeParticles2");
    robotLegSmokeParticles.SetActive(false);
    robotLegSmokeParticles2.SetActive(false);

    StopCoroutine(LVL2_PickUpLegs());
  }

  private IEnumerator LVL2_ReceiveArms() {

    LevelSettings.Instance.setSetting("canSelfDestruct", false);
    LevelSettings.Instance.setSetting("canMove", false);
    LevelSettings.Instance.setSetting("canJump", false);
    LevelSettings.Instance.setSetting("canMorphToCircle", false);
    LevelSettings.Instance.setSetting("canMorphToTriangle", false);
    LevelSettings.Instance.setSetting("canMorphToRectangle", false);

    GameObject robotObject = GameObject.Find("RobotFallingDown");
    GameObject robotObjectTexture = GameObject.Find("RobotFallingDownTexture");

    robotObjectTexture.GetComponent<Animator>().SetBool("FallDown", false);
    yield return new WaitForSeconds(.2f);
    robotObjectTexture.GetComponent<Animator>().SetBool("FallDown", true);
    yield return new WaitForSeconds(.2f);
    robotObject.transform.position = new Vector2(3900f, 248f);
    yield return new WaitForSeconds(.4f);
    SoundController.Instance.playSound("robotScreamSound");
    yield return new WaitForSeconds(2.3f);
    // robot lands on ground
    GameObject.Find("RobotLandingParticles").GetComponent<ParticleSystem>().Play();
    CameraShake.Instance.play(.4f, 34f, 34f);
    yield return new WaitForSeconds(1.2f);

    DialogSystem.loadDialog("lvl2_thank_you_my_little_friend");
    yield return new WaitForSeconds(9f);

    SpriteRenderer getLegsObjectSR = GameObject.Find("RobotAttachingLegsTexture").GetComponent<SpriteRenderer>();
    SpriteRenderer standUpObjectSR = GameObject.Find("RobotStandUpTexture").GetComponent<SpriteRenderer>();
    Sprite[] getLegsSprites = Resources.LoadAll<Sprite>("RobotGetLegsAnimation");
    int spriteNum = getLegsSprites.Length;

    // repair sound
    SoundController.Instance.playSound("robotRepairSound");

    // remove legs from player
    PlayerManager.Instance.setValue("holdingItem", false);

    // hide 'fallen down' robot
    robotObjectTexture.SetActive(false);

    // play 'attaching legs' animation
    for (int i = 0; i < spriteNum; i++) {
      if (i < 32) {
        if (i == 28) {
          yield return new WaitForSeconds(.4f);
        }
        getLegsObjectSR.sprite = getLegsSprites[i];
      }
      else {
        if (i == 32) {
          getLegsObjectSR.sprite = null;
        }
        else if (i == 39) {
          yield return new WaitForSeconds(1.1f);
          SoundController.Instance.playSound("robotElectricDefect");
          yield return new WaitForSeconds(.3f);
          robotLegSmokeParticles.SetActive(true);
          robotLegSmokeParticles.GetComponent<ParticleSystem>().Play();
          robotLegSmokeParticles2.SetActive(true);
          robotLegSmokeParticles2.GetComponent<ParticleSystem>().Play();
        }
        standUpObjectSR.sprite = getLegsSprites[i];
      }
      yield return new WaitForSeconds(.08f);
    }

    // robot is standing up, activate 2nd line renderer
    LineRenderer robotStandUpVoiceLineRenderer = DialogSystem.getLineRenderer("RobotStandUpVoiceLineRenderer");
    robotStandUpVoiceLineRenderer.gameObject.SetActive(true);

    yield return new WaitForSeconds(3.5f);
    DialogSystem.loadDialog("lvl2_where_did_you_pick_up_these_legs");
    yield return new WaitForSeconds(11f);
    LevelEnd.Instance.endLevel();
    StopCoroutine(LVL2_ReceiveArms());
  }






  /* ===============
   * === LEVEL 1 ===
   * ===============
   */

  private IEnumerator StartFrequenceLvl1() {
    LevelSettings.Instance.setSetting("canSelfDestruct", false);
    LevelSettings.Instance.setSetting("canMove", false);
    LevelSettings.Instance.setSetting("canJump", false);
    LevelSettings.Instance.setSetting("canMorphToCircle", false);
    LevelSettings.Instance.setSetting("canMorphToTriangle", false);
    LevelSettings.Instance.setSetting("canMorphToRectangle", false);

    yield return new WaitForSeconds(4f);
    DialogSystem.loadDialog("lvl1_hello");
    yield return new WaitForSeconds(8.5f);
    DialogSystem.loadDialog("lvl1_asleep");
    yield return new WaitForSeconds(5f);
    CameraShake.Instance.play(2f, 10f, 10f, "earthquake_2_5_secs_loud");
    yield return new WaitForSeconds(2f);
    GameObject.Find("LVL1_SleepingAnimation").SetActive(false);
    GameObject.Find("LVL1_WakeUpAnimation").GetComponent<Animator>().SetTrigger("WakeUp");
    yield return new WaitForSeconds(3f);
    DialogSystem.loadDialog("lvl1_move");
    yield return new WaitForSeconds(6f);
    TooltipManager.showTooltip("Move");
    yield return new WaitForSeconds(1f);
    LevelSettings.Instance.setSetting("canSelfDestruct", true);
    LevelSettings.Instance.setSetting("canMove", true);
    virtualCameraAnimator.SetTrigger("StartFrequenceOver");
    StopCoroutine(StartFrequenceLvl1());
  }
  private IEnumerator Lvl1_JumpTooltip() {
    DialogSystem.loadDialog("lvl1_jump");
    yield return new WaitForSeconds(1.5f);
    LevelSettings.Instance.setSetting("canJump", true);
    TooltipManager.showTooltip("Jump");
    StopCoroutine(Lvl1_JumpTooltip());
  }
  private IEnumerator Lvl1_RobotAppearScene() {
    LevelSettings.Instance.setSetting("canSelfDestruct", false);
    LevelSettings.Instance.setSetting("canMove", false);
    yield return new WaitForSeconds(.2f);
    CameraShake.Instance.play(.5f, 2f, 2f);
    yield return new WaitForSeconds(1f);
    CameraShake.Instance.play(.5f, 3f, 3f);
    yield return new WaitForSeconds(1.5f);
    CameraShake.Instance.play(.7f, 5f, 5f);
    yield return new WaitForSeconds(.4f);
    CameraShake.Instance.play(3f, 10f, 10f, "earthquake_3_secs");
    GameObject.Find("RobotAppearingParticles").GetComponent<ParticleSystem>().Play();
    yield return new WaitForSeconds(.45f);
    Animator lvl1robotAnimator = GameObject.Find("RobotFigure").GetComponent<Animator>();
    lvl1robotAnimator.SetTrigger("RobotAppear");
    yield return new WaitForSeconds(2.5f);
    lvl1robotAnimator.SetTrigger("RobotStraighten");
    yield return new WaitForSeconds(.45f);
    DialogSystem.loadDialog("lvl1_its_me");
    yield return new WaitForSeconds(5.5f);
    DialogSystem.loadDialog("lvl1_arms_are_further_ahead");
    yield return new WaitForSeconds(4f);
    LevelSettings.Instance.setSetting("canMove", true);
    LevelSettings.Instance.setSetting("canSelfDestruct", true);
    StopCoroutine(Lvl1_RobotAppearScene());
  }
  private IEnumerator Lvl1_PickUpArms() {
    LevelSettings.Instance.setSetting("canSelfDestruct", false);
    LevelSettings.Instance.setSetting("canMove", false);
    yield return new WaitForSeconds(.2f);
    DialogSystem.loadDialog("lvl1_pick_up_arms");
    yield return new WaitForSeconds(4f);
    LevelSettings.Instance.setSetting("canMove", true);
    LevelSettings.Instance.setSetting("canSelfDestruct", true);
    StopCoroutine(Lvl1_PickUpArms());
  }
  private IEnumerator Lvl1_BringArmsBack() {
    GameObject.Find("RoboterArms").SetActive(false);
    PlayerManager.Instance.setValue("holdingItem", true);
    yield return new WaitForSeconds(.2f);
    DialogSystem.loadDialog("lvl1_bring_arms_back");
    StopCoroutine(Lvl1_BringArmsBack());
  }
  private IEnumerator Lvl1_RobotGetArmsScene() {
    LevelSettings.Instance.setSetting("canSelfDestruct", false);
    LevelSettings.Instance.setSetting("canMove", false);

    PlayerManager.Instance.setValue("holdingItem", false);

    SpriteRenderer robotTexture = GameObject.Find("RobotFigureTexture").GetComponent<SpriteRenderer>();
    Sprite[] robotArmsAnimation = Resources.LoadAll<Sprite>("RobotGetArmsAnimation");

    for (int i = 0; i < robotArmsAnimation.Length; i++) {
      robotTexture.sprite = robotArmsAnimation[i];

      float multiplier = 1f;
      switch (i) {
        case 0:
          yield return new WaitForSeconds(.2f);
          break;
        case 1:
          SoundController.Instance.playSound("robotRepairSound");
          break;
        case 24: multiplier = 6f; break;
        case 27: multiplier = 4f; break;
        default: break;
      }

      yield return new WaitForSeconds(.08f * multiplier);
    }

    // make robot take its arms back down
    int[] sprites = new int[5] { 28, 27, 26, 25, 24 };
    for (int i = 0; i < sprites.Length; i++) {
      robotTexture.sprite = robotArmsAnimation[sprites[i]];
      yield return new WaitForSeconds(.1f);
    }
    yield return new WaitForSeconds(.3f);


    yield return new WaitForSeconds(.2f);
    DialogSystem.loadDialog("lvl1_thank_you");
    yield return new WaitForSeconds(6.3f);

    DialogSystem.loadDialog("lvl1_where_did_i_leave_my_legs");
    yield return new WaitForSeconds(5.2f);

    DialogSystem.loadDialog("lvl1_im_off");

    yield return new WaitForSeconds(7.4f);

    /* ROBOT FLY OFF ANIMATION */

    // animate the robot to fly off
    GameObject.Find("RobotFigure").GetComponent<Animator>().SetTrigger("FlyOff");

    // deactive voice line renderer on robot
    // (as it would stay in position, while robot flys up)
    LineRenderer[] voiceLineRenderers = DialogSystem.getLineRenderers();
    foreach (LineRenderer lr in voiceLineRenderers) {
      lr.gameObject.SetActive(false);
    }

    // play 'take off' sound effect
    SoundController.Instance.playSound("earthquake_2_secs");
    yield return new WaitForSeconds(2.3f);
    SoundController.Instance.playSound("earthquake_3_secs");
    yield return new WaitForSeconds(6f);

    LevelEnd.Instance.endLevel();
    StopCoroutine(Lvl1_RobotGetArmsScene());
  }


}
