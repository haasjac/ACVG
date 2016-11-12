﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class crGame : MonoBehaviour {
    public GameObject playerVehicle;
    public GameObject chicken;
    public GameObject farPoint;
    public GameObject nearPoint;
    public GameObject behindPoint;
    public GameObject modeUI;
    public GameObject scoreUI;
    public GameObject usernameUI;

    myInput input;
    string difficulty;
    string levelObstacles;
    int score = 0;
    int mistakes = 0;
    int position = 0;
    List<GameObject> obstacles;
	List<gesture> commandList;
    GameObject displayedObstacle;
    Vector3 startingPosition = new Vector3(0, 1, -3);
    bool checkForCommand;
    bool showTutorial;
    float reactionTime;
    float cruiseTime;

    // Use this for initialization
    void Start () {
        input = GetComponent<myInput>();
        obstacles = new List<GameObject>();
        commandList = new List<gesture>();
        playerVehicle = Instantiate(playerVehicle, startingPosition, transform.rotation) as GameObject;

        // temp
        showTutorial = false;

        if (PlayerPrefs.HasKey("loggedInUser")) {
            usernameUI.GetComponent<Text>().text = PlayerPrefs.GetString("loggedInUser");
        }

        if (PlayerPrefs.HasKey("difficulty")) {
            difficulty = PlayerPrefs.GetString("difficulty");
        }

        if (PlayerPrefs.HasKey("levelTitle")) {
            modeUI.GetComponent<Text>().text = PlayerPrefs.GetString("levelTitle");
        }

		if (PlayerPrefs.HasKey("levelObstacles")) {
			levelObstacles = PlayerPrefs.GetString("levelObstacles");

			if (levelObstacles.Length == 0) {
                // ERROR: no value given
                Debug.LogError("Empty Level Obstacles");
			}

            // build obstacle list
			foreach (char c in levelObstacles) {
                switch (c) {
                    case 'C':
                        obstacles.Add(chicken);
                        break;
                    default:
                        Debug.LogError("Unknown key: " + c);
                        break;
                }
			}

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
        // wait until tutorial for new obstacle finishes
        if (showTutorial) {
            Debug.Log("Tutorial...");

            yield return new WaitForSeconds(2f);
        }

        // play background audio for incoming obstacles
        Debug.Log("Playing noise of all obstacles...");

		// wait for background to finish (add time for delay until car starts)
		yield return new WaitForSeconds(1f);

        // start the car (audio)
        // wait for car audio length

        Debug.Log("Playing car audio...");

		yield return new WaitForSeconds(1f);

		// start the car driving loop

		// start first obstacle
		StartCoroutine(SetObstacle());
	}

	// Update is called once per frame
	void Update () {
        if (input.touch != gesture.NONE) {
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
                    break;
            }

            // wait for command and input, otherwise do nothing
            if (checkForCommand) {
                checkForCommand = false;

                if (commandList.Contains(input.touch)) {
                    // successful input
                    score += (20 / commandList.Count);
                    scoreUI.GetComponent<Text>().text = "Score: " + score;

                    // play good audio
                    Debug.Log("Correct Input");
                }
                else {
                    // mistake made, hit obstacle
                    // play bad audio
                    mistakes++;
                    Debug.Log("Incorrect Input");
                }

                Destroy(displayedObstacle);
            }
        }
    }

	IEnumerator SetObstacle() {
		checkForCommand = false;

		// play until passed all obstacles
		while (position < obstacles.Count) {
			GameObject currentObstacle = obstacles[position];
            string obstacleName = currentObstacle.name;

			// remove old commands
			commandList.Clear();

			// add required commands
			switch (obstacleName) {
				case "Chicken":
					commandList.Add(gesture.LEFT);
					commandList.Add(gesture.RIGHT);
					break;
				default:
                    // ERROR: unknown obstacle
                    Debug.LogError("Unknown obstacle: " + obstacleName);
					break;
			}

            Debug.Log("Cruising...");
			// wait (continue driving) based on difficulty
			yield return new WaitForSeconds(1f + cruiseTime);

            // display current obstacle in the distance
            // play background audio for obstacle
            displayedObstacle = Instantiate(currentObstacle, farPoint.transform.position, transform.rotation) as GameObject;

            // wait random time
            Debug.Log(obstacleName + " in the distance...");
			yield return new WaitForSeconds(Random.Range(0.5f, 3f));

            Debug.Log("Dodge the " + obstacleName + "!");
			// display obstacle
			// play "Dodge the 'obstacle_name'" based on obstacle
			checkForCommand = true;
            displayedObstacle.transform.position = nearPoint.transform.position;

			// reaction time based on Difficulty
			yield return new WaitForSeconds(1f + reactionTime);

            // after alloted time, check to see if no input was given
            if (checkForCommand) {
                Debug.Log("No input given... obstacle hit");
                checkForCommand = false;
                mistakes++;

                Destroy(displayedObstacle);

                // play bad audio
                // hit the obstacle
            }

            Debug.Log("Fetching next obstacle...");
            position++;
		}

        Debug.Log("End of Level");
        Debug.Log("Score: " + score);

        // reached end of level
        checkForCommand = false;

        // put score into PlayerPrefs for Gameover to take
        PlayerPrefs.SetInt("score", score);

        // pass accuracy along
        if (obstacles.Count > 0) {
            PlayerPrefs.SetInt("accuracy", (obstacles.Count - mistakes) / obstacles.Count);
        }

		// stop the car
		// play car turn off sound
		// wait for sound to finish
		yield return new WaitForSeconds(1f);

		SceneManager.LoadScene("crGameover");
	}
}