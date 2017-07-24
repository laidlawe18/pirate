using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dock : PlayerControllable {

    public GameObject boat;
    float wood;

	// Use this for initialization
	void Start () {
        wood = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive)
        {

        }
	}

    public override void Activate(PlayerControl pc)
    {
        base.Activate(pc);
    }

    public override void Deactivate(PlayerControl pc)
    {
        base.Deactivate(pc);
    }

    public void AddWood(float amt)
    {
        wood += amt;
    }

    public float GetWood()
    {
        return wood;
    }

    public override void CreateInfo(GameObject panel)
    {
        GameObject newTitle = Instantiate(title, panel.transform);
        newTitle.GetComponent<Text>().text = "Dock";
        GameObject newWood = Instantiate(woodNum, panel.transform);
        newWood.GetComponentInChildren<Text>().text = (int)wood + "";
    }

    public override void UpdateInfo(GameObject panel)
    {
        panel.transform.Find("Wood Panel(Clone)").GetComponentInChildren<Text>().text = (int)wood + "";
    }

    public override void useAbility(string name)
    {
        if (name.Equals("Create Boat"))
        {
            CreateBoat();
        }
    }

    public void CreateBoat()
    {
        if(wood >= .5f)
        {
            GameObject newBoat = Instantiate(boat, transform.position + transform.rotation * new Vector3(0, -0.8f, 0), Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 180)), transform.parent);
            //GetComponentInParent<PlayerControl>().AddControllable(newBoat.GetComponent<BoatMovement>());
        }
    }
}
