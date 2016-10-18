using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum gesture {NONE, DOUBLE, LEFT, RIGHT, UP, DOWN};

public class myInput : MonoBehaviour {

    // public variables
    public gesture touch;

    // private variables
    bool swipe;
    Vector2 startPos;
    float minDist = 100;
    gesture dir = gesture.NONE;
    
	
	// Update is called once per frame
	void Update () {

        // check for a touch
        if (Input.touchCount > 0) {
            Touch t = Input.GetTouch(0);
            
            // determine which phase the touch is in
            switch (t.phase) {

                // init values when touch begins
                case TouchPhase.Began:
                    swipe = false;
                    startPos = t.position;
                    dir = gesture.NONE;
                    break;
                
                // check for possible type of touch while moving
                case TouchPhase.Moved:
                    gesture temp;

                    
                    // check that its not a tap
                    if (Vector2.Distance(t.position, startPos) > minDist) {

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
                case TouchPhase.Ended:
                    if (swipe) {
                        touch = dir;
                    } else if (t.tapCount > 1) {
                        touch = gesture.DOUBLE;
                    } else {
                        touch = gesture.NONE;
                    }
                    break;
            }
        } 
        // No touches
        else {
            touch = gesture.NONE;
        }

    }
}
