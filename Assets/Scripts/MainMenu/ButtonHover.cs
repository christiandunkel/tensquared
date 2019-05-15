using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHover : MonoBehaviour
{

  Animator animator;

  void Start()
  {
    animator = gameObject.GetComponent<Animator>();
  }

  private bool isHovering = false;

  public void Update()
  {
    animator.SetBool("Hovering", isHovering);
  }

  public void hoverEnter()
  {
    isHovering = true;
  }

  public void hoverExit()
  {
    isHovering = false;
  }

}
