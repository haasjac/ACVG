using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccessibilityMode : MonoBehaviour {

    private const int WAIT_TIME = 3;

    private Component currentComponent = null;
    private string currentType = "";

    //private bool pauseAccessibilityMode = false;

    // Call this function in Start()
    // Order elements of sceneComponents array in order you want
    // them to be read aloud by Accessibility Mode
    public void runAccessibilityMode(Component[] sceneComponents) {
        StartCoroutine(run(sceneComponents));
    }

    private IEnumerator run(Component[] sceneComponents) {
        disableSceneComponents(sceneComponents);
        while (global.S.accessibility) {
            foreach (Component component in sceneComponents) {
                currentComponent = component;
                currentType = currentComponent.GetType().ToString();

                Text description = currentComponent.GetComponentInChildren<Text>();

                enableComponent(component);

                if (description != null) {
                    EasyTTSUtil.SpeechAdd(description.text);
                } else {
                    Debug.Log("Accessibility Mode given an object with no text."); 
                }

                yield return new WaitForSeconds(WAIT_TIME);

                /*
                while (pauseAccessibilityMode) {
                    yield return new WaitForSeconds(1); 
                }
                */

                disableComponent(component);
            }
        }
    }

    // Call this function in update()
    public void check() {
        switch (currentType) {
            case "UnityEngine.UI.Button":
                if (Input.GetMouseButtonDown(0)) {
                    pressButton();
                }
                break;
            /*
            case "UnityEngine.UI.InputField":
                if (!pauseAccessibilityMode) {
                    if (Input.GetMouseButtonDown(0)) {
                        focusInputField();
                        pauseAccessibilityMode = true;
                    }
                } else {
                    pauseAccessibilityMode = isInputFieldSelected();
                }
                break;
            */
            default:
                break;
        }
    }

    private void disableSceneComponents(Component[] sceneComponents) {
        foreach (Component component in sceneComponents) {
            disableComponent(component);
        }
    }

    private void enableComponent(Component component) {
        isComponentInteractable(component, true); 
    }

    private void disableComponent(Component component) {
        isComponentInteractable(component, false);
    }

    private void isComponentInteractable(Component component, bool interactable) {
        Selectable selectable = component.GetComponentInChildren<Selectable>();
        if (selectable != null) {
            selectable.interactable = interactable;
        } else {
            Debug.Log("Accessibility Mode given a non-selectable object.");
        }
    }

    private void pressButton() {
        Button button = currentComponent.GetComponentInChildren<Button>();
        if (button != null) {
            button.onClick.Invoke();
        } else {
            Debug.Log("Button is apparently not a button.");
        }
    }

    /*
    private void focusInputField() {
        InputField inputField = currentComponent.GetComponentInChildren<InputField>();
        inputField.Select();
        //inputField.ActivateInputField();
    }

    private bool isInputFieldSelected() {
        InputField inputField = currentComponent.GetComponentInChildren<InputField>();
        return inputField.isFocused;
    }
    */
}
