using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : PlayerControllable {

	Rigidbody2D rb2d;
    Cannon[] cannons;
    GameObject island;
    public GameObject dock;
	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
        cannons = GetComponentsInChildren<Cannon>();
        isActive = false;
        island = null;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isActive)
        {
            rb2d.AddForce(100 * transform.up * Input.GetAxis("Vertical"));
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.z -= 0.5f * Input.GetAxis("Horizontal");
            transform.rotation = Quaternion.Euler(rotation);
            if (Input.GetButton("Fire"))
            {
                foreach (Cannon cannon in cannons)
                {
                    if (Input.GetAxis("Fire") > 0 == cannon.right)
                    {
                        cannon.Invoke("Fire", cannon.delay);
                    }
                }
            }
            if (Input.GetButton("BuildDock") && island != null)
            {
                Vector2 point1 = new Vector2(0, 0);
                Vector2 point2 = new Vector2(0, 0);
                float min = 10000000;
                Vector2[] pts = island.GetComponent<PolygonCollider2D>().GetPath(0);
                for (int i = 0; i < pts.Length; i ++)
                {
                    float distance = Vector2.Distance(transform.position,pts[i] + (Vector2)island.transform.position) + Vector2.Distance(new Vector2(transform.position.x, transform.position.y), pts[(i + 1)%pts.Length] + (Vector2)island.transform.position);
                    if (distance < min)
                    {
                        min = distance;
                        point1 = pts[i] + (Vector2)island.transform.position;
                        point2 = pts[(i + 1)%pts.Length] + (Vector2)island.transform.position;
                    }
                }
                GameObject newDock = Instantiate(dock, new Vector2((point1.x + point2.x) / 2, (point1.y + point2.y) / 2), Quaternion.LookRotation(Vector3.forward, -(new Vector2((point2-point1).y,-(point2 - point1).x))));
            }
        }

        
	}

    public override void Activate()
    {
        isActive = true;
        GetComponentInChildren<Select>().Activate();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Island>() != null)
        {
            other.gameObject.GetComponent<Island>().InRange();
            island = other.gameObject;
        }
    }

    private void CreateDock()
    {

    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Island>() != null)
        {
            other.gameObject.GetComponent<Island>().OutOfRange();
        }
    }
}
