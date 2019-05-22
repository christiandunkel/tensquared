using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL1_ShowMorphTooltip : MonoBehaviour
{

  private bool hasSeenTooltip;

  private void Awake()
  {
    hasSeenTooltip = false;
  }

  private void OnTriggerEnter2D(Collider2D col)
  {

    if (hasSeenTooltip)
    {
      return;
    }

    if (col.gameObject.tag == "Player")
    {
      // only load once
      hasSeenTooltip = true;
      ScriptedEventsManager.Instance.LoadEvent(1, "morph_tooltip");
    }

  }

}
