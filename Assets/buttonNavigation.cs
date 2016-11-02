using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class buttonNavigation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void touchPlayGameButton() {
    SceneManager.LoadScene("mainGamesList");
  }

  public void touchProfileButton() {
    // alert box here...

    SceneManager.LoadScene("mainProfile");
  }

  public void touchSettingsButon() {
    SceneManager.LoadScene("mainSettings");
  }

  public void touchAboutButton() {
    SceneManager.LoadScene("mainAbout");
  }
}
