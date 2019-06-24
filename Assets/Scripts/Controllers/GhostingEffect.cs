using UnityEngine;

/*
 * powers the 'ghosting' effect when a player is moving
 * (leaving behind transparent after images when walking)
 */

public class GhostingEffect : MonoBehaviour {

  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  // ghost prefab
  [SerializeField] private GameObject ghost = null;

  // player's texture
  [SerializeField] private GameObject textureContainer = null;
  [SerializeField] private SpriteRenderer textureObject = null;

  // settings and attributes
  private float delayBetweenGhosts = 0.1f;
  private float durationOfAnimation = 0.3f;
  private float ghostDelaySeconds;
  private bool displayGhost = false;





  /*
   * ===============
   * === GENERAL ===
   * ===============
   */

  // Start is called before the first frame update
  void Start() {
    ghostDelaySeconds = delayBetweenGhosts;
  }

  // Update is called once per frame
  void Update() {

    if (displayGhost) {

      if (ghostDelaySeconds > 0) {
        ghostDelaySeconds -= Time.fixedDeltaTime;
      }
      else {
        ghostDelaySeconds = delayBetweenGhosts;
        spawnGhost();
      }

    }

  }

  private void spawnGhost() {

    /*
     * spawns a single transparent ghosting object
     * at the player's position
     */

    // generate fading ghost object
    GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
    currentGhost.transform.SetParent(transform.parent);

    // rotate ghosting game object like player texture object
    currentGhost.transform.eulerAngles = PlayerManager.Instance.getObject("textureObject").transform.eulerAngles;

    // get current sprite of player and set it as ghost
    Sprite currentSprite = textureObject.GetComponent<SpriteRenderer>().sprite;
    currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;

    // destroy the new ghost object after the fade-out animation
    Destroy(currentGhost, durationOfAnimation);

  }





  /*
   * ==============
   * === SETTER ===
   * ==============
   */

  public void set(bool enabled) {

    /*
     * enable or disabled ghosting effect
     */

    displayGhost = enabled;

  }

  public void enable() {

    /*
     * enable ghosting effect
     */

    displayGhost = true;

  }

  public void disable() {

    /*
     * disable ghosting effect
     */

    displayGhost = false;

  }





  /*
   * ==============
   * === GETTER ===
   * ==============
   */

  public bool getStatus() {

    /*
     * get status of ghosting effect
     * (either if enabled or disabled)
     */

    return displayGhost;

  }

}
