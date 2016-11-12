using UnityEngine;
using System.Collections;

public class playerVehicle : MonoBehaviour {
    bool isMoving;

    bool speedingUp;
    bool slowingDown;
    bool swervingLeft;
    bool swervingRight;

    Vector3 startingPosition;
    float duration = 1f;
    float actionTime;

	// Use this for initialization
	void Start () {
        startingPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (speedingUp || slowingDown || swervingLeft || swervingRight) {
            if (Time.time - actionTime > duration) {
                speedingUp = false;
                slowingDown = false;
                swervingLeft = false;
                swervingRight = false;
                transform.position = startingPosition;
            }
        }
	}

    public void StartEngine() {
        Debug.Log("Starting Engine");
        isMoving = true;
    }

    public void StopEngine() {
        Debug.Log("Stopping Engine");
        isMoving = false;
    }

    public void SpeedUp() {
        Debug.Log("Speeding Up");
        speedingUp = true;
        transform.position = startingPosition + new Vector3(0, 0, 5);
        actionTime = Time.time;
    }

    public void SlowDown() {
        Debug.Log("Slowing Down");
        slowingDown = true;
        transform.position = startingPosition + new Vector3(0, 0, -5);
        actionTime = Time.time;
    }

    public void Swerve(string direction) {
        Debug.Log("Swerving " + direction);

        if (direction == "left") {
            swervingLeft = true;
            transform.position = startingPosition + new Vector3(-5, 0, 0);
            actionTime = Time.time;
        }
        else {
            swervingRight = true;
            transform.position = startingPosition + new Vector3(5, 0, 0);
            actionTime = Time.time;
        }
    }

    public void Honk() {
        Debug.Log("Honk");
    }
}
