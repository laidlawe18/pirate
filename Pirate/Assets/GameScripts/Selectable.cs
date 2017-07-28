using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Selectable : NetworkBehaviour {

    [SyncVar]
    public int ownerID;

    public Player localPlayer;

    public bool selected;

    public GameObject[] abilityPrefabs;

	// Use this for initialization
	public void Start () {
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

    public virtual void CreateInfo()
    {

    }

    public virtual void UpdateInfo()
    {

    }

    public void CreateAbilites()
    {
        GameManager.instance.ui.ClearAbilities();
        foreach (GameObject ability in abilityPrefabs)
        {
            GameManager.instance.ui.CreateAbility(ability);
        }
        
    }

    public virtual void UseAbility(string name)
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
