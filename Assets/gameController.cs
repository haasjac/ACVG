using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class gameController : MonoBehaviour {

    public int perfectScore = 3;
    public int goodScore = 1;
    gesture o = gesture.DOUBLE;
    gestures g;
    public Text output;
    public Text input;
    public Text check;
    bool checking_command = false;
    bool perfect = false;
    int score;
    bool endGame = false;
    bool canRestart = false;

	// Use this for initialization
	void Start () {
        g = GetComponent<gestures>();
        score = 0;
        input.text = "";
        output.text = "";
        check.text = "";
        StartCoroutine(command());
    }
	
	// Update is called once per frame
	void Update () {
	    if (checking_command && g.touch != gesture.NONE) {
            checking_command = false;
            input.text = g.touch.ToString();
            if (g.touch == o) {
                if (perfect) {
                    check.text = "perfect";
                    score += perfectScore;
                } else {
                    check.text = "good";
                    score += goodScore;
                }
            } else {
                check.text = "bad";
                endGame = true;
            }
        }
	}

    IEnumerator command() {
        while (!endGame) {
            o = get_random_command();
            output.text = o.ToString();
            checking_command = true;
            perfect = true;
            yield return new WaitForSeconds(1f);
            perfect = false;
            yield return new WaitForSeconds(1f);
            if (checking_command) {
                check.text = "missed";
                endGame = true;
            }
            output.text = "";
            yield return new WaitForSeconds(1f);
        }
        input.text = "";
        check.text = "";
        output.text = score.ToString();
        yield return new WaitForSeconds(3f);
        endGame = false;
        canRestart = true;
        score = 0;
        input.text = "";
        output.text = "";
        check.text = "";
        StartCoroutine(command());
    }

    gesture get_random_command() {
        int r = Random.Range(0, 5);
        switch (r) {
            case 0:
                return gesture.DOUBLE;
            case 1:
                return gesture.DOWN;
            case 2:
                return gesture.LEFT;
            case 3:
                return gesture.RIGHT;
            case 4:
                return gesture.UP;
        }
        print("this code should never be reached: gameController.get_random_command");
        return gesture.UP;
    }
}
