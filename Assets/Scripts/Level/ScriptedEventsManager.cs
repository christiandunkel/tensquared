using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * powers all scripted events in the different levels
 */

public class ScriptedEventsManager : MonoBehaviour {

  public static ScriptedEventsManager Instance;

  public Animator virtualCameraAnimator;
  public int levelID = 1;
  public bool playStartFrequence = true,
              playEvents = true;

  private List<string> eventsAlreadyRun = new List<string>();

  void Awake() {

    Instance = this;

    Debug.Log("ScriptedEventManager: Initialised for level " + levelID + ".");

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
        PlayerManager.Instance != null &&
        DialogSystem.Instance != null &&
        TooltipManager.Instance != null &&
        SoundController.Instance != null
      ) {
        yield return new WaitForSeconds(.1f);
      }

      Debug.Log("ScriptedEventManager: Successfully loaded as all required scripts are loaded.");

      // start frequence of each level
      switch (levelID) {
        case 1: StartCoroutine(StartFrequenceLvl1()); break;
        case 2: StartCoroutine(StartFrequenceLvl2()); break;
      }

      StopCoroutine(delayedAwake());

    }

  }

  

  public void LoadEvent(int lvl, string name) {

    // only play events of current level
    if (lvl != levelID || !playEvents) return;

    // don't play an event twice
    if (eventsAlreadyRun.Contains(lvl + "_" + name)) return;

    switch (lvl) {
      case 1: LoadLevel1Event(); break;
      case 2: LoadLevel2Event(); break;
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
          DialogSystem.LoadDialog("lvl1_dont_jump_into_water"); break;
        case "dialog_about_water_death":
          DialogSystem.LoadDialog("lvl1_not_the_smartest_circle"); break;
        case "dialog_you_are_quick":
          DialogSystem.LoadDialog("lvl1_quick_compared_to_other_circles"); break;
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
        case "lvl2_bring_arms_back":
          DialogSystem.LoadDialog("lvl2_you_are_out_bring_me_legs"); break;
      }
    }

  }





  /* ===============
   * === LEVEL 2 ===
   * ===============
   */


  private IEnumerator StartFrequenceLvl2() {
    yield return new WaitForSeconds(2f);
    DialogSystem.LoadDialog("lvl2_no_legs_over_here");
    yield return new WaitForSeconds(13.5f);

    GameObject robotObj = GameObject.Find("RobotFallingDown");
    SpriteRenderer robotObjSR = robotObj.GetComponent<SpriteRenderer>();
    Sprite takeArmsDown = Resources.Load<Sprite>("RobotGetArmsAnimation/0026"),
           armsAreDown = Resources.Load<Sprite>("RobotGetArmsAnimation/0025"),
           armsAreDown2 = Resources.Load<Sprite>("RobotGetArmsAnimation/0024"),
           lookRight = Resources.Load<Sprite>("Other/level2_robot_look_right");

    robotObj.GetComponent<Animator>().SetTrigger("FallDown");
    yield return new WaitForSeconds(.6f);
    SoundController.Instance.PlaySound("robotScreamSound");
    yield return new WaitForSeconds(2f);
    robotObjSR.sprite = takeArmsDown;
    yield return new WaitForSeconds(.1f);
    robotObjSR.sprite = armsAreDown;
    yield return new WaitForSeconds(.1f);
    robotObjSR.sprite = armsAreDown2;
    yield return new WaitForSeconds(.1f);
    // robot lands on ground
    GameObject.Find("RobotLandingParticles").GetComponent<ParticleSystem>().Play();
    CameraShake.Instance.Play(.4f, 17f, 17f);

    yield return new WaitForSeconds(1.5f);
    robotObjSR.sprite = lookRight;
    yield return new WaitForSeconds(1f);
    DialogSystem.LoadDialog("lvl2_you_are_here_as_well");
    yield return new WaitForSeconds(9f);
    DialogSystem.LoadDialog("lvl2_do_you_want_to_get_out_of_here");
    yield return new WaitForSeconds(12f);

    virtualCameraAnimator.SetTrigger("StartFrequenceOver");
    yield return new WaitForSeconds(2f);
    LevelSettings.Instance.setSetting("canMorphToTriangle", true);
    TooltipManager.showTooltip("MorphTriangle");
    StopCoroutine(StartFrequenceLvl2());
  }
  private IEnumerator Lvl2_FirstMorphToTriangle() {
    TooltipManager.hideTooltips();
    yield return new WaitForSeconds(.3f);
    DialogSystem.LoadDialog("lvl2_full_of_surprises");
    DialogSystem.LoadDialog("lvl2_send_blueprint_for_science");
    yield return new WaitForSeconds(18f);
    TooltipManager.showTooltip("DoubleJumpTriangle");
    yield return new WaitForSeconds(4f);
    TooltipManager.hideTooltip("DoubleJumpTriangle");
    StopCoroutine(Lvl2_FirstMorphToTriangle());
  }





  /* ===============
   * === LEVEL 1 ===
   * ===============
   */

  private IEnumerator StartFrequenceLvl1() {
    LevelSettings.Instance.setSetting("canMove", false);
    LevelSettings.Instance.setSetting("canJump", false);
    LevelSettings.Instance.setSetting("canMorphToCircle", false);
    LevelSettings.Instance.setSetting("canMorphToTriangle", false);
    LevelSettings.Instance.setSetting("canMorphToRectangle", false);
    yield return new WaitForSeconds(4f);
    DialogSystem.LoadDialog("lvl1_hello");
    yield return new WaitForSeconds(8.5f);
    DialogSystem.LoadDialog("lvl1_asleep");
    yield return new WaitForSeconds(5f);
    CameraShake.Instance.Play(2f, 10f, 10f, "earthquake_2_5_secs_loud");
    yield return new WaitForSeconds(2f);
    GameObject.Find("LVL1_SleepingAnimation").SetActive(false);
    GameObject.Find("LVL1_WakeUpAnimation").GetComponent<Animator>().SetTrigger("WakeUp");
    yield return new WaitForSeconds(3f);
    DialogSystem.LoadDialog("lvl1_move");
    yield return new WaitForSeconds(6f);
    TooltipManager.showTooltip("Move");
    yield return new WaitForSeconds(1f);
    LevelSettings.Instance.setSetting("canMove", true);
    virtualCameraAnimator.SetTrigger("StartFrequenceOver");
    StopCoroutine(StartFrequenceLvl1());
  }
  private IEnumerator Lvl1_JumpTooltip() {
    DialogSystem.LoadDialog("lvl1_jump");
    yield return new WaitForSeconds(1.5f);
    LevelSettings.Instance.setSetting("canJump", true);
    TooltipManager.showTooltip("Jump");
    StopCoroutine(Lvl1_JumpTooltip());
  }
  private IEnumerator Lvl1_RobotAppearScene() {
    LevelSettings.Instance.setSetting("canMove", false);
    yield return new WaitForSeconds(.2f);
    CameraShake.Instance.Play(.5f, 1.3f, 1.3f);
    yield return new WaitForSeconds(1f);
    CameraShake.Instance.Play(.5f, 2f, 2f);
    yield return new WaitForSeconds(1.5f);
    CameraShake.Instance.Play(.7f, 3f, 3f);
    yield return new WaitForSeconds(.4f);
    CameraShake.Instance.Play(3f, 3f, 3f, "earthquake_3_secs");
    GameObject.Find("RobotAppearingParticles").GetComponent<ParticleSystem>().Play();
    yield return new WaitForSeconds(.45f);
    GameObject.Find("RobotFigure").GetComponent<Animator>().SetTrigger("RobotAppear");
    yield return new WaitForSeconds(2.5f);
    DialogSystem.LoadDialog("lvl1_its_me");
    yield return new WaitForSeconds(5.5f);
    DialogSystem.LoadDialog("lvl1_arms_are_further_ahead");
    yield return new WaitForSeconds(4f);
    LevelSettings.Instance.setSetting("canMove", true);
    StopCoroutine(Lvl1_RobotAppearScene());
  }
  private IEnumerator Lvl1_PickUpArms() {
    LevelSettings.Instance.setSetting("canMove", false);
    yield return new WaitForSeconds(.2f);
    DialogSystem.LoadDialog("lvl1_pick_up_arms");
    yield return new WaitForSeconds(4f);
    LevelSettings.Instance.setSetting("canMove", true);
    StopCoroutine(Lvl1_PickUpArms());
  }
  private IEnumerator Lvl1_BringArmsBack() {
    GameObject.Find("RoboterArms").SetActive(false);
    PlayerController.Instance.setValue("holdingItem", true);
    yield return new WaitForSeconds(.2f);
    DialogSystem.LoadDialog("lvl1_bring_arms_back");
    StopCoroutine(Lvl1_BringArmsBack());
  }
  private IEnumerator Lvl1_RobotGetArmsScene() {
    LevelSettings.Instance.setSetting("canMove", false);
    PlayerController.Instance.setValue("holdingItem", false);

    SpriteRenderer robotTexture = GameObject.Find("RobotFigureTexture").GetComponent<SpriteRenderer>();
    Sprite[] robotArmsAnimation = Resources.LoadAll<Sprite>("RobotGetArmsAnimation");

    for (int i = 0; i < robotArmsAnimation.Length; i++) {
      robotTexture.sprite = robotArmsAnimation[i];

      float multiplier = 1f;
      switch (i) {
        case 0:
          GameObject.Find("RobotFigure").GetComponent<Animator>().SetBool("RobotStraighten", true);
          yield return new WaitForSeconds(.5f);
          break;
        case 1:
          GameObject.Find("RobotFigure").GetComponent<Animator>().SetBool("RobotStraighten", false);
          SoundController.Instance.PlaySound("robotRepairSound");
          break;
        case 24: multiplier = 6f; break;
        case 28: multiplier = 4f; break;
        default: break;
      }

      yield return new WaitForSeconds(.08f * multiplier);
    }

    // last image of animation -> looks to the right at player

    yield return new WaitForSeconds(.2f);
    DialogSystem.LoadDialog("lvl1_thank_you");
    yield return new WaitForSeconds(6f);

    // return to look straight forward at camera
    robotTexture.sprite = robotArmsAnimation[robotArmsAnimation.Length - 2];
    yield return new WaitForSeconds(.3f);

    // make robot take its arms back down
    int[] sprites = new int[5] { 28, 27, 26, 25, 24 };
    for (int i = 0; i < sprites.Length; i++) {
      robotTexture.sprite = robotArmsAnimation[sprites[i]];
      yield return new WaitForSeconds(.1f);
    }
    yield return new WaitForSeconds(.3f);

    DialogSystem.LoadDialog("lvl1_where_did_i_leave_my_legs");
    yield return new WaitForSeconds(8f);
    LevelEnd.Instance.endLevel();
    StopCoroutine(Lvl1_RobotGetArmsScene());
  }


}
