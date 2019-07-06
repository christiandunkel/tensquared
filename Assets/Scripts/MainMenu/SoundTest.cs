using System.Collections;
using UnityEngine;

/*
 * plays the sounds for the sound test in the options menu (inside main menu)
 */

public class SoundTest : MonoBehaviour {

  /*
   * ==================
   * === COMPONENTS ===
   * ==================
   */

  [SerializeField] private GameObject soundTestMessage = null;

  [SerializeField] private AudioClip musicClip = null;
  [SerializeField] private AudioClip speechClip = null;
  [SerializeField] private AudioClip[] soundClips = null;

  [SerializeField] private AudioSource mainMenuMusicPlayer = null;
  [SerializeField] private AudioSource musicPlayer = null;
  [SerializeField] private AudioSource speechPlayer = null;
  [SerializeField] private AudioSource soundPlayer = null;

  private bool soundTestIsPlaying = false;





  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  private IEnumerator soundTestCoroutine() {

    /*
     * starts playing the sound test and
     * manages the audio clips playing
     */

    Log.Print("Started sound test.", this);

    mainMenuMusicPlayer.Pause();
    gameObject.GetComponent<CanvasGroup>().alpha = 0f;
    gameObject.GetComponent<CanvasGroup>().interactable = false;
    soundTestMessage.SetActive(true);

    yield return new WaitForSeconds(.2f);

    musicPlayer.PlayOneShot(musicClip);

    yield return new WaitForSeconds(.2f);

    speechPlayer.PlayOneShot(speechClip);

    yield return new WaitForSeconds(speechClip.length + .5f);

    foreach (AudioClip c in soundClips) {
      soundPlayer.PlayOneShot(c);
      yield return new WaitForSeconds(c.length + .3f);
    }

    yield return new WaitForSeconds(.2f);

    musicPlayer.Stop();

    yield return new WaitForSeconds(.2f);

    soundTestMessage.SetActive(false);
    gameObject.GetComponent<CanvasGroup>().alpha = 1f;
    gameObject.GetComponent<CanvasGroup>().interactable = true;
    mainMenuMusicPlayer.UnPause();
    soundTestIsPlaying = false;

    Log.Print("Sound test ended.", this);

    StopCoroutine(soundTestCoroutine());

  }





  /*
   * ================
   * === EXTERNAL ===
   * ================
   */

  public void playSoundTest() {

    /*
     * starts playing the sound test
     */

    if (!soundTestIsPlaying) {
      soundTestIsPlaying = true;
      StartCoroutine(soundTestCoroutine());
    }

  }

  public void stopSoundTest() {

    /*
     * stops the current sound test playing
     */

    if (soundTestIsPlaying) {
      StopCoroutine(soundTestCoroutine());
      soundTestIsPlaying = false;
      soundTestMessage.SetActive(false);
      gameObject.GetComponent<CanvasGroup>().alpha = 1f;
      gameObject.GetComponent<CanvasGroup>().interactable = true;
      musicPlayer.Stop();
      speechPlayer.Stop();
      soundPlayer.Stop();
      mainMenuMusicPlayer.UnPause();
    }

  }

}
