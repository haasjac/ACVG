using UnityEngine;
using System.Collections;

public class loadOnStart : MonoBehaviour {

    static bool hasLoaded = false;

	// Use this for initialization
	void Start () {
        if (!hasLoaded) {
            Global.functions.load();
            hasLoaded = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
