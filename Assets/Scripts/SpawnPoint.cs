using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

  public Sprite activatedSprite, deactivatedSprite;
  public bool isActivated = false,
              activeInLastFrame = false;

  private GameObject textureObject, spawnPointMessageObject;
  private Vector2 spawnCoordinates;

  void Awake() {
    
    foreach (Transform child in gameObject.transform) {

      GameObject obj = child.gameObject;

      switch (obj.name) {
        case "Texture":
          textureObject = obj;
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
    
    // set it active
    if (isActivated && activeInLastFrame) {
      textureObject.GetComponent<SpriteRenderer>().sprite = activatedSprite;
      spawnPointMessageObject.SetActive(true);
      LevelSettings.Instance.SetSetting("playerSpawn", spawnCoordinates);
    }
    // set it deactive
    else if (!isActivated && !activeInLastFrame) {
      textureObject.GetComponent<SpriteRenderer>().sprite = deactivatedSprite;
      spawnPointMessageObject.SetActive(false);
    }

    // to check if spawn point activated in this frame in next update
    activeInLastFrame = isActivated;

  }

  private void OnTriggerEnter2D(Collider2D collision) {
    
    if (!isActivated && LevelSettings.Instance.playerSpawn.x < spawnCoordinates.x) {

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