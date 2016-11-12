﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class crLevelSelect : MonoBehaviour {
    List<string> difficulty = new List<string>() {
        "easy", "medium", "hard"
    };

	public List<string> defaultLevels;

    // Use this for initialization
	void Start () {
		if (!PlayerPrefs.HasKey("playedEasyTutorial")) {
			PlayerPrefs.SetInt("playedEasyTutorial", 0);
		}
		if (!PlayerPrefs.HasKey("playedMediumTutorial")) {
			PlayerPrefs.SetInt("playedMediumTutorial", 0);
		}
		if (!PlayerPrefs.HasKey("playedHardTutorial")) {
			PlayerPrefs.SetInt("playedHardTutorial", 0);
		}


		PlayerPrefs.SetInt("playedEasyTutorial", 0);

		PlayerPrefs.SetInt("playedMediumTutorial", 0);

		PlayerPrefs.SetInt("playedHardTutorial", 0);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void touchReturnToMenuButton() {
		SceneManager.LoadScene("crMenu");
	}

	public void touchFreePlayButton() {
		loadLevel(difficulty[2], "Free Play", "FREEPLAY");
	}

    public void touchLevelButton(int levelId) {
		PlayerPrefs.SetString("tutorials", "none");

		switch(levelId) {
		case 1:
			if (PlayerPrefs.GetInt("playedEasyTutorial") != 1) {
				PlayerPrefs.SetString("tutorials", "ICDG");
				PlayerPrefs.SetInt("playedEasyTutorial", 1);
			}
			
			loadLevel(difficulty[0], "Level " + levelId, defaultLevels[levelId-1]);
			break;
		case 2:
			loadLevel(difficulty[0], "Level " + levelId, defaultLevels[levelId-1]);
			break;
		case 3:
			loadLevel(difficulty[0], "Level " + levelId, defaultLevels[levelId-1]);
			break;
		case 4:
			if (PlayerPrefs.GetInt("playedMediumTutorial") != 1) {
				PlayerPrefs.SetString("tutorials", "FEL");
				PlayerPrefs.SetInt("playedMediumTutorial", 1);
			}

			loadLevel(difficulty[1], "Level " + levelId, defaultLevels[levelId-1]);
			break;
		case 5:
			loadLevel(difficulty[1], "Level " + levelId, defaultLevels[levelId-1]);
			break;
		case 6:
			loadLevel(difficulty[1], "Level " + levelId, defaultLevels[levelId-1]);
			break;
		case 7:
			if (PlayerPrefs.GetInt("playedHardTutorial") != 1) {
				PlayerPrefs.SetString("tutorials", "PV");
				PlayerPrefs.SetInt("playedHardTutorial", 1);
			}

			loadLevel(difficulty[2], "Level " + levelId, defaultLevels[levelId-1]);
			break;
		case 8:
			loadLevel(difficulty[2], "Level " + levelId, defaultLevels[levelId-1]);
			break;
		case 9:
			loadLevel(difficulty[2], "Level " + levelId, defaultLevels[levelId-1]);
			break;
		default:
			// Unknown LevelID
			break;
		}
    }

    void loadLevel(string difficulty, string levelTitle, string levelObstacles) {
        PlayerPrefs.SetString("difficulty", difficulty);
        PlayerPrefs.SetString("levelTitle", levelTitle);
        PlayerPrefs.SetString("levelObstacles", levelObstacles);

        SceneManager.LoadScene("crGame");
    }
}
