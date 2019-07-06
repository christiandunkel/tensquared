using System.Collections.Generic;
using UnityEngine;

/*
 * manages the tool tips displayed throughout the levels
 */

public class TooltipManager : MonoBehaviour {

  /*
   * =================
   * === SINGLETON ===
   * =================
   */

  public static TooltipManager Instance;

  private void Awake() {
    Instance = this;
    initialize();
  }





  /*
   * ==================
   * === COMPONENTS ===
   * ==================
   */

  private static GameObject tooltipManager;
  private static List<GameObject> tooltipList;





  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  private void initialize() {

    /*
     * initializes the manager
     */

    Instance = this;

    tooltipManager = gameObject;
    tooltipList = new List<GameObject>();

    // add all children (tooltips) to list
    foreach (Transform child in tooltipManager.transform) {
      tooltipList.Add(child.gameObject);
      child.gameObject.SetActive(true);
    }

    Log.Print($"Initialized on object named '{gameObject.name}'.", this);

  }





  /*
   * ================
   * === EXTERNAL ===
   * ================
   */

  public static void showTooltip(string name) {

    /*
     * fades in a tooltip with a specific name
     */

    GameObject obj = null;

    foreach (GameObject o in tooltipList) {
      if (o.name == name) {
        obj = o;
      }
      else {
        o.GetComponent<Animator>().SetBool("Visible", false);
      }
    }

    if (obj == null) {
      Log.Warn($"Could not find the tooltip '{name}'.", Instance.gameObject);
      return;
    }

    obj.GetComponent<Animator>().SetBool("Visible", true);
    Log.Print($"Displaying the tooltip '{name}'.", Instance.gameObject);

  }

  public static void hideTooltip(string name) {

    /*
     * hides a tooltip with a specific name
     */

    GameObject obj = null;

    foreach (GameObject o in tooltipList) {
      if (o.name == name) {
        obj = o;
        break;
      }
    }

    if (obj == null) {
      Log.Warn($"Could not find the tooltip '{name}'.", Instance.gameObject);
      return;
    }
    else {
      obj.GetComponent<Animator>().SetBool("Visible", false);
    }

  }

  public static void hideTooltips() {

    /*
     * hides all tooltips
     */

    foreach (GameObject o in tooltipList) {
      o.GetComponent<Animator>().SetBool("Visible", false);
    }

  }

}
