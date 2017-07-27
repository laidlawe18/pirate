using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Ability : MonoBehaviour, IPointerDownHandler
{
    public string abilityName;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UseAbility();
    }

    public void UseAbility ()
    {
        GameManager.instance.localPlayer.UseAbility(abilityName);
    }
}
