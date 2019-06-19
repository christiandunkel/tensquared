using UnityEngine;

/*
 * simple script that triggers an event 
 */

public class LVL2_BringMeMyLegs : MonoBehaviour {

  private bool playedDialogAlready;

  private void Awake() {
    playedDialogAlready = false;
  }

  private void OnTriggerEnter2D(Collider2D col) {

    if (!playedDialogAlready && col.gameObject.tag == "Player") {
      playedDialogAlready = true; // only load once
      ScriptedEventsManager.Instance.LoadEvent(1, "lvl2_bring_arms_back");
    }

  }

}
