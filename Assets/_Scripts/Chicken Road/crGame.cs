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
	public List<AudioClip> tutorialClips;

    myInput input;
    string difficulty;
    string levelObstacles;
    int score = 0;
    int position = 0;
	int doubleTapCount = 0;
	int correctInputs = 0;
    List<GameObject> obstacles;
	List<gesture> commandList;
	List<AudioClip> playTutorials;
    GameObject displayedObstacle;
    Vector3 startingPosition = new Vector3(0, 1, -3);
    bool checkForCommand;
    bool showTutorial = false;
	bool isFreePlay = false;
    float reactionTime;
    float cruiseTime;
	float lastDoubleTapTime = 0f;
	gesture lastInput = gesture.NONE;

    // Use this for initialization
    void Start () {
        input = GetComponent<myInput>();
        obstacles = new List<GameObject>();
        commandList = new List<gesture>();
		playTutorials = new List<AudioClip>();
        playerVehicle = Instantiate(playerVehicle, startingPosition, transform.rotation) as GameObject;

        if (PlayerPrefs.HasKey("loggedInUser")) {
            usernameUI.GetComponent<Text>().text = PlayerPrefs.GetString("loggedInUser");
        }

        if (PlayerPrefs.HasKey("difficulty")) {
            difficulty = PlayerPrefs.GetString("difficulty");
        }

        if (PlayerPrefs.HasKey("levelTitle")) {
            modeUI.GetComponent<Text>().text = PlayerPrefs.GetString("levelTitle");
        }

		if (!PlayerPrefs.GetString("tutorials").Equals("none")) {
			showTutorial = true;

			string tutorials = PlayerPrefs.GetString("tutorials");

			for(int i = 0; i < tutorials.Length; i++) {
				playTutorials.Add(getTutorialClip(tutorials[i]));
			}
		}

		if (PlayerPrefs.HasKey("levelObstacles")) {
			levelObstacles = PlayerPrefs.GetString("levelObstacles");

			if (levelObstacles.Equals("FREEPLAY")) {
				isFreePlay = true;
				AddRandomObstacle();
			}
			else {
				if (levelObstacles.Length == 0) {
	                // ERROR: no value given
	                Debug.LogError("Empty Level Obstacles");
				}

	            // build obstacle list
				for (int i = 0; i < levelObstacles.Length; i++) {
					convertToObstacle(levelObstacles[i]);
				}
			}

			SetDifficulty(difficulty);

			StartCoroutine(StartGame());
		}
		else {
            // should abort...
            Debug.LogError("levelObstacles not set");
			SceneManager.LoadScene("crMenu");
		}

    }

	IEnumerator StartGame() {
        // play tutorial based on level/difficulty of introducing obstacle
        if (showTutorial) {
            Debug.Log("Tutorial...");

			for(int i = 0; i < playTutorials.Count; i++) {
				tutorialSource.PlayOneShot(playTutorials[i]);

				yield return new WaitForSeconds(playTutorials[i].length + 0.5f);
			}
        }

		yield return new WaitForSeconds(1f);

        Debug.Log("Playing car audio...");
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
		if (checkForCommand && input.touch != gesture.NONE) {
            checkForCommand = false;

			Debug.Log ("INPUT: " + input.touch);

            if (commandList.Contains(input.touch)) {
                // successful input
                score += (20 / commandList.Count);
                scoreUI.GetComponent<Text>().text = "Score: " + score;

				Debug.Log("Correct Input");
				correctInputs++;

				AudioClip carAction;

				switch(input.touch) {
				case gesture.UP:
					carAction = playerVehicle.GetComponent<playerVehicle>().SpeedUp();
					break;
				case gesture.DOWN:
					carAction = playerVehicle.GetComponent<playerVehicle>().SlowDown();
					break;
				case gesture.LEFT:
					carAction = playerVehicle.GetComponent<playerVehicle>().Swerve("left");
					break;
				case gesture.RIGHT:
					carAction = playerVehicle.GetComponent<playerVehicle>().Swerve("right");
					break;
				default:
					carAction = playerVehicle.GetComponent<playerVehicle>().Honk();
					break;
				}
					
				carSound.PlayOneShot(carAction);
            }
            else {
                // mistake made, hit obstacle
                Debug.Log("Incorrect Input");
				AudioClip badNoise = badSound;
				carSound.PlayOneShot(badNoise);
            }

            Destroy(displayedObstacle);
        }

		// Free Play check to quit
		if (isFreePlay) {
			if (Time.time - lastDoubleTapTime > 1f) {
				doubleTapCount = 0;
			}

			if (input.touch != gesture.NONE) {
				if (input.touch == gesture.DOUBLE) {
					lastDoubleTapTime = Time.time;
					doubleTapCount++;
				}
				else {
					doubleTapCount = 0;
				}
			}

			if (doubleTapCount >= 3) {
				Gameover();
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

            Debug.Log("Cruising...");

			yield return new WaitForSeconds(Random.Range(cruiseTime, cruiseTime + 2f));

			checkForCommand = true;

            Debug.Log("Dodge the " + obstacleName + "!");

			if (currentObstacle.GetComponent<obstacle> ().inFront) {
				displayedObstacle = Instantiate (currentObstacle, nearPoint.transform.position, transform.rotation) as GameObject;
			}
			else {
				displayedObstacle = Instantiate (currentObstacle, behindPoint.transform.position, transform.rotation) as GameObject;
			}

			AudioClip obstacleNoise = displayedObstacle.GetComponent<obstacle>().getSound();
			obstacleSound.PlayOneShot(obstacleNoise);

			// reaction time based on Difficulty
			yield return new WaitForSeconds(1f + reactionTime);

            // after alloted time, check to see if no input was given
            if (checkForCommand) {
                Debug.Log("No input given... obstacle hit");
                checkForCommand = false;

                Destroy(displayedObstacle);

				carSound.PlayOneShot(badSound);
            }

            Debug.Log("Fetching next obstacle...");
            position++;

			if (isFreePlay) {
				AddRandomObstacle();
			}
		}

		yield return new WaitForSeconds(1f);

		Gameover();
	}

	void Gameover() {
		// reached end of level
		checkForCommand = false;

		// put score into PlayerPrefs for Gameover
		PlayerPrefs.SetInt("score", score);

		// put accuracy into PlayerPrefs
		if (obstacles.Count > 0) {
			PlayerPrefs.SetInt("accuracy", (int) (100 * correctInputs / (float) obstacles.Count));
		}

		engine.Stop();

		SceneManager.LoadScene("crGameover");
	}

	void AddRandomObstacle() {
		convertToObstacle(Random.Range(0, 9));
	}

	void convertToObstacle(char c) {
		switch (c) {
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
				Debug.LogError("Unknown key: " + c);
				break;
		}
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

	AudioClip getTutorialClip(char c) {
		switch (c) {
			case 'I':
				return tutorialClips[0];
			case 'C':
				return tutorialClips[1];
			case 'D':
				return tutorialClips[2];
			case 'G':
				return tutorialClips[3];
			case 'F':
				return tutorialClips[4];
			case 'E':
				return tutorialClips[5];
			case 'L':
				return tutorialClips[6];
			case 'R':
				return tutorialClips[6];
			case 'P':
				return tutorialClips[7];
			case 'V':
				return tutorialClips[8];
			default:
				Debug.LogError("Unknown Tutorial Key: " + c);
				break;
		}

		return null;
	}

	void SetDifficulty(string diff) {
		switch(difficulty) {
			case "easy":
				reactionTime = 1f;
				cruiseTime = 4f;
				break;
			case "medium":
				reactionTime = 0.5f;
				cruiseTime = 2f;
				break;
			case "hard":
				reactionTime = 0.25f;
				cruiseTime = 1f;
				break;
			default:
				// ERROR: unknown difficulty
				Debug.LogError("Unknown difficulty: " + difficulty);
				break;
		}
	}
}
