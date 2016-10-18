using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testGestures : MonoBehaviour {

    public myInput g;
    public Text t;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (g.touch != gesture.NONE) {
            t.text = g.touch.ToString();
        }
	}
}
