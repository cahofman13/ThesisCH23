using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InputIf : BlockUI
{
    CanvasGroup routineCanvas;

    public Condition condition = new Condition();

    internal override void startup()
    {
        base.startup();
        blockName = "IfBlock";
        routineCanvas = transform.GetChild(1).GetChild(1).GetComponent<CanvasGroup>();
    }

    public void dropdownSetExecutionTarget(GameObject dropdownGO)
    {
        TMP_Dropdown dropdown = dropdownGO.GetComponent<TMP_Dropdown>();
        string text = dropdown.options[dropdown.value].text;
        switch (text)
        {
            case "<": condition.comp = Comp.LOWER; break;
            case "=": condition.comp = Comp.EQUAL; break;
            case ">": condition.comp = Comp.GREATER; break;

            default: condition.comp = Comp.EQUAL; break;
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        routineCanvas.blocksRaycasts = false;
        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Vector3 hitPos = eventData.pointerCurrentRaycast.worldPosition;
        if (hitPos == new Vector3(0, 0, 0))
            routineCanvas.alpha = 0;
        else
        {
            routineCanvas.alpha = 1;
        }
        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        routineCanvas.blocksRaycasts = true;
        base.OnEndDrag(eventData);
    }

}
