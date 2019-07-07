using UnityEngine;

/*
 * powers the 'ghosting' effect when a player is moving
 * (leaving behind transparent after images when walking)
 */

public class GhostingEffect : MonoBehaviour {

  /*
   * ==================
   * === COMPONENTS ===
   * ==================
   */

  // ghost prefab
  [SerializeField] private GameObject ghostPrefab = null;

  // player's texture
  [SerializeField] private GameObject textureObject = null;
  [SerializeField] private SpriteRenderer textureObjectSR = null;





  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

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
  private void Start() {

    ghostDelaySeconds = delayBetweenGhosts;

    Log.Print($"Initialized on object named '{gameObject.name}'.", this);

  }

  // Update is called once per frame
  private void Update() {

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
     * spawns an object with the player's current sprites 
     * transparent at the player's position
     */

    // generate fading ghost object
    GameObject currentGhost = Instantiate(ghostPrefab, transform.position, transform.rotation);
    currentGhost.transform.SetParent(transform.parent);

    // rotate ghosting game object like player texture object
    currentGhost.transform.eulerAngles = textureObject.transform.eulerAngles;

    // get current sprite of player and set it as ghost
    Sprite currentSprite = textureObjectSR.sprite;
    currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;

    // destroy the new ghost object after the fade-out animation
    Destroy(currentGhost, durationOfAnimation);

  }





  /*
   * ================
   * === EXTERNAL ===
   * ================
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

  public bool getStatus() {

    /*
     * get status of ghosting effect
     * (either if enabled or disabled)
     */

    return displayGhost;

  }

  public float getFloat(string name) {

    /*
     * returns the float value of the given name
     */

    switch (name) {

      case "delayBetweenGhosts":
        return delayBetweenGhosts;

      case "durationOfAnimation":
        return durationOfAnimation;

      case "ghostDelaySeconds":
        return ghostDelaySeconds;

    }

    Log.Warn($"Could not find variable of the name {name}.", this);
    return 0f;

  }

}
