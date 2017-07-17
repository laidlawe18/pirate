using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oars : MonoBehaviour
{

    Rigidbody2D boat;
    Animator anim;

    // Use this for initialization
    void Start()
    {
        boat = GetComponentInParent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (boat.velocity.magnitude < .01)
        {
            anim.SetInteger("State", 1);
        }
        else
        {
            anim.SetInteger("State", 2);
        }
    }
}
