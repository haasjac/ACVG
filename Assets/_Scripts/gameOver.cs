﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameOver : MonoBehaviour {

    public List<Button> buttons;
    public AudioSource audioSource;
    public List<AudioClip> audioClips;
    public AudioClip newHighScore;
    public Text scoreText;

    private string nextScene;
    private List<string> scenes;

	// Use this for initialization
	void Start () {
        if (global.S.accessibility) {
            scenes = new List<string>();
            scenes.Add("si_game");
            scenes.Add("game_menu");
            scenes.Add("mainMenu");

            for (int i = 0; i < buttons.Count; ++i) {
                buttons[i].interactable = false;
            }
        }
        if (global.S.checkHighScore()) {
            audioSource.PlayOneShot(newHighScore);
        }
        scoreText.text = "Your Score: " + global.S.currentScore + "\nHigh Score: " + global.S.highScore;

        if (global.S.checkHighScore()) {
            audioSource.PlayOneShot(newHighScore);
        }

        if (global.S.accessibility) {
            StartCoroutine(scoreAudio());
        }		
	}

    IEnumerator scoreAudio() {
        
        if (global.S.checkHighScore())
            yield return new WaitForSeconds(newHighScore.length + 0.2f);

        EasyTTSUtil.SpeechAdd(scoreText.text);
            
        yield return new WaitForSeconds(5);
        StartCoroutine(fakeVoiceOver());
        yield return null;
    }

    private IEnumerator fakeVoiceOver() {
        int i = 0;

        while (true) {
            nextScene = scenes[i];

            buttons[i].interactable = true;

            audioSource.clip = audioClips[i];
            audioSource.Play();

            yield return new WaitForSeconds(2);

            buttons[i].interactable = false;

            ++i;
            if (i == audioClips.Count) {
                i = 0; 
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	    if (global.S.accessibility) {
            //if (Input.touchCount == 1) {
                //if (Input.GetTouch(0).phase == TouchPhase.Ended) {
                if (Input.GetMouseButtonDown(0)) {
                    SceneManager.LoadScene(nextScene);  
                } 
            //}
        
        }
	}
}
