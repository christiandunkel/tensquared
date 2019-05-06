using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AboutMenu : MonoBehaviour
{

  public GameObject[] obj = null;
  public GameObject[] btns = null;
  private Vector2[] startPos = null;

  private float moveValue = 839.0f;

  void Start()
  {

    startPos = new Vector2[obj.Length];

    int counter = 0;
    foreach (GameObject o in obj)
    {
      startPos[counter] = new Vector2(o.transform.localPosition.x, o.transform.localPosition.y);
      counter++;
    }

  }

  private float duration = 1.2f;
  private float timer = 0.0f;

  void Update()
  {
    if (move > 0)
    {

      // new run, disable button
      if (timer == 0.0f)
      {
        foreach (GameObject b in btns) {
          b.GetComponent<Button>().interactable = false;
        }
      }

      timer += Time.deltaTime / duration;

      int counter = 0;
      foreach (GameObject o in obj)
      {

        Vector2 posA = new Vector2(startPos[counter].x, startPos[counter].y + moveValue);

        if (move == 1)
        {
          o.transform.localPosition = Vector2.Lerp(startPos[counter], posA, timer);
        }
        else if (move == 2)
        {
          o.transform.localPosition = Vector2.Lerp(posA, startPos[counter], timer);
        }

        counter++;

      }

      // end of animation
      if (timer > duration)
      {
        move = 0;
        timer = 0;

        // enable all buttons again
        foreach (GameObject b in btns)
        {
          b.GetComponent<Button>().interactable = true;
        }

      }

    }
  }

  private int move = 0;
  private int lastMove = 2;

  public void MoveCamera()
  {
    if (lastMove == 2)
    {
      move = 1;
      lastMove = 1;
    }
    else
    {
      move = 2;
      lastMove = 2;
    }
  }


}
