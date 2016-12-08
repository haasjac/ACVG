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
    public List<Image> leaderboardPics = new List<Image>();

    string userName;


	// Use this for initialization
	void Start () {    

        for (int i = 0; i < leaderboardNames.Count; i++) {
            leaderboardNames[i].text = "";
            leaderboardScores[i].text = "";
            leaderboardPics[i].sprite = null;
        }

        /*for (int i = 0; i < swipeIt.leaderboardNames.Count; i++) {
            leaderboardNames[i].text = swipeIt.leaderboardNames[swipeIt.leaderboardScores[i].Key];
            leaderboardScores[i].text = swipeIt.leaderboardScores[i].Value.ToString();
        }*/

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

    IEnumerator updateLeaderboard() {
        yield return StartCoroutine(swipeIt.updateLeaderboard());
        print("Count: " + swipeIt.leaderboardNames.Count);
        yield return new WaitForSeconds(3);
        print("Count: " + swipeIt.leaderboardNames.Count);
        for (int i = 0; i < swipeIt.leaderboardNames.Count; i++) {
            leaderboardNames[i].text = swipeIt.leaderboardNames[swipeIt.leaderboardScores[i].Key];
            //print(i + ": " + swipeIt.leaderboardNames[swipeIt.leaderboardScores[i].Key]);
            leaderboardScores[i].text = swipeIt.leaderboardScores[i].Value.ToString();
            if (swipeIt.leaderboardPics[swipeIt.leaderboardScores[i].Key] = null) {
                print("wat");
            }
            leaderboardPics[i].sprite = Sprite.Create(swipeIt.leaderboardPics[swipeIt.leaderboardScores[i].Key], new Rect(0, 0, 128, 128), new Vector2()); ;
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
