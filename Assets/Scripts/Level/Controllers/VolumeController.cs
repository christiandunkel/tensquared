using UnityEngine;
using UnityEngine.UI;

/*
 * controls volume over all scenes
 */

public class VolumeController : MonoBehaviour {

  public static VolumeController Instance;

  // reference to audio source components
  public AudioSource[] musicSources;
  public AudioSource[] soundSources;
  public AudioSource[] speechSources;

  // reference to volume slider components
  public Slider[] musicSliders;
  public Slider[] soundSliders;
  public Slider[] speechSliders;

  // default volume values without player pref
  private static float musicVolume = 1f;
  private static float soundVolume = 1f;
  private static float speechVolume = 1f;

  private void Awake() {

    Instance = this;

    Log.Print($"Initialised on object '{gameObject.name}'.", this);

    loadPlayerPrefs();

  }

  // called once per frame
  private bool soundIsPaused = false;
  private void Update() {

    updateVolume();

    if (PauseMenu.Instance != null) {
      if (!soundIsPaused && PauseMenu.Instance.isPaused) {
        soundIsPaused = true;
        PauseAudioSources();
      }
      else if (soundIsPaused && !PauseMenu.Instance.isPaused) {
        soundIsPaused = false;
        UnPauseAudioSources();
      }
    }

    

  }

  public void pauseBackgroundMusic() {

    /*
     * pauses all sound sources playing background music
     */

    foreach (AudioSource src in musicSources) {
      src.Pause();
    }

  }

  private void PauseAudioSources() {
    foreach (AudioSource src in musicSources) src.Pause();
    foreach (AudioSource src in soundSources) src.Pause();
    foreach (AudioSource src in speechSources) src.Pause();
  }

  private void UnPauseAudioSources() {
    foreach (AudioSource src in musicSources) src.UnPause();
    foreach (AudioSource src in soundSources) src.UnPause();
    foreach (AudioSource src in speechSources) src.UnPause();
  }

  // norm a value to not be smaller than 0 or larger than 1
  private static float normValue(float val) {
    return val < 0f ? 0f : (val > 1f ? 1f : val);
  }

  public void loadPlayerPrefs() {
    
    Log.Print("Loaded PlayerPrefs for volume settings.", this);

    // load volume data from playerprefs

    if (PlayerPrefs.HasKey("music_volume")) {
      musicVolume = normValue(PlayerPrefs.GetFloat("music_volume"));
    }
    else {
      PlayerPrefs.SetFloat("music_volume", musicVolume);
    }

    if (PlayerPrefs.HasKey("sound_volume")) {
      soundVolume = normValue(PlayerPrefs.GetFloat("sound_volume"));
    }
    else {
      PlayerPrefs.SetFloat("sound_volume", soundVolume);
    }

    if (PlayerPrefs.HasKey("speech_volume")) {
      speechVolume = normValue(PlayerPrefs.GetFloat("speech_volume"));
    }
    else {
      PlayerPrefs.SetFloat("speech_volume", speechVolume);
    }

    setSliderValues();

  }


  // set slider values in volume menu
  public void setSliderValues() {

    foreach (Slider s in musicSliders) s.value = musicVolume;
    foreach (Slider s in soundSliders) s.value = soundVolume;
    foreach (Slider s in speechSliders) s.value = speechVolume;

  }

  public void updateVolume() {
    
    foreach (AudioSource src in musicSources) src.volume = musicVolume;
    foreach (AudioSource src in soundSources) src.volume = soundVolume;
    foreach (AudioSource src in speechSources) src.volume = speechVolume;

  }


  public void SetMusicVolume(float newVolume) {
    musicVolume = newVolume;
    PlayerPrefs.SetFloat("music_volume", newVolume);
  }

  public void SetSoundVolume(float newVolume) {
    soundVolume = newVolume;
    PlayerPrefs.SetFloat("sound_volume", newVolume);
  }

  public void SetSpeechVolume(float newVolume) {
    speechVolume = newVolume;
    PlayerPrefs.SetFloat("speech_volume", newVolume);
  }

  public float getVolume(string name) {
    switch (name) {
      case "music":
        return musicVolume;
      case "sound":
        return soundVolume;
      case "speech":
        return speechVolume;
    }

    Log.Warn($"Could not find a volume option called {name}.", this);
    return 0f;

  }

}