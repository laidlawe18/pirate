using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public RectTransform healthRT;
    public Text healthText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateHealth(Health health)
    {
        healthRT.anchoredPosition = new Vector2(-healthRT.sizeDelta.x * (1 - health.GetHealthFloat()) + healthRT.sizeDelta.x / 2, -healthRT.sizeDelta.y / 2);
        healthText.text = (int)health.health + " / " + (int)health.maxHealth;
    }
}
