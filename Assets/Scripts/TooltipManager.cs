using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoBehaviour {


  public static TooltipManager Instance;

  private static GameObject tooltipManager;
  private static List<GameObject> tooltipList;

  void Awake() {

    Instance = this;

    tooltipManager = gameObject;
    tooltipList = new List<GameObject>();

    // add all children (tooltips) to list
    foreach (Transform child in tooltipManager.transform) {
      tooltipList.Add(child.gameObject);
      child.gameObject.SetActive(true);
    }

  }

  public static void showTooltip(string name) {

    GameObject obj = null;

    foreach (GameObject o in tooltipList) {
      if (o.name == name) {
        obj = o;
        o.GetComponent<Animator>().SetBool("Visible", true);
      }
      else {
        o.GetComponent<Animator>().SetBool("Visible", false);
      }
    }

    if (obj == null) {
      Debug.Log("TooltipManager: Could not find tooltip with the name " + name);
      return;
    }

  }

  public static void hideTooltip(string name)
  {

    GameObject obj = null;

    foreach (GameObject o in tooltipList)
    {
      if (o.name == name)
      {
        obj = o;
        break;
      }
    }

    if (obj == null)
    {
      Debug.Log("TooltipManager: Could not find tooltip with the name " + name);
      return;
    }
    else
    {
      obj.GetComponent<Animator>().SetBool("Visible", false);
    }

  }

  public static void hideTooltips()  {

    foreach (GameObject o in tooltipList) {
      o.GetComponent<Animator>().SetBool("Visible", false);
    }

  }

}
