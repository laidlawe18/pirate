using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Island : Selectable {
    float woodMult;
    float woodQuant;
    public GameObject[] buildingPrefabs;

    List<GameObject> docks;
    List<GameObject> buildings;
    float lastBuild = 0;
    float nextBuildWait = 10;

    Vector2 bottomLeft;
    Vector2 topRight;
    List<Vector2> buildingSpots;
    public float buildingDistance;
    int buildingCapacity;
    public float woodGatherRate;

    // Use this for initialization
    void Start()
    {
        docks = new List<GameObject>();
        buildings = new List<GameObject>();
        buildingSpots = new List<Vector2>();

        float minX = 1000000;
        float minY = 1000000;
        float maxX = -1000000;
        float maxY = -1000000;
        foreach (Vector2 vec in GetComponent<PolygonCollider2D>().GetPath(0))
        {
            if (vec.x < minX)
            {
                minX = vec.x;
            } else if (vec.x > maxX)
            {
                maxX = vec.x;
            }
            if (vec.y < minY)
            {
                minY = vec.y;
            } else if (vec.y > maxY)
            {
                maxY = vec.y;
            }

            buildingCapacity = (int) (Area(GetComponent<PolygonCollider2D>().GetPath(0)) * Random.Range(1.5f, 1.9f));
        }

        bottomLeft = new Vector2(minX + transform.position.x, minY + transform.position.y);
        topRight = new Vector2(maxX + transform.position.x, maxY + transform.position.y);

        for (float i = bottomLeft.x; i < topRight.x; i += buildingDistance)
        {
            for (float j = bottomLeft.y; j < topRight.y; j += buildingDistance)
            {
                if (GetComponentInParent<Map>().SafeForBuilding(i, j))
                {
                    buildingSpots.Add(new Vector2(i + Random.Range(buildingDistance / -4, buildingDistance / 4), j + Random.Range(buildingDistance / -4, buildingDistance / 4)));
                }
            }
        }
        
    }

    public void SetResourceMult(float wood)
    {
        woodMult = wood;
        woodQuant = Area(GetComponent<PolygonCollider2D>().GetPath(0)) * woodMult * 50;
    }

    // Update is called once per frame
    void Update () {
		if (docks.Count > 0 && buildingSpots.Count > 0 && buildings.Count <= buildingCapacity && Time.time - lastBuild > nextBuildWait)
        {
            int ind = (int) Mathf.Pow(Random.Range(0, Mathf.Pow(buildingSpots.Count, 1f / 3f)), 3);
            buildings.Add(Instantiate(buildingPrefabs[Random.Range(0, buildingPrefabs.Length)], buildingSpots[ind], Quaternion.Euler(0, 0, docks[0].transform.rotation.eulerAngles.z + Random.Range(-ind * 3, ind * 3))));
            buildingSpots.RemoveAt(ind);
            buildings[buildings.Count - 1].transform.parent = transform;
            lastBuild = Time.time;
            nextBuildWait = Random.Range(10, 20);
        }
    }

    private void FixedUpdate()
    {
        foreach (GameObject dock in docks)
        {
            float amt = Mathf.Min(woodQuant, Random.Range(.9f, 1.1f) * (10 + buildings.Count) * .0001f * woodGatherRate / (docks.Count + 5f));
            dock.GetComponent<Dock>().AddWood(amt);
            woodQuant -= amt;
        }
    }

    public void InRange()
    {

    }

    public void OutOfRange()
    {

    }

    private float Area(Vector2[] pts)
    {
        float sum = 0;
        for (int i = 0; i < pts.Length; i++)
        {
            sum += pts[i].x * pts[(i + 1) % pts.Length].y;
            sum -= pts[i].y * pts[(i + 1) % pts.Length].x;
        }
        return Mathf.Abs(sum / 2);
    }

    public void DockAdded(GameObject dock)
    {
        docks.Add(dock);
        lastBuild = Time.time;
        buildingSpots.Sort((x, y) => Vector2.Distance(x, docks[0].transform.position).CompareTo(Vector2.Distance(y, docks[0].transform.position)));
    }

    /*public override void CreateInfo(GameObject panel)
    {
        GameObject newTitle = Instantiate(title, panel.transform);
        newTitle.GetComponent<Text>().text = "Island";
        GameObject newWood = Instantiate(woodNum, panel.transform);
        newWood.GetComponentInChildren<Text>().text = (int)woodQuant + "";
    }

    public override void UpdateInfo(GameObject panel)
    {
        panel.transform.Find("Wood Panel(Clone)").GetComponentInChildren<Text>().text = (int)woodQuant + "";
    }*/
}
