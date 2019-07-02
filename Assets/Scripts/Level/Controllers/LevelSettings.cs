using System.Collections.Generic;
using UnityEngine;

/*
 * script managing the global settings for every level
 */

public class LevelSettings : MonoBehaviour {

  /*
   * =================
   * === SINGLETON ===
   * =================
   */

  public static LevelSettings Instance;

  private void Awake() {
    Instance = this;
  }





  /*
   * ================
   * === SETTINGS ===
   * ================
   */

  // id of level scene
  [SerializeField] private int levelID = 1;

  // player stats
  [SerializeField] private bool canMove = true; // if player can use input to influence movement of character
  // if player can jump with space bar
  [SerializeField] private bool canJump = true;
  // can use button '0' to self-destruct
  [SerializeField] private bool canSelfDestruct = true;
  // if player can change the form of the character
  [SerializeField] private bool canMorphToCircle = true;
  [SerializeField] private bool canMorphToTriangle = true;
  [SerializeField] private bool canMorphToRectangle = true;

  // world stats
  [SerializeField] private Vector2 worldSpawn;
  [SerializeField] private Vector2 playerSpawn;

  // ground prefab colliders 
  // if false, ground prefabs won't use their inbuilt colliders,
  // so custom colliders can be set and used for every level
  [SerializeField] private bool enableGroundColliders = false;





  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  private void Start() {

    Log.Print($"Initialised level settings for level {levelID} on object {gameObject.name}.", this);

    // find player object
    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

    // set spawn points at beginning to location of player object on entry in level
    worldSpawn = playerObject.transform.localPosition;
    playerSpawn = playerObject.transform.localPosition;

    if (enableGroundColliders) {

      GameObject[] groundPrefabs = GameObject.FindGameObjectsWithTag("GroundPrefab");

      // go through all placed ground prefabs 
      // and get all of their (and their children's) box collider components
      foreach (GameObject prefab in groundPrefabs) {

        BoxCollider2D[] childColliders = prefab.GetComponentsInChildren<BoxCollider2D>();
        foreach (BoxCollider2D col in childColliders) {
          col.enabled = true;
        }

      }

    }

  }





  /*
   * ================
   * === EXTERNAL ===
   * ================
   */

  public void setSetting(string name, bool value) {

    switch (name) {

      case "canMove":
        canMove = value;
        PlayerManager.Instance.setSetting(name, value);
        break;

      case "canJump":
        canJump = value;
        PlayerManager.Instance.setSetting(name, value);
        break;

      case "canSelfDestruct":
        canSelfDestruct = value;
        PlayerManager.Instance.setSetting(name, value);
        break;

      case "canMorphToCircle":
        canMorphToCircle = value;
        PlayerManager.Instance.setSetting(name, value);
        break;

      case "canMorphToTriangle":
        canMorphToTriangle = value;
        PlayerManager.Instance.setSetting(name, value);
        break;

      case "canMorphToRectangle":
        canMorphToRectangle = value;
        PlayerManager.Instance.setSetting(name, value);
        break;

      default:
        Log.Warn($"Setting {name} couldn't be found.", this);
        break;

    }

  }
  
  public void setSetting(string name, Vector2 pos) {

    switch (name) {

      case "worldSpawn":
        worldSpawn = pos;
        break;

      case "playerSpawn":
        playerSpawn = pos;
        break;

      default:
        Log.Warn($"Setting {name} couldn't be found.", this);
        break;

    }

  }

  public int getInt(string name) {

    switch (name) {

      case "levelID":
        return levelID;

    }

    Log.Warn($"Setting {name} couldn't be found.", this);
    return 0;

  }

  public bool getBool(string name) {

    switch (name) {

      case "canMove":
        return canMove;

      case "canJump":
        return canJump;

      case "canSelfDestruct":
        return canSelfDestruct;

      case "canMorphToCircle":
        return canMorphToCircle;

      case "canMorphToTriangle":
        return canMorphToTriangle;

      case "canMorphToRectangle":
        return canMorphToRectangle;

      case "enableGroundColliders":
        return enableGroundColliders;

    }

    Log.Warn($"Setting {name} couldn't be found.", this);
    return false;

  }

  public Vector2 getVector2(string name) {

    switch (name) {

      case "worldSpawn":
        return worldSpawn;

      case "playerSpawn":
        return playerSpawn;

    }

    Log.Warn($"Setting {name} couldn't be found.", this);
    return Vector2.zero;

  }

}
