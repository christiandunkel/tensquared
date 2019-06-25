using UnityEngine;

public class LVL2_PickUpLegs : MonoBehaviour {

  private bool playedAlready;

  void Awake() {
    playedAlready = false;
  }

  void OnTriggerEnter2D(Collider2D col) {

    if (!playedAlready && col.gameObject.tag == "Player") {
      playedAlready = true; // only load once
      ScriptedEventsManager.Instance.LoadEvent(2, "pick_up_legs");
    }

  }

}
