using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : PlayerControllable {

	Rigidbody2D rb2d;
    Cannon[] cannons;
    GameObject island;
    public GameObject dock;
    Queue<GameObject> clickPoints;
    public GameObject clickPoint;
    bool oarsActive;
	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
        cannons = GetComponentsInChildren<Cannon>();
        isActive = false;
        island = null;
        clickPoints = new Queue<GameObject>();
        oarsActive = false;
	}


	
	// Update is called once per frame
	void FixedUpdate () {
        if (isActive)
        {
            oarsActive = false;
            if (Mathf.Abs(Input.GetAxis("Vertical")) > .005 || Mathf.Abs(Input.GetAxis("Horizontal")) > .005)
            {
                oarsActive = true;
                while (clickPoints.Count > 0)
                {
                    Destroy(clickPoints.Dequeue());
                }
            }
            rb2d.AddForce(100 * transform.up * Input.GetAxis("Vertical"));
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.z -= 0.5f * Input.GetAxis("Horizontal");
            transform.rotation = Quaternion.Euler(rotation);
            
            

            
        }
        if (clickPoints.Count > 0)
        {
            Vector3 pt = clickPoints.Peek().transform.position;
            if ((pt - transform.position).magnitude < .1)
            {
                GameObject go = clickPoints.Dequeue();
                Destroy(go);
            }
            else
            {
                Vector3 rot = Quaternion.FromToRotation(transform.up, pt - transform.position).eulerAngles;
                Vector3 boatRot = transform.rotation.eulerAngles;
                rot.z = rot.z > 180 ? rot.z - 360 : rot.z;
                boatRot.z += Mathf.Sign(rot.z) * Mathf.Min(Mathf.Abs(rot.z), .5f);
                transform.rotation = Quaternion.Euler(boatRot);
                oarsActive = true;
                rb2d.AddForce(100 * transform.up * Vector2.Dot((pt - transform.position).normalized, transform.up.normalized));
            }
        }

        
	}

    void Update()
    {
        if (isActive)
        {
            if (Input.GetButtonDown("Click"))
            {
                Vector2 pt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (!Physics2D.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction))
                {
                    if (!Input.GetButton("Shift"))
                    {
                        while (clickPoints.Count > 0)
                        {
                            Destroy(clickPoints.Dequeue());
                        }
                    }
                    clickPoints.Enqueue(Instantiate(clickPoint, pt, Quaternion.identity));
                }
            }

            if (Input.GetButtonDown("BuildDock") && island != null)
            {
                Vector2 point1 = new Vector2(0, 0);
                Vector2 point2 = new Vector2(0, 0);
                float min = 10000000;
                Vector2[] pts = island.GetComponent<PolygonCollider2D>().GetPath(0);
                for (int i = 0; i < pts.Length; i++)
                {
                    float distance = Vector2.Distance(transform.position, pts[i] + (Vector2)island.transform.position) + Vector2.Distance(new Vector2(transform.position.x, transform.position.y), pts[(i + 1) % pts.Length] + (Vector2)island.transform.position);
                    if (distance < min)
                    {
                        min = distance;
                        point1 = pts[i] + (Vector2)island.transform.position;
                        point2 = pts[(i + 1) % pts.Length] + (Vector2)island.transform.position;
                    }
                }
                GameObject newDock = Instantiate(dock, new Vector2((point1.x + point2.x) / 2, (point1.y + point2.y) / 2), Quaternion.LookRotation(Vector3.forward, -(new Vector2((point2 - point1).y, -(point2 - point1).x))));
                island.GetComponent<Island>().DockAdded(newDock);
                GetComponentInParent<PlayerControl>().AddControllable(newDock.GetComponent<Dock>());
                newDock.GetComponent<Dock>().SetPlayerControl(GetComponentInParent<PlayerControl>());
            }

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
        }
    }

    public override void Activate()
    {
        base.Activate();
        GetComponentInChildren<Select>().Activate();
        foreach (GameObject go in clickPoints)
        {
            go.SetActive(true);
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        foreach (GameObject go in clickPoints)
        {
            go.SetActive(false);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Island>() != null)
        {
            other.gameObject.GetComponent<Island>().InRange();
            island = other.gameObject;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Island>() != null)
        {
            other.gameObject.GetComponent<Island>().OutOfRange();
            island = null;
        }
    }

    public bool OarsActive()
    {
        return oarsActive;
    }
}
