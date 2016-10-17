using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class toSettings : MonoBehaviour {

    public void moveToSettings() {
        SceneManager.LoadScene("settings");
    }
}
