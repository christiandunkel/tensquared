using UnityEngine;

/*
 * powers the 'spawnpoint' prefab
 */

public class SpawnPoint : MonoBehaviour {

  public Sprite activatedSprite, deactivatedSprite;
  public bool isActivated = false,
              activeInLastFrame = false;

  private SpriteRenderer textureObjectSR;
  private GameObject spawnPointMessageObject;
  private Vector2 spawnCoordinates;

  void Awake() {
    
    foreach (Transform child in gameObject.transform) {

      GameObject obj = child.gameObject;

      switch (obj.name) {
        case "Texture":
          textureObjectSR = obj.GetComponent<SpriteRenderer>();
          break;
        case "SpawnPointMessage":
          spawnPointMessageObject = obj;
          spawnPointMessageObject.SetActive(false);
          break;
        case "SpawnPointCoordinates":
          spawnCoordinates = obj.transform.position;
          break;
        default:
          break;
      }

    }

  }

  void Update() {

    // while death animation, display textures in front of player
    if (isActivated && PlayerController.Instance.getBool("isDead")) {
      textureObjectSR.sortingOrder = 8;
    }
    // otherwise display player in front of textures
    else {
      textureObjectSR.sortingOrder = 2;
    }
    
    // only change settings once every time the spawnpoint activated or deactivated
    if (!activeInLastFrame) {

      // set it active
      if (isActivated) {
        textureObjectSR.sprite = activatedSprite;
        spawnPointMessageObject.SetActive(true);
        LevelSettings.Instance.SetSetting("playerSpawn", spawnCoordinates);
        PlayerController.Instance.PlaySound("activateSpawnpointSound");
      }
      // set it deactive
      else {
        textureObjectSR.sprite = deactivatedSprite;
        spawnPointMessageObject.SetActive(false);
      }

    }
    
    // to check if spawn point activated in this frame in next update
    activeInLastFrame = isActivated;

  }

  private void OnTriggerEnter2D(Collider2D collision) {
    
    if (!isActivated && LevelSettings.Instance.playerSpawn.x < spawnCoordinates.x) {

      PlayerController.Instance.setSpawnpoint = true;

      // deactivate all spawn points
      GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
      foreach (GameObject sp in spawnPoints) {
        if (sp != gameObject) {
          SpawnPoint sp_script = sp.GetComponent<SpawnPoint>();
          if (sp_script != null) {
            sp_script.isActivated = false;
          }
        }
      }

      isActivated = true;
    }

  }

}