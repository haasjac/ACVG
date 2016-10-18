using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour {

    public List<Button> buttons;
    public AudioSource audioSource;
    public List<AudioClip> audioClips;

    private string nextScene;
    private List<string> scenes;

    void Start () {
        if (global.S.accessibility) {
            scenes = new List<string>();
            scenes.Add("next");
            scenes.Add("settings");
            scenes.Add("next");

            for (int i = 0; i < buttons.Count; ++i) {
                buttons[i].interactable = false; 
            }

            StartCoroutine(fakeVoiceOver());
        }
    }

    private IEnumerator fakeVoiceOver() {
        int i = 0;

        while (true) {
            nextScene = scenes[i]; 

            buttons[i].interactable = true;

            audioSource.clip = audioClips[i];
            audioSource.Play();

            yield return new WaitForSeconds(2);

            buttons[i].interactable = false; 

            ++i;
            if (i == audioClips.Count) {
                i = 0; 
            }
        } 
    }

    void Update () {
        if (global.S.accessibility) {
            //if (Input.touchCount == 1) {
                //if (Input.GetTouch(0).phase == TouchPhase.Ended) {
                if (Input.GetMouseButtonDown(0)) {
                    SceneManager.LoadScene(nextScene); 
                } 
            //}
        }
    }
}
