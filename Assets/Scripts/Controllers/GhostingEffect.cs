﻿using UnityEngine;

/*
 * powers the 'ghosting' effect when a player is moving
 */

public class GhostingEffect : MonoBehaviour {

  public GameObject ghost, textureContainer;
  public SpriteRenderer textureObject;
  private float delayBetweenGhosts = 0.1f,
                durationOfAnimation = 0.3f,
                ghostDelaySeconds;
  private bool displayGhost = false;

  // Start is called before the first frame update
  void Start() {
    ghostDelaySeconds = delayBetweenGhosts;
  }

  public void SetGhosting(bool b) {
    displayGhost = b;
  }

  // Update is called once per frame
  void Update() {

    if (!displayGhost) return;

    if (ghostDelaySeconds > 0) {
      ghostDelaySeconds -= Time.fixedDeltaTime;
      return;
    }

    // generate fading ghost object
    GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
    currentGhost.transform.SetParent(transform.parent);
    ghostDelaySeconds = delayBetweenGhosts;

    // rotate ghosting game object like player texture object
    currentGhost.transform.eulerAngles = PlayerManager.Instance.getObject("textureObject").transform.eulerAngles;

    // get current sprite of player and set it as ghost
    Sprite currentSprite = textureObject.GetComponent<SpriteRenderer>().sprite;
    currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;

    // destroy the new ghost object after the fade-out animation
    Destroy(currentGhost, durationOfAnimation);
    
  }

}