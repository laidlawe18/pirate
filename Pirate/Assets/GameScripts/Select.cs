using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour {
    
    Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponentInParent<PlayerControllable>().isActive)
        {
            anim.SetInteger("State", 1);
        } else
        {
            anim.SetInteger("State", 0);
        }
    }
}
