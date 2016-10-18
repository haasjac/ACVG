using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class scenechange : MonoBehaviour {

    public void nextLevel() {
        SceneManager.LoadScene("next");
    }
}
