﻿using System.Collections;
using UnityEngine;

/*
 * manages the 'floating block' prefab
 * and how much it sinks into the water on player landing on it
 */

public class FloatingBlock : MonoBehaviour {

  private Rigidbody2D rb2d;
  private SoundController soundController;
  public ParticleSystem splashParticles;
  private float sinkInTimer = 0f;

  void Awake() {

    rb2d = GetComponent<Rigidbody2D>();

    // delay; start up scripted events once other scripts are ready
    StartCoroutine(delayedAwake());

    IEnumerator delayedAwake() {
      // wait for another loop if scripts aren't ready yet
      while (SoundController.Instance == null) {
        yield return new WaitForSeconds(.1f);
      }
      soundController = SoundController.Instance;
      StopCoroutine(delayedAwake());
    }

  }

  void Update() {
    
    // sink the floating block further into water
    // when player lands on it
    if (sinkInTimer > 0f) {
      sinkInTimer -= Time.deltaTime;

      if (rb2d.mass == 1f) {
        soundController.PlaySound("waterSplashFloatingBlockSound");
        splashParticles.Play();

        rb2d.mass = 60f;
      }
      
    }
    else if (rb2d.mass > 1f) {
      rb2d.mass = 1f;
    }

  }

  void OnCollisionStay2D(Collision2D col) {

    if (col.gameObject.tag == "Player") {
      sinkInTimer = 1f;
    }

  }

}
