using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class crGame : MonoBehaviour {
    public GameObject chicken;
    public List<GameObject> obstacles;
    public string levelString;
	public string difficulty;
	public string levelTitle;
	public myInput input;

	int score = 0;
	bool checkForCommand;
	List<gesture> commandList;
	int position = 0;
	float reactionTime, cruiseTime;
	bool showTutorial;

    // Use this for initialization
    void Start () {
		if (PlayerPrefs.HasKey("crLevelString")) {
			string temp = PlayerPrefs.GetString("crLevelString");

			if (temp.Length > 0) {
				levelString = PlayerPrefs.GetString("crLevelString");
			}
			else {
				levelString = "C";
			}

			foreach (char c in levelString) {
				switch (c) {
					case 'C': obstacles.Add(chicken); break;
					default: break;
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
					break;
			}

			StartCoroutine(StartGame());
		}
		else {
			// should abort...
			SceneManager.LoadScene("crMenu");
		}

    }

	IEnumerator StartGame() {
		// play tutorial based on level/difficulty of introducing obstacle
		// wait until tutorial for new obstacle finishes

		yield return new WaitForSeconds(2f);

		// play background audio for incoming obstacles
		// show them in the distance

		// wait for background to finish (add time for delay until car starts)
		yield return new WaitForSeconds(1f);

		// start the car (audio)
		// wait for car audio length
		yield return new WaitForSeconds(1f);

		// start the car driving loop

		// start first obstacle
		StartCoroutine(SetObstacle());
	}

	// Update is called once per frame
	void Update () {
		// wait for command and input, otherwise do nothing
		if (checkForCommand && input.touch != gesture.NONE) {
			checkForCommand = false;

			if (commandList.Contains(input.touch)) {
				// successful input
				// add points
				score += (20 / commandList.Count);

				// play good audio
			}
			else {
				// mistake made, hit obstacle
				// don't add points
				// play bad audio
			}
		}
    }

	IEnumerator SetObstacle() {
		checkForCommand = false;

		// play until passed all obstacles
		while (position < obstacles.Count) {
			GameObject currentObstacle = obstacles[position];

			// remove old commands
			commandList.Clear();

			// add required commands
			switch (currentObstacle.name) {
				case "Chicken":
					commandList.Add(gesture.LEFT);
					commandList.Add(gesture.RIGHT);
					break;
				default:
					// ERROR: unknown obstacle
					break;
			}

			// wait (continue driving) based on difficulty
			yield return new WaitForSeconds(1f + cruiseTime);

			// display current obstacle in the distance (moves towards user)
			// play background audio for obstacle

			// wait random time until object is in front of you
			yield return new WaitForSeconds(Random.Range(0.5f, 3f));

			// display obstacle
			// play "Dodge the 'obstacle_name'" based on obstacle
			checkForCommand = true;

			// reaction time based on Difficulty
			yield return new WaitForSeconds(1f + reactionTime);

			checkForCommand = false;
		}
		checkForCommand = false;

		// reached end of level

		// put score into PlayerPrefs for Gameover to take
		PlayerPrefs.SetInt("score", score);

		// stop the car
		// play car turn off sound
		// wait for sound to finish
		yield return new WaitForSeconds(1f);

		SceneManager.LoadScene("crGameover");
	}
}
