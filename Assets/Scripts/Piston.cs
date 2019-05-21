using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston : MonoBehaviour
{

  // singleton
  public static Piston Instance;
  void Awake()
  {
    Instance = this;
    Debug.Log("Piston: Script is attached to " + this.name);
  }

  private const float delayBeforePush = 0.2f;

  private float timer = 0.0f;
  public bool pistonIsPlaying = false;
  private Animator pistonAni;

  public void GoUp(GameObject piston)
  {

    if (timer <= 0.0f)
    {
      pistonIsPlaying = true;
      pistonAni = piston.GetComponent<Animator>();
      timer = delayBeforePush;
    }
    
  }

  // Update is called once per frame
  void Update()
  {
    if (pistonIsPlaying && timer <= 0.0f)
    {
      pistonIsPlaying = false;
      pistonAni.SetTrigger("PushUp");
      PlayerController.Instance.setValue("steppedOnPiston", true);
    }
    else
    {
      timer -= Time.deltaTime;
    }
  }
}
