using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedEventsManager : MonoBehaviour
{

  public static ScriptedEventsManager Instance;

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

  /* ===============
   * === LEVEL 1 ===
   * ===============
   */
  private IEnumerator StartFrequenceLvl1() {

    yield return new WaitForSeconds(9.0f);

    DialogSystem.LoadDialog("lvl1_greeting");

    yield return new WaitForSeconds(5.0f);

  }

}
