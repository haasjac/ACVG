using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class touchButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void touchCreateAccountButton() {
    SceneManager.LoadScene("mainCreateAccount");
  }
}
