using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    PlayerControllable[] controllables;
    public int currActive;

	// Use this for initialization
	void Start () {
        controllables = GetComponentsInChildren<PlayerControllable>();
        currActive = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (controllables.Length == 0)
        {
            currActive = -1;
        }
        else
        {
            if (Input.GetButtonDown("Switch") && Input.GetAxis("Switch") > 0)
            {
                controllables[currActive].isActive = false;
                currActive += 1;
                currActive %= controllables.Length;
                controllables[currActive].isActive = true;
            }
            if (Input.GetButtonDown("Switch") && Input.GetAxis("Switch") < 0)
            {
                controllables[currActive].isActive = false;
                currActive -= 1;
                if (currActive < 0)
                {
                    currActive = controllables.Length - 1;
                }
                controllables[currActive].isActive = true;
            }
        }
    }
}
