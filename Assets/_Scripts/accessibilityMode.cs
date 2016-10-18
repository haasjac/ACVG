using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class accessibilityMode : MonoBehaviour {

    public static bool accessibilityCheck = false;
    public Button settingsButton;
    public Text settingsButtonText;

    public void accessibilityOnOff() {
        if (global.S.accessibility) {
            global.S.accessibility = false;

            settingsButton.interactable = true;
        } else {
            if (!accessibilityCheck) {
                global.S.accessibility = true;

                settingsButton.interactable = false;
                settingsButtonText.text = "Accessibility Mode On";
            }
        }
    }
}
