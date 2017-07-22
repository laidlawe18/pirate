using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllable : MonoBehaviour {

	public bool isActive;
    public GameObject title;
    public GameObject woodNum;
    public GameObject[] abilities;
    PlayerControl pc;

	// Use this for initialization
	void Start () {
        pc = GetComponentInParent<PlayerControl>();
	}

    public void SetPlayerControl(PlayerControl playerControl)
    {
        pc = playerControl;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown(){
        if (pc == null)
        {
            pc = GetComponentInParent<PlayerControl>();
        }
        pc.OnChildClick(this);
	}

    public virtual void Activate(PlayerControl pc)
    {
        isActive = true;
        GameObject panel = pc.transform.Find("Canvas").Find("Info Panel").gameObject;
        CreateInfo(panel);
        
        CreateAbilities(pc.transform.Find("Canvas").gameObject);
    }

    public virtual void Deactivate(PlayerControl pc)
    {
        isActive = false;
        GameObject panel = pc.transform.Find("Canvas").Find("Info Panel").gameObject;
        for (int i = panel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(panel.transform.GetChild(i).gameObject);
        }
        DeleteAbilities(pc.transform.Find("Canvas").gameObject);
    }

    public virtual void CreateInfo(GameObject panel)
    {
        
    }

    public virtual void UpdateInfo(GameObject panel)
    {

    }

    public virtual void useAbility(string name)
    {

    }

    public void CreateAbilities(GameObject canvas)
    {
        for(int i = 0; i < abilities.Length; i ++)
        {
            GameObject newAbility = Instantiate(abilities[i], canvas.transform);
            newAbility.GetComponent<RectTransform>().anchoredPosition = new Vector2(-305 - i * 85, 47.5f);
        }
    }

    public void DeleteAbilities(GameObject canvas)
    {
        foreach (Ability ability in canvas.GetComponentsInChildren<Ability>())
        {
            Destroy(ability.gameObject);
        }
    }

}
