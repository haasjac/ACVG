using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Global;
using Facebook.Unity;

public class globalLeaderboard : MonoBehaviour {
	public GameObject button;

	// Use this for initialization
	void Start () {
		if (!FB.IsLoggedIn) {
			ColorBlock temp = button.GetComponent<Button>().colors;
			temp.colorMultiplier = 5f;
			button.GetComponent<Image>().color = Color.red;
			button.GetComponent<Button>().colors = temp;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
