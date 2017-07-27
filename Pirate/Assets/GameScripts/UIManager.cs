using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public Minimap minimap;
    public MinimapFromCamera minimapFromCamera;
    public InfoPanel infoPanel;
    
    int abilityXOffset = -315;
    int abilityYOffset = 45;
    int abilitySpacing = -85;

    List<GameObject> abilities;

    void Start()
    {
        abilities = new List<GameObject>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void InitUI()
    {
        minimap.InitUI();
        minimapFromCamera.InitUI();
    }

    public void CreateAbility(GameObject ability)
    {
        GameObject newAbility = Instantiate(ability, transform);
        newAbility.GetComponent<RectTransform>().anchoredPosition = new Vector2(abilityXOffset + abilitySpacing * abilities.Count, abilityYOffset);
        abilities.Add(newAbility);
    }

    public void ClearAbilities()
    {
        foreach (GameObject go in abilities)
        {
            Destroy(go);
        }
        abilities = new List<GameObject>();
    }
}
