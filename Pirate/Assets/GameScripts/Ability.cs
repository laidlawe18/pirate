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
        useAbility();
    }

    public void useAbility ()
    {
        transform.parent.parent.gameObject.GetComponent<PlayerControl>().useAbility(abilityName);
    }
}
