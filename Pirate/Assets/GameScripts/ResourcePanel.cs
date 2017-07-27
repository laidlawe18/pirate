using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePanel : MonoBehaviour {

    public Sprite woodImage;
    public Sprite gunpowderImage;
    public Image image;
    public Text text;
    string type;


	// Use this for initialization
	void Start () {
		
	}

    public void SetWood(Resources res)
    {
        type = "wood";
        image.sprite = woodImage;
        text.text = (int)res.wood + "";
    }

    public void SetGunpowder(Resources res)
    {
        type = "gunpowder";
        image.sprite = gunpowderImage;
        text.text = (int)res.gunpowder + "";
    }

    // Update is called once per frame
    public void UpdateResources(Resources res) {
		if (type != null)
        {
            switch (type)
            {
                case "wood":
                    text.text = (int)res.wood + "";
                    break;
                case "gunpowder":
                    text.text = (int)res.gunpowder + "";
                    break;
            }

        }
	}
}
