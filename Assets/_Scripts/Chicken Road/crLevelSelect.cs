﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Global;

public class crLevelSelect : MonoBehaviour {
    List<string> difficulty = new List<string>() {
        "easy", "medium", "hard"
    };

	myInput input;
	int swipeUpCount = 0;
	public List<string> defaultLevels;
	public List<GameObject> buttons;
	public AudioSource honkSource;
	public AudioClip honk;
	float lastSwipeUp;

    // Use this for initialization
	void Start () {
		input = GetComponent<myInput>();

		// temporary fix to eliminating the hack
		chickenRoad.hacked = false;

		// setting locked levels for visual people
		string lockedText = "Locked - Level ";
		ColorBlock temp = buttons[0].GetComponent<Button>().colors;
		temp.colorMultiplier = 5f;

		// 0 == free play, 1-9 == levels
		// free play and level 1 unlocked by default

		// locks levels
		for(int i = chickenRoad.highestLevelBeaten + 2; i <= 9; i++) {
			buttons[i].GetComponentInChildren<Text>().text = lockedText + i;
			buttons[i].GetComponent<Image>().color = Color.red;
			buttons[i].GetComponent<Button>().colors = temp;
		}
	}
	
	// Update is called once per frame
	void Update () {
		/*
		// hack to unlock all the levels (used for presentation)
		if (!chickenRoad.hacked && input.touch == gesture.UP) {
			float currentTime = Time.time;

			if (currentTime - lastSwipeUp > 1f) {
				swipeUpCount = 0;
			}

			lastSwipeUp = currentTime;
			swipeUpCount++;

			if (swipeUpCount >= 6) {
				chickenRoad.hacked = true;

				honkSource.PlayOneShot(honk);
			}
		}
		*/
	}

	public void touchReturnToMenuButton() {
		SceneManager.LoadScene("crMenu");
	}

	public void touchFreePlayButton() {
		chickenRoad.tutorials = "none";
		loadLevel(difficulty[2], "Free Play", "FREEPLAY");
	}

    public void touchLevelButton(int levelId) {
		chickenRoad.tutorials = "none";
		EasyTTSUtil.Stop();

		if (!chickenRoad.hacked && (levelId > chickenRoad.highestLevelBeaten + 1)) {
			EasyTTSUtil.SpeechAdd("Level " + levelId + " is not unlocked. Pass level " + (levelId - 1) + " first.", 1f, 0.6f, 1f);
			return;
		}

		switch(levelId) {
		case 1:
			if (!chickenRoad.easyTutorialPlayed) {
				chickenRoad.tutorials = "ICDG";
                chickenRoad.easyTutorialPlayed = true;
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
			if (!chickenRoad.mediumTutorialPlayed) {
				chickenRoad.tutorials = "FEL";
                chickenRoad.mediumTutorialPlayed = true;
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
			if (!chickenRoad.hardTutorialPlayed) {
				chickenRoad.tutorials = "PV";
				chickenRoad.hardTutorialPlayed = true;
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
        chickenRoad.difficulty = difficulty;
        chickenRoad.levelTitle = levelTitle;
        chickenRoad.levelObstacles = levelObstacles;

        SceneManager.LoadScene("crGame");
    }
}
