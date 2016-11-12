using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class obstacle : MonoBehaviour {
	public bool inFront;
	public List<gesture> commands;
	public AudioClip sound;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public List<gesture> getCommands() {
		return commands;
	}

	public AudioClip getSound() {
		return sound;
	}
}
