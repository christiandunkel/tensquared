﻿using System.Collections.Generic;
using UnityEngine;

/*
 * manages the framework (variables, virtual and global methods, getter, setter)
 * used by the player controller and the physics object
 */

public class PlayerManager : MonoBehaviour {

  /* 
   * ===========================
   * === INSTANCES & OBJECTS ===
   * ===========================
   */

  // singleton
  public static PlayerController Instance;
  public static GameObject playerObject;

  // instances
  private protected static MorphIndicator morphIndicator;
  private protected static SoundController soundController;
  private protected static GhostingEffect ghostingEffect;
  private protected static ScriptedEventsManager scriptedEvents;

  // game objects
  private protected GameObject parentObject;
  [SerializeField] private protected GameObject textureObject;
  [SerializeField] private protected GameObject heldItemObject;
  [SerializeField] private protected GameObject textureContainer;
  [SerializeField] private protected GameObject movementParticles;
  [SerializeField] private protected GameObject deathParticles;
  [SerializeField] private protected GameObject doubleJumpParticles;





  /* 
   * ==============
   * === PHYSIC ===
   * ==============
   */

  // physics components
  private protected Rigidbody2D rb2d;
  private protected ContactFilter2D contactFilter;
  private protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
  private protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
  private protected LineRenderer triangleLineRenderer;

  // physics variables
  [SerializeField] private protected float minGroundNormalY = .65f;
  [SerializeField] private protected float gravityModifier = 4f;
  private protected float maxSpeed;
  private protected float jumpTakeOffSpeed;
  private protected Vector2 velocity;
  private protected Vector2 targetVelocity;
  private protected Vector2 groundNormal;
  private protected const float minMoveDistance = 0.001f;
  private protected const float shellRadius = 0.01f;

  // grounded
  private protected bool grounded;
  private protected float secondsNotGrounded = 0f;
  private protected float secondsSinceLastJump = 0f;
  private protected float secondsAsRectangleFalling = 0f;
  private protected bool groundedInLastFrame = true;

  // movement
  private protected float lastX;
  private protected float lastY;
  private protected bool leftwards; // direction on last movement
  private protected bool movingX;
  private protected bool movingY;

  // double jump
  protected bool inDoubleJump = false;
  protected bool doubleJumpMovementIsAssigned = false;
  protected Vector2 doubleJumpMovement = Vector2.zero;

  // frozen player
  private protected bool isFrozen = false;
  private protected bool frozenInLastFrame = false;
  private protected float frozenYPos = 0.0f;





  /* 
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  // player attributes structure
  [System.Serializable]
  public struct Attributes {
    [SerializeField] public string name;
    [SerializeField] public float gravityModifier;
    [SerializeField] public float maxSpeed;
    [SerializeField] public float jumpTakeOffSpeed;
    [SerializeField] public Sprite sprite;
    [SerializeField] public Color particleColor;
  }
  // saves the attributes of each character state
  public Attributes[] characterAttributes = new Attributes[3];

  // settings (internal)
  private protected bool canMove = false;
  private protected bool canJump = false;
  private protected bool canSelfDestruct = false;
  private protected bool canMorphToCircle = true;
  private protected bool canMorphToTriangle = false;
  private protected bool canMorphToRectangle = false;





  /* 
   * ==============
   * === OTHERS ===
   * ==============
   */

  // camera controls
  [SerializeField] private protected Animator cameraAnimator = null;
  private protected float zoomedInCameraTimer = 0f;
  private protected float zoomedOutCameraTimer = 0f;
  private protected float zoomedOutCameraFarTimer = 0f;
  private protected float zoomedOutCameraCinematicTimer = 0f;
  private protected float zoomedDownCameraTimer = 0f;

  // morph states
  private protected string state = "Circle";
  private protected string newState = "";
  private protected bool isChangingState = false;

  // morphing sprites
  private protected Sprite[] rectToCircle;
  private protected Sprite[] rectToTriangle;
  private protected Sprite[] triangleToCircle;

  // special states
  private protected bool isDead = false;
  private protected bool holdingItem = false;
  private protected bool steppedOnPiston = false;
  private protected bool hasSpawnpointSet = false;





  /* 
   * =========================
   * === FRAMEWORK METHODS ===
   * =========================
   */

  private void Awake() {

    /*
     * initialise important components for framework
     */

    // initialize markers to important objects
    playerObject = gameObject;
    parentObject = gameObject.transform.parent.gameObject;

    // initialize instances
    Instance = GetComponent<PlayerController>();
    ghostingEffect = GetComponent<GhostingEffect>();
    soundController = SoundController.Instance;
    morphIndicator = MorphIndicator.Instance;
    scriptedEvents = ScriptedEventsManager.Instance;

    // initialize physic components
    rb2d = GetComponent<Rigidbody2D>();
    triangleLineRenderer = GetComponent<LineRenderer>();

    // load morphing sprites into memory
    loadMorphAnimationSprites();

    Log.Print($"Initialised player with parent object '{parentObject.name}'.", parentObject);

    OnAwake();

  }

