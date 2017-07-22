using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dock : PlayerControllable {


    float wood;

	// Use this for initialization
	void Start () {
        wood = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive)
        {
            GetComponentInChildren<Text>().text = (int) wood + " wood";
        }
	}

    public override void Activate()
    {
        base.Activate();
        transform.Find("Canvas").gameObject.SetActive(true);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        transform.Find("Canvas").gameObject.SetActive(false);
    }

    public void AddWood(float amt)
    {
        wood += amt;
    }

    public float GetWood()
    {
        return wood;
    }
}
