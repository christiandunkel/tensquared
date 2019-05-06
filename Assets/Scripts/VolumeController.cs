using UnityEngine;

public class VolumeController : MonoBehaviour {

  // reference to audio source component
  public AudioSource[] musicSources;
  public AudioSource[] soundSources;
  public AudioSource[] speechSources;
  private AudioSource audioSrc;
  private float musicVolume = 1f;
  private float soundVolume = 1f;
  private float speechVolume = 1f;

  // called once per frame
  void Update() {

    // set volumes of all music sources equal to musicVolume given in options
    foreach (AudioSource src in musicSources)
    {
      src.volume = musicVolume;
    }

    // set volumes of all sound sources
    foreach (AudioSource src in soundSources)
    {
      src.volume = soundVolume;
    }

    // set volumes of all speech sources
    foreach (AudioSource src in speechSources)
    {
      src.volume = speechVolume;
    }

  }

  public void SetMusicVolume(float newVolume) {

    /*Debug.Log("Changed music volume to " + newVolume);//*/

    musicVolume = newVolume;

    // save volume in player prefs
    PlayerPrefs.SetFloat("music_volume", newVolume);

  }

  public void SetSoundVolume(float newVolume)
  {

    /*Debug.Log("Changed sound volume to " + newVolume);//*/

    soundVolume = newVolume;

    // save volume in player prefs
    PlayerPrefs.SetFloat("sound_volume", newVolume);

  }

  public void SetSpeechVolume(float newVolume)
  {

    /*Debug.Log("Changed speech volume to " + newVolume);//*/

    speechVolume = newVolume;

    // save volume in player prefs
    PlayerPrefs.SetFloat("speech_volume", newVolume);

    /*Debug.Log(PlayerPrefs.GetFloat("speech_volume").ToString());//*/

  }

}