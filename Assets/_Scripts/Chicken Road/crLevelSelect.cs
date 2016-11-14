using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class crLevelSelect : MonoBehaviour {
    List<string> difficulty = new List<string>() {
        "easy", "medium", "hard"
    };

	myInput input;
	bool hackedLevels = false;
	int swipeUpCount = 0;
	public List<string> defaultLevels;
	public AudioSource honkSource;
	public AudioClip honk;
	float lastSwipeUp;

    // Use this for initialization
	void Start () {
		input = GetComponent<myInput>();

		PlayerPrefs.SetInt("hackedLevels", 0);

		if (!PlayerPrefs.HasKey("playedEasyTutorial")) {
			PlayerPrefs.SetInt("playedEasyTutorial", 0);
		}
		if (!PlayerPrefs.HasKey("playedMediumTutorial")) {
			PlayerPrefs.SetInt("playedMediumTutorial", 0);
		}
		if (!PlayerPrefs.HasKey("playedHardTutorial")) {
			PlayerPrefs.SetInt("playedHardTutorial", 0);
		}

		if (!PlayerPrefs.HasKey("level1Unlocked")) {
			PlayerPrefs.SetInt("level1Unlocked", 1);
			PlayerPrefs.SetInt("level2Unlocked", 0);
			PlayerPrefs.SetInt("level3Unlocked", 0);
			PlayerPrefs.SetInt("level4Unlocked", 0);
			PlayerPrefs.SetInt("level5Unlocked", 0);
			PlayerPrefs.SetInt("level6Unlocked", 0);
			PlayerPrefs.SetInt("level7Unlocked", 0);
			PlayerPrefs.SetInt("level8Unlocked", 0);
			PlayerPrefs.SetInt("level9Unlocked", 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// hack to unlock all the levels (used for presentation)
		if (!hackedLevels && input.touch == gesture.UP) {
			float currentTime = Time.time;

			if (currentTime - lastSwipeUp > 1f) {
				swipeUpCount = 0;
			}

			lastSwipeUp = currentTime;
			swipeUpCount++;

			if (swipeUpCount >= 6) {
				hackedLevels = true;
				PlayerPrefs.SetInt("hackedLevels", 1);

				honkSource.PlayOneShot(honk);
			}
		}
	}

	public void touchReturnToMenuButton() {
		SceneManager.LoadScene("crMenu");
	}

	public void touchFreePlayButton() {
		loadLevel(difficulty[2], "Free Play", "FREEPLAY");
	}

    public void touchLevelButton(int levelId) {
		PlayerPrefs.SetString("tutorials", "none");

		if (!hackedLevels && PlayerPrefs.GetInt("level" + levelId + "Unlocked") == 0) {
			EasyTTSUtil.SpeechAdd("Level " + levelId + " is not unlocked. Pass level " + (levelId - 1) + " first.", 1f, 0.6f, 1f);
			return;
		}

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
