﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class back : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void backToMain() {
    SceneManager.LoadScene("mainMenu");
  }
}
