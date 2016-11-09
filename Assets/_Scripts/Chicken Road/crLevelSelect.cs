using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class crLevelSelect : MonoBehaviour {
    List<string> difficulty = new List<string>() {
        "easy", "medium", "hard"
    };

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

    public void touchLevelButton(int levelId) {
        loadLevel(difficulty[0], "Challenge " + levelId, "CCCCC");
    }

    void loadLevel(string difficulty, string levelTitle, string levelObstacles) {
        PlayerPrefs.SetString("difficulty", difficulty);
        PlayerPrefs.SetString("levelTitle", levelTitle);
        PlayerPrefs.SetString("levelObstacles", levelObstacles);

        SceneManager.LoadScene("crGame");
    }
}
