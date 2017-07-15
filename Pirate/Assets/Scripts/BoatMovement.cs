using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour {

	Rigidbody2D rb2d;
	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		print (Input.GetAxis ("Horizontal"));
		rb2d.AddForce (transform.right * -1 * Input.GetAxis ("Vertical"));
		Vector3 rotation = transform.rotation.eulerAngles;
		rotation.z -= 0.5f * Input.GetAxis ("Horizontal");
		transform.rotation = Quaternion.Euler(rotation);
	}
}
