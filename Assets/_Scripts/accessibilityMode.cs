using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct NarratableObject {
    public Component component;
    public float waitTime;
}

public class AccessibilityMode : myInput {

    private const float VOLUME = 1f;
    private const float RATE = 0.6f;
    private const float PITCH = 1f;

    private Component currentComponent = null;
    private string currentType = "";
    private bool swipeLeft = false;
    private bool swipeRight = false;

    // Call this function in Start()
    // Order elements of sceneComponents array in order you want
    // them to be read aloud by Accessibility Mode
    public void runAccessibilityMode(NarratableObject[] sceneObjects) {
        StartCoroutine(run(sceneObjects));
    }

    private IEnumerator run(NarratableObject[] sceneObjects) {
        disableSceneComponents(sceneObjects);
        while (global.S.accessibility) {
            int currentIndex = 0;
            while (currentIndex < sceneObjects.Length) {
                NarratableObject narratableObject = sceneObjects[currentIndex];

                currentComponent = narratableObject.component;
                float waitTime = narratableObject.waitTime;

                currentType = currentComponent.GetType().ToString();

                Text description = currentComponent.GetComponentInChildren<Text>();

                enableComponent(narratableObject.component);

                if (description != null) {
                    EasyTTSUtil.SpeechAdd(description.text, VOLUME, RATE, PITCH);
                } else {
                    Debug.Log("Accessibility Mode given an object with no text."); 
                }

                float t = 0.0f;
                while (t <= waitTime && (!swipeLeft && !swipeRight)) {
                    t += Time.deltaTime;    
                    yield return null;
                }

                disableComponent(narratableObject.component);

                currentIndex = updateIndex(currentIndex, sceneObjects.Length);
            }
        }
    }

    // Call this function in update()
    public void check() {
        if (touch == gesture.RIGHT) {
            EasyTTSUtil.StopSpeech();
            swipeRight = true; 
        }

        if (touch == gesture.LEFT) {
            EasyTTSUtil.StopSpeech();
            swipeLeft = true;
        }
        
        if (touch == gesture.DOUBLE) {
        	pressButton();
        }
    }

    private int updateIndex(int currentIndex, int max) {
        if (swipeLeft) {
            --currentIndex;
            if (currentIndex < 0) {
                currentIndex = max - 1; 
            }
        } else {
            ++currentIndex;
        }

        swipeLeft = false;
        swipeRight = false;

        return currentIndex;
    }

    private void disableSceneComponents(NarratableObject[] sceneComponents) {
        foreach (NarratableObject narratableObject in sceneComponents) {
            disableComponent(narratableObject.component);
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
}
