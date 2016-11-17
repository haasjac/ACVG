using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Global;

public class crHowToPlay : MonoBehaviour {

	// Use this for initialization
	void Start () {
		chickenRoad.crHowToPlay = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void touchTutorialButton(int id) {

		string levelTitle = "How To Play";
		string levelObstacles = "";
		string tutorials = "";

		switch(id) {
		case 0:
			levelObstacles = "";
			tutorials = "I";
			break;
		case 1:
			levelObstacles = "CCC";
			tutorials = "C";
			break;
		case 2:
			levelObstacles = "DDD";
			tutorials = "D";
			break;
		case 3:
			levelObstacles = "GGG";
			tutorials = "G";
			break;
		case 4:
			levelObstacles = "FFF";
			tutorials = "F";
			break;
		case 5:
			levelObstacles = "EEE";
			tutorials = "E";
			break;
		case 6:
			levelObstacles = "LRLR";
			tutorials = "L";
			break;
		case 7:
			levelObstacles = "PPP";
			tutorials = "P";
			break;
		case 8:
			levelObstacles = "VVV";
			tutorials = "V";
			break;
		}

		chickenRoad.difficulty = "easy";
		chickenRoad.levelTitle = levelTitle;
        chickenRoad.levelObstacles = levelObstacles;
        chickenRoad.tutorials = tutorials;

        chickenRoad.crHowToPlay = true;

		SceneManager.LoadScene("crGame");
	}

	public void touchReturnToMenuButton() {
		SceneManager.LoadScene("crMenu");
	}
}
