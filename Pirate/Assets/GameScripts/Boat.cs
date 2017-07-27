using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Boat : Selectable {

	Rigidbody2D rb2d;
    Cannon[] cannons;
    List<Island> islandsInRange;
    Queue<GameObject> clickPoints;
    public GameObject clickPoint;
    bool oarsActive;

    

    // Use this for initialization
    new void Start()
    {
        base.Start();
        selected = false;
        GameManager.instance.AddSelectable(this);

        rb2d = GetComponent<Rigidbody2D>();
        cannons = GetComponentsInChildren<Cannon>();
        islandsInRange = new List<Island>();
        clickPoints = new Queue<GameObject>();
        oarsActive = false;
    }



    // Update is called once per frame
    void FixedUpdate () {
        oarsActive = false;
        if (selected && ownerID == localPlayer.playerID)
        {
            
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
        if (selected && ownerID == localPlayer.playerID)
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

            if (Input.GetButtonDown("BuildDock") && islandsInRange.Count > 0)
            {
                BuildDock();
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

    public void BuildDock()
    {
        Vector2 point1 = new Vector2(0, 0);
        Vector2 point2 = new Vector2(0, 0);
        float min = 10000000;
        Vector2[] pts = islandsInRange[0].GetComponent<PolygonCollider2D>().GetPath(0);
        for (int i = 0; i < pts.Length; i++)
        {
            float distance = Vector2.Distance(transform.position, pts[i] + (Vector2)islandsInRange[0].transform.position) + Vector2.Distance(new Vector2(transform.position.x, transform.position.y), pts[(i + 1) % pts.Length] + (Vector2)islandsInRange[0].transform.position);
            if (distance < min)
            {
                min = distance;
                point1 = pts[i] + (Vector2)islandsInRange[0].transform.position;
                point2 = pts[(i + 1) % pts.Length] + (Vector2)islandsInRange[0].transform.position;
            }
        }
        localPlayer.CmdMakeDock(new Vector2((point1.x + point2.x) / 2, (point1.y + point2.y) / 2), Quaternion.LookRotation(Vector3.forward, -(new Vector2((point2 - point1).y, -(point2 - point1).x))), islandsInRange[0].islandID);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Island>() != null)
        {
            other.gameObject.GetComponent<Island>().InRange();
            islandsInRange.Add(other.gameObject.GetComponent<Island>());
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Island>() != null)
        {
            other.gameObject.GetComponent<Island>().OutOfRange();
            islandsInRange.Remove(other.gameObject.GetComponent<Island>());
        }
    }

    public bool OarsActive()
    {
        return oarsActive;
    }

    public override void CreateInfo()
    {
        InfoPanel infoPanel = GameManager.instance.ui.infoPanel;
        infoPanel.Clear();
        infoPanel.AddTitle("Boat");
    }

    public override void UseAbility(string name)
    {
        switch (name)
        {
            case "Build Dock":
                BuildDock();
                break;
        }
    }
}
