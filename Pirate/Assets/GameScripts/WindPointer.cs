using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPointer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.activeInHierarchy)
        {
            Vector3 angles = Quaternion.LookRotation(GameManager.instance.map.wind).eulerAngles;
            angles.z = angles.x - 90;
            angles.x = 0;
            angles.y = 0;
            transform.rotation = Quaternion.Euler(angles);
        }
	}
}
