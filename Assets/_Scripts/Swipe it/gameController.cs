using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Global;

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
    public AudioClip passIt;
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
    public myAudio _audio;
    public float turnTime = 1.5f;
    public float waitTime = 1f;
    public float increaseAmount = 0.05f;
    public Sprite arrowUp;
    public Sprite arrowDown;
    public Sprite arrowLeft;
    public Sprite arrowRight;
    public Sprite doubleTap;
    public Sprite passIt;
    public Image image;

    // private variables
    gesture command = gesture.DOUBLE;
    myInput input;
    bool checking_command = false;
    bool perfect = false;
    bool endGame = false;
    AudioClip chosenCommand;
    Sprite chosenSprite;

    // Use this for initialization
    void Start() {
        input = GetComponent<myInput>();
        swipeIt.currentScore = 0;
        startText.text = "";
        scoreText.text = "";
        StartCoroutine(countdown());
    }

    // Update is called once per frame
    void Update() {

        // pass it
        if (checking_command && command == gesture.NONE) {
            checking_command = false;
        }

        // check for input
        if (checking_command && input.touch != gesture.NONE) {
            checking_command = false;
            if (input.touch == command) {
                if (perfect) {
                    _audio.rating.PlayOneShot(_audio.perfect);
                    swipeIt.currentScore += perfectScore;
                    Handheld.Vibrate();
                } else {
                    _audio.rating.PlayOneShot(_audio.good);
                    swipeIt.currentScore += goodScore;
                    Handheld.Vibrate();
                }
            } else {
                _audio.rating.PlayOneShot(_audio.bad);
                if (swipeIt.mode != swipeIt.gameMode.tutorial)
                    endGame = true;
                Handheld.Vibrate();
                Handheld.Vibrate();
            }
        }
    }

    IEnumerator countdown() {
        int time = 3;
        while (time > 0) {
            if (accessibility.getAccessibility()) {
                EasyTTSUtil.SpeechAdd(time.ToString());
            }
            scoreText.text = time.ToString();
            yield return new WaitForSeconds(1);
            time--;
        }
        StartCoroutine(setCommand());
    }


    // coroutine for setting commands
    IEnumerator setCommand() {

        // set up conditions for game to start
        endGame = false;
        float r = -0.5f;
        int chance = 0;
        checking_command = false;
        swipeIt.currentScore = 0;
        scoreText.text = "";
        startText.text = "";
        Time.timeScale = 1 - increaseAmount;
        _audio.music.pitch = 1 - increaseAmount;
        StartCoroutine(setSpeed());

        // start game
        _audio.music.Play();
        yield return new WaitForSeconds(waitTime);
        
        // give commands until game ends
        while (!endGame) {

            switch (swipeIt.mode) {
                case swipeIt.gameMode.single:
                    // pick a random command except pass it
                    r = Random.Range(0, 5);
                    break;
                case swipeIt.gameMode.multi:
                    // pick a random command including pass it
                    r = Random.Range(0, 5);
                    if (chance >= 5) {
                        r = 5;
                        chance = 0;
                    } else {
                        chance += Random.Range(1, 6);
                    }
                    break;
                case swipeIt.gameMode.tutorial:
                    r += 0.5f;
                    if (r > 4f)
                        endGame = true;
                    break;
                default:
                    // pick a random command except pass it
                    r = Random.Range(0, 5);
                    break;
            }
            
            switch (Mathf.FloorToInt(r)) {
                case 0:
                    command = gesture.DOUBLE;
                    chosenCommand = _audio.doubleTap;
                    chosenSprite = doubleTap;
                    break;
                case 1:
                    command = gesture.DOWN;
                    chosenCommand = _audio.down;
                    chosenSprite = arrowDown;
                    break;
                case 2:
                    command = gesture.LEFT;
                    chosenCommand = _audio.left;
                    chosenSprite = arrowLeft;
                    break;
                case 3:
                    command = gesture.RIGHT;
                    chosenCommand = _audio.right;
                    chosenSprite = arrowRight;
                    break;
                case 4:
                    command = gesture.UP;
                    chosenCommand = _audio.up;
                    chosenSprite = arrowUp;
                    break;
                case 5:
                    command = gesture.NONE;
                    chosenCommand = _audio.passIt;
                    chosenSprite = passIt;
                    break;
                default:
                    command = gesture.UP;
                    chosenCommand = _audio.perfect;
                    chosenSprite = arrowUp;
                    break;
            }

            // play the audio for the command
            _audio.command.PlayOneShot(chosenCommand);
            // 0.4 is average audio clip length. wait to try to sync audio with visual
            yield return new WaitForSecondsRealtime(0.4f);
            if (command == gesture.NONE) {
                scoreText.text = "PASS IT";
            } else {
                scoreText.text = command.ToString();
            }
            image.sprite = chosenSprite;
            checking_command = true;
            perfect = true;
            yield return new WaitForSeconds(0.8f * turnTime);
            perfect = false;
            yield return new WaitForSeconds(0.2f * turnTime);
            if (checking_command) {
                checking_command = false;
                _audio.rating.PlayOneShot(_audio.bad);
                if (swipeIt.mode != swipeIt.gameMode.tutorial)
                    endGame = true;
                Handheld.Vibrate();
                Handheld.Vibrate();
            }
            scoreText.text = "";
            image.sprite = null;
            yield return new WaitForSeconds(waitTime);
        }

        // play game over sound and stop music
        _audio.rating.PlayOneShot(_audio.gameOver);
        _audio.music.Stop();

        yield return new WaitForSeconds(_audio.gameOver.length + 0.5f);

        if (swipeIt.mode == swipeIt.gameMode.tutorial)
            swipeIt.mode = swipeIt.gameMode.single;

        SceneManager.LoadScene("gameover");
    }

    // increase the games speed every few seconds
    IEnumerator setSpeed() {
        while (!endGame) {
            //print("speed up " + Time.time);
            Time.timeScale += increaseAmount;
            _audio.music.pitch += increaseAmount / 2;
            yield return new WaitForSecondsRealtime(4 * turnTime);
        }
    }

}
