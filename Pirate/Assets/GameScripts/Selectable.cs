using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Selectable : NetworkBehaviour {

    [SyncVar]
    public int ownerID;

    protected Player localPlayer;

    public bool selected;

	// Use this for initialization
	public override void OnStartClient () {
        localPlayer = GameManager.instance.localPlayer;
	}

    public void Select()
    {
        selected = true;
    }

    public void Deselect()
    {
        selected = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
