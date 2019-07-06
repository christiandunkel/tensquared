using System.Collections;
using UnityEngine;

/*
 * can toggle CanvasGroup(s) visibility with a smooth fade effect
 */

public class CanvasGroupFader : MonoBehaviour {

  /*
   * =================
   * === SINGLETON ===
   * =================
   */

  public static CanvasGroupFader Instance;

  private void Awake() {
    Instance = this;
  }





  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  [SerializeField] private CanvasGroup[] uiElements = null;
  [SerializeField] private float fadeinTime = 0.3f;
  [SerializeField] private float fadeoutTime = 0.1f;





  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime, bool interactableDuring, bool interactableEnd, bool isFadeInAnimation) {

    /*
     * fade a canvas group in or out of frame
     * 
     * @ interactableDuring = child elements can be interacted with at start and during animation
     *@ interactableEnd = child elements can be interacted after the animation
     */

    float timeStartedLerping = Time.time;
    float timeSinceStarted = 0f;

    // child elements interactable or not
    cg.interactable = interactableDuring;

    // start values of canvasgroup scaling
    float scaleX = cg.transform.localScale.x + 0.3f,
          scaleY = cg.transform.localScale.y + 0.3f,
          scaleZ = cg.transform.localScale.z;

    if (isFadeInAnimation) {
      cg.gameObject.SetActive(true);
      cg.transform.localScale += new Vector3(scaleX, scaleY, scaleZ);
    }

    while (true) {

      timeSinceStarted = Time.time - timeStartedLerping;
      float percentageCompleted = timeSinceStarted / lerpTime;

      // opacity
      float currentValue = Mathf.Lerp(start, end, percentageCompleted);
      cg.alpha = currentValue;

      if (isFadeInAnimation) {
        // scale in
        float transformVal = currentValue / 10 * 3;

        cg.transform.localScale = new Vector3(
          scaleX - transformVal,
          scaleY - transformVal,
          scaleZ
        );
      }

      if (percentageCompleted >= 1) break;

      // runs at speed of update function
      yield return new WaitForEndOfFrame();

    }

    // child elements interactable afterwards
    cg.interactable = interactableEnd;

    // if it faded out, set as inactive
    if (!isFadeInAnimation) {
      cg.gameObject.SetActive(false);
    }

  }





  /*
   * ================
   * === EXTERNAL ===
   * ================
   */

  public CanvasGroup[] getUIElements() {

    /*
     * get array of saved ui elements
     */

    return uiElements;

  }

  public void FadeIn(GameObject obj) {

    /*
     * fade in the given game object
     */

    if (obj == null) {
      Log.Error($"Object parameter can't be null.", gameObject);
      return;
    }

    CanvasGroup uiElement = obj.GetComponent<CanvasGroup>();

    if (uiElement == null) {
      Log.Error($"Given object has no 'CanvasGroup' component.", obj);
      return;
    }

    Log.Print($"Fading in object '{obj.name}'.", obj);

    // start fading in process
    StartCoroutine(FadeCanvasGroup(uiElement, 0, 1, fadeinTime, false, true, true));

  }

  public void FadeIn(string name) {

    /*
     * fade in the game object with the given name
     */

    // find game object with name
    GameObject obj = GameObject.Find(name);

    if (obj == null) {
      Log.Error($"Object of name {name} couldn't be found.", gameObject);
      return;
    }

    CanvasGroup uiElement = obj.GetComponent<CanvasGroup>();

    Log.Print($"Fading in object of name '{name}'.", obj);

    // start fading in process
    StartCoroutine(FadeCanvasGroup(uiElement, 0, 1, fadeinTime, false, true, true));

  }

  public void FadeIn(int id) {

    /*
     * look through 'uiElements' array of objects
     * and fade in object at index of number 'id'
     */

    CanvasGroup uiElement = uiElements[id];

    Log.Print($"Fading in object of id {id} in array ({uiElement.name}).", uiElement);

    // start fading in process
    StartCoroutine(FadeCanvasGroup(uiElement, 0, 1, fadeinTime, false, true, true));

  }

  public void FadeOut(GameObject obj) {

    /*
     * fade out the given game object
     */

    if (obj == null) {
      Log.Error($"Object parameter can't be null.", gameObject);
      return;
    }

    CanvasGroup uiElement = obj.GetComponent<CanvasGroup>();

    if (uiElement == null) {
      Log.Error($"Given object has no 'CanvasGroup' component.", obj);
      return;
    }

    Log.Print($"Fading out object '{obj.name}'.", obj);

    // start fading out process
    StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0, fadeoutTime, false, false, false));

  }

  public void FadeOut(string name) {

    /*
     * fade out the game object with the given name
     */

    // find game object with name
    GameObject obj = GameObject.Find(name);

    if (obj == null) {
      Log.Error($"Object of name {name} couldn't be found.", gameObject);
      return;
    }

    CanvasGroup uiElement = obj.GetComponent<CanvasGroup>();

    Log.Print($"Fading out object of name '{name}'.", obj);

    // start fading out process
    StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0, fadeoutTime, false, false, false));

  }

  public void FadeOut(int id) {

    /*
     * look through 'uiElements' array of objects
     * and fade out object at index of number 'id'
     */

    CanvasGroup uiElement = uiElements[id];

    Log.Print($"Fading out object of id {id} in array ({uiElement.name}).", uiElement);

    // start fading out process
    StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0, fadeoutTime, false, false, false));

  }

}