  private void Start() {
    OnStart();
  }

  private void Update() {
    PhysicsUpdate();
    UpdateBeforeVelocity();
    ComputeVelocity();
    UpdateAfterVelocity();
  }

  private void FixedUpdate() {
    OnFixedUpdate();
  }

  // initialisation
  private protected virtual void OnStart() { }
  private protected virtual void OnAwake() { }
  // fixed update
  private protected virtual void OnFixedUpdate() { }
  // update
  private protected virtual void PhysicsUpdate() { }
  private protected virtual void UpdateBeforeVelocity() { }
  private protected virtual void ComputeVelocity() { }
  private protected virtual void UpdateAfterVelocity() { }





  /* 
   * ================
   * === INTERNAL ===
   * ================
   */

  private void loadMorphAnimationSprites() {

    /*
     * loads 'morph animation' sprites into arrays 
     * for the later use in the morphing process
     */

    rectToCircle = AddFiles(Resources.LoadAll<Sprite>("Morph/Rectangle_to_Circle"), "Rectangle", "Circle");
    rectToTriangle = AddFiles(Resources.LoadAll<Sprite>("Morph/Rectangle_to_Triangle"), "Rectangle", "Triangle");
    triangleToCircle = AddFiles(Resources.LoadAll<Sprite>("Morph/Triangle_to_Circle"), "Triangle", "Circle");



    Sprite[] AddFiles(Sprite[] arr, string fileFront, string fileEnd) {

      /*
       * returns all needed files in an array
       */

      Sprite[] newArr = AddFileAtFront(arr, fileFront);
      return AddFileAtEnd(newArr, fileEnd);

    }

    Sprite[] AddFileAtFront(Sprite[] arr, string file) {

      /*
       * adds a file to the front of a sprite array
       * (is sprite for original state e.g. Circle, Triangle, Rectangle)
       */

      Sprite[] newArr = new Sprite[arr.Length + 1];
      for (int i = 0; i < newArr.Length; i++) {
        if (i == 0) {
          switch (file) {
            case "Circle":
              newArr[i] = Resources.Load<Sprite>("Morph/Final/circle_with_light");
              break;
            case "Triangle":
              newArr[i] = Resources.Load<Sprite>("Morph/Final/triangle_with_light");
              break;
            case "Rectangle":
              newArr[i] = Resources.Load<Sprite>("Morph/Final/rectangle_with_light");
              break;
          }
        }
        else newArr[i] = arr[i - 1];
      }
      return newArr;
    }

    Sprite[] AddFileAtEnd(Sprite[] arr, string file) {

      /*
       * adds a file to the end of a sprite array
       * (is sprite for final state e.g. Circle, Triangle, Rectangle)
       */

      Sprite[] newArr = new Sprite[arr.Length + 1];
      for (int i = 0; i < newArr.Length; i++) {
        if (i == newArr.Length - 1) {
          switch (file) {
            case "Circle":
              newArr[i] = Resources.Load<Sprite>("Morph/Final/circle_with_light");
              break;
            case "Triangle":
              newArr[i] = Resources.Load<Sprite>("Morph/Final/triangle_with_light");
              break;
            case "Rectangle":
              newArr[i] = Resources.Load<Sprite>("Morph/Final/rectangle_with_light");
              break;
          }
        }
        else newArr[i] = arr[i];
      }
      return newArr;
    }

  }





  /* 
   * =========================
   * === ATTRIBUTE METHODS ===
   * =========================
   */

  private protected Attributes getAttributes() {

    /*
     * get attributes of current player state
     */

    return getAttributes(state);

  }

  private protected Attributes getAttributes(string stateName) {

    /*
     * get attributes of given state
     */

    // look for given name
    foreach (Attributes a_temp in characterAttributes) {
      if (a_temp.name == stateName) {
        return a_temp;
      }
    }

    // if fails return attributes for current state
    Attributes a = new Attributes();
    foreach (Attributes a_temp in characterAttributes) {
      if (a_temp.name == state) {
        a = a_temp;
        break;
      }
    }
    return a;

  }

  private protected void resetAttributes() {

    /*
     * reset player attributes to those of current player state
     */

    Attributes resA = getAttributes();

    // movement attributes
    gravityModifier = resA.gravityModifier;
    maxSpeed = resA.maxSpeed;
    jumpTakeOffSpeed = resA.jumpTakeOffSpeed;

    // texture
    textureObject.GetComponent<SpriteRenderer>().sprite = resA.sprite;

    // particle color for movement particles
    ParticleSystem.MainModule mainModule = movementParticles.GetComponent<ParticleSystem>().main;
    mainModule.startColor = resA.particleColor;

    // particle color for death particles
    mainModule = deathParticles.GetComponent<ParticleSystem>().main;
    mainModule.startColor = resA.particleColor;
  }





  /*
   * ==============
   * === SETTER ===
   * ==============
   */

  public void setSetting(int name, bool value) {
    
    /*
     * replaces a variable's content
     * and load required methods after settings change
     */

    setValue(name, value);
    morphIndicator.loadMorphIndicators();

    // test if canMove was disabled, and if so, remove all velocity
    if (name == Player.CAN_MOVE && value == false) {
      rb2d.velocity = Vector2.zero;
      doubleJumpMovement = Vector2.zero;
    }

  }

