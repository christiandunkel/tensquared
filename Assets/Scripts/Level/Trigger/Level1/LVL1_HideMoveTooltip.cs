using UnityEngine;

/*
 * simple script that triggers an event 
 */

public class LVL1_HideMoveTooltip : MonoBehaviour {

  private bool playedEventAlready;

  private void Awake() {
    playedEventAlready = false;
  }

  private void OnTriggerEnter2D(Collider2D col) {
    
    if (!playedEventAlready && col.gameObject.tag == "Player") {
      playedEventAlready = true; // only load once
      ScriptedEventsManager.Instance.LoadEvent(1, "hide_move_tooltip");
    }

  }

}
