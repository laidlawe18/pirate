using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour {

    [SyncVar]
    public int playerID;

    public GameObject boat;
    public GameObject dock;
    public GameObject cannonball;

    Selectable selected;

	// Use this for initialization
	void Start () {
        playerID = GameManager.instance.AddPlayer(this);
        if (isLocalPlayer)
        {
            CmdMakeSpawnBoat();
        }
    }
    
    /*[ClientRpc]
    public void RpcSetPlayerID(int id)
    {
        playerID = id;
    }*/

    public override void OnStartLocalPlayer()
    {
        GameManager.instance.localPlayer = this;
        
    }

    // Update is called once per frame
    void Update () {
		if (isLocalPlayer)
        {
            if (selected != null)
            {
                selected.UpdateInfo();
            }

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 15);
                if (hit.collider != null && hit.collider.gameObject.GetComponent<Selectable>() != null)
                {
                    SelectNew(hit.collider.gameObject.GetComponent<Selectable>());
                } else
                {
                    //CmdMakeBoat(Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
                }
                
            }
        }
	}

    [Command]
    public void CmdMakeBoat (Vector2 pos, Quaternion rotation)
    {
        GameObject newBoat = Instantiate(boat, pos, rotation);
        newBoat.GetComponent<Selectable>().ownerID = playerID;
        NetworkServer.SpawnWithClientAuthority(newBoat, gameObject);
    }

    [Command]
    public void CmdMakeDock(Vector2 pos, Quaternion rotation, int islandID)
    {
        GameObject newDock = Instantiate(dock, pos, rotation);
        newDock.GetComponent<Selectable>().ownerID = playerID;
        newDock.GetComponent<Dock>().islandID = islandID;
        NetworkServer.SpawnWithClientAuthority(newDock, gameObject);
    }

    [Command]
    public void CmdFireCannonball(Vector2 pos, Quaternion rotation, Vector2 force)
    {
        GameObject newCannonball = Instantiate(cannonball, pos, rotation);
        newCannonball.GetComponent<Rigidbody2D>().velocity = force;
        print(newCannonball.GetComponent<Rigidbody2D>().velocity);
        newCannonball.GetComponent<Cannonball>().Invoke("DestroyWaterSplash", 1.5f);
        NetworkServer.Spawn(newCannonball);
    }

    [Command]
    public void CmdMakeSpawnBoat()
    {
        GameObject newBoat = Instantiate(boat, GameManager.instance.map.spawnPoints[playerID], Quaternion.identity);
        newBoat.GetComponent<Selectable>().ownerID = playerID;
        newBoat.GetComponent<Boat>().res = new Resources(10, 0);
        NetworkServer.SpawnWithClientAuthority(newBoat, gameObject);
    }

    public void UseAbility(string name)
    {
        selected.UseAbility(name);
    }

    void SelectNew(Selectable selectable)
    {
        if (selected != null)
        {
            selected.Deselect();
        }
        selected = selectable;
        selectable.Select();
        selected.CreateInfo();
        if (selected.ownerID == GameManager.instance.localPlayer.playerID)
        {
            selected.CreateAbilites();
        }
        
    }

    void SelectNull()
    {
        if (selected != null)
        {
            selected.Deselect();
        }
        selected = null;
        GameManager.instance.ui.ClearAbilities();
        GameManager.instance.ui.infoPanel.Clear();
    }
}
