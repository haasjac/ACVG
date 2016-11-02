using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class test : MonoBehaviour {

    gesture g;

    public Text text;

    public myInput i;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (i.touch != gesture.NONE)
        text.text = i.touch.ToString();
	}
}
