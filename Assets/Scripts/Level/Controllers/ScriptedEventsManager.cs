using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * powers all scripted events in the different levels
 */

public class ScriptedEventsManager : MonoBehaviour {

  /*
   * =================
   * === SINGLETON ===
   * =================
   */

  public static ScriptedEventsManager Instance;

  private void Awake() {
    Instance = this;
    initialize();
  }





  /*
   * ==================
   * === COMPONENTS ===
   * ==================
   */

  [SerializeField] private Animator virtualCameraAnimator = null;





  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  private List<string> eventsAlreadyRun = new List<string>();

  [SerializeField] private bool playStartFrequence = true;
  [SerializeField] private bool playEvents = true;





  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  private void initialize() {

    /*
     * initializes the script
     */

    // clear list of 'events already run' on start/restart of level
    eventsAlreadyRun.Clear();

    Log.Print($"Initialised on object named {gameObject.name}.", this);

    // block start frequence if events or start frequence are disabled
    if (!playStartFrequence || !playEvents) {
      virtualCameraAnimator.SetTrigger("StartFrequenceOver");
      return;
    }

    // delay; start up scripted events once other scripts are ready
    StartCoroutine(delayedAwake());

    IEnumerator delayedAwake() {

      // wait for another loop if scripts aren't ready yet
      while (
        PlayerManager.Instance == null ||
        DialogSystem.Instance == null ||
        TooltipManager.Instance == null ||
        SoundController.Instance == null
      ) {
        yield return new WaitForSeconds(.1f);
      }

      Log.Print($"Successfully loaded as all required scripts.", this);

      // start frequence of each level
      switch (LevelSettings.Instance.getInt("levelID")) {

        case 1:
          StartCoroutine(StartFrequenceLvl1());
          break;

        case 2:
          StartCoroutine(StartFrequenceLvl2());
          break;

        case 3:
          StartCoroutine(StartFrequenceLvl3());
          break;

        case 4:
          StartCoroutine(StartFrequenceLvl4());
          break;

      }

      StopCoroutine(delayedAwake());

    }

  }

  

  public void LoadEvent(int lvl, string name) {

    // only play events of current level
    if (lvl != LevelSettings.Instance.getInt("levelID") || !playEvents) {
      return;
    }

    // don't play an event twice
    if (eventsAlreadyRun.Contains(lvl + "_" + name)) {
      return;
    }

    switch (lvl) {

      case 1:
        LoadLevel1Event();
        break;

      case 2:
        LoadLevel2Event();
        break;

      case 3:
        LoadLevel3Event();
        break;

      case 4:
        LoadLevel4Event();
        break;

    }

    // add event to lists of already run events
    eventsAlreadyRun.Add(lvl + "_" + name);

    void LoadLevel1Event() {
      switch (name) {
        case "hide_move_tooltip":
          TooltipManager.hideTooltip("Move");
          break;
        case "jump_tooltip":
          StartCoroutine(Lvl1_JumpTooltip());
          break;
        case "hide_jump_tooltip":
          TooltipManager.hideTooltip("Jump");
          break;
        case "dialog_about_water":
          DialogSystem.loadDialog("lvl1_dont_jump_into_water");
          break;
        case "dialog_about_water_death":
          DialogSystem.loadDialog("lvl1_not_the_smartest_circle");
          break;
        case "dialog_you_are_quick":
          DialogSystem.loadDialog("lvl1_quick_compared_to_other_circles");
          break;
        case "robot_appear_scene":
          StartCoroutine(Lvl1_RobotAppearScene());
          break;
        case "dialog_pick_up_arms":
          StartCoroutine(Lvl1_PickUpArms());
          break;
        case "dialog_bring_arms_back":
          StartCoroutine(Lvl1_BringArmsBack());
          break;
        case "robot_get_arms_scene":
          StartCoroutine(Lvl1_RobotGetArmsScene());
          break;
      }
    }

    void LoadLevel2Event() {
      switch (name) {
        case "morph_to_triangle":
          StartCoroutine(Lvl2_FirstMorphToTriangle());
          break;
        case "bring_arms_back":
          DialogSystem.loadDialog("lvl2_you_are_out");
          break;
        case "can_you_morph_into_other_forms":
          StartCoroutine(LVL2_CanYouMorphIntoOtherForms());
          break;
        case "rectangle_morph_praises":
          StartCoroutine(LVL2_RectangleMorphPraises());
          break;
        case "breakable_block":
          DialogSystem.loadDialog("lvl2_breakable_block");
          break;
        case "smash_right":
          DialogSystem.loadDialog("lvl2_smash_right");
          break;
        case "second_hand_legs":
          DialogSystem.loadDialog("lvl2_second_hand_legs");
          break;
        case "pick_up_legs":
          StartCoroutine(LVL2_PickUpLegs());
          break;
        case "morph_force_field":
          StartCoroutine(LVL2_ForceField());
          break;
        case "receive_arms":
          StartCoroutine(LVL2_ReceiveArms());
          break;
      }
    }

    void LoadLevel3Event() {
      switch (name) {
        case "robot_humming2":
          DialogSystem.loadDialog("lvl3_robot_humming2");
          break;
        case "robot_humming3":
          DialogSystem.loadDialog("lvl3_robot_humming3");
          break;
        case "like_the_melody":
          DialogSystem.loadDialog("lvl3_EVIL_like_the_melody");
          break;
        case "neck_nose_ear_doctor":
          DialogSystem.loadDialog("lvl3_neck_nose_ear_specialist");
          DialogSystem.loadDialog("lvl3_EVIL_beautiful_melody");
          break;
        case "smoking_legs":
          DialogSystem.loadDialog("lvl3_smoking_legs");
          break;
        case "safe_laser_turrets":
          GameObject.Find("MalfunctioningLaserTurret").GetComponent<LaserTurret>().disable();
          GameObject.Find("MalfunctioningLaserTurret2").GetComponent<LaserTurret>().disable();
          DialogSystem.loadDialog("lvl3_EVIL_fruit_juice_shooters");
          break;
        case "surprised_about_laser_cannons":
          DialogSystem.loadDialog("lvl3_surprised_about_laser_cannons");
          break;
        case "set_on_fire":
          DialogSystem.loadDialog("lvl3_EVIL_set_on_fire");
          break;
        case "toxic_gases":
          DialogSystem.loadDialog("lvl3_toxic_gases");
          break;
        case "end_scene":
          StartCoroutine(LVL3_EndScene());
          break;
      }
    }

    void LoadLevel4Event() {
      switch (name) {
        case "four_limbed_animals":
          DialogSystem.loadDialog("lvl4_four_limbed_animals");
          DialogSystem.loadDialog("lvl4_inspired_me");
          break;
        case "new_friends":
          StartCoroutine(LVL4_NewFriends());
          break;
        case "bomberling_explosion":
          StartCoroutine(LVL4_BomberlingExplosion());
          break;
        case "jealous":
          StartCoroutine(LVL4_Jealous());
          break;
        case "tea_and_cookies":
          DialogSystem.loadDialog("lvl4_tea_and_cookies");
          break;
        case "am_i_untrustworthy":
          DialogSystem.loadDialog("lvl4_am_i_not_trustworthy");
          break;
        case "you_are_near":
          DialogSystem.loadDialog("lvl4_you_are_near");
          break;
        case "level_end":
          StartCoroutine(LVL4_EndScene());
          break;
      }
    }

  }





