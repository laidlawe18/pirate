using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Cannonball : NetworkBehaviour {

    public float damage = 5f;
    public GameObject waterSplash;
    public GameObject sandSplash;
    public GameObject woodSplash;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isServer)
        {
            return;
        }
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.RpcTakeDamage(damage);
            DestroyWoodSplash();
        } else if (collision.gameObject.GetComponent<Island>() != null)
        {
            DestroySandSplash();
        }
    }

    [Server]
    public void DestroyWaterSplash()
    {
        GameObject newWaterSplash = Instantiate(waterSplash, transform.position, Quaternion.identity);
        Destroy(newWaterSplash, 0.5f);
        NetworkServer.Spawn(newWaterSplash);
        Destroy(gameObject);
    }

    [Server]
    public void DestroySandSplash()
    {
        GameObject newSandSplash = Instantiate(sandSplash, transform.position, Quaternion.identity);
        Destroy(newSandSplash, 0.5f);
        NetworkServer.Spawn(newSandSplash);
        Destroy(gameObject);
    }

    [Server]
    public void DestroyWoodSplash()
    {
        GameObject newWoodSplash = Instantiate(woodSplash, transform.position, Quaternion.identity);
        Destroy(newWoodSplash, 0.5f);
        NetworkServer.Spawn(newWoodSplash);
        Destroy(gameObject);
    }
}
