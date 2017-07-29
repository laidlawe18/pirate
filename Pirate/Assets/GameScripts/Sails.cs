using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sails : MonoBehaviour {

    public Animator anim;
    public SpriteRenderer sr;
    public Boat boat;

	// Use this for initialization
	void Start () {
		if (boat.ownerID == GameManager.instance.localPlayer.playerID)
        {
            sr.color = Color.blue;
        } else
        {
            sr.color = Color.red;
        }
	}
	
	// Update is called once per frame
	void Update () {
        /*Vector2 forward = transform.up;
        Vector2 wind = GameManager.instance.map.wind.normalized;
        Vector2.Dot(forward, wind);*/
        if (GetComponentInParent<Boat>().sailsOut)
        {
            anim.SetInteger("State", 1);
        } else
        {
            anim.SetInteger("State", 0);
        }
	}
}
