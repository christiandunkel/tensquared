using System.Collections;

using UnityEngine;

/*
 * powers the toxic bubbles jumping out of the 'toxic water' prefab
 */

public class ToxicWater : MonoBehaviour {

  public GameObject textureObject;
  public GameObject toxicBubble;

  public float secondsBetweenBubbles = 4f;
  private float bubbleSpawnTimer = 0f;

  private float halfWidth = 0f;  // water prefab width / 2 - offset
                                 // applied to the right and left of water x position
                                 // to calculate range in which to spawn toxic bubbles

  private Vector3 pos = Vector3.zero; // water prefab position

  private bool spawnToxicBubbles = true;

  private void Awake() {
    bubbleSpawnTimer = secondsBetweenBubbles;

    float offset = 20f;
    halfWidth = 150f / 2f - offset;
    
    pos = transform.position;

    if (secondsBetweenBubbles <= 2f) {
      spawnToxicBubbles = false;
      Debug.LogError("ToxicWater: secondsBetweenBubbles is " + secondsBetweenBubbles + ", but has to be higher than 1.");
    }
    
  }

  private void Update() {

    if (!spawnToxicBubbles) return;

    bubbleSpawnTimer -= Time.fixedDeltaTime;

    if (bubbleSpawnTimer < 0f) {
      bubbleSpawnTimer = secondsBetweenBubbles;

      if (playerIsInRange()) {
        StartCoroutine(spawnBubble());
      }
      
    }

  }

  private bool playerIsInRange() {

    float validRadius = 180f;

    return (Mathf.Abs(pos.x - PlayerController.playerObject.transform.position.x) <= validRadius);

  }

  private IEnumerator spawnBubble() {

    Vector3 spawnPos = Vector3.zero;

    spawnPos.x = Random.Range(pos.x - halfWidth, pos.x + halfWidth);
    spawnPos.y = pos.y + 15f;

    GameObject newBubble = Instantiate(toxicBubble, spawnPos, Quaternion.identity);
    newBubble.transform.parent = gameObject.transform;
    newBubble.transform.eulerAngles = new Vector3(0f, 0f, Random.Range(-5f, 5f));

    yield return new WaitForSeconds(2f);

    Destroy(newBubble);

    StopCoroutine(spawnBubble());

  }

}
