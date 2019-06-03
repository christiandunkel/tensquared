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
      default: break;
    }

    void LoadLevel1Event() {
      switch (name) {
        case "jump_tooltip": StartCoroutine(Lvl1_JumpTooltip()); break;
        case "morph_tooltip": StartCoroutine(Lvl1_MorphTooltip()); break;
        case "morph_to_triangle": TooltipManager.hideTooltips(); break;
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
    yield return new WaitForSeconds(8f);
    DialogSystem.LoadDialog("lvl1_asleep");
    yield return new WaitForSeconds(8f);
    CameraShake.Instance.Play(2f, 10f, 10f, "earthquake_2_5_secs_loud");
    GameObject.Find("LVL1_SleepingAnimation").SetActive(false);
    GameObject.Find("LVL1_WakeUpAnimation").GetComponent<Animator>().SetTrigger("WakeUp");
    yield return new WaitForSeconds(5f);
    DialogSystem.LoadDialog("lvl1_move");
    yield return new WaitForSeconds(10f);
    virtualCameraAnimator.GetComponent<Animator>().SetTrigger("StartFrequenceOver");
    TooltipManager.showTooltip("Move");
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
  private IEnumerator Lvl1_MorphTooltip() {
    DialogSystem.LoadDialog("lvl1_morph");
    yield return new WaitForSeconds(7f);
    LevelSettings.Instance.SetSetting("canMorph", true);
    TooltipManager.showTooltip("MorphTriangle");
    StopCoroutine(Lvl1_MorphTooltip());
  }

}
