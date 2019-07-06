using UnityEngine;

/*
 * animates the game object it's attached to from the left to the right
 * with a given speed for its full width, before resetting its position and restarting
 */

public class ScrollingBackground : MonoBehaviour {

  /*
   * ==================
   * === ATTRIBUTES ===
   * ==================
   */

  [SerializeField] private bool useLocalTransform = true;

  [SerializeField] private float scrollPercentage = 0.3f;

  // calculated with width and scrollPercentage
  private float scrollBy;

  private float startPosX;
  private float counter;





  /*
   * ================
   * === INTERNAL ===
   * ================
   */

  private void Start() {

    RectTransform rt = (RectTransform)transform;
    float width = rt.rect.width;

    // set position of game object on x axis at game start
    startPosX = useLocalTransform ? transform.localPosition.x : transform.position.x;
    
    scrollBy = width / 100 * scrollPercentage;
    counter = 0.0f;

  }

  private void Update() {

    // current x position of element
    counter += 1f;

    // work with local space
    if (useLocalTransform) { 

      // move object in relation to parent
      Vector3 newPosition = transform.localPosition;
      newPosition.x -= scrollBy;
      transform.localPosition = newPosition;

      // reset counter on 100 %
      if (counter * scrollPercentage >= 100f) {

        counter = 0.0f;

        // reset position of element back to beginning
        Vector3 startPosition = transform.localPosition;
        startPosition.x = startPosX;
        transform.localPosition = startPosition;

      }

    }
    // work with world space
    else {

      // move object's position in world space
      Vector3 newPosition = transform.position;
      newPosition.x -= scrollBy;
      transform.position = newPosition;

      // reset counter on 100 %
      if (counter * scrollPercentage >= 100f) {

        counter = 0f;

        // reset position of element back to beginning
        Vector3 startPosition = transform.position;
        startPosition.x = startPosX;
        transform.position = startPosition;

      }

    }

  }

}