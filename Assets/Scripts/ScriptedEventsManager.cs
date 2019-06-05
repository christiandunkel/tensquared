using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedEventsManager : MonoBehaviour {

  public static ScriptedEventsManager Instance;

  public Animator virtualCameraAnimator;
  public int levelID = 1;
  public bool playStartFrequence = true;
  public bool playEvents = true;

  void Awake() {

    Instance = this;

    if (!playStartFrequence || !playEvents) {
      return;
    }

    // start frequence of each level
    switch (levelID) {
      case 1: StartCoroutine(StartFrequenceLvl1()); break;
      default: break;
    }

  }

  public void LoadEvent(int lvl, string name) {

    // only play events of current level
    if (lvl != levelID || !playEvents) {
      return;
    }

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
    yield return new WaitForSeconds(5f);
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
    GameObject.Find("RobotAppearingParticles").GetComponent<ParticleSystem>().Play();
    yield return new WaitForSeconds(.45f);
    GameObject.Find("RobotFigure").GetComponent<Animator>().SetTrigger("RobotAppear");
    yield return new WaitForSeconds(2.5f);
    DialogSystem.LoadDialog("lvl1_its_me");
    yield return new WaitForSeconds(5f);
    DialogSystem.LoadDialog("lvl1_arms_are_further_ahead");
    yield return new WaitForSeconds(4f);
    LevelSettings.Instance.SetSetting("canMove", true);
    StopCoroutine(Lvl1_RobotAppearScene());
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
