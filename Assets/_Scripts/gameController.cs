using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class myAudio {
    public AudioSource music;
    public AudioSource command;
    public AudioSource rating;
    public AudioClip up;
    public AudioClip right;
    public AudioClip left;
    public AudioClip down;
    public AudioClip doubleTap;
    public AudioClip perfect;
    public AudioClip good;
    public AudioClip bad;
    public AudioClip gameOver;
}

public class gameController : MonoBehaviour {

    // public variables
    public int perfectScore = 3;
    public int goodScore = 1;
    public Text scoreText; 
    public Text startText;
    public new myAudio audio;
    public float turnTime = 1.5f;
    public float waitTime = 1f;
    public float increaseAmount = 0.05f;

    // private variables
    gesture command = gesture.DOUBLE;
    myInput input;
    bool checking_command = false;
    bool perfect = false;
    bool endGame = false;
    bool canRestart = false;
    AudioClip chosenCommand;

    // Use this for initialization
    void Start () {
        input = GetComponent<myInput>();
        global.S.currentScore = 0;
        startText.text = "double tap to start";
        scoreText.text = "";
        canRestart = true;
        EasyTTSUtil.Initialize(EasyTTSUtil.UnitedStates);
        EasyTTSUtil.SpeechAdd("double tap to start");
    }
	
	// Update is called once per frame
	void Update () {

        // start/restart game with double tap
        if (canRestart && input.touch == gesture.DOUBLE) {
            canRestart = false;
            StartCoroutine(setCommand());
        }

        // check for input
	    if (checking_command && input.touch != gesture.NONE) {
            checking_command = false;
            if (input.touch == command) {
                if (perfect) {
                    audio.rating.PlayOneShot(audio.perfect);
                    global.S.currentScore += perfectScore;
                } else {
                    audio.rating.PlayOneShot(audio.good);
                    global.S.currentScore += goodScore;
                }
            } else {
                audio.rating.PlayOneShot(audio.bad);
                endGame = true;
            }
        }
	}


    // coroutine for setting commands
    IEnumerator setCommand() {

        // set up conditions for game to start
        endGame = false;
        canRestart = false;
        checking_command = false;
        global.S.currentScore = 0;
        scoreText.text = "";
        startText.text = "";
        Time.timeScale = 1 - increaseAmount;
        audio.music.pitch = 1 - increaseAmount;
        StartCoroutine(setSpeed());

        // start game
        audio.music.Play();
        yield return new WaitForSeconds(waitTime);
        
        // give commands until game ends
        while (!endGame) {

            // pick a random command
            int r = Random.Range(0, 5);
            switch (r) {
                case 0:
                    command = gesture.DOUBLE;
                    chosenCommand = audio.doubleTap;
                    break;
                case 1:
                    command = gesture.DOWN;
                    chosenCommand = audio.down;
                    break;
                case 2:
                    command = gesture.LEFT;
                    chosenCommand = audio.left;
                    break;
                case 3:
                    command = gesture.RIGHT;
                    chosenCommand = audio.right;
                    break;
                case 4:
                    command = gesture.UP;
                    chosenCommand = audio.up;
                    break;
                default:
                    command = gesture.UP;
                    chosenCommand = audio.up;
                    break;
            }

            // play the audio for the command
            audio.command.PlayOneShot(chosenCommand);
            // 0.4 is average audio clip length. wait to try to sync audio with visual
            yield return new WaitForSeconds(0.4f);
            scoreText.text = command.ToString();
            checking_command = true;
            perfect = true;
            yield return new WaitForSeconds(0.8f * turnTime);
            perfect = false;
            yield return new WaitForSeconds(0.2f * turnTime);
            if (checking_command) {
                checking_command = false;
                endGame = true;
            }
            scoreText.text = "";
            yield return new WaitForSeconds(waitTime);
        }

        // play game over sound and stop music
        audio.rating.PlayOneShot(audio.gameOver);
        audio.music.Stop();

        // display score
        startText.text = "";
        scoreText.text = "Score: " + global.S.currentScore.ToString();

        // after a delay allow the game to restart
        yield return new WaitForSeconds(waitTime);
        EasyTTSUtil.SpeechAdd("Score: " + global.S.currentScore.ToString());
        EasyTTSUtil.SpeechAdd("double tap to restart");
        canRestart = true;
        startText.text = "double tap to restart";
    }

    // increase the games speed every few seconds
    IEnumerator setSpeed() {
        while (!endGame) {
            //print("speed up " + Time.time);
            Time.timeScale += increaseAmount;
            audio.music.pitch += increaseAmount;
            yield return new WaitForSecondsRealtime(4 * turnTime);
        }
    }

}
