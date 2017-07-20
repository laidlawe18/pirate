using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Select : MonoBehaviour {
    
    Animator anim;
    float selectedTime;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        selectedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponentInParent<PlayerControllable>().isActive)
        {
            if (anim.GetInteger("State") == 1 && Time.time - selectedTime > 2)
            {
                anim.SetInteger("State", 0);
            }
        } else
        {
            anim.SetInteger("State", 0);
        }
    }

    public void Activate()
    {
        selectedTime = Time.time;
        anim.SetInteger("State", 1);
    }
}
