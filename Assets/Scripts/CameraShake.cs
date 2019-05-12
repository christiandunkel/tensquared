using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class CameraShake : MonoBehaviour
{

  // singleton
  public static CameraShake Instance;
  void Awake()
  {
    Instance = this;
  }

  private float shakeAmplitude = 1.2f;
  private float shakeFrequency = 2.0f;

  private float elapsedTime = 0.0f;

  // Cinemachine Shake
  public CinemachineVirtualCamera VirtualCamera;
  private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
  
  void Start()
  {
    // get virtual camera noise profile
    if (VirtualCamera != null)
    {
      virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }
      
  }

  public void Play(float duration, float amplitude, float frequency)
  {
    elapsedTime = duration;
    shakeAmplitude = amplitude;
    shakeFrequency = frequency;
  }
  
  void Update()
  {
    
    if (VirtualCamera != null && virtualCameraNoise != null)
    {

      // shaking effect is playing
      if (elapsedTime > 0f)
      {
        // set camera noise parameters
        virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
        virtualCameraNoise.m_FrequencyGain = shakeFrequency;

        // update timer
        elapsedTime -= Time.deltaTime;
      }
      else
      {
        virtualCameraNoise.m_AmplitudeGain = 0.0f;
        elapsedTime = 0.0f;
      }

    }
  }


  /*
  public IEnumerator Play(float duration, float magnitude)
  {
    Vector3 originalPos = camera.gameObject.transform.localPosition;

    float timer = 0.0f;

    while (timer < duration)
    {

      float x = Random.Range(-1f, 1f) * magnitude,
            y = Random.Range(-1f, 1f) * magnitude;

      Debug.Log(x + " " + y);

      camera.gameObject.transform.localPosition = new Vector3(x, y, originalPos.z);

      timer += Time.deltaTime;

      // before next iteration of while loop, wait for update function to run once (a new frame to be drawn)
      yield return null;

    }

    camera.gameObject.transform.localPosition = originalPos;

  }*/

}
