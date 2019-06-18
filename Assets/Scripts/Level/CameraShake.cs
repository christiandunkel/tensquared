using UnityEngine;
using Cinemachine;

/*
 * can generate shaking effects of the camera of differing frequency, duration and amplitude
 */

public class CameraShake : MonoBehaviour
{

  // singleton
  public static CameraShake Instance;
  void Awake() {
    Instance = this;
  }

  private float shakeAmplitude = 1.2f,
                shakeFrequency = 2f,
                elapsedTime = 0f;

  // Cinemachine Shake
  public CinemachineVirtualCamera VirtualCamera;
  private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
  private SoundController soundController = null;
  
  void Start() {

    // get virtual camera noise profile
    if (VirtualCamera != null) {
      virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

  }

  public void Play(float duration, float amplitude, float frequency, string sound) {

    Play(duration, amplitude, frequency);
    soundController.PlaySound(sound);
    
  }

  public void Play(float duration, float amplitude, float frequency) {
    elapsedTime = duration;
    shakeAmplitude = amplitude;
    shakeFrequency = frequency;
  }
  
  void Update() {
    
    if (VirtualCamera != null && virtualCameraNoise != null) {

      // shaking effect is playing
      if (elapsedTime > 0f) {

        // set camera noise parameters
        virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
        virtualCameraNoise.m_FrequencyGain = shakeFrequency;

        // update timer
        elapsedTime -= Time.deltaTime;
      }
      else {
        virtualCameraNoise.m_AmplitudeGain = 0.0f;
        elapsedTime = 0.0f;
      }

    }
  }

}