  public void setValue(int name, bool value) {

    /*
     * replaces content of variable with given content
     */

    switch (name) {

      case Player.CAN_MOVE:
        canMove = value;
        break;

      case Player.CAN_JUMP:
        canJump = value;
        break;

      case Player.CAN_SELF_DESTRUCT:
        canSelfDestruct = value;
        break;

      case Player.CAN_MORPH_TO_CIRCLE:
        canMorphToCircle = value;
        break;

      case Player.CAN_MORPH_TO_TRIANGLE:
        canMorphToTriangle = value;
        break;

      case Player.CAN_MORPH_TO_RECTANGLE:
        canMorphToRectangle = value;
        break;

      case Player.HAS_SPAWN_POINT:
        hasSpawnpointSet = value;
        break;

      case Player.STEPPED_ON_PISTON:
        steppedOnPiston = value;
        break;

      case Player.HOLDING_ITEM:
        holdingItem = value;
        break;

      default:
        Log.Warn($"Value with id '{name}' could not be found.", this);
        break;

    }
  }

  protected void loadLevelSettingsIntoPlayer() {

    /*
     * load the level settings from the 'level settings' script
     * and replace the values of the internal variables with them
     */

    LevelSettings settings = LevelSettings.Instance;
    canMove = settings.getBool(Level.CAN_MOVE);
    canJump = settings.getBool(Level.CAN_JUMP);
    canSelfDestruct = settings.getBool(Level.CAN_SELF_DESTRUCT);
    canMorphToCircle = settings.getBool(Level.CAN_MORPH_TO_CIRCLE);
    canMorphToTriangle = settings.getBool(Level.CAN_MORPH_TO_TRIANGLE);
    canMorphToRectangle = settings.getBool(Level.CAN_MORPH_TO_RECTANGLE);

    // reload 'morph indicator' UI in case morph settings are different now
    morphIndicator.loadMorphIndicators();

  }





  /*
   * ==============
   * === GETTER ===
   * ==============
   */

  public bool getBool(int name) {

    /*
     * get the value of a boolean variable in the manager
     */

    switch (name) {

      case Player.CAN_MOVE:
        return canMove;

      case Player.CAN_JUMP:
        return canJump;

      case Player.CAN_SELF_DESTRUCT:
        return canSelfDestruct;

      case Player.CAN_MORPH_TO_CIRCLE:
        return canMorphToCircle;

      case Player.CAN_MORPH_TO_TRIANGLE:
        return canMorphToTriangle;

      case Player.CAN_MORPH_TO_RECTANGLE:
        return canMorphToRectangle;

      case Player.IS_DEAD:
        return isDead;

      case Player.HOLDING_ITEM:
        return holdingItem;

    }

    Log.Warn($"Boolean '{name}' couldn't be found.", this);
    return false;

  }

  public float getFloat(int name) {

    /*
     * get the value of a float variable in the manager
     */

    switch (name) {

      case Player.SECONDS_NOT_GROUNDED:
        return secondsNotGrounded;

      case Player.SECONDS_AS_RECTANGLE_FALLING:
        return secondsAsRectangleFalling;

    }

    Log.Warn($"Float with the id '{name}' couldn't be found.", this);
    return 0f;

  }

  public string getString(int name) {

    /*
     * get the value of a string variable in the manager
     */

    switch (name) {

      case Player.STATE:
        return state;

    }

    Log.Warn($"String with the id '{name}' couldn't be found.", this);
    return null;

  }

  private Sprite[] emptySpriteArray = new Sprite[0];
  public ref Sprite[] getSprites(int name) {

    /*
     * returns a sprite array
     */

    switch (name) {

      case Player.Sprites.TRIANGLE_TO_CIRCLE:
        return ref triangleToCircle;

      case Player.Sprites.RECTANGLE_TO_CIRCLE:
        return ref rectToCircle;

      case Player.Sprites.RECTANGLE_TO_TRIANGLE:
        return ref rectToTriangle;

    }

    Log.Warn($"Sprite array with the id '{name}' couldn't be found.", this);
    return ref emptySpriteArray;

  }

  public GameObject getObject(int name) {

    /*
     * get an object from the manager
     */

    switch (name) {

      case Player.Objects.PARENT_OBJECT:
        return parentObject;

      case Player.Objects.TEXTURE_OBJECT:
        return textureObject;

      case Player.Objects.HELD_ITEM_OBJECT:
        return heldItemObject;

      case Player.Objects.TEXTURE_CONTAINER:
        return textureContainer;

      case Player.Objects.MOVEMENT_PARTICLES:
        return movementParticles;

      case Player.Objects.DEATH_PARTICLES:
        return deathParticles;

      case Player.Objects.DOUBLE_JUMP_PARTICLES:
        return doubleJumpParticles;

    }

    Log.Warn($"Object with the id '{name}' couldn't be found.", this);
    return null;

  }

}
