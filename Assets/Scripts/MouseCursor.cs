using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour
{

  private Animator animator;

  public bool noParticleEffects = false;

  public ParticleSystem clickEffect;
  public ParticleSystem trailEffect;

  public SpriteRenderer cursorImage;

  private Vector2 mousePos;
  private Vector2 lastMousePos;

  private float trailTimer = 0.0f;
  private float timeBetweenTrailParticles = 0.1f;

  void Start()
  {
    Cursor.visible = false;
    animator = gameObject.GetComponent<Animator>();
    trailTimer = timeBetweenTrailParticles;
  }

  void Update()
  {
    bool pauseMenuExists = PauseMenu.Instance != null;

    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    gameObject.transform.position = mousePos;
    

    if (pauseMenuExists)
    {

      if (PauseMenu.Instance.isPaused) {
        Cursor.visible = true;
        // make image transparent
        cursorImage.color = new Color(1f, 1f, 1f, 0f);
      }
      else {
        // deactivate cursor once more, if visible through bug
        if (Cursor.visible) {
          Cursor.visible = false;
        }
        if (cursorImage.color.a < 1f) {
          // make image visible 
          cursorImage.color = new Color(1f,1f,1f,1f);
        }
      }

    }
    else if (Cursor.visible) {
      Cursor.visible = false;
    }

    // play different animation on holding mouse button
    if (Input.GetMouseButton(0))
    {
      animator.SetBool("IsClicking", true);
    }
    else
    {
      animator.SetBool("IsClicking", false);
    }

    if (!noParticleEffects) {
      playParticles();
    }

    lastMousePos = mousePos;

  }

  private void playParticles()
  {

    // spawn in particle effect on click
    if (Input.GetMouseButtonDown(0))
    {
      GameObject ce = Instantiate(clickEffect.gameObject, gameObject.transform.position, Quaternion.identity);
      ce.transform.SetParent(gameObject.transform.parent.gameObject.transform);
      Destroy(ce, 1.5f);
    }

    // spawn trail particles behind cursor, if it's moving
    if (lastMousePos.x != mousePos.x || lastMousePos.y != mousePos.y)
    {

      if (trailTimer <= 0f)
      {
        trailTimer = timeBetweenTrailParticles;
        GameObject te = Instantiate(trailEffect.gameObject, gameObject.transform.position, Quaternion.identity);
        te.transform.SetParent(gameObject.transform.parent.gameObject.transform);
        Destroy(te, .8f);
      }
      else
      {
        trailTimer -= Time.deltaTime;
      }

    }

  }

}
