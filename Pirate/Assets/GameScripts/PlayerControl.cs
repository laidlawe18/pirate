using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {

    List<PlayerControllable> controllables;
    public int currActive;

	// Use this for initialization
	void Start () {
        controllables = new List<PlayerControllable>(GetComponentsInChildren<PlayerControllable>());
        currActive = 0;
	}
	
	// Update is called once per frame
	void Update () {
        GameObject panel = transform.Find("Canvas/Info Panel").gameObject;
        if (controllables.Count == 0)
        {
            currActive = -1;
        }
        else
        {
            
            if (Input.GetButtonDown("Switch") && Input.GetAxis("Switch") > 0)
            {
                controllables[currActive].Deactivate(this);
                currActive += 1;
                currActive %= controllables.Count;
                controllables[currActive].Activate(this);
                
            }
            if (Input.GetButtonDown("Switch") && Input.GetAxis("Switch") < 0)
            {
                controllables[currActive].Deactivate(this);
                currActive -= 1;
                if (currActive < 0)
                {
                    currActive = controllables.Count - 1;
                }
                controllables[currActive].Activate(this);
                
            }
        }
        float woodTot = 0;
        foreach (PlayerControllable pc in controllables)
        {
            if (pc.GetType() == typeof(Dock))
            {
                woodTot += ((Dock)pc).GetWood();
            }
        }

        GetComponentInChildren<Text>().text = ((int)woodTot).ToString();


        panel = transform.Find("Canvas/Info Panel").gameObject;
        controllables[currActive].UpdateInfo(panel);
        
    }
    

    public void OnChildClick (PlayerControllable pc) {
        

        for (int i = 0; i < controllables.Count; i++)
        {
            if (controllables[i].Equals(pc))
            {
                controllables[currActive].Deactivate(this); 
                currActive = i;
                controllables[currActive].Activate(this);
                GameObject panel = transform.Find("Canvas/Info Panel").gameObject;
                
            }
        }
	}

    public void AddControllable(PlayerControllable pc)
    {
        controllables.Add(pc);
    }

    public void useAbility(string name)
    {
        controllables[currActive].useAbility(name);
    }

}
