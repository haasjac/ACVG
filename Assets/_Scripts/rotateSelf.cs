using UnityEngine;
using System.Collections;

public class rotateSelf : MonoBehaviour {

    public float rotationSpeed;
    public float jumpStrength;
    Vector3 startingPostion;
    Rigidbody rigid;

	// Use this for initialization
	void Start () {
        startingPostion = transform.position;
        rigid = this.GetComponent<Rigidbody>();

        if (jumpStrength <= 0) {
            rigid.useGravity = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        // rotate the gameObject by it's y axis
        transform.Rotate(new Vector3(0, Time.deltaTime * rotationSpeed, 0));

        // bounce the gameObject
        if (transform.position.y <= startingPostion.y) {
            rigid.velocity = new Vector3(0, Random.Range(jumpStrength, jumpStrength * 3), 0);
        }
    }
}
