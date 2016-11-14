using UnityEngine;
using System.Collections;

public class setButtonLeaderboardMode : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setGlobal() {
        PlayerPrefs.SetString("leaderboard_mode", "global");
    }

    public void setFriend() {
        PlayerPrefs.SetString("leaderboard_mode", "friend");
    }
}
