using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEx : Button
{

    public bool IsOn { get; private set; }

    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        IsOn = true;
    }

    public override void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        IsOn = false;
    }

    public override void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        IsOn = false;
    }

    // これはイラナイ
    void Update()
    {
        Debug.Log("押されている？ " + IsOn);
    }
}
