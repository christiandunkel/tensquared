using System.Collections;
using UnityEngine;

/*
 * plays the sounds for the sound test in the options menu (inside main menu)
 */

public class SoundTest : MonoBehaviour {

  public GameObject soundTestMessage;

  public AudioClip musicClip, speechClip;
  public AudioClip[] soundClips;

  public AudioSource mainMenuMusicPlayer, musicPlayer, speechPlayer, soundPlayer;

  private bool soundTestIsPlaying = false;

  public void playSoundTest() {
    if (!soundTestIsPlaying) {
      soundTestIsPlaying = true;
      StartCoroutine(soundTestCoroutine());
    }
  }

  public void stopSoundTest()  {
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

  private IEnumerator soundTestCoroutine() {

    Debug.Log("SoundTest: Started sound test.");

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

    Debug.Log("SoundTest: Sound test ended.");

    StopCoroutine(soundTestCoroutine());

  }

}
