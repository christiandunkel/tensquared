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
  }

  public float delayBeforePush = 1.0f;
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

  public void PlayAnimation()
  {
    pistonAni.SetTrigger("PushUp");
    PlayerController.Instance.setValue("steppedOnPiston", true);
  }

  // Update is called once per frame
  void Update()
  {
    if (pistonIsPlaying && timer <= 0.0f)
    {
      pistonIsPlaying = false;
      PlayAnimation();
    }
    else
    {
      timer -= Time.deltaTime;
    }
  }
}