  /* ===============
   * === LEVEL 4 ===
   * ===============
   */

  private GameObject voiceLineRendererRedLvl4 = null;
  private GameObject voiceLineRendererBlueLvl4 = null;
  private AudioClip evilElectroSwingTheme = null;
  private LineRenderer leftEyeLaserLR = null;
  private LineRenderer rightEyeLaserLR = null;
  private IEnumerator StartFrequenceLvl4() {

    // load evil electro swing theme for later use
    evilElectroSwingTheme = Resources.Load<AudioClip>("Music/EvilElectroSwingTheme");

    // find voice line renderers in level 4, and deactivate blue one
    // for it to be reactivated at the end of the level
    voiceLineRendererRedLvl4 = GameObject.Find("VoiceLineRendererRed");
    voiceLineRendererBlueLvl4 = GameObject.Find("VoiceLineRendererBlue");
    voiceLineRendererBlueLvl4.SetActive(false);

    // eye laser elements; deactivate, then reactivate at a later point
    leftEyeLaserLR = GameObject.Find("LeftEyeLaserLineRenderer").GetComponent<LineRenderer>();
    rightEyeLaserLR = GameObject.Find("RightEyeLaserLineRenderer").GetComponent<LineRenderer>();
    leftEyeLaserLR.gameObject.SetActive(false);
    rightEyeLaserLR.gameObject.SetActive(false);

    yield return new WaitForSeconds(3f);
    DialogSystem.loadDialog("lvl4_where_have_you_gone");
    yield return new WaitForSeconds(9f);
    virtualCameraAnimator.SetTrigger("StartFrequenceOver");
    yield return new WaitForSeconds(0.1f);
    StopCoroutine(StartFrequenceLvl4());
  }
  private AudioSource backgroundMusicPlayer = null;
  private IEnumerator LVL4_NewFriends() {
    DialogSystem.loadDialog("lvl4_new_friends");
    yield return new WaitForSeconds(1.5f);
    // mute the music
    backgroundMusicPlayer = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
    backgroundMusicPlayer.mute = true;
    StopCoroutine(LVL4_NewFriends());
  }
  private IEnumerator LVL4_BomberlingExplosion() {
    yield return new WaitForSeconds(1.5f);
    DialogSystem.loadDialog("lvl4_hear_explosion");
    StopCoroutine(LVL4_BomberlingExplosion());
  }
  private IEnumerator LVL4_Jealous() {
    DialogSystem.loadDialog("lvl4_jealous");
    yield return new WaitForSeconds(1.5f);
    // play evil electro swing theme
    backgroundMusicPlayer.clip = evilElectroSwingTheme;
    backgroundMusicPlayer.mute = false;
    backgroundMusicPlayer.Play();
    StopCoroutine(LVL4_Jealous());
  }
  private IEnumerator LVL4_EndScene() {

    // freeze player
    LevelSettings.Instance.setSetting("canMove", false);
    LevelSettings.Instance.setSetting("canJump", false);
    LevelSettings.Instance.setSetting("canSelfDestruct", false);
    LevelSettings.Instance.setSetting("canMorphToCircle", false);
    LevelSettings.Instance.setSetting("canMorphToTriangle", false);
    LevelSettings.Instance.setSetting("canMorphToRectangle", false);

    // get robot object at end of level
    GameObject robot = GameObject.Find("RobotEndTexture");
    SpriteRenderer robotSR = robot.GetComponent<SpriteRenderer>();

    // load animation sprites
    Sprite[] robotTurningWholeAnimation = Resources.LoadAll<Sprite>("RobotTurnWholeAnimation");
    int spritesNumber = robotTurningWholeAnimation.Length;

    // load references to morphing sprites for later use
    Sprite[] rectToCircleSprites = PlayerManager.Instance.getSprites("rectToCircle");
    Sprite[] rectToTriangleSprites = PlayerManager.Instance.getSprites("rectToTriangle");
    Sprite[] triangleToCircleSprites = PlayerManager.Instance.getSprites("triangleToCircle");

    // eye laser elements
    Vector3 leftEyePos = GameObject.Find("LaserPositionLeftEye").transform.position;
    Vector3 rightEyePos = GameObject.Find("LaserPositionRightEye").transform.position;

    // play some dialogue
    DialogSystem.loadDialog("lvl4_end_evil_1");
    yield return new WaitForSeconds(11f);
    DialogSystem.loadDialog("lvl4_end_evil_2");
    yield return new WaitForSeconds(3f);

    // create player-replacement object
    GameObject playerReplacement = new GameObject("PlayerReplacement");
    SpriteRenderer playerReplacementSR = playerReplacement.AddComponent<SpriteRenderer>();
    playerReplacement.transform.SetParent(PlayerManager.Instance.getObject("parentObject").transform);

    // adjust replacement objects transform values to be the same as the players
    GameObject playerTextureObject = PlayerManager.Instance.getObject("textureObject");
    playerReplacement.transform.position = playerTextureObject.transform.position;
    playerReplacement.transform.eulerAngles = playerTextureObject.transform.eulerAngles;
    Vector3 newScale = playerTextureObject.transform.localScale;
    newScale.x *= playerTextureObject.transform.parent.gameObject.transform.localScale.x;
    newScale.y *= playerTextureObject.transform.parent.gameObject.transform.localScale.y;
    playerReplacement.transform.localScale = (newScale);

    // set proper sorting layer and order
    SpriteRenderer playerTextureObjectSR = playerTextureObject.GetComponent<SpriteRenderer>();
    playerReplacementSR.sortingLayerName = playerTextureObjectSR.sortingLayerName;
    playerReplacementSR.sortingOrder = playerTextureObjectSR.sortingOrder;

    // make player invisible, but replacement in place
    playerReplacementSR.sprite = playerTextureObjectSR.sprite;
    playerTextureObjectSR.sprite = null;

    // animate laser beams from robots eyes onto player
    leftEyeLaserLR.gameObject.SetActive(true);
    rightEyeLaserLR.gameObject.SetActive(true);
    bool stopAnimatedEyeLaser = false;
    StartCoroutine(animateEyeLaser());

    IEnumerator animateEyeLaser() {
      while (!stopAnimatedEyeLaser) {
        Vector3 playerPos = playerReplacement.transform.position;
        // draw laser beams from robot eyes to the player
        leftEyeLaserLR.SetPositions(new Vector3[2] { leftEyePos, playerPos });
        rightEyeLaserLR.SetPositions(new Vector3[2] { rightEyePos, playerPos });
        yield return new WaitForFixedUpdate();
      }
      StopCoroutine(animateEyeLaser());
    }

    // move player replacement up to the robot 
    // and move camera (player) in direction of robot as well
    Vector3 playerStartPos = playerReplacement.transform.position;
    Vector3 cameraFinalPos = Vector3.zero;
    cameraFinalPos.x = GameObject.Find("CameraXPos").transform.position.x;
    cameraFinalPos.y = playerStartPos.y;
    Vector3 movePlayerToPos = GameObject.Find("MovePlayerToPosition").transform.position;
    float movePlayerTimer = 0f;
    float animationDuration = 3.5f;
    while (true) {

      // move player replacement
      Vector3 newPlayerPos = Vector3.Lerp(playerStartPos, movePlayerToPos, (movePlayerTimer / animationDuration));
      playerReplacement.transform.position = newPlayerPos;

      // move camera (actual player)
      Vector3 newCameraPos = Vector3.Lerp(playerStartPos, cameraFinalPos, (movePlayerTimer / animationDuration));
      PlayerManager.Instance.gameObject.transform.position = newCameraPos;

      movePlayerTimer += Time.deltaTime;

      if (movePlayerTimer > animationDuration) {
        playerReplacement.transform.position = movePlayerToPos;
        PlayerManager.Instance.gameObject.transform.position = cameraFinalPos;
        break;
      }

      yield return new WaitForFixedUpdate();
    }

    // prepare 'player continuously morphing' animation
    bool stopMorphingPlayer = false;
    Sprite[] morphStates = new Sprite[0];

    // load all morphing sprite arrays into this array
    switch (PlayerManager.Instance.getString("state")) {

      case "Circle":
        morphStates = AddCircleToRectSprites(morphStates);
        morphStates = AddRectToTriangleSprites(morphStates);
        morphStates = AddTriangleToCircleSprites(morphStates);
        break;

      case "Rectangle":
        morphStates = AddRectToTriangleSprites(morphStates);
        morphStates = AddTriangleToCircleSprites(morphStates);
        morphStates = AddCircleToRectSprites(morphStates);
        break;

      case "Triangle":
        morphStates = AddTriangleToCircleSprites(morphStates);
        morphStates = AddCircleToRectSprites(morphStates);
        morphStates = AddRectToTriangleSprites(morphStates);
        break;

    }

    Sprite[] AddCircleToRectSprites(Sprite[] oldArray) {
      int startLength = oldArray.Length;
      Sprite[] spriteArray = new Sprite[startLength + rectToCircleSprites.Length];
      // transfer existing sprites from morphStates to spriteArray
      for (int i = 0; i < startLength; i++) {
        spriteArray[i] = oldArray[i];
      }
      // add new sprites
      for (int i = 0; i < rectToCircleSprites.Length; i++) {
        // reverse rectToCircle Sprite array to circleToRect
        spriteArray[i + startLength] = rectToCircleSprites[rectToCircleSprites.Length - 1 - i];
      }
      return spriteArray;
    }

    Sprite[] AddRectToTriangleSprites(Sprite[] oldArray) {
      int startLength = oldArray.Length;
      Sprite[] spriteArray = new Sprite[startLength + rectToTriangleSprites.Length];
      // transfer existing sprites from morphStates to spriteArray
      for (int i = 0; i < startLength; i++) {
        spriteArray[i] = oldArray[i];
      }
      // add new sprites
      for (int i = 0; i < rectToTriangleSprites.Length; i++) {
        spriteArray[i + startLength] = rectToTriangleSprites[i];
      }
      return spriteArray;
    }

    Sprite[] AddTriangleToCircleSprites(Sprite[] oldArray) {
      int startLength = morphStates.Length;
      Sprite[] spriteArray = new Sprite[startLength + triangleToCircleSprites.Length];
      // transfer existing sprites from morphStates to spriteArray
      for (int i = 0; i < startLength; i++) {
        spriteArray[i] = oldArray[i];
      }
      // add new sprites
      for (int i = 0; i < triangleToCircleSprites.Length; i++) {
        spriteArray[i + startLength] = triangleToCircleSprites[i];
      }
      return spriteArray;
    }
    
    IEnumerator morphPlayerReplacementContinuously() {
      int counter = 0;
      while (!stopMorphingPlayer) {
        playerReplacementSR.sprite = morphStates[counter];
        counter++;
        if (counter > morphStates.Length - 1) {
          counter = 0;
        }
        yield return new WaitForSeconds(0.05f);
      }
      StopCoroutine(morphPlayerReplacementContinuously());
    }

    // play some dialogue
    DialogSystem.loadDialog("lvl4_end_evil_3");
    yield return new WaitForSeconds(4f);
    DialogSystem.loadDialog("lvl4_end_evil_4");

    // continuously morph the player replacement
    // from one state to another (in appearance)
    StartCoroutine(morphPlayerReplacementContinuously());

    // play even more dialogue
    yield return new WaitForSeconds(4f);
    DialogSystem.loadDialog("lvl4_end_evil_5");
    yield return new WaitForSeconds(4f);
    DialogSystem.loadDialog("lvl4_end_evil_6");
    yield return new WaitForSeconds(4f);

    // remove eye lasers
    stopAnimatedEyeLaser = true;
    leftEyeLaserLR.gameObject.SetActive(false);
    rightEyeLaserLR.gameObject.SetActive(false);

    // shrink the player replacement and move it to the robot's shoulder
    Vector3 movePlayerToRobotShoulderPos = GameObject.Find("MovePlayerIntoRobotPosition").transform.position;
    Vector3 playerReplacementStartScale = playerReplacement.gameObject.transform.localScale;
    Vector3 playerReplacementNewScale = playerReplacement.gameObject.transform.localScale / 2.5f;
    playerStartPos = playerReplacement.transform.position;
    movePlayerTimer = 0f;
    animationDuration = 5f;
    while (true) {

      // move player replacement
      Vector3 newPlayerPos = Vector3.Lerp(playerStartPos, movePlayerToRobotShoulderPos, (movePlayerTimer / animationDuration));
      playerReplacement.transform.position = newPlayerPos;

      // shrink player replacement
      Vector3 newPlayerScale = Vector3.Lerp(playerReplacementStartScale, playerReplacementNewScale, (movePlayerTimer / animationDuration));
      playerReplacement.transform.localScale = newPlayerScale;

      movePlayerTimer += Time.deltaTime;

      if (movePlayerTimer > animationDuration) {
        playerReplacement.transform.position = movePlayerToRobotShoulderPos;
        playerReplacement.transform.localScale = playerReplacementNewScale;
        break;
      }

      yield return new WaitForFixedUpdate();
    }

    // shrink the player replacement and move it to the robot's shoulder
    playerStartPos = playerReplacement.transform.position;
    Vector3 movePlayerIntoRobotPos = GameObject.Find("MovePlayerIntoRobotPosition2").transform.position;
    movePlayerTimer = 0f;
    animationDuration = 2f;
    while (true) {

      // move player replacement
      Vector3 newPlayerPos = Vector3.Lerp(playerStartPos, movePlayerIntoRobotPos, (movePlayerTimer / animationDuration));
      playerReplacement.transform.position = newPlayerPos;

      movePlayerTimer += Time.deltaTime;

      if (movePlayerTimer > animationDuration) {
        playerReplacement.transform.position = movePlayerIntoRobotPos;
        break;
      }

      yield return new WaitForFixedUpdate();
    }

    // deactivate all voice line renderers
    voiceLineRendererRedLvl4.SetActive(false);
    voiceLineRendererBlueLvl4.SetActive(false);

    // stop continuously morphing the player replacement
    stopMorphingPlayer = true;
    StopCoroutine(morphPlayerReplacementContinuously());

    // hide player replacement
    playerReplacement.SetActive(false);
    
    // remove robot player cover
    yield return new WaitForSeconds(.1f);
    GameObject.Find("RobotCoverTexture").SetActive(false);

    // play 'electrical' sound effects
    yield return new WaitForSeconds(2f);
    SoundController.Instance.playSound("robotElectricDefect");
    yield return new WaitForSeconds(1.5f);
    SoundController.Instance.playSound("robotElectricDefect");

    // play sprite animation
    for (int i = 0; i < spritesNumber; i++) {

      robotSR.sprite = robotTurningWholeAnimation[i];

      // robot has circle in its heart's place
      if (i == 9) {
        SoundController.Instance.playSound("robotElectricDefect");
        yield return new WaitForSeconds(1f);
        SoundController.Instance.playSound("robotRepairSound");
        yield return new WaitForSeconds(1f);
      }
      // robot has all new parts
      else if (i == 21) {
        SoundController.Instance.playSound("robotElectricDefect");

        // deactivate background music
        if (backgroundMusicPlayer != null) {
          backgroundMusicPlayer.clip = null;
          backgroundMusicPlayer.mute = false;
        }

        // un-loop all smoke particles coming from the robot
        ParticleSystem.MainModule main1 = GameObject.Find("RobotLegSmokeParticles").GetComponent<ParticleSystem>().main;
        main1.loop = false;
        ParticleSystem.MainModule main2 = GameObject.Find("RobotLegSmokeParticles2").GetComponent<ParticleSystem>().main;
        main2.loop = false;
        ParticleSystem.MainModule main3 = GameObject.Find("RobotShoulderSmokeParticles").GetComponent<ParticleSystem>().main;
        main3.loop = false;

        yield return new WaitForSeconds(2f);
      }
      // rotate the robot so it's once again standing vertically
      else if (i > 21 && i < 31) {
        Vector3 newRobotRotation = Vector3.zero;
        newRobotRotation.z = 1f;
        robot.transform.localEulerAngles -= newRobotRotation;
      }

      yield return new WaitForSeconds(.08f);

    }

    // play dialogue
    yield return new WaitForSeconds(2f);
    DialogSystem.loadDialog("lvl4_end_normal_1");
    yield return new WaitForSeconds(.3f);
    // switch voice line renderer to blue
    voiceLineRendererBlueLvl4.SetActive(true);
    yield return new WaitForSeconds(4f);
    DialogSystem.loadDialog("lvl4_end_normal_2");
    yield return new WaitForSeconds(6f);
    DialogSystem.loadDialog("lvl4_end_normal_3");
    yield return new WaitForSeconds(5f);
    DialogSystem.loadDialog("lvl4_end_normal_4");
    yield return new WaitForSeconds(9f);
    DialogSystem.loadDialog("lvl4_end_normal_5");
    yield return new WaitForSeconds(5f);

    // wait for level end
    yield return new WaitForSeconds(7f);
    LevelEnd.Instance.endLevel("levelCompleteSound");

    StopCoroutine(LVL4_EndScene());
  }





