using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class General : MonoBehaviour
{

  public void disableButton(Button btn)
  {
    btn.interactable = false;
  }

  public void enableButton(Button btn)
  {
    btn.interactable = true;
  }

}
