using UnityEngine;

/*
 * simple script that triggers an event 
 */

public class LVL4_LevelEnd : MonoBehaviour {

  private bool playedEventAlready;

  private void Awake() {
    playedEventAlready = false;
  }

  private void OnTriggerEnter2D(Collider2D col) {

    if (!playedEventAlready && col.gameObject.tag == "Player") {
      playedEventAlready = true; // only load once
      ScriptedEventsManager.Instance.LoadEvent(4, "level_end");
    }

  }

}
