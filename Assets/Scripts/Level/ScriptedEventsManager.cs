﻿using System.Collections;
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

  void Awake() {

    Instance = this;

    // block start frequence if events or start frequence are disabled
    if (!playStartFrequence || !playEvents) return;

    // start frequence of each level
    switch (levelID) {
      case 1: StartCoroutine(StartFrequenceLvl1()); break;
      default: break;
    }

  }

  public void LoadEvent(int lvl, string name) {

    // only play events of current level
    if (lvl != levelID || !playEvents) return;

    switch (lvl) {
      case 1: LoadLevel1Event(); break;
      case 2: LoadLevel2Event(); break;
      default: break;
    }

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
        default: break;
      }
    }

    void LoadLevel2Event() {
      switch (name) {
        case "morph_tooltip":
          StartCoroutine(Lvl2_MorphTooltip()); break;
        case "morph_to_triangle":
          TooltipManager.hideTooltips(); break;
        default: break;
      }
    }

  }

  /* ===============
   * === LEVEL 1 ===
   * ===============
   */
  private IEnumerator StartFrequenceLvl1() {
    LevelSettings.Instance.SetSetting("canMove", false);
    LevelSettings.Instance.SetSetting("canJump", false);
    LevelSettings.Instance.SetSetting("canMorph", false);
    yield return new WaitForSeconds(4f);
    DialogSystem.LoadDialog("lvl1_hello");
    yield return new WaitForSeconds(8.5f);
    DialogSystem.LoadDialog("lvl1_asleep");
    yield return new WaitForSeconds(5f);
    CameraShake.Instance.Play(2f, 10f, 10f, "earthquake_2_5_secs_loud");
    GameObject.Find("LVL1_SleepingAnimation").SetActive(false);
    GameObject.Find("LVL1_WakeUpAnimation").GetComponent<Animator>().SetTrigger("WakeUp");
    yield return new WaitForSeconds(5f);
    DialogSystem.LoadDialog("lvl1_move");
    yield return new WaitForSeconds(6f);
    TooltipManager.showTooltip("Move");
    yield return new WaitForSeconds(1f);
    virtualCameraAnimator.GetComponent<Animator>().SetTrigger("StartFrequenceOver");
    LevelSettings.Instance.SetSetting("canMove", true);
    StopCoroutine(StartFrequenceLvl1());
  }
  private IEnumerator Lvl1_JumpTooltip() {
    DialogSystem.LoadDialog("lvl1_jump");
    yield return new WaitForSeconds(1.5f);
    LevelSettings.Instance.SetSetting("canJump", true);
    TooltipManager.showTooltip("Jump");
    StopCoroutine(Lvl1_JumpTooltip());
  }
  private IEnumerator Lvl1_RobotAppearScene() {
    LevelSettings.Instance.SetSetting("canMove", false);
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
    LevelSettings.Instance.SetSetting("canMove", true);
    StopCoroutine(Lvl1_RobotAppearScene());
  }
  private IEnumerator Lvl1_PickUpArms() {
    LevelSettings.Instance.SetSetting("canMove", false);
    yield return new WaitForSeconds(.2f);
    DialogSystem.LoadDialog("lvl1_pick_up_arms");
    yield return new WaitForSeconds(4f);
    LevelSettings.Instance.SetSetting("canMove", true);
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
    LevelSettings.Instance.SetSetting("canMove", false);
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
          PlayerController.Instance.PlaySound("robotRepairSound");
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
    
    DialogSystem.LoadDialog("lvl1_where_did_i_leave_my_legs");
    yield return new WaitForSeconds(6f);
    LevelEnd.Instance.endLevel();
    StopCoroutine(Lvl1_RobotGetArmsScene());
  }


  /* ===============
   * === LEVEL 2 ===
   * ===============
   */
  private IEnumerator Lvl2_MorphTooltip() {
    DialogSystem.LoadDialog("lvl2_morph");
    yield return new WaitForSeconds(7f);
    LevelSettings.Instance.SetSetting("canMorph", true);
    TooltipManager.showTooltip("MorphTriangle");
    StopCoroutine(Lvl2_MorphTooltip());
  }

}