using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Island : MonoBehaviour {

    Text text;

    // Use this for initialization
    void Start()
    {
        transform.Find("Canvas").gameObject.SetActive(true);
        text = GetComponentInChildren<Text>();
        transform.Find("Canvas").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void InRange()
    {
        text.text = "island";
        transform.Find("Canvas").gameObject.SetActive(true);
    }

    public void OutOfRange()
    {
        transform.Find("Canvas").gameObject.SetActive(false);
    }
}
