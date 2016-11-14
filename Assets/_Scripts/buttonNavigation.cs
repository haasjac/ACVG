using UnityEngine;
using UnityEngine.SceneManagement;

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
        // profile button
    }

    public void touchAboutButton() {
        SceneManager.LoadScene("mainAbout");
    }
}
