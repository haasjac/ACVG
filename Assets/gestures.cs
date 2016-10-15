using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class gestures : MonoBehaviour {

    public Text txt;
    bool possibleSwipe;
    bool possibleHorizontal;
    bool possibleVertical;
    Vector2 startPos;
    float startTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //print(Input.touchCount);

        if (Input.touchCount > 0) {
            Touch t = Input.GetTouch(0);
            string swipe = "no swipe";

            switch (t.phase) {
                case TouchPhase.Began:
                    possibleSwipe = true;
                    possibleVertical = true;
                    possibleHorizontal = true;
                    startPos = t.position;
                    startTime = Time.time;
                    break;
                case TouchPhase.Stationary:
                    possibleSwipe = false;
                    break;
                case TouchPhase.Moved:
                    break;
                case TouchPhase.Ended:
                    if (possibleSwipe) {
                        if ((Time.time - startTime < 1)) {
                            swipe = "SWIPE\ntime: " + (Time.time - startTime);
                        }
                        swipe = "possible swipe\ntime: " + (Time.time - startTime);
                    }
                    break;
            }
            
            txt.text = t.phase + "\n" + t.pressure + "\n" + t.tapCount + "\n" + swipe;
        } else {
            //txt.text = "no touching";
        }

	}
}
