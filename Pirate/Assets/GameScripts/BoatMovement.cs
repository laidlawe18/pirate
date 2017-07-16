using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : PlayerControllable {

	Rigidbody2D rb2d;
	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
        isActive = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isActive)
        {
            rb2d.AddForce(transform.right * -1 * Input.GetAxis("Vertical"));
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.z -= 0.5f * Input.GetAxis("Horizontal");
            transform.rotation = Quaternion.Euler(rotation);
        }
        
	}
}
