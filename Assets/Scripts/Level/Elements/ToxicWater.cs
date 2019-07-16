using System.Collections;

using UnityEngine;

/*
 * powers the toxic bubbles jumping out of the 'toxic water' prefab
 */

public class ToxicWater : MonoBehaviour {

  /* 
   * ==================
   * === COMPONENTS ===
   * ==================
   */

  [SerializeField] private GameObject toxicBubble = null;
  private SoundController soundController;





  /* 
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  public float secondsBetweenBubbles = 5f;
  private float bubbleSpawnTimer = 0f;

  private float halfWidth = 0f;  // water prefab width / 2 - offset
                                 // applied to the right and left of water x position
                                 // to calculate range in which to spawn toxic bubbles

  private Vector3 pos = Vector3.zero; // water prefab position

  private bool spawnToxicBubbles = true;





  /* 
   * ================
   * === INTERNAL ===
   * ================
   */

  private void Awake() {

    soundController = SoundController.Instance;

    bubbleSpawnTimer = secondsBetweenBubbles;

    float offset = 20f;
    halfWidth = 150f / 2f - offset;
    
    pos = transform.position;

    if (secondsBetweenBubbles <= 2f) {
      spawnToxicBubbles = false;
      Log.Error($"Variable 'secondsBetweenBubbles' is {secondsBetweenBubbles}, but has to be higher than 2.", this);
    }
    
  }

  private void Update() {

    if (!spawnToxicBubbles) {
      return;
    }

    bubbleSpawnTimer -= Time.deltaTime;

    if (bubbleSpawnTimer < 0f) {

      // reset timer, and add some randomness to it
      bubbleSpawnTimer = secondsBetweenBubbles + Random.Range(0f, secondsBetweenBubbles / 3f);

      if (playerIsInRange()) {
        StartCoroutine(spawnBubble());
      }
      
    }

  }

  private bool playerIsInRange() {

    /*
     * tests if the player is in range for 
     * the toxic bubble spawning process to activate
     */

    return Util.distanceOnAxisX(pos, PlayerManager.playerObject) <= 230f;

  }

  private IEnumerator spawnBubble() {

    /*
     * spawns a toxic bubble (prefab) jumping out of the water
     */

    // determine spawn position of toxic bubble
    Vector3 spawnPos = Vector3.zero;
    spawnPos.x = Random.Range(pos.x - halfWidth, pos.x + halfWidth);
    spawnPos.y = pos.y + 15f;

    // position where the player currently is
    float playerXPos = PlayerManager.Instance.gameObject.transform.position.x;

    // range on x axis, where player is located - don't spawn toxic bubbles there
    float rangeNoSpawnOffset = 20f;
    float rangeNoSpawnMin = playerXPos - rangeNoSpawnOffset;
    float rangeNoSpawnMax = playerXPos + rangeNoSpawnOffset;

    // toxic bubble is spawning below the player -> change position
    if (spawnPos.x < rangeNoSpawnMax && spawnPos.x > rangeNoSpawnMin) {

      // offset toxic bubble by given offset, so it doesn't spawn below player
      if (spawnPos.x > playerXPos) {
        spawnPos.x += rangeNoSpawnOffset;
      }
      else {
        spawnPos.x -= rangeNoSpawnOffset;
      }

    }

    // create a new toxic bubble object with the calculated position
    GameObject newBubble = Instantiate(toxicBubble, spawnPos, Quaternion.identity);
    newBubble.transform.parent = gameObject.transform;
    newBubble.transform.eulerAngles = new Vector3(0f, 0f, Random.Range(-5f, 5f));

    soundController.playSound("bubbleIndicatorSound");

    // wait a moment, while "spawn position indicator" particles of the bubble play
    yield return new WaitForSeconds(1.2f);
    soundController.playSound("bubbleShootOutSound");
    yield return new WaitForSeconds(.8f);

    // wait for toxic bubble to go through its whole animation
    yield return new WaitForSeconds(2f);

    Destroy(newBubble);

    StopCoroutine(spawnBubble());

  }

}
