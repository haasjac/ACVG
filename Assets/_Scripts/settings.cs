using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class settings : MonoBehaviour {

    enum state {
        buttonOn,
        buttonOff,
        buttonNotOff,
        buttonConfirmOff,
        backToGameMenu 
    };

    public Button accessibilityModeButton;
    public Button backButton;
    public Text accModeButtonText;

    public AudioSource audioSource;
    public AudioClip accModeOn;
    public AudioClip accModeOff;
    public AudioClip accModeNotOff;
    public AudioClip accModeConfirmOff;
    public AudioClip backToGameMenu;

    private state next;

	// Use this for initialization
	void Start () {
        ColorBlock cb = accessibilityModeButton.colors;
        cb.normalColor = Color.red;
        cb.highlightedColor = Color.red;
        cb.pressedColor = Color.red;
        cb.disabledColor = Color.green;
        accessibilityModeButton.colors = cb;

        if (global.S.accessibility) {
            next = state.buttonOn;

            accModeButtonText.text = "Accessibility Mode On";
            accessibilityModeButton.interactable = false;
            backButton.interactable = false;
        } else {
            next = state.buttonOff;
            accModeButtonText.text = "Accessibility Mode Off";
        }

        StartCoroutine(fakeVoiceOver());
	}

    private IEnumerator fakeVoiceOver() {
        while (true) {
            if (!global.S.accessibility) {
                next = state.buttonOff; 
            }
            switch (next) {
                case state.buttonOff:
                    if (accessibilityMode.accessibilityCheck) {
                        accessibilityMode.accessibilityCheck = false;
                    }

                    if (!backButton.interactable) {
                        backButton.interactable = true;
                    }

                    if (global.S.accessibility) {
                        next = state.buttonOn;
                        backButton.interactable = false;
                    }

                    yield return new WaitForSeconds(2); 

                    break;
                case state.buttonOn:
                    audioSource.clip = accModeOn;
                    audioSource.Play();

                    yield return new WaitForSeconds(5); 

                    if (next != state.buttonConfirmOff) {
                        next = state.backToGameMenu; 
                    }

                    break;
                case state.buttonNotOff: 
                    audioSource.clip = accModeNotOff;
                    audioSource.Play();

                    yield return new WaitForSeconds(4); 

                    next = state.backToGameMenu;
                    break;
                case state.buttonConfirmOff:
                    audioSource.clip = accModeConfirmOff;
                    audioSource.Play();

                    yield return new WaitForSeconds(4); 

                    if (next != state.buttonOff) {
                        next = state.buttonNotOff;
                    }

                    break;
                case state.backToGameMenu:
                    backButton.interactable = true;
                    audioSource.clip = backToGameMenu;
                    audioSource.Play();

                    yield return new WaitForSeconds(2); 

                    next = state.buttonOn;

                    backButton.interactable = false;
                    break;
                default:
                    break;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.touchCount == 1) {
            //if (Input.GetTouch(0).phase == TouchPhase.Ended) { 
            if (Input.GetMouseButtonDown(0)) {
                if (global.S.accessibility) {
                    switch (next) {
                        case state.buttonOff:
                            break;
                        case state.buttonOn:
                            next = state.buttonConfirmOff;
                            break; 
                        case state.buttonNotOff:
                            break;
                        case state.buttonConfirmOff:
                            accessibilityMode.accessibilityCheck = true;
                            global.S.accessibility = false;
                            accessibilityModeButton.interactable = true;
                            accModeButtonText.text = "Accessibility Mode Off";

                            next = state.buttonOff;
                            break;
                        case state.backToGameMenu:
                            SceneManager.LoadScene("test");
                            break;
                        default:
                            break;
                    }
                }
            }	
        //}
	}
}
