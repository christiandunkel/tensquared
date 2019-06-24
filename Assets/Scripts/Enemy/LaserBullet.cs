﻿using System.Collections;

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LaserBullet : MonoBehaviour {

  private float secondsBeforeVanishing = 3f;
  private float speed = 260f;
  private bool hasHit = false;
  private Rigidbody2D rb2d;

  // particle effects
  public GameObject explodeParticles;

  private void Start() {
    rb2d = GetComponent<Rigidbody2D>();
    rb2d.velocity = transform.right * speed;
  }

  private void Update() {
    secondsBeforeVanishing -= Time.fixedDeltaTime;
  }

  private void OnCollisionEnter2D(Collision2D col) {
    
    if (!hasHit || secondsBeforeVanishing <= 0f) {
      hasHit = true;

      if (col.gameObject.tag == "Player") PlayerController.Instance.die();

      StartCoroutine(destroyBullet());
    }

  }

  private IEnumerator destroyBullet() {

    rb2d.velocity = Vector2.zero;

    GetComponent<SpriteRenderer>().sprite = null;
    GetComponent<CapsuleCollider2D>().isTrigger = true;

    SoundController.Instance.playSound("laserBulletHit");

    explodeParticles.SetActive(true);
    explodeParticles.GetComponent<ParticleSystem>().Play(true);

    yield return new WaitForSeconds(.5f);

    // remove the bullet
    LaserTurret.destroyBullet(gameObject);

  }

}
