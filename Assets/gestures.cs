using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum gesture {NONE, DOUBLE, LEFT, RIGHT, UP, DOWN};

public class gestures : MonoBehaviour {

    public Text txt;
    bool possibleSwipe;
    Vector2 startPos;
    float minDist = 200;
    string g = "";
    bool updatetext = false;
    gesture dir = gesture.NONE;
    public gesture touch;

    // Use this for initialization
    void Start () {
        txt.text = "";
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.touchCount > 0) {
            Touch t = Input.GetTouch(0);
            

            switch (t.phase) {
                case TouchPhase.Began:
                    possibleSwipe = false;
                    startPos = t.position;
                    //dir = "";
                    break;
                case TouchPhase.Stationary:
                    possibleSwipe = false;
                    break;
                case TouchPhase.Moved:
                    //string tempdir = "hmm";
                    //if (Vector2.Distance(t.position, startPos) > minDist) {
                        if (Mathf.Abs(t.position.x - startPos.x) > Mathf.Abs(t.position.y - startPos.y)) {
                            if (t.position.x - startPos.x > 0) {
                                dir = gesture.RIGHT;
                            } else {
                                dir = gesture.LEFT;
                            }
                        } else {
                            if (t.position.y - startPos.y > 0) {
                                dir = gesture.UP;
                            } else {
                                dir = gesture.DOWN;
                            }
                        }
                        //if (dir != "") {
                            //if (dir != tempdir) {
                                //possibleSwipe = false;
                            //}
                        //} else {
                            possibleSwipe = true;
                            //dir = tempdir;
                        //}
                    //}
                    break;
                case TouchPhase.Ended:
                    updatetext = true;
                    if (possibleSwipe) {
                        g = dir.ToString();
                        touch = dir;
                    } else if (t.tapCount > 1) {
                        g = "double tap " + t.tapCount;
                        touch = gesture.DOUBLE;
                    } else {
                        g = "no gesture";
                        touch = gesture.NONE;
                    }
                    break;
            }
        } else {
            g = "no gesture";
            touch = gesture.NONE;
        }

        if (updatetext) {
            //txt.text = g;
            updatetext = false;
        }

    }
}
