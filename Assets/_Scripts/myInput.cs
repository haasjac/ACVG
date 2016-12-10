using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum gesture {NONE, DOUBLE, LEFT, RIGHT, UP, DOWN, HOLD};

public class myInput : MonoBehaviour {

    // public variables
    public gesture touch;

    enum myPhase { NONE, BEGAN, MOVED, ENDED};
    class input {
        public input() {
            position = Vector2.zero;
            tapCount = 0;
        }
        public Vector2 position;
        public int tapCount;
    }

    // private variables
    bool swipe;
    bool hold;
    Vector2 startPos;
    float minDist = 100;
    gesture dir = gesture.NONE;
    myPhase phase = myPhase.NONE;
    input t = new input();
    float timeForDoubleTap = 0.150f;
    public float touchLength = 1.25f;
    float startTime;


	// Update is called once per frame
	void Update () {

        mobile();

        // determine which phase the touch is in
        switch (phase) {

            // init values when touch begins
            case myPhase.BEGAN:
                swipe = false;
                hold = true;
                startPos = t.position;
                startTime = Time.time;
                dir = gesture.NONE;
                break;
                
            // check for possible type of touch while moving
            case myPhase.MOVED:
                float time = Time.time - startTime;
                if (hold && (time > touchLength)) {
                    touch = gesture.HOLD;
                }
                gesture temp;
                // check that its not a tap
                if (Vector2.Distance(t.position, startPos) > minDist) {
                    hold = false;
                    // determine what direction touch is in
                    if (Mathf.Abs(t.position.x - startPos.x) > Mathf.Abs(t.position.y - startPos.y)) {
                        if (t.position.x - startPos.x > 0) {
                            temp = gesture.RIGHT;
                        } else {
                            temp = gesture.LEFT;
                        }
                    } else {
                        if (t.position.y - startPos.y > 0) {
                            temp = gesture.UP;
                        } else {
                            temp = gesture.DOWN;
                        }
                    }

                    // touch is not a swipe if it changes directions
                    if (dir != gesture.NONE) {
                        if (dir != temp) {
                            swipe = false;
                        }
                    } 

                    // otherwise it is a swipe
                    else {
                        swipe = true;
                        dir = temp;
                    }
                }
                break;

            // determine type of touch as touch ends
            case myPhase.ENDED:
                
                if (touch == gesture.HOLD) {
                    touch = gesture.NONE;
                } else if (swipe) {
                    touch = dir;
                } else if (t.tapCount > 1) {
                    touch = gesture.DOUBLE;
                } else {
                    touch = gesture.NONE;
                }
                break;
            case myPhase.NONE:
                touch = gesture.NONE;
                break;
        }

    }


    void mobile() {
        // check for a touch
        if (Input.touchCount > 0) {
            Touch mt = Input.GetTouch(0);
            switch (mt.phase) {
                case TouchPhase.Began:
                    phase = myPhase.BEGAN;
                    break;
                case TouchPhase.Stationary:
                    phase = myPhase.MOVED;
                    break;
                case TouchPhase.Moved:
                    phase = myPhase.MOVED;
                    break;
                case TouchPhase.Ended:
                    phase = myPhase.ENDED;
                    break;
            }
            t.position = mt.position;
            t.tapCount = mt.tapCount;
        }
        else {
            phase = myPhase.NONE;
        }

        //check for mouse
        if (Input.GetMouseButtonDown(0)) {
            phase = myPhase.BEGAN;
            t.tapCount++;
            t.position = Input.mousePosition;
        } else if (Input.GetMouseButton(0)) {
            phase = myPhase.MOVED;
            t.position = Input.mousePosition;
        } else if (Input.GetMouseButtonUp(0)) {
            phase = myPhase.ENDED;
            t.position = Input.mousePosition;
            timer();
        } else {
            phase = myPhase.NONE;
        }
    }

    IEnumerator timer() {
        int tt = t.tapCount;
        yield return new WaitForSecondsRealtime(timeForDoubleTap);
        //if there hasn't been another tap since last one
        if (tt <= t.tapCount) {
            t.tapCount = 0;
        }
    }
}
