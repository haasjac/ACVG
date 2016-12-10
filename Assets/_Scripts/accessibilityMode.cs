using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Global;

public struct NarratableObject {
    public Component component;
    public float waitTime;
}

public class AccessibilityMode : myInput {

    private const float VOLUME = 1f;
    private const float RATE = 0.6f;
    private const float PITCH = 1f;

    public Image border;

    private Component currentComponent = null;
    private bool swipeLeft = false;
    private bool swipeRight = false;
    private bool loop = false;

    public void runAccessibilityMode(NarratableObject[] sceneObjects) {
        StartCoroutine(run(sceneObjects));
    }

    private IEnumerator run(NarratableObject[] sceneObjects) {
        while (!IsVoiceOverOn.isVoiceOverOn()) {
            yield return null;
        }

        disableSceneComponents(sceneObjects);
        while (true) {
            if (!IsVoiceOverOn.isVoiceOverOn()) {
                enableSceneComponents(sceneObjects);
            }

            while (!IsVoiceOverOn.isVoiceOverOn()) {
                yield return null;
            }

            int currentIndex = 0;
            while (currentIndex < sceneObjects.Length) {
                if (!IsVoiceOverOn.isVoiceOverOn()) {
                    break; 
                } else {
                    disableSceneComponents(sceneObjects);
                }

                NarratableObject narratableObject = sceneObjects[currentIndex];

                currentComponent = narratableObject.component;
                float waitTime = narratableObject.waitTime;

                Text description = currentComponent.GetComponentInChildren<Text>();

                enableComponent(narratableObject.component);

                if (description != null) {
                    EasyTTSUtil.SpeechAdd(description.text, VOLUME, RATE, PITCH);
                } else {
                    Debug.Log("Accessibility Mode given an object with no text."); 
                }

                float t = 0.0f;
                while (t <= waitTime && (!swipeLeft && !swipeRight) && IsVoiceOverOn.isVoiceOverOn()) {
                    t += Time.deltaTime;    
                    yield return null;
                }

                if (loop || (swipeLeft || swipeRight) || !IsVoiceOverOn.isVoiceOverOn()) {
                    disableComponent(narratableObject.component);
                }

                currentIndex = updateIndex(currentIndex, sceneObjects.Length);
            }
        }
    }

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
            EasyTTSUtil.StopSpeech();
            pressButton();
        }

        if (touch == gesture.UP) {
            loop = false; 
        }

        if (touch == gesture.DOWN) {
            loop = true; 
        }
    }

    private int updateIndex(int currentIndex, int max) {
        if (swipeLeft) {
            --currentIndex;
            if (currentIndex < 0) {
                currentIndex = max - 1; 
            }
        } else if (swipeRight || loop) {
            ++currentIndex;
        }

        swipeLeft = false;
        swipeRight = false;

        return currentIndex;
    }

    private void enableSceneComponents(NarratableObject[] sceneComponents) {
        foreach (NarratableObject narratableObject in sceneComponents) {
            enableComponent(narratableObject.component);
        }
    }

    private void disableSceneComponents(NarratableObject[] sceneComponents) {
        foreach (NarratableObject narratableObject in sceneComponents) {
            disableComponent(narratableObject.component);
        }
    }

    private void enableComponent(Component component) {
        isComponetEnabled(component, true); 
    }

    private void disableComponent(Component component) {
        isComponetEnabled(component, false);
    }

    private void isComponetEnabled(Component component, bool interactable) {
        Selectable selectable = component.GetComponentInChildren<Selectable>();
        if (selectable != null) {
            selectable.interactable = interactable;
            Text text = component.GetComponentInChildren<Text>();
            if (text.text == "Go Back") {
                if (IsVoiceOverOn.isVoiceOverOn() && interactable) {
                    border.enabled = true; 
                } else {
                    border.enabled = false; 
                }
                border.transform.SetParent(text.transform, false);
                border.rectTransform.sizeDelta = new Vector2(20, 20);
            }
        } else {
            border.enabled = interactable;
            if (interactable) {
                border.transform.SetParent(component.transform, false);
                border.rectTransform.sizeDelta = new Vector2(20, 20);
            }
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
