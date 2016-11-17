using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Global;

public class gameOver : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip newHighScore;
    public Text scoreText;

	// Use this for initialization
	void Start () {
        if (swipeIt.checkHighScore()) {
            audioSource.PlayOneShot(newHighScore);
        }
        scoreText.text = "Your Score: " + swipeIt.currentScore + "\nHigh Score: " + swipeIt.getHighScore();	
	}
}
