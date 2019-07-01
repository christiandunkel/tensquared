using UnityEngine;

/*
 * simple script that triggers an event 
 */

public class LVL2_SecondHandLegs : MonoBehaviour {

  private bool playedAlready;

  private void Awake() {
    playedAlready = false;
  }

  private void OnTriggerEnter2D(Collider2D col) {

    if (!playedAlready && col.gameObject.tag == "Player") {
      playedAlready = true; // only load once
      ScriptedEventsManager.Instance.LoadEvent(2, "second_hand_legs");
    }

  }

}
