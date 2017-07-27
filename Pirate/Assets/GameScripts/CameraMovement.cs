using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    public float speed = .1f;
    public int width = 50;

    float horBound;
    float vertBound;
    bool canMove = false;
	// Use this for initialization
	void Start () {
        
	}

    public void setBounds(float h, float v)
    {
        canMove = true;
        horBound = h;
        vertBound = v;
        transform.position = new Vector3(0, 0, -10);
        GetComponentInChildren<SpriteRenderer>().size = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)) - Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (canMove)
        {
            if (Input.mousePosition.x > Screen.width - width)
            {
                transform.Translate(new Vector3(Mathf.Min(horBound - transform.position.x, speed * Mathf.Min((Input.mousePosition.x - Screen.width + width) / width, 1)), 0, 0));
            }
            else if (Input.mousePosition.x < width)
            {
                transform.Translate(new Vector3(-Mathf.Min(transform.position.x + horBound, (speed * Mathf.Min((width - Input.mousePosition.x) / width, 1))), 0, 0));
            }
            if (Input.mousePosition.y > Screen.height - width)
            {
                transform.Translate(new Vector3(0, Mathf.Min(vertBound - transform.position.y, speed * Mathf.Min((Input.mousePosition.y - Screen.height + width) / width, 1)), 0));
            }
            else if (Input.mousePosition.y < width)
            {
                transform.Translate(new Vector3(0, -Mathf.Min(transform.position.y + vertBound, (speed * Mathf.Min((width - Input.mousePosition.y) / width, 1))), 0));
            }
        }
    }
}
