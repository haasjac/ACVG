using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class settings : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (SettingVars.accessibility) {
            StartCoroutine(fakeVoiceOver());
        }
	}

    private IEnumerator fakeVoiceOver() {
        while (true) {
            yield return new WaitForSeconds(2); 
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
