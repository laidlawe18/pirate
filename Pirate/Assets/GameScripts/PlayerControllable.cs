using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllable : MonoBehaviour {

	public bool isActive;
    PlayerControl pc;

	// Use this for initialization
	void Start () {
        pc = GetComponentInParent<PlayerControl>();
	}

    public void SetPlayerControl(PlayerControl playerControl)
    {
        pc = playerControl;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown(){
        if (pc == null)
        {
            pc = GetComponentInParent<PlayerControl>();
        }
        pc.OnChildClick(this);
	}

    public virtual void Activate()
    {
        isActive = true;
    }

    public virtual void Deactivate()
    {
        isActive = false;
    }
}
