using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class buttonNavigation : MonoBehaviour {
    public GameObject loginAlert;

	// Use this for initialization
	void Start () {
        if (loginAlert)
            loginAlert.SetActive(false);
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
        SceneManager.LoadScene("FB_test");
        /*
        // check logged in user (-1 == not logged in)
        if (PlayerPrefs.HasKey("loggedInUser")) {
            if (PlayerPrefs.GetInt("loggedInUser") != -1) {
                // logged in, show profile page
                SceneManager.LoadScene("mainProfile");
            }
            else {
                // not logged in, show alert
                loginAlert.SetActive(true);
            }
        }
        else {
            // never logged in before, create key, show alert
            PlayerPrefs.SetInt("loggedInUser", -1);
            loginAlert.SetActive(true);
        }
        */
    }

    public void touchSettingsButon() {
        SceneManager.LoadScene("mainSettings");
    }

    public void touchAboutButton() {
        SceneManager.LoadScene("mainAbout");
    }

    public void touchLoginWithFacebookButton() {
        // do facebook login here...

        loginAlert.SetActive(false);
    }

    public void touchCloseAlertButton() {
        // do not login, stay at main menu

        loginAlert.SetActive(false);
    }
}
