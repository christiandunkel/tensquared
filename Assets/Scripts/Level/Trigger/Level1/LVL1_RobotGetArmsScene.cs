using UnityEngine;

/*
 * simple script that triggers an event 
 */

public class LVL1_RobotGetArmsScene : MonoBehaviour {

  private bool playedSceneAlready;

  private void Awake() {
    playedSceneAlready = false;
  }

  private void OnTriggerEnter2D(Collider2D col) {

    if (!playedSceneAlready && col.gameObject.tag == "Player" && PlayerManager.Instance.getBool("holdingItem")) {
      playedSceneAlready = true; // only load once
      ScriptedEventsManager.Instance.LoadEvent(1, "robot_get_arms_scene");
    }

  }

}
