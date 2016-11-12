using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class createCustomLevel : MonoBehaviour {

	public int customLevelCount;
	public Button[] customButtons;


	// Use this for initialization
	void Start () {
		for (int i = 0; i < 16; ++i) {
			if (!PlayerPrefs.HasKey("custom_level_" + i.ToString())) {
				customLevelCount = i;
				break;
			} else if (i == 15) {
				customLevelCount = 16;
			}
		}

		for (int i = (customLevelCount); i < 16; ++i) {
			customButtons[i].interactable = false;
			print(i);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
