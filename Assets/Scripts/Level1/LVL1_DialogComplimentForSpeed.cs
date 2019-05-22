﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL1_DialogComplimentForSpeed : MonoBehaviour
{

  private bool playedDialogAlready;

  private void Awake()
  {
    playedDialogAlready = false;
  }

  private void OnTriggerEnter2D(Collider2D col)
  {

    if (playedDialogAlready)
    {
      return;
    }

    if (col.gameObject.tag == "Player")
    {
      // only load once
      playedDialogAlready = true;
      DialogSystem.LoadDialog("lvl1_quick_compared_to_other_circles");
    }

  }

}