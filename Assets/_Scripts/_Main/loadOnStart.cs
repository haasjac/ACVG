using UnityEngine;
using Facebook.Unity;
using System.Collections;

public class loadOnStart : MonoBehaviour {

    static bool hasLoaded = false;

	// Use this for initialization
	void Start () {
        if (!hasLoaded) {
            Global.functions.load();
            hasLoaded = true;
        }
        if (!FB.IsInitialized) {
            FB.Init(SetInit, OnHideUnity);
        }
    }

    void SetInit() {

        if (FB.IsLoggedIn) {
            Debug.Log("FB is logged in");
        } else {
            Debug.Log("FB is not logged in");

        }

    }

    void OnHideUnity(bool isGameShown) {

        if (!isGameShown) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }

    }

    // Update is called once per frame
    void Update () {
	
	}
}
