using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Minimap : MonoBehaviour {

    public void InitUI()
    {
        Sprite sprite = GameManager.instance.map.GetMinimapSprite((int)GetComponent<RectTransform>().sizeDelta.x);
        GetComponent<RectTransform>().sizeDelta = new Vector2(sprite.texture.width, sprite.texture.height);
        transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(sprite.texture.width + 30, sprite.texture.height + 30);
        transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(sprite.texture.width / 2 + 25, sprite.texture.height / 2 + 25);

        GetComponent<Image>().sprite = sprite;
    }

}
