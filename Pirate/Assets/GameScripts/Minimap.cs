using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Minimap : MonoBehaviour, IPointerDownHandler {

    RectTransform rt;
    Camera camera;

	// Use this for initialization
	void Start () {
        Camera camera = GameObject.Find("Minimap Camera").GetComponent<Camera>();
        RectTransform rt = GetComponent<RectTransform>();

    }
    

    // Update is called once per frame
    void Update () {
		
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        if (rt == null)
        {
            rt = GetComponent<RectTransform>();
        }
        if (camera == null)
        {
            camera = GameObject.Find("Minimap Camera").GetComponent<Camera>();
        }
        Vector2 rectPos = new Vector2(0, 0);
        
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, null, out rectPos);
        rectPos += rt.sizeDelta / 2;

        Vector2 viewportPos = new Vector2(rectPos.x / rt.sizeDelta.x, rectPos.y / rt.sizeDelta.y);

        Vector3 worldPos = camera.ViewportToWorldPoint(viewportPos);
        worldPos.z = -10;
        Camera.main.transform.position = worldPos;
    }
}
