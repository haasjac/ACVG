using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class mainNavigator : MonoBehaviour {
	public AccessibilityMode am;

	public Text title;
	public Button playAGame;
	public Button myProfile;
	public Button settings;
	public Button about;

	// Use this for initialization
	void Start () {
		am = gameObject.AddComponent<AccessibilityMode> () as AccessibilityMode;
		//am = GetComponent<AccessibilityMode>();

		global.S.accessibility = true;

		Component[] sceneComponents = new Component[5];
		sceneComponents [0] = title;
		sceneComponents [1] = playAGame;
		sceneComponents [2] = myProfile;
		sceneComponents [3] = settings;
		sceneComponents [4] = about;

		am.runAccessibilityMode (sceneComponents);
	}
	
	// Update is called once per frame
	void Update () {
		am.check ();
	}
}
