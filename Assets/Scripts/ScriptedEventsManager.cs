﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedEventsManager : MonoBehaviour
{

  public static ScriptedEventsManager Instance;

  public Animator virtualCameraAnimator;

  public int levelID = 1;

  void Awake()
  {
    Instance = this;

    // start frequence of each level
    switch (levelID) {

      case 1:
        StartCoroutine(StartFrequenceLvl1());
        break;

      default:
        break;
    }

  }

  public void LoadEvent(int lvl, string name) {

    // only play events of current level
    if (lvl != levelID) {
      return;
    }

    switch (lvl)
    {

      case 1: LoadLevel1Event(name); break;

      default:
        break;

    }

  }

  /* ===============
   * === LEVEL 1 ===
   * ===============
   */
  private IEnumerator StartFrequenceLvl1() {

    yield return new WaitForSeconds(9f);

    DialogSystem.LoadDialog("lvl1_hello");

    yield return new WaitForSeconds(11f);

    DialogSystem.LoadDialog("lvl1_asleep");

    yield return new WaitForSeconds(6f);

    CameraShake.Instance.Play(1f, 6f, 6f);

    yield return new WaitForSeconds(2f);

    DialogSystem.LoadDialog("lvl1_wake_up");

    yield return new WaitForSeconds(10f);

    CameraShake.Instance.Play(2f, 10f, 10f);
    GameObject.Find("LVL1_SleepingAnimation").SetActive(false);
    GameObject.Find("LVL1_WakeUpAnimation").SetActive(true);

    yield return new WaitForSeconds(1.5f);

    GameObject.Find("LVL1_WakeUpAnimation").SetActive(false);

    yield return new WaitForSeconds(6.5f);

    DialogSystem.LoadDialog("lvl1_move");

    yield return new WaitForSeconds(10f);

    virtualCameraAnimator.GetComponent<Animator>().SetTrigger("StartFrequenceOver");

    TooltipManager.showTooltip("Move");

    LevelSettings.Instance.SetSetting("canMove", true);

  }

  private float water_deaths = 0;

  private void LoadLevel1Event(string name) {

    switch (name) {

      case "water_death":
        water_deaths++;
        if (water_deaths == 1) {
          DialogSystem.LoadDialog("lvl1_water_death");
        }
        else if (water_deaths == 2) {
          DialogSystem.LoadDialog("lvl1_water_death2");
        }
        break;

      default:
        break;

    }

  }

}