using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostingEffect : MonoBehaviour
{

  public GameObject ghost;
  public SpriteRenderer textureObject;
  public float delayBetweenGhosts;
  private float ghostDelaySeconds;
  public bool makeGhost = false;

  // Start is called before the first frame update
  void Start()
  {
    ghostDelaySeconds = delayBetweenGhosts;
  }

  // Update is called once per frame
  void Update()
  {

    if (makeGhost)
    {

      if (ghostDelaySeconds > 0)
      {
        ghostDelaySeconds -= Time.deltaTime;
      }
      else
      {
        // generate fading ghost object
        GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
        ghostDelaySeconds = delayBetweenGhosts;
        // scale current ghost to player object
        currentGhost.transform.localScale = this.transform.localScale;
        // get current sprite of player
        Sprite currentSprite = textureObject.GetComponent<SpriteRenderer>().sprite;
        currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
        // destroy the new ghost object after the fade-out animation
        Destroy(currentGhost, 0.5f);
      }

    }
    
  }
}
