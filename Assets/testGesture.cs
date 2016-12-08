using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class testGesture : MonoBehaviour {

    Text text;
    public Text s;
    myInput touch;
    public Slider slider;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        touch = GetComponent<myInput>();
	}
	
	// Update is called once per frame
	void Update () {
        s.text = slider.value.ToString();
        touch.touchLength = slider.value;
	    if (touch.touch != gesture.NONE) {
            text.text = touch.touch.ToString();
        }
	}
}
