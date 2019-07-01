using UnityEngine;
using Cinemachine;

/*
 * can generate shaking effects of the camera
 * (takes the attributes frequency, duration and amplitude)
 */

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraShake : MonoBehaviour {

  /*
   * =================
   * === SINGLETON ===
   * =================
   */

  public static CameraShake Instance;

  private void Awake() {
    Instance = this;
  }





  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  //  cinemachine shake components
  [SerializeField] private CinemachineVirtualCamera VirtualCamera = null;
  private CinemachineBasicMultiChannelPerlin virtualCameraNoise = null;
  private SoundController soundController = null;

  // attributes
  private float shakeAmplitude = 1.2f;
  private float shakeFrequency = 2f;
  private float elapsedTime = 0f;





  /*
   * ================
   * === EXTERNAL ===
   * ================
   */

  public void play(float duration, float amplitude, float frequency, string sound) {

    /*
     * plays the camera shake and an accompanying sound effect
     */

    play(duration, amplitude, frequency);

    // access the sound controller to play given sound effect
    soundController.playSound(sound);

  }

  public void play(float duration, float amplitude, float frequency) {

    /*
     * sets the attributes for the camera shake effect
     * and plays it if the duration is bigger than 0
     */

    elapsedTime = duration;
    shakeAmplitude = amplitude;
    shakeFrequency = frequency;

  }





  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  private void Start() {

    soundController = SoundController.Instance;

    // get virtual camera noise profile
    if (VirtualCamera != null) {
      virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

  }

  private void Update() {

    // if required components are defined, generate shaking
    if (VirtualCamera != null && virtualCameraNoise != null) {
      generateShaking();
    }

  }

  private void generateShaking() {

    /*
     * generates the shaking effect of the Cinemachine (virtual) camera
     */

    // shaking effect is playing
    if (elapsedTime > 0f) {

      // set camera noise parameters
      virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
      virtualCameraNoise.m_FrequencyGain = shakeFrequency;

      // update timer
      elapsedTime -= Time.fixedDeltaTime;
    }
    else {
      virtualCameraNoise.m_AmplitudeGain = 0.0f;
      elapsedTime = 0.0f;
    }

  }

}