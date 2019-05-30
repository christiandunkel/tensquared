﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL1_HideMoveTooltip : MonoBehaviour {

  private void OnTriggerEnter2D(Collider2D col) {
    
    if (col.gameObject.tag == "Player") {
      TooltipManager.hideTooltip("Move");
    }

  }

}
