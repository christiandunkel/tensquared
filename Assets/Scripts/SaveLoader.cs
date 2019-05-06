using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoader : MonoBehaviour
{
    
  void Start()
  {

    Debug.Log(Resources.Load("test.txt"));

    if (false)
    {
      Debug.Log("yes");
    }
    else
    {
      Debug.Log("no");
    }

  }

}
