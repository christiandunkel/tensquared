using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour {

  // reference to audio source components
  public AudioSource[] musicSources;
  public AudioSource[] soundSources;
  public AudioSource[] speechSources;
  private static AudioSource[] musicSources_;
  private static AudioSource[] soundSources_;
  private static AudioSource[] speechSources_;

  private static List<AudioSource> totalSources_ = new List<AudioSource>();

  // reference to volume slider components
  public Slider[] musicSliders;
  public Slider[] soundSliders;
  public Slider[] speechSliders;
  private static Slider[] musicSliders_;
  private static Slider[] soundSliders_;
  private static Slider[] speechSliders_;

  // default volume values without player pref
  private static float musicVolume = 1f;
  private static float soundVolume = 1f;
  private static float speechVolume = 1f;

  private void Start() {

    Debug.Log("VolumeController: Loaded.");

    // static private variables holding gameobjects

    musicSources_ = new AudioSource[musicSources.Length];
    soundSources_ = new AudioSource[soundSources.Length];
    speechSources_ = new AudioSource[speechSources.Length];
    musicSliders_ = new Slider[musicSliders.Length];
    soundSliders_ = new Slider[soundSliders.Length];
    speechSliders_ = new Slider[speechSliders.Length];

    // add audio sources and sliders given in object to static private variables

    int counter = 0;
    foreach (AudioSource src in musicSources) {
      musicSources_[counter] = src;
      totalSources_.Add(src);
      counter++;
    }

    counter = 0;
    foreach (AudioSource src in soundSources) {
      soundSources_[counter] = src;
      totalSources_.Add(src);
      counter++;
    }

    counter = 0;
    foreach (AudioSource src in speechSources) {
      speechSources_[counter] = src;
      totalSources_.Add(src);
      counter++;
    }

    counter = 0;
    foreach (Slider slider in musicSliders) {
      musicSliders_[counter] = slider;
      counter++;
    }

    counter = 0;
    foreach (Slider slider in soundSliders) {
      soundSliders_[counter] = slider;
      counter++;
    }

    counter = 0;
    foreach (Slider slider in speechSliders) {
      speechSliders_[counter] = slider;
      counter++;
    }

    loadPlayerPrefs();

  }

  // called once per frame
  private bool soundIsPaused = false;
  void Update() {

    updateVolume();

    if (!soundIsPaused && PauseMenu.Instance.isPaused) {
      soundIsPaused = true;
      PauseAudioSources();
    }
    else if (soundIsPaused && !PauseMenu.Instance.isPaused) {
      soundIsPaused = false;
      UnPauseAudioSources();
    }

  }

  private void PauseAudioSources() {
    foreach (AudioSource src in totalSources_) {
      src.Pause();
    }
  }

  private void UnPauseAudioSources() {
    foreach (AudioSource src in totalSources_) {
      src.UnPause();
    }
  }

  // norm a value to not be smaller than 0 or larger than 1
  private static float normValue(float val) {
    val = val < 0f ? 0f : val;
    val = val > 1f ? 1f : val;
    return val;
  }

  public static void loadPlayerPrefs() {
    
    Debug.Log("VolumeController: Loaded PlayerPrefs for volume.");

    // load volume data from playerprefs

    if (PlayerPrefs.HasKey("music_volume")) {
      musicVolume = PlayerPrefs.GetFloat("music_volume");
      speechVolume = normValue(musicVolume);
    }
    else {
      PlayerPrefs.SetFloat("music_volume", musicVolume);
    }

    if (PlayerPrefs.HasKey("sound_volume")) {
      soundVolume = PlayerPrefs.GetFloat("sound_volume");
      speechVolume = normValue(soundVolume);
    }
    else {
      PlayerPrefs.SetFloat("sound_volume", soundVolume);
    }

    if (PlayerPrefs.HasKey("speech_volume")) {
      speechVolume = PlayerPrefs.GetFloat("speech_volume");
      speechVolume = normValue(speechVolume);
    }
    else {
      PlayerPrefs.SetFloat("speech_volume", speechVolume);
    }

    setSliderValues();

  }


  // set slider values in volume menu
  public static void setSliderValues() {

    Debug.Log("VolumeController: Set slider values.");

    foreach (Slider s in musicSliders_) {
      s.value = musicVolume;
    }

    foreach (Slider s in soundSliders_) {
      s.value = soundVolume;
    }

    foreach (Slider s in speechSliders_) {
      s.value = speechVolume;
    }

  }

  public static void updateVolume() {

    // set volumes of all music sources equal to musicVolume given in options
    foreach (AudioSource src in musicSources_) {
      src.volume = musicVolume;
    }

    // set volumes of all sound sources
    foreach (AudioSource src in soundSources_) {
      src.volume = soundVolume;
    }

    // set volumes of all speech sources
    foreach (AudioSource src in speechSources_) {
      src.volume = speechVolume;
    }

  }

  public void SetMusicVolume(float newVolume) {
    SetMusicVolume_(newVolume);
  }

  public static void SetMusicVolume_(float newVolume) {

    musicVolume = newVolume;

    // save volume in player prefs
    PlayerPrefs.SetFloat("music_volume", newVolume);

  }

  public void SetSoundVolume(float newVolume) {
    SetSoundVolume_(newVolume);
  }

  public static void SetSoundVolume_(float newVolume) {

    soundVolume = newVolume;

    // save volume in player prefs
    PlayerPrefs.SetFloat("sound_volume", newVolume);

  }

  public void SetSpeechVolume(float newVolume) {
    SetSpeechVolume_(newVolume);
  }

  public static void SetSpeechVolume_(float newVolume) {

    speechVolume = newVolume;

    // save volume in player prefs
    PlayerPrefs.SetFloat("speech_volume", newVolume);

  }

}