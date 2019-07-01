using UnityEngine;

/*
 * simple script that triggers an event 
 */

public class LVL2_CanYouMorphIntoOtherForms : MonoBehaviour {

  private bool playedDialogAlready;

  private void Awake() {
    playedDialogAlready = false;
  }

  private void OnTriggerEnter2D(Collider2D col) {

    if (!playedDialogAlready && col.gameObject.tag == "Player") {
      playedDialogAlready = true; // only load once
      ScriptedEventsManager.Instance.LoadEvent(2, "can_you_morph_into_other_forms");
    }

  }

}
