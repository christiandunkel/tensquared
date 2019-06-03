using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL1_DialogAboutWater : MonoBehaviour
{

  private bool playedDialogAlready;

  private void Awake() {
    playedDialogAlready = false;
  }

  private void OnTriggerEnter2D(Collider2D col) {

    if (!playedDialogAlready && col.gameObject.tag == "Player") {
      playedDialogAlready = true; // only load once
      ScriptedEventsManager.Instance.LoadEvent(1, "dialog_about_water");
    }

  }

}
