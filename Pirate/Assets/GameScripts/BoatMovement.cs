using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : PlayerControllable {

	Rigidbody2D rb2d;
    Cannon[] cannons;
	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
        cannons = GetComponentsInChildren<Cannon>();
        isActive = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isActive)
        {
            rb2d.AddForce(100 * transform.up * Input.GetAxis("Vertical"));
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.z -= 0.5f * Input.GetAxis("Horizontal");
            transform.rotation = Quaternion.Euler(rotation);
            if (Input.GetButton("Fire"))
            {
                foreach (Cannon cannon in cannons)
                {
                    if (Input.GetAxis("Fire") > 0 == cannon.right)
                    {
                        cannon.Invoke("Fire", cannon.delay);
                    }
                }
            }
        }

        
	}

    public override void Activate()
    {
        isActive = true;
        GetComponentInChildren<Select>().Activate();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Island>() != null)
        {
            other.gameObject.GetComponent<Island>().InRange();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Island>() != null)
        {
            other.gameObject.GetComponent<Island>().OutOfRange();
        }
    }
}
