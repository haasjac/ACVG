using UnityEngine;
using UnityEngine.SceneManagement;
using Global;
using Facebook.Unity;

public class buttonNavigation : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void gotoLevel(string level) {
        SceneManager.LoadScene(level);
    }

    public void touchPlayGameButton() {
        SceneManager.LoadScene("mainGamesList");
    }

    public void touchProfileButton() {
		SceneManager.LoadScene("mainProfile");
    }

    public void touchAboutButton() {
        SceneManager.LoadScene("mainAbout");
    }

	public void touchGlobalLeaderboardButton() {
		if (FB.IsLoggedIn) {
			SceneManager.LoadScene("si_leaderboard");
		}
		else {
			EasyTTSUtil.StopSpeech();
			EasyTTSUtil.SpeechAdd("You must be logged in to view the leaderboard.", 1f, 0.6f, 1f);
		}
	}
}
