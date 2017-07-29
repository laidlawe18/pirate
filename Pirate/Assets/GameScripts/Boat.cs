using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Boat : Selectable {

    Rigidbody2D rb2d;
    Cannon[] cannons;
    List<Island> islandsInRange;
    List<Dock> docksInRange;
    Queue<GameObject> clickPoints;
    public GameObject clickPoint;

    public SpriteRenderer minimapIcon;

    [SyncVar]
    bool oarsActive;

    [SyncVar]
    public bool sailsOut = false;

    [SyncVar]
    public Resources res;

    public Health health;

    public float oarSpeed;
    public float turnSpeed;
    public float sailMultiplier;

    public Vector2 anchorPoint;
    public float anchorDistance;

    [SyncVar]
    bool anchorDown;

    DistanceJoint2D anchor;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        selected = false;
        GameManager.instance.AddSelectable(this);

        rb2d = GetComponent<Rigidbody2D>();
        cannons = GetComponentsInChildren<Cannon>();
        islandsInRange = new List<Island>();
        docksInRange = new List<Dock>();
        clickPoints = new Queue<GameObject>();
        oarsActive = false;

        if (ownerID != localPlayer.playerID)
        {
            minimapIcon.color = Color.red;
            //GetComponent<SpriteRenderer>().color = Color.red;
        } else
        {
            minimapIcon.color = Color.blue;
            //GetComponent<SpriteRenderer>().color = Color.blue;
        }
        
    }



    // Update is called once per frame
    void FixedUpdate() {

        if (localPlayer.isLocalPlayer)
        {
            //print(GameManager.instance.map.wind);
            if (sailsOut)
            {
                rb2d.AddForce(GameManager.instance.map.wind + (Vector2)transform.up * Vector2.Dot(GameManager.instance.map.wind, transform.up) * sailMultiplier);
            } else
            {
                rb2d.AddForce(GameManager.instance.map.wind);
            }
            
        }
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
            rb2d.AddForce(oarSpeed * transform.up * Input.GetAxis("Vertical"));
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.z -= turnSpeed * Input.GetAxis("Horizontal");
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
                boatRot.z += Mathf.Sign(rot.z) * Mathf.Min(Mathf.Abs(rot.z), turnSpeed);
                transform.rotation = Quaternion.Euler(boatRot);
                oarsActive = true;
                rb2d.AddForce(oarSpeed * transform.up * Vector2.Dot((pt - transform.position).normalized, transform.up.normalized));
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

            if (Input.GetButtonDown("Space"))
            {
                ToggleSails();
            }
        }

        if (anchorDown && anchor == null)
        {
            anchor = gameObject.AddComponent<DistanceJoint2D>();
            anchor.autoConfigureDistance = false;
            anchor.distance = anchorDistance;
            anchor.maxDistanceOnly = true;
            anchor.connectedAnchor = transform.position + transform.up * .5f;
            anchor.anchor = anchorPoint;
        } else if (!anchorDown && anchor != null)
        {
            Destroy(anchor);
            anchor = null;
        }
    }

    public void BuildDock()
    {
        if (res.wood < 5 || islandsInRange.Count == 0)
        {
            return;
        }
        res -= new Resources(5, 0);
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
        } else if (other.gameObject.GetComponent<Dock>() != null && other.gameObject.GetComponent<Dock>().ownerID == localPlayer.playerID)
        {
            docksInRange.Add(other.gameObject.GetComponent<Dock>());
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Island>() != null)
        {
            other.gameObject.GetComponent<Island>().OutOfRange();
            islandsInRange.Remove(other.gameObject.GetComponent<Island>());
        }
        else if (other.gameObject.GetComponent<Dock>() != null && other.gameObject.GetComponent<Dock>().ownerID == localPlayer.playerID)
        {
            docksInRange.Remove(other.gameObject.GetComponent<Dock>());
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
        infoPanel.AddHealthBar(health);
        infoPanel.AddResources(res);
    }

    public override void UpdateInfo()
    {
        InfoPanel infoPanel = GameManager.instance.ui.infoPanel;
        infoPanel.UpdateHealth(health);
        infoPanel.UpdateResources(res);
    }

    public void WoodAway()
    {
        if (res.wood >= 1 && docksInRange.Count > 0)
        {
            res -= new Resources(1, 0);
            docksInRange[0].res += new Resources(1, 0);
        }
    }

    public void WoodHere()
    {
        if (docksInRange.Count > 0 && docksInRange[0].res.wood >= 1)
        {
            res += new Resources(1, 0);
            docksInRange[0].res -= new Resources(1, 0);
        }
    }

    public void ToggleSails()
    {
        sailsOut = !sailsOut;
    }

    public void ToggleAnchor()
    {
        anchorDown = !anchorDown;
    }

    public override void UseAbility(string name)
    {
        switch (name)
        {
            case "Build Dock":
                BuildDock();
                break;
            case "Wood Away":
                WoodAway();
                break;
            case "Wood Here":
                WoodHere();
                break;
            case "Sails":
                ToggleSails();
                break;
            case "Drop Anchor":
                ToggleAnchor();
                break;
        }
    }
}
