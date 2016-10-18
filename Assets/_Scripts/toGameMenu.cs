using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class toGameMenu : MonoBehaviour {

    public void returnToGameMenu() {
        SceneManager.LoadScene("test");
    }
}
