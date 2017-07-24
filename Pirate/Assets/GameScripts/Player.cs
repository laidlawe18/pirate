using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    [SyncVar]
    public int playerID;

    public GameObject boat;

    Selectable selected;

	// Use this for initialization
	void Start () {
        playerID = GameManager.instance.AddPlayer(this);
	}
    

    public override void OnStartLocalPlayer()
    {
        GameManager.instance.localPlayer = this;
    }

    // Update is called once per frame
    void Update () {
		if (isLocalPlayer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 15);
                if (hit.collider != null && hit.collider.gameObject.GetComponent<Selectable>() != null)
                {
                    if (selected != null)
                    {
                        selected.Deselect();
                    }
                    selected = hit.collider.gameObject.GetComponent<Selectable>();
                    selected.Select();
                } else
                {
                    CmdMakeBoat(Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
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
}
