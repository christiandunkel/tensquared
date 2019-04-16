using UnityEngine;

public class VolumeValueChange : MonoBehaviour {

  // reference to audio source component
  private AudioSource audioSrc;
  private float musicVolume = 1f;

  // initialization
  void Start() {
    // assign audio source component
    audioSrc = GetComponent<AudioSource>();
  }

  // called once per frame
  void Update() {
    // set volume of audio source equal to musicVolume given in options
    audioSrc.volume = musicVolume;
  }

  // called by slider game object
  public void SetVolume(float newVolume) {

    Debug.Log("Change music volume to " + newVolume);

    musicVolume = newVolume;
  }

}