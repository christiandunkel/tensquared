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

  public void setSettings(params object[] settings) {

    /*
     * sets multiple boolean settings with one method
     * required format = setSettings(Level.name, bool value, Level.name2, bool value2,...)
     */

    if (settings.Length % 2 != 0) {
      Log.Error($"Requested settings change with uneven number of parameters ({settings.Length}).", this);
      return;
    }

    for (int i = 0; i < settings.Length; i++) {

      // all even numbers must be paramter names
      if (i % 2 == 0) {
        if (settings[i + 1].GetType() == typeof(bool)) {
          setSetting((int)settings[i], (bool)settings[i + 1]);
        }
        else {
          Log.Error($"This method can only set boolean settings " + 
                    $"and can't be given a {settings[i + 1].GetType().ToString()}.", this);
        }
      }

    }

  }

  public void setSetting(int name, bool value) {

    /*
      * changes a settings value to the given value
      */

    switch (name) {

      case Level.CAN_MOVE:
        canMove = value;
        PlayerManager.Instance.setSetting(name, value);
        break;

      case Level.CAN_JUMP:
        canJump = value;
        PlayerManager.Instance.setSetting(name, value);
        break;

      case Level.CAN_SELF_DESTRUCT:
        canSelfDestruct = value;
        PlayerManager.Instance.setSetting(name, value);
        break;

      case Level.CAN_MORPH_TO_CIRCLE:
        canMorphToCircle = value;
        PlayerManager.Instance.setSetting(name, value);
        break;

      case Level.CAN_MORPH_TO_TRIANGLE:
        canMorphToTriangle = value;
        PlayerManager.Instance.setSetting(name, value);
        break;

      case Level.CAN_MORPH_TO_RECTANGLE:
        canMorphToRectangle = value;
        PlayerManager.Instance.setSetting(name, value);
        break;

      default:
        Log.Warn($"Setting with the id '{name}' couldn't be found.", this);
        break;

    }

  }
  
  public void setSetting(int name, Vector2 pos) {

    /*
     * changes a settings value to the given value
     */

    switch (name) {

      case Level.WORLD_SPAWN:
        worldSpawn = pos;
        break;

      case Level.PLAYER_SPAWN:
        playerSpawn = pos;
        break;

      default:
        Log.Warn($"Setting with the id '{name}' couldn't be found.", this);
        break;

    }

  }

  public int getInt(int name) {

    /*
     * gets an integer variable's value
     */

    switch (name) {

      case Level.LEVEL_ID:
        return levelID;

    }

    Log.Warn($"Setting with the id '{name}' couldn't be found.", this);
    return 0;

  }

  public bool getBool(int name) {

    /*
     * gets a boolean variable's value
     */

    switch (name) {

      case Level.CAN_MOVE:
        return canMove;

      case Level.CAN_JUMP:
        return canJump;

      case Level.CAN_SELF_DESTRUCT:
        return canSelfDestruct;

      case Level.CAN_MORPH_TO_CIRCLE:
        return canMorphToCircle;

      case Level.CAN_MORPH_TO_TRIANGLE:
        return canMorphToTriangle;

      case Level.CAN_MORPH_TO_RECTANGLE:
        return canMorphToRectangle;

      case Level.ENABLE_GROUND_COLLIDERS:
        return enableGroundColliders;

    }

    Log.Warn($"Setting with the id '{name}' couldn't be found.", this);
    return false;

  }

  public Vector2 getVector2(int name) {

    /*
     * gets a vector2 variable's value
     */

    switch (name) {

      case Level.WORLD_SPAWN:
        return worldSpawn;

      case Level.PLAYER_SPAWN:
        return playerSpawn;

    }

    Log.Warn($"Setting with the id '{name}' couldn't be found.", this);
    return Vector2.zero;

  }

}
