using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour {

    public GameObject titlePrefab;
    public GameObject resourcePanelPrefab;
    public GameObject healthBarPrefab;

    List<ResourcePanel> resourcePanels;
    HealthBar healthBar;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Clear()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        resourcePanels = null;
    }

    public void AddTitle(string title)
    {
        GameObject newTitle = Instantiate(titlePrefab, transform);
        newTitle.GetComponent<Text>().text = title;
    }

    public void AddResources(Resources res)
    {
        resourcePanels = new List<ResourcePanel>();
        GameObject newResourcePanel = Instantiate(resourcePanelPrefab, transform);
        newResourcePanel.GetComponent<ResourcePanel>().SetWood(res);
        resourcePanels.Add(newResourcePanel.GetComponent<ResourcePanel>());
    }

    public void UpdateResources(Resources res)
    {
        foreach (ResourcePanel resPanel in resourcePanels)
        {
            resPanel.UpdateResources(res);
        }
    }

    public void AddHealthBar(Health health)
    {
        healthBar = Instantiate(healthBarPrefab, transform).GetComponent<HealthBar>();
        healthBar.UpdateHealth(health);
    }

    public void UpdateHealth(Health health)
    {
        healthBar.UpdateHealth(health);
    }
}
