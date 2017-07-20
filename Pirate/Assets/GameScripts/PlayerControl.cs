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
                controllables[currActive].Deactivate();
                currActive += 1;
                currActive %= controllables.Length;
                controllables[currActive].Activate();
            }
            if (Input.GetButtonDown("Switch") && Input.GetAxis("Switch") < 0)
            {
                controllables[currActive].Deactivate();
                currActive -= 1;
                if (currActive < 0)
                {
                    currActive = controllables.Length - 1;
                }
                controllables[currActive].Activate();
            }
        }
    }

	public void OnChildClick (PlayerControllable pc) {
        

        for (int i = 0; i < controllables.Length; i++)
        {
            if (controllables[i].Equals(pc))
            {
                controllables[currActive].Deactivate(); 
                currActive = i;
                controllables[currActive].Activate();
            }
        }
	}
}
