using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class crGameover : MonoBehaviour {
	public GameObject passFailUI;
	public GameObject scoreUI;
	public GameObject accuracyUI;

	// Use this for initialization
	void Start() {
		int score = PlayerPrefs.GetInt("score");
		int accuracy = PlayerPrefs.GetInt("accuracy");

		if (!PlayerPrefs.GetString("levelTitle").Equals("Free Play")) {
			string results;

			if (accuracy == 100) {
				results = "PERFECT! You passed!";
			}
			else if (accuracy >= 80) {
				results = "Excellent! You passed!";
			}
			else if (accuracy >= 60) {
				results = "You passed!";
			}
			else {
				results = "You failed. Try again!";
			}

			passFailUI.GetComponent<Text>().text = results;
		}
		else {
			passFailUI.GetComponent<Text>().text = "Free play completed.";
		}

		scoreUI.GetComponent<Text>().text = "Score: " + score + " points";
		accuracyUI.GetComponent<Text>().text = "Accuracy: " + accuracy + "%";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void touchReturnToMenuButton() {
		SceneManager.LoadScene("crMenu");
	}

	public void touchReturnToLevelSelectButton() {
		SceneManager.LoadScene("crLevelSelect");
	}
}
