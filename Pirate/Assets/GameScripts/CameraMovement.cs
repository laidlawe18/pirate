using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    public float speed = .1f;
    public int width = 50;
	// Use this for initialization
	void Start () {
        transform.position = new Vector3(0, 0, -10);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.mousePosition.x > Screen.width - width)
        {
            transform.Translate(new Vector3(speed * Mathf.Min((Input.mousePosition.x - Screen.width + width) / width, 1), 0, 0));
        }
        else if (Input.mousePosition.x < width) {
            transform.Translate(new Vector3(-(speed * Mathf.Min((width - Input.mousePosition.x) / width, 1)), 0, 0));
        }
        if (Input.mousePosition.y > Screen.height - width)
        {
            transform.Translate(new Vector3(0, speed * Mathf.Min((Input.mousePosition.y - Screen.height + width) / width, 1), 0));
        }
        else if (Input.mousePosition.y < width)
        {
            transform.Translate(new Vector3(0, -(speed * Mathf.Min((width - Input.mousePosition.y) / width, 1)), 0));
        }
    }
}
