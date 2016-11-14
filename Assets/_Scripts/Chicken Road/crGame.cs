using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class crGame : MonoBehaviour {
	public GameObject playerVehicle;
	public GameObject nearPoint;
	public GameObject behindPoint;
	public GameObject modeUI;
	public GameObject scoreUI;
	public GameObject usernameUI;

    public GameObject chicken;
	public GameObject ducks;
	public GameObject goose;
	public GameObject fence;
	public GameObject deer;
	public GameObject parrot;
	public GameObject racecar;
	public GameObject forkLeft;
	public GameObject forkRight;

	public AudioSource tutorialSource;
	public AudioSource engine;
	public AudioSource obstacleSound;
	public AudioSource carSound;
	public AudioClip badSound;

    myInput input;
    string levelObstacles;
	string tutorials;
    int score = 0;
    int position = 0;
	int doubleTapCount = 0;
	int correctInputs = 0;
    List<GameObject> obstacles;
	List<gesture> commandList;
    GameObject displayedObstacle;
    Vector3 startingPosition = new Vector3(0, 1, -3);
    bool checkForCommand;
    bool showTutorial = false;
	bool isFreePlay = false;
	bool isHowToPlay = false;
    float reactionTime;
    float cruiseTime;
	float lastDoubleTapTime = 0f;

    // Use this for initialization
    void Start () {
        input = GetComponent<myInput>();
        obstacles = new List<GameObject>();
        commandList = new List<gesture>();
        playerVehicle = Instantiate(playerVehicle, startingPosition, transform.rotation) as GameObject;

        if (PlayerPrefs.HasKey("loggedInUser")) {
            usernameUI.GetComponent<Text>().text = PlayerPrefs.GetString("loggedInUser");
        }

        if (PlayerPrefs.HasKey("difficulty")) {
			SetDifficulty(PlayerPrefs.GetString("difficulty"));
        }

        if (PlayerPrefs.HasKey("levelTitle")) {
            modeUI.GetComponent<Text>().text = PlayerPrefs.GetString("levelTitle");
        }

		if (!PlayerPrefs.GetString("tutorials").Equals("none")) {
			showTutorial = true;
			tutorials = PlayerPrefs.GetString("tutorials");
		}

		if (PlayerPrefs.GetInt("crHowToPlay") == 1) {
			isHowToPlay = true;
		}

		if (PlayerPrefs.HasKey("levelObstacles")) {
			levelObstacles = PlayerPrefs.GetString("levelObstacles");

			if (levelObstacles.Equals("FREEPLAY")) {
				isFreePlay = true;
				AddRandomObstacle();
			}
			else {
	            // build obstacle list
				for (int i = 0; i < levelObstacles.Length; i++) {
					convertToObstacle(levelObstacles[i]);
				}
			}

			StartCoroutine(StartGame());
		}
		else {
			SceneManager.LoadScene("crMenu");
		}

    }

	IEnumerator StartGame() {
		// how to quit
		EasyTTSUtil.SpeechAdd("Double Tap 3 times to quit.", 1f, 0.6f, 1f);

		yield return new WaitForSeconds(2f);

        // play tutorial based on level/difficulty of introducing obstacle
		if (showTutorial) {
			for(int i = 0; i < tutorials.Length; i++) {
				AudioClip tutorialClip = getTutorialClip(tutorials[i]);

				tutorialSource.PlayOneShot(tutorialClip);

				yield return new WaitForSeconds(tutorialClip.length + 0.5f);
			}
        }

		for(int i = 1; i <= 2; i++) {
			carSound.PlayOneShot(playerVehicle.GetComponent<playerVehicle>().SpeedUp());
			yield return new WaitForSeconds(0.65f);
		}

		engine.clip = playerVehicle.GetComponent<playerVehicle>().StartEngine();
		engine.loop = true;
		engine.Play();

		yield return new WaitForSeconds(1f);

		StartCoroutine(SetObstacle());
	}

	// Update is called once per frame
	void Update () {
        // wait for command and input, otherwise do nothing
		if (input.touch != gesture.NONE) {
			
			// check to quit
			if (input.touch == gesture.DOUBLE) {
				float currentTime = Time.time;

				if (currentTime - lastDoubleTapTime > 1f) {
					doubleTapCount = 0;
				}

				lastDoubleTapTime = currentTime;
				doubleTapCount++;

				if (doubleTapCount >= 3) {
					Gameover();
				}
			}

			if (checkForCommand) {
				checkForCommand = false;

				if (commandList.Contains(input.touch)) {
					// successful input
					score += (20 / commandList.Count);
					scoreUI.GetComponent<Text>().text = "Score: " + score;

					correctInputs++;

					switch(input.touch) {
						case gesture.UP:
							carSound.PlayOneShot(playerVehicle.GetComponent<playerVehicle>().SpeedUp());
							break;
						case gesture.DOWN:
							carSound.PlayOneShot(playerVehicle.GetComponent<playerVehicle>().SlowDown());
							break;
						case gesture.LEFT:
							carSound.PlayOneShot(playerVehicle.GetComponent<playerVehicle>().Swerve("left"));
							break;
						case gesture.RIGHT:
							carSound.PlayOneShot(playerVehicle.GetComponent<playerVehicle>().Swerve("right"));
							break;
						case gesture.DOUBLE:
							carSound.PlayOneShot(playerVehicle.GetComponent<playerVehicle>().Honk());
							break;
						default:
							// ???
							break;
					}
				}
				else {
					// mistake made, hit obstacle
					carSound.PlayOneShot(badSound);
				}

				Destroy(displayedObstacle);
			}
        }
    }

	IEnumerator SetObstacle() {
		checkForCommand = false;

		// play until seen all obstacles
		while (position < obstacles.Count) {
			GameObject currentObstacle = obstacles[position];
			string obstacleName = currentObstacle.name;

			// remove old commands
			commandList = new List<gesture>();

			commandList = currentObstacle.GetComponent<obstacle>().getCommands();

			yield return new WaitForSeconds(Random.Range(1f, cruiseTime));

			if (currentObstacle.GetComponent<obstacle>().inFront) {
				displayedObstacle = Instantiate (currentObstacle, nearPoint.transform.position, transform.rotation) as GameObject;
			}
			else {
				displayedObstacle = Instantiate (currentObstacle, behindPoint.transform.position, transform.rotation) as GameObject;
			}

			checkForCommand = true;

			AudioClip obstacleNoise = displayedObstacle.GetComponent<obstacle>().getSound();
			obstacleSound.PlayOneShot(obstacleNoise);

			// reaction time based on Difficulty
			yield return new WaitForSeconds(obstacleNoise.length + reactionTime);

            // after alloted time, check to see if no input was given
            if (checkForCommand) {
                checkForCommand = false;
                Destroy(displayedObstacle);
				carSound.PlayOneShot(badSound);
            }

            position++;

			if (isFreePlay) {
				AddRandomObstacle();
			}
		}

		yield return new WaitForSeconds(1f);

		Gameover();
	}

	void Gameover() {
		checkForCommand = false;

		// free play misses an obstacle when quitting
		int seenObstacles = isFreePlay ? obstacles.Count - 1 : obstacles.Count;

		int accuracy = 0;

		if (obstacles.Count > 0) {
			accuracy = (int)(100 * correctInputs / (float)seenObstacles);
		}

		// eliminate edge case for free play
		if (accuracy > 100) {
			accuracy = 100;
		}

		PlayerPrefs.SetInt("accuracy", accuracy);
		PlayerPrefs.SetInt("score", score);

		if (accuracy >= 60 && !isFreePlay && !isHowToPlay) {
			unlockNextLevel(PlayerPrefs.GetString("levelTitle"));
		}

		engine.Stop();

		if (isHowToPlay) {
			SceneManager.LoadScene("crHowToPlay");
		}
		else {
			SceneManager.LoadScene("crGameover");
		}
	}

	void convertToObstacle(char obstacleChar) {
		switch (obstacleChar) {
			case 'C':
				obstacles.Add(chicken);
				break;
			case 'D':
				obstacles.Add(ducks);
				break;
			case 'G':
				obstacles.Add(goose);
				break;
			case 'F':
				obstacles.Add(fence);
				break;
			case 'E':
				obstacles.Add(deer);
				break;
			case 'L':
				obstacles.Add(forkLeft);
				break;
			case 'R':
				obstacles.Add(forkRight);
				break;
			case 'P':
				obstacles.Add(parrot);
				break;
			case 'V':
				obstacles.Add(racecar);
				break;
			default:
				Debug.LogError("Unknown key: " + obstacleChar);
				break;
		}
	}

	void AddRandomObstacle() {
		convertToObstacle(Random.Range(0, 9));
	}

	void convertToObstacle(int i) {
		switch (i) {
		case 0:
			obstacles.Add(chicken);
			break;
		case 1:
			obstacles.Add(ducks);
			break;
		case 2:
			obstacles.Add(goose);
			break;
		case 3:
			obstacles.Add(fence);
			break;
		case 4:
			obstacles.Add(deer);
			break;
		case 5:
			obstacles.Add(forkLeft);
			break;
		case 6:
			obstacles.Add(forkRight);
			break;
		case 7:
			obstacles.Add(parrot);
			break;
		case 8:
			obstacles.Add(racecar);
			break;
		default:
			Debug.LogError("Unknown key: " + i);
			break;
		}
	}

	AudioClip getTutorialClip(char clipID) {
		switch (clipID) {
			case 'I':
				return (AudioClip) Resources.Load("CR_Intro");
			case 'C':
				return (AudioClip) Resources.Load("CR_Tutorial_Chicken");
			case 'D':
				return (AudioClip) Resources.Load("CR_Tutorial_Duck");
			case 'G':
				return (AudioClip) Resources.Load("CR_Tutorial_Goose");
			case 'F':
				return (AudioClip) Resources.Load("CR_Tutorial_Fence");
			case 'E':
				return (AudioClip) Resources.Load("CR_Tutorial_Deer");
			case 'L':
				return (AudioClip) Resources.Load("CR_Tutorial_Fork");
			case 'R':
				return (AudioClip) Resources.Load("CR_Tutorial_Fork");
			case 'P':
				return (AudioClip) Resources.Load("CR_Tutorial_Parrot");
			case 'V':
				return (AudioClip) Resources.Load("CR_Tutorial_Racecar");
			default:
				Debug.LogError("Unknown Tutorial Key: " + clipID);
				break;
		}

		return null;
	}

	void SetDifficulty(string diff) {
		switch(diff) {
			case "easy":
				reactionTime = 1.5f;
				cruiseTime = 3f;
				break;
			case "medium":
				reactionTime = 1.25f;
				cruiseTime = 2.5f;
				break;
			case "hard":
				reactionTime = 1f;
				cruiseTime = 2f;
				break;
			default:
				SceneManager.LoadScene("crMenu");
				break;
		}
	}

	void unlockNextLevel(string levelTitle) {
		int id = (int)levelTitle.Substring(6) + 1;
		PlayerPrefs.SetInt("level" + id + "Unlocked", 1);
	}
}
