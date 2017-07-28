using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    public RectTransform healthRT;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateHealth(float health)
    {
        healthRT.anchoredPosition = new Vector2(-healthRT.sizeDelta.x * (1 - health) + healthRT.sizeDelta.x / 2, -healthRT.sizeDelta.y / 2);
    }
}
