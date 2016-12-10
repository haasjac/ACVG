using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Global;

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
	public AudioSource answerSource;
	public AudioClip badSound;
	public AudioClip correctSound;

    myInput input;
    string levelObstacles;
	string tutorials;
    int score = 0;
    int position = 0;
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

    // Use this for initialization
    void Start () {
        input = GetComponent<myInput>();
        obstacles = new List<GameObject>();
        commandList = new List<gesture>();
        playerVehicle = Instantiate(playerVehicle, startingPosition, transform.rotation) as GameObject;

        if (facebook.name != "") {
            usernameUI.GetComponent<Text>().text = facebook.name;
        }

        SetDifficulty(chickenRoad.difficulty);
        modeUI.GetComponent<Text>().text = chickenRoad.levelTitle;

		if (chickenRoad.tutorials != "none") {
			showTutorial = true;
			tutorials = chickenRoad.tutorials;
		}

		isHowToPlay = chickenRoad.crHowToPlay;

		levelObstacles = chickenRoad.levelObstacles;

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

	IEnumerator StartGame() {
		// how to quit
		EasyTTSUtil.SpeechAdd("Long press at anytime to quit.", 1f, 0.6f, 1f);

		yield return new WaitForSeconds(1.5f);

        // play tutorial based on level/difficulty of introducing obstacle
		if (showTutorial) {
			for(int i = 0; i < tutorials.Length; i++) {
				List<AudioClip> noises = new List<AudioClip>();
				string commandText = "";

				switch (tutorials[i]) {
					case 'I':
						commandText = "Welcome to Chicken Road! Your goal is to dodge the obstacles. You will hear a sound, and you must respond with the correct gesture. Beat a level by dodging 60% or more of the obstacles. You can also try free play so the fun never ends!";
						break;
					case 'C':
						EasyTTSUtil.SpeechAdd("Chicken sound", 1f, 0.6f, 1f);
						yield return new WaitForSeconds(1f);
						noises.Add(chicken.GetComponent<obstacle>().getSound());
						commandText = "Swipe left or right to dodge the chicken";
						break;
					case 'D':
						EasyTTSUtil.SpeechAdd("Duck sound", 1f, 0.6f, 1f);
						yield return new WaitForSeconds(1f);
						noises.Add(ducks.GetComponent<obstacle>().getSound());
						commandText = "Swipe down to avoid hitting the ducks";
						break;
					case 'G':
						EasyTTSUtil.SpeechAdd("Goose sound", 1f, 0.6f, 1f);
						yield return new WaitForSeconds(1f);
						noises.Add(goose.GetComponent<obstacle>().getSound());
						commandText = "Swipe up to escape the goose";
						break;
					case 'F':
						EasyTTSUtil.SpeechAdd("Fence sound", 1f, 0.6f, 1f);
						yield return new WaitForSeconds(1f);
						noises.Add(fence.GetComponent<obstacle>().getSound());
						commandText = "Swipe up to break through the fence";
						break;
					case 'E':
						EasyTTSUtil.SpeechAdd("Deer sound", 1f, 0.6f, 1f);
						yield return new WaitForSeconds(1f);
						noises.Add(deer.GetComponent<obstacle>().getSound());
						commandText = "Swipe down to avoid hitting the deer";
						break;
					case 'L':
						EasyTTSUtil.SpeechAdd("Fork sound", 1f, 0.6f, 1f);
						yield return new WaitForSeconds(1f);
						noises.Add(forkLeft.GetComponent<obstacle>().getSound());
						noises.Add(forkRight.GetComponent<obstacle>().getSound());
						commandText = "Swipe with the command to correctly follow the fork";
						break;
					case 'P':
						EasyTTSUtil.SpeechAdd("Parrot sound", 1f, 0.6f, 1f);
						yield return new WaitForSeconds(1f);
						noises.Add(parrot.GetComponent<obstacle>().getSound());
						commandText = "Double tap to get rid of the parrot";
						break;
					case 'V':
						EasyTTSUtil.SpeechAdd("Race car sound", 1f, 0.6f, 1f);
						yield return new WaitForSeconds(1f);
						noises.Add(racecar.GetComponent<obstacle>().getSound());
						commandText = "Swipe up or down to avoid hitting the race car";
						break;
					default:
						break;
				}

				int currentNoise = 0;
				while (currentNoise < noises.Count) {
					tutorialSource.PlayOneShot(noises[currentNoise]);
					yield return new WaitForSeconds(noises[currentNoise].length + 0.2f);

					currentNoise++;
				}

				EasyTTSUtil.SpeechAdd(commandText, 1f, 0.6f, 1f);

				if (tutorials[i] == 'I') {
					yield return new WaitForSeconds(13f);
				}
				else {
					yield return new WaitForSeconds(3f);
				}
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
			obstacleSound.Stop();
			
			// check to quit
			if (input.touch == gesture.HOLD) {
				Gameover();
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
							playerVehicle.GetComponent<playerVehicle>().SpeedUp();
							break;
						case gesture.DOWN:
							playerVehicle.GetComponent<playerVehicle>().SlowDown();
							break;
						case gesture.LEFT:
							playerVehicle.GetComponent<playerVehicle>().Swerve("left");
							break;
						case gesture.RIGHT:
							playerVehicle.GetComponent<playerVehicle>().Swerve("right");
							break;
						case gesture.DOUBLE:
							playerVehicle.GetComponent<playerVehicle>().Honk();
							break;
						default:
							// ???
							break;
					}
						
					answerSource.PlayOneShot(correctSound);
				}
				else {
					// mistake made, hit obstacle
					answerSource.PlayOneShot(badSound);
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
				answerSource.PlayOneShot(badSound);
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

        chickenRoad.accuracy = accuracy;
        chickenRoad.score = score;

		if (!chickenRoad.hacked) {
			if (accuracy >= 60 && !isFreePlay && !isHowToPlay) {
				unlockNextLevel(chickenRoad.levelTitle);
			}
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
		int id = int.Parse(levelTitle.Substring(6));

        if (id > chickenRoad.highestLevelBeaten) {
            chickenRoad.highestLevelBeaten = id;
            myApi.S.StartCoroutine(myApi.S.postLevel(id, true));
            functions.save();
        }
	}
}