  /* ===============
   * === LEVEL 3 ===
   * ===============
   */

  private GameObject voiceLineRendererRedLvl3 = null;
  private GameObject shoulderSmokeParticlesEnd = null;
  private IEnumerator StartFrequenceLvl3() {
    yield return new WaitForSeconds(2f);

    // set evil red voice line renderer inactive for later
    voiceLineRendererRedLvl3 = GameObject.Find("VoiceLineRendererRed");
    voiceLineRendererRedLvl3.SetActive(false);

    // find smote particle system for end animation,
    // and deactivate for reactivation in end sequence
    shoulderSmokeParticlesEnd = GameObject.Find("RobotShoulderSmokeParticles");
    shoulderSmokeParticlesEnd.SetActive(false);

    virtualCameraAnimator.SetTrigger("StartFrequenceOver");
    yield return new WaitForSeconds(3f);
    DialogSystem.loadDialog("lvl3_robot_humming1");
    StopCoroutine(StartFrequenceLvl3());
  }
  private IEnumerator LVL3_EndScene() {
    
    LevelSettings.Instance.setSetting("canMove", false);
    LevelSettings.Instance.setSetting("canJump", false);
    LevelSettings.Instance.setSetting("canSelfDestruct", false);
    LevelSettings.Instance.setSetting("canMorphToCircle", false);
    LevelSettings.Instance.setSetting("canMorphToTriangle", false);
    LevelSettings.Instance.setSetting("canMorphToRectangle", false);

    // load animation sprites
    Sprite[] robotFallingApartAnimation = Resources.LoadAll<Sprite>("RobotFallingApartAnimation");
    int spritesNumber = robotFallingApartAnimation.Length;

    // game object to target for animation at end of level
    SpriteRenderer robotEndObject = GameObject.Find("RobotFallingApartTexture").GetComponent<SpriteRenderer>();

    yield return new WaitForSeconds(1f);
    DialogSystem.loadDialog("lvl3_melody_is_gone");

    yield return new WaitForSeconds(6.5f);
    SoundController.Instance.playSound("robotElectricDefect");

    for (int i = 0; i < spritesNumber; i++) {

      robotEndObject.sprite = robotFallingApartAnimation[i];

      // disabled blue voice line renderer
      if (i == 1) {
        GameObject.Find("VoiceLineRendererBlue").SetActive(false);
      }

      // add pauses in animation (still-frames)
      if (i == 5) {
        yield return new WaitForSeconds(0.4f);
      }
      else if (i == 10) {
        // part of robot is falling and landing on the ground
        SoundController.Instance.playSound("robotPartsFallOff");
      }
      else if (i == 15) {
        // active big shoulder smoke particles
        shoulderSmokeParticlesEnd.SetActive(true);
        shoulderSmokeParticlesEnd.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.2f);
        SoundController.Instance.playSound("robotElectricDefect");
      }
      else if (i == 19) {
        yield return new WaitForSeconds(1f);
        // enable evil red voice line renderer
        voiceLineRendererRedLvl3.SetActive(true);
      }

      yield return new WaitForSeconds(0.1f);

    }

