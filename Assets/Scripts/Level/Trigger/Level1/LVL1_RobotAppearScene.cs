using UnityEngine;

/*
 * simple script that triggers an event 
 */

public class LVL1_RobotAppearScene : MonoBehaviour {

  private bool playedSceneAlready;

  private void Awake() {
    playedSceneAlready = false;
  }

  private void OnTriggerEnter2D(Collider2D col) {

    if (!playedSceneAlready && col.gameObject.tag == "Player") {
      playedSceneAlready = true; // only load once
      ScriptedEventsManager.Instance.LoadEvent(1, "robot_appear_scene");
    }

  }

}
