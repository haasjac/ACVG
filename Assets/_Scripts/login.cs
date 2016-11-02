using UnityEngine;
using System.Collections;

public class login : MonoBehaviour {
    public GameObject notLoggedInPanel;
    public GameObject profilePanel;

	// Use this for initialization
	void Start () {
        notLoggedInPanel.SetActive(false);
        profilePanel.SetActive(false);

        if (!PlayerPrefs.HasKey("loggedInUser"))
        {
            PlayerPrefs.SetInt("loggedInUser", -1);
        }

        int userId = PlayerPrefs.GetInt("loggedInUser");
        
        if (userId != -1)
        {
            // show profile page with information
            profilePanel.SetActive(true);
        }
        else
        {
            // show not logged in panel
            notLoggedInPanel.SetActive(true);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
