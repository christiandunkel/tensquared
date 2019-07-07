using UnityEngine;
using UnityEngine.UI;

/*
 * controls volume over all scenes
 */

public class VolumeController : MonoBehaviour {

  /* 
   * =================
   * === SINGLETON ===
   * =================
   */

  public static VolumeController Instance;

  private void Awake() {
    Instance = this;
    Log.Print($"Initialised on object '{gameObject.name}'.", this);
    loadPlayerPrefs();
  }





  /* 
   * ==================
   * === COMPONENTS ===
   * ==================
   */

  // reference to audio source components
  [SerializeField] private AudioSource[] musicSources = null;
  [SerializeField] private AudioSource[] soundSources = null;
  [SerializeField] private AudioSource[] speechSources = null;

  // reference to volume slider components
  [SerializeField] private Slider[] musicSliders = null;
  [SerializeField] private Slider[] soundSliders = null;
  [SerializeField] private Slider[] speechSliders = null;





  /* 
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  // default volume values without player pref
  private static float musicVolume = 1f;
  private static float soundVolume = 1f;
  private static float speechVolume = 1f;

  // called once per frame
  private bool soundIsPaused = false;





  /* 
   * ================
   * === INTERNAL ===
   * ================
   */

  private void Update() {

    updateVolume();

    if (PauseMenu.Instance != null) {
      if (!soundIsPaused && PauseMenu.Instance.isPaused()) {
        soundIsPaused = true;
        PauseAudioSources();
      }
      else if (soundIsPaused && !PauseMenu.Instance.isPaused()) {
        soundIsPaused = false;
        UnPauseAudioSources();
      }
    }

  }





  /* 
   * ================
   * === EXTERNAL ===
   * ================
   */

  public void pauseBackgroundMusic() {

    /*
     * pauses all sound sources playing background music
     */

    foreach (AudioSource src in musicSources) {
      src.Pause();
    }

  }

  private void PauseAudioSources() {

    /*
     * pauses all audio sources
     */

    foreach (AudioSource src in musicSources) src.Pause();
    foreach (AudioSource src in soundSources) src.Pause();
    foreach (AudioSource src in speechSources) src.Pause();

  }

  private void UnPauseAudioSources() {

    /*
     * un-pauses all audio sources
     */

    foreach (AudioSource src in musicSources) src.UnPause();
    foreach (AudioSource src in soundSources) src.UnPause();
    foreach (AudioSource src in speechSources) src.UnPause();

  }

  private static float normValue(float val) {

    /*
     * normalizes a float to be between 0f and 1f
     */

    return val < 0f ? 0f : (val > 1f ? 1f : val);

  }

  public void loadPlayerPrefs() {

    /*
     * loads volume settings from player prefs into local variables
     */
    
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

  public void setSliderValues() {

    /*
     * set slider values in volume menu
     */

    foreach (Slider s in musicSliders) s.value = musicVolume;
    foreach (Slider s in soundSliders) s.value = soundVolume;
    foreach (Slider s in speechSliders) s.value = speechVolume;

  }

  public void updateVolume() {

    /*
     * update the current audio sources with the volume values
     */
    
    foreach (AudioSource src in musicSources) src.volume = musicVolume;
    foreach (AudioSource src in soundSources) src.volume = soundVolume;
    foreach (AudioSource src in speechSources) src.volume = speechVolume;

  }

  public void SetMusicVolume(float newVolume) {

    /*
     * sets the music volume to the given volume
     */

    musicVolume = newVolume;
    PlayerPrefs.SetFloat("music_volume", newVolume);

  }

  public void SetSoundVolume(float newVolume) {

    /*
     * sets the sound volume to the given volume
     */

    soundVolume = newVolume;
    PlayerPrefs.SetFloat("sound_volume", newVolume);

  }

  public void SetSpeechVolume(float newVolume) {

    /*
     * sets the speech volume to the given volume
     */

    speechVolume = newVolume;
    PlayerPrefs.SetFloat("speech_volume", newVolume);

  }

  public float getVolume(string name) {

    /*
     * gets the volume value for the named volume setting
     */

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