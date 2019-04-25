using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableButton : MonoBehaviour
{

  public GameObject[] button;

  public void disableButton(int id)
  {
    button[id].GetComponent<Button>().interactable = false;
  }

  public void enableButton(int id)
  {
    button[id].GetComponent<Button>().interactable = true;
  }

}
