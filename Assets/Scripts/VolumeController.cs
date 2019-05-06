using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour {

  // reference to audio source components
  public AudioSource[] musicSources;
  public AudioSource[] soundSources;
  public AudioSource[] speechSources;

  // reference to volume slider components
  public Slider[] musicSliders;
  public Slider[] soundSliders;
  public Slider[] speechSliders;

  // default volume values without player pref
  private float musicVolume = 1f;
  private float soundVolume = 1f;
  private float speechVolume = 1f;

  private void Start()
  {

    // load volume data from playerprefs

    if (PlayerPrefs.HasKey("music_volume"))
    {
      musicVolume = PlayerPrefs.GetFloat("music_volume");
      speechVolume = normValue(musicVolume);
    }
    else
    {
      PlayerPrefs.SetFloat("music_volume", musicVolume);
    }

    if (PlayerPrefs.HasKey("sound_volume"))
    {
      soundVolume = PlayerPrefs.GetFloat("sound_volume");
      speechVolume = normValue(soundVolume);
    }
    else
    {
      PlayerPrefs.SetFloat("sound_volume", soundVolume);
    }

    if (PlayerPrefs.HasKey("speech_volume"))
    {
      speechVolume = PlayerPrefs.GetFloat("speech_volume");
      speechVolume = normValue(speechVolume);
    }
    else
    {
      PlayerPrefs.SetFloat("speech_volume", speechVolume);
    }

    // set slider values in volume menu

    foreach (Slider s in musicSliders)
    {
      s.value = musicVolume;
    }

    foreach (Slider s in soundSliders)
    {
      s.value = soundVolume;
    }

    foreach (Slider s in speechSliders)
    {
      s.value = speechVolume;
    }

  }

  // norm a value to not be smaller than 0 or larger than 1
  private float normValue(float val)
  {

    if (val < 0.0f)
    {
      val = 0.0f;
    }
    else if (val > 1.0f)
    {
      val = 1.0f;
    }

    return val;

  }

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