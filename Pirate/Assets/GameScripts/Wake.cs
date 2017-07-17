using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wake : MonoBehaviour {

    Rigidbody2D boat;
    Animator anim;

	// Use this for initialization
	void Start () {
        boat = GetComponentInParent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (boat.velocity.magnitude < .1)
        {
            anim.SetInteger("State", 0);
        } else if (boat.velocity.magnitude < .3)
        {
            anim.SetInteger("State", 1);
        } else if (boat.velocity.magnitude < .5)
        {
            anim.SetInteger("State", 2);
        } else
        {
            anim.SetInteger("State", 3);
        }
    }
}
