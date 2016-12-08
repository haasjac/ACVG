using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Global;

public class crHowToPlay : MonoBehaviour {
	public AudioSource noiseSource;
	public GameObject chicken, ducks, goose, fence, deer, forkLeft, forkRight, parrot, racecar;

	// Use this for initialization
	void Start () {
		chickenRoad.crHowToPlay = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void touchTutorialButton(int id) {
		noiseSource.Stop();

		List<AudioClip> noises = new List<AudioClip>();
		string commandText = "";

		switch(id) {
			case 0:
				commandText = "Welcome to Chicken Road! Your goal is to dodge the obstacles. You will hear a sound, and you must respond with the correct gesture. Beat a level by earning 60% or higher to unlock the next level. You can also try free play so the fun never ends!";
				break;
			case 1:
				noises.Add(chicken.GetComponent<obstacle>().getSound());
				commandText = "Swipe left or right to dodge the chicken";
				break;
			case 2:
				noises.Add(ducks.GetComponent<obstacle>().getSound());
				commandText = "Swipe down to avoid hitting the ducks";
				break;
			case 3:
				noises.Add(goose.GetComponent<obstacle>().getSound());
				commandText = "Swipe up to escape the goose";
				break;
			case 4:
				noises.Add(fence.GetComponent<obstacle>().getSound());
				commandText = "Swipe up to break through the fence";
				break;
			case 5:
				noises.Add(deer.GetComponent<obstacle>().getSound());
				commandText = "Swipe down to avoid hitting the deer";
				break;
			case 6:
				noises.Add(forkLeft.GetComponent<obstacle>().getSound());
				noises.Add(forkRight.GetComponent<obstacle>().getSound());
				commandText = "Swipe with the command to correctly follow the fork";
				break;
			case 7:
				noises.Add(parrot.GetComponent<obstacle>().getSound());
				commandText = "Double tap to get rid of the parrot";
				break;
			case 8:
				noises.Add(racecar.GetComponent<obstacle>().getSound());
				commandText = "Swipe up or down to avoid hitting the race car";
				break;
			default:
				break;
		}

		StartCoroutine(playTutorial(noises, commandText));
	}

	IEnumerator playTutorial(List<AudioClip> noises, string commandText) {
		int i = 0;
		while (i < noises.Count) {
			noiseSource.PlayOneShot(noises[i]);
			yield return new WaitForSeconds(noises[i].length + 0.2f);
			i++;
		}

		EasyTTSUtil.SpeechAdd(commandText, 1f, 0.6f, 1f);
		yield return new WaitForSeconds(2f);
	}

	public void touchReturnToMenuButton() {
		SceneManager.LoadScene("crMenu");
	}

/*
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
	*/
}
