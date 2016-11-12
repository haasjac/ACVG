using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class crMenu : MonoBehaviour {
    public GameObject chicken;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void touchPlayGameButton() {
        SceneManager.LoadScene("crLevelSelect");
    }

    public void touchHowToPlayButton() {
        SceneManager.LoadScene("crHowToPlay");
    }

    public void touchExitButton() {
        SceneManager.LoadScene("mainMenu");
    }
   
}
