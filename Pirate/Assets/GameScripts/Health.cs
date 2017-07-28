using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    [SyncVar]
    public float health;

    public float maxHealth;

	// Use this for initialization
	void Start () {
        health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [ClientRpc]
    public void RpcTakeDamage(float amt)
    {
        if (!hasAuthority)
        {
            return;
        }
        if (health <= amt)
        {
            health = 0;
            Destroy(gameObject);
        } else
        {
            health -= amt;
        }
    }

    [ClientRpc]
    public void RpcAddHealth(float amt)
    {
        if (!hasAuthority)
        {
            return;
        }
        if (health + amt > maxHealth)
        {
            health = maxHealth;
        } else
        {
            health += amt;
        }
    }

    public float GetHealthFloat()
    {
        return health / maxHealth;
    }
}
