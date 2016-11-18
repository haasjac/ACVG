using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Global;
using Facebook.Unity;

public class leaderboards : MonoBehaviour {

    public Image leaderboardImage;
    public Text leaderBoardText;

    public Sprite globalSprite;
    public Sprite friendSprite;

    public List<Text> leaderboardNames = new List<Text>();
    public List<Text> leaderboardScores = new List<Text>();

    string userName;


	// Use this for initialization
	void Start () {    

        for (int i = 0; i < leaderboardNames.Count; i++) {
            leaderboardNames[i].text = "";
            leaderboardScores[i].text = "";
        }

        if (PlayerPrefs.HasKey("leaderboard_mode"))
        {
            string mode = PlayerPrefs.GetString("leaderboard_mode");
            if (mode == "global")
            {
                leaderboardImage.sprite = globalSprite;
                leaderBoardText.text = "Global Leader boards";
            } else
            {
                leaderboardImage.sprite = friendSprite;
                leaderBoardText.text = "Friend Leader boards";
            }
        }

        StartCoroutine(updateLeaderboard());

        
	}

    void getName(string id) {
            FB.API("/" + id + "?fields=first_name", HttpMethod.GET, callBack);
    }

    IEnumerator updateLeaderboard() {
        yield return StartCoroutine(swipeIt.updateLeaderboard());
        for (int i = 0; i < swipeIt.leaderboardNames.Count; i++) {
            //getName(swipeIt.leaderboardNames[i]);
            leaderboardNames[i].text = swipeIt.leaderboardNames[i];
            leaderboardScores[i].text = swipeIt.leaderboardScores[i];
        }
    }

    void callBack(IResult result) {
        if (result.Error == null) {
            userName = result.ResultDictionary["first_name"].ToString();
        } else {
            Debug.Log(result.Error);
            userName = "India";
        }
        
    }

    void displayText() {
        for (int i = 0; i < leaderboardNames.Count; i++) {
            leaderboardNames[i].text = swipeIt.leaderboardNames[i];
            leaderboardScores[i].text = swipeIt.leaderboardScores[i];
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
