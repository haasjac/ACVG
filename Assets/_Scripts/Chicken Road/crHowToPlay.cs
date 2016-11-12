using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class crHowToPlay : MonoBehaviour {
	public AudioSource tutorialSource;
	public List<AudioClip> tutorials;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void touchTutorialButton(int id) {
		if (tutorialSource.isPlaying) {
			tutorialSource.Stop();
		}

		tutorialSource.PlayOneShot(tutorials[id]);
	}

	public void touchReturnToMenuButton() {
		if (tutorialSource.isPlaying) {
			tutorialSource.Stop();
		}

		SceneManager.LoadScene("crMenu");
	}
}