    DialogSystem.loadDialog("lvl3_EVIL_unfortunate");
    DialogSystem.loadDialog("lvl3_EVIL_hand_over_your_body");
    yield return new WaitForSeconds(21f);

    LevelEnd.Instance.endLevel("levelCompleteSoundEvil");

    StopCoroutine(LVL3_EndScene());
  }



  /* ===============
    * === LEVEL 2 ===
    * ===============
    */

  private IEnumerator StartFrequenceLvl2() {
    LevelSettings.Instance.setSetting("canMorphToCircle", true);
    LevelSettings.Instance.setSetting("canMorphToTriangle", false);
    LevelSettings.Instance.setSetting("canMorphToRectangle", false);
    
    // disabled 2nd voice linerenderer for standing robot,
    // until the robot stands up at the end of the level
    LineRenderer robotStandUpVoiceLineRenderer = DialogSystem.getLineRenderer("RobotStandUpVoiceLineRenderer");
    robotStandUpVoiceLineRenderer.gameObject.SetActive(false);

    yield return new WaitForSeconds(2f);
    DialogSystem.loadDialog("lvl2_no_legs_over_here");
    yield return new WaitForSeconds(13.5f);

    GameObject robotObject = GameObject.Find("RobotFallingDown");
    GameObject robotObjectTexture = GameObject.Find("RobotFallingDownTexture");

    robotObjectTexture.GetComponent<Animator>().SetBool("FallDown", true);
    yield return new WaitForSeconds(.6f);
    SoundController.Instance.playSound("robotScreamSound");
    yield return new WaitForSeconds(2.3f);
    // robot lands on ground
    GameObject.Find("RobotLandingParticles").GetComponent<ParticleSystem>().Play();
    CameraShake.Instance.play(.4f, 34f, 34f);

    yield return new WaitForSeconds(1.5f);
    robotObjectTexture.GetComponent<Animator>().SetBool("LookRight", true);
    yield return new WaitForSeconds(1f);
    DialogSystem.loadDialog("lvl2_you_are_here_as_well");
    yield return new WaitForSeconds(9f);
    robotObjectTexture.GetComponent<Animator>().SetBool("LookRight", false);
    DialogSystem.loadDialog("lvl2_do_you_want_to_get_out_of_here");
    yield return new WaitForSeconds(12f);

    virtualCameraAnimator.SetTrigger("StartFrequenceOver");
    yield return new WaitForSeconds(.3f);
    LevelSettings.Instance.setSetting("canMorphToTriangle", true);
    TooltipManager.showTooltip("MorphTriangle");
    StopCoroutine(StartFrequenceLvl2());
  }
  private IEnumerator Lvl2_FirstMorphToTriangle() {
    TooltipManager.hideTooltip("MorphTriangle");
    yield return new WaitForSeconds(.9f);
    TooltipManager.showTooltip("DoubleJumpTriangle");
    yield return new WaitForSeconds(3f);
    TooltipManager.hideTooltip("DoubleJumpTriangle");
    yield return new WaitForSeconds(.5f);
    DialogSystem.loadDialog("lvl2_full_of_surprises");
    StopCoroutine(Lvl2_FirstMorphToTriangle());
  }
  private IEnumerator LVL2_CanYouMorphIntoOtherForms() {

    // freeze player until he morphs to rectangle
    LevelSettings.Instance.setSetting("canMove", false);
    LevelSettings.Instance.setSetting("canJump", false);
    LevelSettings.Instance.setSetting("canSelfDestruct", false);

    GameObject robotObject = GameObject.Find("RobotFallingDown");
    GameObject robotObjectTexture = GameObject.Find("RobotFallingDownTexture");

    robotObjectTexture.GetComponent<Animator>().SetBool("FallDown", false);
    yield return new WaitForSeconds(.2f);
    robotObjectTexture.GetComponent<Animator>().SetBool("FallDown", true);
    yield return new WaitForSeconds(.2f);
    robotObject.transform.position = new Vector2(611f, 189.2f);
    yield return new WaitForSeconds(.4f);
    SoundController.Instance.playSound("robotScreamSound");
    yield return new WaitForSeconds(2.3f);
    // robot lands on ground
    GameObject.Find("RobotLandingParticles").GetComponent<ParticleSystem>().Play();
    CameraShake.Instance.play(.4f, 34f, 34f);

    yield return new WaitForSeconds(1.65f);
    DialogSystem.loadDialog("lvl2_can_you_morph_into_other_forms");

    yield return new WaitForSeconds(6.5f);

    LevelSettings.Instance.setSetting("canMorphToRectangle", true);
    TooltipManager.showTooltip("MorphRectangle");
    StopCoroutine(LVL2_CanYouMorphIntoOtherForms());

  }
  private IEnumerator LVL2_RectangleMorphPraises() {
    TooltipManager.hideTooltip("MorphRectangle");
    yield return new WaitForSeconds(.5f);

    // un-freeze player from previous event
    LevelSettings.Instance.setSetting("canMove", true);
    LevelSettings.Instance.setSetting("canJump", true);
    LevelSettings.Instance.setSetting("canSelfDestruct", true);

    DialogSystem.loadDialog("lvl2_rectangle_great");
    StopCoroutine(LVL2_RectangleMorphPraises());
  }
  private IEnumerator LVL2_ForceField() {
    yield return new WaitForSeconds(.8f);
    DialogSystem.loadDialog("lvl2_force_fields_everywhere");
    StopCoroutine(LVL2_ForceField());
  }

  private GameObject robotLegSmokeParticles = null;
  private GameObject robotLegSmokeParticles2 = null;

  private IEnumerator LVL2_PickUpLegs() {
    GameObject.Find("RobotLegs").SetActive(false);
    PlayerManager.Instance.setValue("holdingItem", true);
    yield return new WaitForSeconds(.3f);
    DialogSystem.loadDialog("lvl2_found_legs");

    robotLegSmokeParticles = GameObject.Find("RobotLegSmokeParticles");
    robotLegSmokeParticles2 = GameObject.Find("RobotLegSmokeParticles2");
    robotLegSmokeParticles.SetActive(false);
    robotLegSmokeParticles2.SetActive(false);

    StopCoroutine(LVL2_PickUpLegs());
  }

  private IEnumerator LVL2_ReceiveArms() {

    LevelSettings.Instance.setSetting("canSelfDestruct", false);
    LevelSettings.Instance.setSetting("canMove", false);
    LevelSettings.Instance.setSetting("canJump", false);
    LevelSettings.Instance.setSetting("canMorphToCircle", false);
    LevelSettings.Instance.setSetting("canMorphToTriangle", false);
    LevelSettings.Instance.setSetting("canMorphToRectangle", false);

    GameObject robotObject = GameObject.Find("RobotFallingDown");
    GameObject robotObjectTexture = GameObject.Find("RobotFallingDownTexture");

    robotObjectTexture.GetComponent<Animator>().SetBool("FallDown", false);
    yield return new WaitForSeconds(.2f);
    robotObjectTexture.GetComponent<Animator>().SetBool("FallDown", true);
    yield return new WaitForSeconds(.2f);
    robotObject.transform.position = new Vector2(3900f, 248f);
    yield return new WaitForSeconds(.4f);
    SoundController.Instance.playSound("robotScreamSound");
    yield return new WaitForSeconds(2.3f);
    // robot lands on ground
    GameObject.Find("RobotLandingParticles").GetComponent<ParticleSystem>().Play();
    CameraShake.Instance.play(.4f, 34f, 34f);
    yield return new WaitForSeconds(1.2f);

    DialogSystem.loadDialog("lvl2_thank_you_my_little_friend");
    yield return new WaitForSeconds(9f);

    SpriteRenderer getLegsObjectSR = GameObject.Find("RobotAttachingLegsTexture").GetComponent<SpriteRenderer>();
    SpriteRenderer standUpObjectSR = GameObject.Find("RobotStandUpTexture").GetComponent<SpriteRenderer>();
    Sprite[] getLegsSprites = Resources.LoadAll<Sprite>("RobotGetLegsAnimation");
    int spriteNum = getLegsSprites.Length;

    // repair sound
    SoundController.Instance.playSound("robotRepairSound");

    // remove legs from player
    PlayerManager.Instance.setValue("holdingItem", false);

    // hide 'fallen down' robot
    robotObjectTexture.SetActive(false);

    // play 'attaching legs' animation
    for (int i = 0; i < spriteNum; i++) {
      if (i < 32) {
        if (i == 28) {
          yield return new WaitForSeconds(.4f);
        }
        getLegsObjectSR.sprite = getLegsSprites[i];
      }
      else {
        if (i == 32) {
          getLegsObjectSR.sprite = null;
        }
        else if (i == 39) {
          yield return new WaitForSeconds(1.1f);
          SoundController.Instance.playSound("robotElectricDefect");
          yield return new WaitForSeconds(.3f);
          robotLegSmokeParticles.SetActive(true);
          robotLegSmokeParticles.GetComponent<ParticleSystem>().Play();
          robotLegSmokeParticles2.SetActive(true);
          robotLegSmokeParticles2.GetComponent<ParticleSystem>().Play();
        }
        standUpObjectSR.sprite = getLegsSprites[i];
      }
      yield return new WaitForSeconds(.08f);
    }

    // robot is standing up, activate 2nd line renderer
    LineRenderer robotStandUpVoiceLineRenderer = DialogSystem.getLineRenderer("RobotStandUpVoiceLineRenderer");
    robotStandUpVoiceLineRenderer.gameObject.SetActive(true);

    yield return new WaitForSeconds(3.5f);
    DialogSystem.loadDialog("lvl2_where_did_you_pick_up_these_legs");
    yield return new WaitForSeconds(11f);
    LevelEnd.Instance.endLevel("levelCompleteSound");
    StopCoroutine(LVL2_ReceiveArms());
  }






  /* ===============
   * === LEVEL 1 ===
   * ===============
   */

  private IEnumerator StartFrequenceLvl1() {
    LevelSettings.Instance.setSetting("canSelfDestruct", false);
    LevelSettings.Instance.setSetting("canMove", false);
    LevelSettings.Instance.setSetting("canJump", false);
    LevelSettings.Instance.setSetting("canMorphToCircle", false);
    LevelSettings.Instance.setSetting("canMorphToTriangle", false);
    LevelSettings.Instance.setSetting("canMorphToRectangle", false);
    virtualCameraAnimator.SetTrigger("StartFrequenceOver");
    yield return new WaitForSeconds(4f);
    DialogSystem.loadDialog("lvl1_hello");
    yield return new WaitForSeconds(8.5f);
    DialogSystem.loadDialog("lvl1_asleep");
    yield return new WaitForSeconds(5f);
    CameraShake.Instance.play(2f, 10f, 10f, "earthquake_2_5_secs_loud");
    yield return new WaitForSeconds(1.3f);
    GameObject sleepingAnimation = GameObject.Find("LVL1_SleepingAnimation");
    SpriteRenderer sleepingAnimationSR = sleepingAnimation.GetComponent<SpriteRenderer>();
    // fade out the sleeping animation sprite
    for (int i = 0; i < 10; i++) {
      Color newColor = sleepingAnimationSR.color;
      newColor.a -= 0.1f;
      sleepingAnimationSR.color = newColor;
      yield return new WaitForSeconds(0.06f);
    }
    sleepingAnimation.SetActive(false);
    GameObject.Find("LVL1_WakeUpAnimation").GetComponent<Animator>().SetTrigger("WakeUp");
    yield return new WaitForSeconds(3f);
    DialogSystem.loadDialog("lvl1_move");
    yield return new WaitForSeconds(6f);
    TooltipManager.showTooltip("Move");
    yield return new WaitForSeconds(1f);
    LevelSettings.Instance.setSetting("canSelfDestruct", true);
    LevelSettings.Instance.setSetting("canMove", true);
    StopCoroutine(StartFrequenceLvl1());
  }
  private IEnumerator Lvl1_JumpTooltip() {
    DialogSystem.loadDialog("lvl1_jump");
    yield return new WaitForSeconds(1.5f);
    LevelSettings.Instance.setSetting("canJump", true);
    TooltipManager.showTooltip("Jump");
    StopCoroutine(Lvl1_JumpTooltip());
  }
  private IEnumerator Lvl1_RobotAppearScene() {
    LevelSettings.Instance.setSetting("canSelfDestruct", false);
    LevelSettings.Instance.setSetting("canMove", false);
    yield return new WaitForSeconds(.2f);
    CameraShake.Instance.play(.5f, 2f, 2f);
    yield return new WaitForSeconds(1f);
    CameraShake.Instance.play(.5f, 3f, 3f);
    yield return new WaitForSeconds(1.5f);
    CameraShake.Instance.play(.7f, 5f, 5f);
    yield return new WaitForSeconds(.4f);
    CameraShake.Instance.play(3f, 10f, 10f, "earthquake_3_secs");
    GameObject.Find("RobotAppearingParticles").GetComponent<ParticleSystem>().Play();
    yield return new WaitForSeconds(.45f);
    Animator lvl1robotAnimator = GameObject.Find("RobotFigure").GetComponent<Animator>();
    lvl1robotAnimator.SetTrigger("RobotAppear");
    yield return new WaitForSeconds(2.5f);
    lvl1robotAnimator.SetTrigger("RobotStraighten");
    yield return new WaitForSeconds(.45f);
    DialogSystem.loadDialog("lvl1_its_me");
    yield return new WaitForSeconds(5.5f);
    DialogSystem.loadDialog("lvl1_arms_are_further_ahead");
    yield return new WaitForSeconds(4f);
    LevelSettings.Instance.setSetting("canMove", true);
    LevelSettings.Instance.setSetting("canSelfDestruct", true);
    StopCoroutine(Lvl1_RobotAppearScene());
  }
  private IEnumerator Lvl1_PickUpArms() {
    LevelSettings.Instance.setSetting("canSelfDestruct", false);
    LevelSettings.Instance.setSetting("canMove", false);
    yield return new WaitForSeconds(.2f);
    DialogSystem.loadDialog("lvl1_pick_up_arms");
    yield return new WaitForSeconds(4f);
    LevelSettings.Instance.setSetting("canMove", true);
    LevelSettings.Instance.setSetting("canSelfDestruct", true);
    StopCoroutine(Lvl1_PickUpArms());
  }
  private IEnumerator Lvl1_BringArmsBack() {
    GameObject.Find("RoboterArms").SetActive(false);
    PlayerManager.Instance.setValue("holdingItem", true);
    yield return new WaitForSeconds(.2f);
    DialogSystem.loadDialog("lvl1_bring_arms_back");
    StopCoroutine(Lvl1_BringArmsBack());
  }
  private IEnumerator Lvl1_RobotGetArmsScene() {
    LevelSettings.Instance.setSetting("canSelfDestruct", false);
    LevelSettings.Instance.setSetting("canMove", false);

    PlayerManager.Instance.setValue("holdingItem", false);

    SpriteRenderer robotTexture = GameObject.Find("RobotFigureTexture").GetComponent<SpriteRenderer>();
    Sprite[] robotArmsAnimation = Resources.LoadAll<Sprite>("RobotGetArmsAnimation");

    for (int i = 0; i < robotArmsAnimation.Length; i++) {
      robotTexture.sprite = robotArmsAnimation[i];

      float multiplier = 1f;
      switch (i) {
        case 0:
          yield return new WaitForSeconds(.2f);
          break;
        case 1:
          SoundController.Instance.playSound("robotRepairSound");
          break;
        case 24: multiplier = 6f; break;
        case 27: multiplier = 4f; break;
        default: break;
      }

      yield return new WaitForSeconds(.08f * multiplier);
    }

    // make robot take its arms back down
    int[] sprites = new int[5] { 28, 27, 26, 25, 24 };
    for (int i = 0; i < sprites.Length; i++) {
      robotTexture.sprite = robotArmsAnimation[sprites[i]];
      yield return new WaitForSeconds(.1f);
    }
    yield return new WaitForSeconds(.3f);


    yield return new WaitForSeconds(.2f);
    DialogSystem.loadDialog("lvl1_thank_you");
    yield return new WaitForSeconds(6.3f);

    DialogSystem.loadDialog("lvl1_where_did_i_leave_my_legs");
    yield return new WaitForSeconds(5.2f);

    DialogSystem.loadDialog("lvl1_im_off");

    yield return new WaitForSeconds(7.4f);

    /* ROBOT FLY OFF ANIMATION */

    // animate the robot to fly off
    GameObject.Find("RobotFigure").GetComponent<Animator>().SetTrigger("FlyOff");

    // deactive voice line renderer on robot
    // (as it would stay in position, while robot flys up)
    LineRenderer[] voiceLineRenderers = DialogSystem.getLineRenderers();
    foreach (LineRenderer lr in voiceLineRenderers) {
      lr.gameObject.SetActive(false);
    }

    // play 'take off' sound effect
    SoundController.Instance.playSound("earthquake_2_secs");
    yield return new WaitForSeconds(2.3f);
    SoundController.Instance.playSound("earthquake_3_secs");
    yield return new WaitForSeconds(6f);

    LevelEnd.Instance.endLevel("levelCompleteSound");
    StopCoroutine(Lvl1_RobotGetArmsScene());
  }


}
