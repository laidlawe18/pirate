using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    public Vector2 spawnPoint;
    public Vector2 force;
    public GameObject cannonBall;
    public bool right;
    public float delay;

    float firedTime;
    Animator anim;

	// Use this for initialization
	void Start () {
        firedTime = 0;
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (anim.GetInteger("State") == 1 && Time.time - firedTime > 0.1)
        {
            anim.SetInteger("State", 0);
        }
	}

    public void Fire()
    {
        if (Time.time - firedTime > 2)
        {
            firedTime = Time.time;
            anim.SetInteger("State", 1);

            GameObject ball = Instantiate(cannonBall, transform.rotation * spawnPoint + transform.position, transform.rotation);
            ball.GetComponent<Rigidbody2D>().AddForce(Quaternion.Euler(0, 0, Random.Range(-2, 2)) * transform.rotation * force);
            Destroy(ball, 3);
        }
    }
}
