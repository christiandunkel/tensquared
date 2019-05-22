using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL1_ShowJumpTooltip : MonoBehaviour
{

  private bool hasSeenJumpTooltip;

  private void Awake()
  {
    hasSeenJumpTooltip = false;
  }

  private void OnTriggerEnter2D(Collider2D col)
  {

    if (hasSeenJumpTooltip)
    {
      return;
    }
    
    if (col.gameObject.tag == "Player")
    {
      // only load once
      hasSeenJumpTooltip = true;
      ScriptedEventsManager.Instance.LoadEvent(1, "jump_tooltip");
    }

  }

}
