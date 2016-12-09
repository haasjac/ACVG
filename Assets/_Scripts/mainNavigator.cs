using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Global;

public class mainNavigator : MonoBehaviour {
	public AccessibilityMode am;

	public List<Component> sceneComponents;
	public List<float> waitTimes;
    public Image border;

	// Use this for initialization
	void Start () {
		//if (IsVoiceOverOn.isVoiceOverOn()) {
			am = gameObject.AddComponent<AccessibilityMode> () as AccessibilityMode;
	        //am = GetComponent<AccessibilityMode>();
	        am.border = border;

			accessibility.setAccessibility(true);


			NarratableObject[] sceneObjects = new NarratableObject[sceneComponents.Count];
			for(int i = 0; i < sceneComponents.Count; i++) {
				sceneObjects[i].component = sceneComponents[i];
				sceneObjects[i].waitTime = waitTimes[i];
			}

			am.runAccessibilityMode(sceneObjects);
		//}
	}
	
	// Update is called once per frame
	void Update () {
		if (IsVoiceOverOn.isVoiceOverOn()) {
			am.check ();
		}
	}
}
