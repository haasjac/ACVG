using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class accessibilityMode : MonoBehaviour {

    public static bool accessibility = true;
    public Button settingsButton;
    public Text settingsButtonText;

    public void accessibilityOnOff() {
        if (accessibility) {
            accessibility = false; 

            settingsButton.image.color = Color.red;
        } else {
            accessibility = true; 

            settingsButton.image.color = Color.green;
        }
    }
}
