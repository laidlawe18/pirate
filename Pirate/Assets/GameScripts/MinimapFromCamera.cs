using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MinimapFromCamera : MonoBehaviour, IPointerDownHandler
{

    RectTransform rt;
    public Camera minimapCamera;

    // Use this for initialization
    public void InitUI()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = GameManager.instance.map.SetupMinimapCamera(minimapCamera, (int) rt.sizeDelta.x);
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (rt == null)
        {
            rt = GetComponent<RectTransform>();
        }
        Vector2 rectPos = new Vector2(0, 0);


        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, null, out rectPos);
        rectPos += rt.sizeDelta / 2;

        Vector2 viewportPos = new Vector2(rectPos.x / rt.sizeDelta.x, rectPos.y / rt.sizeDelta.y);

        Vector3 worldPos = minimapCamera.ViewportToWorldPoint(viewportPos);
        worldPos.z = -10;
        Camera.main.transform.position = worldPos;
    }
}

