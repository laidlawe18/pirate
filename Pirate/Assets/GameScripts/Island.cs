using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Island : MonoBehaviour {
    Text text;
    float woodMult;
    float woodQuant;

    // Use this for initialization
    void Start()
    {
        transform.Find("Canvas").gameObject.SetActive(true);
        text = GetComponentInChildren<Text>();
        transform.Find("Canvas").gameObject.SetActive(false);
    }

    public void SetResourceMult(float wood)
    {
        woodMult = wood;
        woodQuant = Area(GetComponent<PolygonCollider2D>().GetPath(0)) * woodMult * 50;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void InRange()
    {
        text.text = (int) woodQuant + " wood";
        transform.Find("Canvas").gameObject.SetActive(true);
    }

    public void OutOfRange()
    {
        transform.Find("Canvas").gameObject.SetActive(false);
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
}
