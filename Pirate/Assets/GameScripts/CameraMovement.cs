using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    public float speed = .1f;
	// Use this for initialization
	void Start () {
        transform.position = new Vector3(0, 0, -10);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.mousePosition.x > 1720)
        {
            transform.Translate(new Vector3(speed, 0, 0));
        }
        else if (Input.mousePosition.x < 200) {
            transform.Translate(new Vector3(-speed, 0, 0));
        }
        if (Input.mousePosition.y > 880)
        {
            transform.Translate(new Vector3(0, speed, 0));
        }
        else if (Input.mousePosition.y < 200)
        {
            transform.Translate(new Vector3(0, -speed, 0));
        }
    }
}
