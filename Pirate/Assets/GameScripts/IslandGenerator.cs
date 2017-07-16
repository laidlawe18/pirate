using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGenerator : MonoBehaviour {
	// Use this for initialization
	public int numIslands = 15;
	public GameObject[] islands;
	GameObject[] allIslands;

	void Start () {
		allIslands = new GameObject[numIslands];
		for(int i = 0; i < numIslands; i ++ ){
			while (allIslands [i] == null || Physics2D.OverlapCircleAll(allIslands[i].transform.position, 1.6f).Length > 1) {
				Destroy (allIslands [i]);
				allIslands [i] = Instantiate (islands [Random.Range (0, 6)], new Vector3 (Random.Range (-9.0f, 9.0f), Random.Range (-4.0f, 4.0f), 0), Quaternion.Euler (0, 0, Random.Range (0, 360)));
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
