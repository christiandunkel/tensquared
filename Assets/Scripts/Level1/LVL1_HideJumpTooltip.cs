using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL1_HideJumpTooltip : MonoBehaviour {

  private void OnTriggerEnter2D(Collider2D col) {
    
    if (col.gameObject.tag == "Player") {
      ScriptedEventsManager.Instance.LoadEvent(1, "hide_jump_tooltip");
    }

  }

}
