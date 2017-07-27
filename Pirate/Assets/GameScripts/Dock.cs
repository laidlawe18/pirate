using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Dock : Selectable {

    public GameObject boat;

    [SyncVar]
    public Resources res;

    [SyncVar]
    public int islandID;

	// Use this for initialization
	new void Start () {
        base.Start();
        res = new Resources();
        GameManager.instance.map.GetIslandByID(islandID).DockAdded(this);
	}
	
	// Update is called once per frame
	void Update () {

	}
    

    public override void CreateInfo()
    {
        InfoPanel infoPanel = GameManager.instance.ui.infoPanel;
        infoPanel.Clear();
        infoPanel.AddTitle("Dock");
        infoPanel.AddResources(res);
    }

    public override void UpdateInfo()
    {
        InfoPanel infoPanel = GameManager.instance.ui.infoPanel;
        infoPanel.UpdateResources(res);
    }

    public override void UseAbility(string name)
    {
        if (name.Equals("Create Boat"))
        {
            CreateBoat();
        }
    }

    public void CreateBoat()
    {
        if(res.wood >= 5f)
        {
            res -= new Resources(5f, 0);
            localPlayer.CmdMakeBoat(transform.position + transform.rotation * new Vector3(0, -0.8f, 0), Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 180)));
        }
    }
}
