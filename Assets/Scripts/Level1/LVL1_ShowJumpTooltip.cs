using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL1_ShowJumpTooltip : MonoBehaviour
{

  private bool hasSeenTooltip;

  private void Awake() {
    hasSeenTooltip = false;
  }

  private void OnTriggerEnter2D(Collider2D col) {

    if (!hasSeenTooltip && col.gameObject.tag == "Player") {
      hasSeenTooltip = true; // only load once
      ScriptedEventsManager.Instance.LoadEvent(1, "jump_tooltip");
    }

  }

}
