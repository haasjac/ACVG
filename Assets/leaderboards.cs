using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class leaderboards : MonoBehaviour {

    public Image leaderboardImage;
    public Text leaderBoardText;

    public Sprite globalSprite;
    public Sprite friendSprite;


	// Use this for initialization
	void Start () {
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
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
