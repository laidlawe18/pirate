using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    List<PlayerControllable> controllables;
    public int currActive;

	// Use this for initialization
	void Start () {
        controllables = new List<PlayerControllable>(GetComponentsInChildren<PlayerControllable>());
        currActive = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (controllables.Count == 0)
        {
            currActive = -1;
        }
        else
        {
            if (Input.GetButtonDown("Switch") && Input.GetAxis("Switch") > 0)
            {
                controllables[currActive].Deactivate();
                currActive += 1;
                currActive %= controllables.Count;
                controllables[currActive].Activate();
            }
            if (Input.GetButtonDown("Switch") && Input.GetAxis("Switch") < 0)
            {
                controllables[currActive].Deactivate();
                currActive -= 1;
                if (currActive < 0)
                {
                    currActive = controllables.Count - 1;
                }
                controllables[currActive].Activate();
            }
        }
    }

	public void OnChildClick (PlayerControllable pc) {
        

        for (int i = 0; i < controllables.Count; i++)
        {
            if (controllables[i].Equals(pc))
            {
                controllables[currActive].Deactivate(); 
                currActive = i;
                controllables[currActive].Activate();
            }
        }
	}

    public void AddControllable(PlayerControllable pc)
    {
        controllables.Add(pc);
    }
}
