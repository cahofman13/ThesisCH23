using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputIf : BlockUI
{
    CanvasGroup routineCanvas;
    Routine routine;

    public Condition condition = new Condition();

    [SerializeField] private TMP_Dropdown compare;
    [SerializeField] private TMP_Dropdown value1Var;
    [SerializeField] private TMP_Dropdown value2Type;
    [SerializeField] private TMP_Dropdown value2Var;
    [SerializeField] private Slider value2Num;

    internal override void startup()
    {
        base.startup();
        blockName = "IfBlock";
        routineCanvas = transform.GetChild(1).GetChild(1).GetComponent<CanvasGroup>();
        routine = canvas.GetComponentInParent<Routine>();
        image = transform.GetComponentInChildren<Image>();
        dropdownSetValue1Var();
        dropdownSetCompare();
        dropdownSetType2();
    }

    internal override void UpdateExtend()
    {
        base.UpdateExtend();
        if (value1Var.gameObject.activeInHierarchy) updateVarDropdown(value1Var);
        if (value2Var.gameObject.activeInHierarchy) updateVarDropdown(value2Var);
    }

    private void updateVarDropdown(TMP_Dropdown dropdown)
    {
        ref Storage storage = ref routine.getStorage();
        List<string> names = new List<string>();
        foreach (string key in storage.listNames())
        {
            names.Add(key);
        }
        foreach (string s in Storage.varNameArray)
        {
            if (!names.Contains(s))
            {
                names.Add(s);
                break;
            }
        }
        foreach (TMP_Dropdown.OptionData option in dropdown.options)
        {
            if (names.Contains(option.text)) names.Remove(option.text);
        }
        dropdown.AddOptions(names);
    }

    public void dropdownSetCompare()
    {
        string text = compare.options[compare.value].text;
        switch (text)
        {
            case "<": condition.comp = Comp.LOWER; break;
            case "=": condition.comp = Comp.EQUAL; break;
            case ">": condition.comp = Comp.GREATER; break;

            default: condition.comp = Comp.EQUAL; break;
        }
    }

    public void dropdownSetValue1Var()
    {
        string text = value1Var.options[value1Var.value].text;
        condition.key1 = text;
        ref Storage storage = ref routine.getStorage();
        if (!storage.listNames().Contains(text)) storage.writeValue(text, null);
    }

    public void dropdownSetType2()
    {
        string text = value2Type.options[value2Type.value].text;
        switch (text)
        {
            case "Variable": setValue2Variable(); break;
            case "Number": setValue2Number(); break;

            default: setValue2Variable(); break;
        }
    }

    public void dropdownSetValue2Var()
    {
        string text = value2Var.options[value2Var.value].text;
        condition.key2 = text;
        ref Storage storage = ref routine.getStorage();
        if (!storage.listNames().Contains(text)) storage.writeValue(text, null);
    }

    public void dropdownSetValue2Num(float f)
    {
        condition.key2 = null;
        condition.value2 = f;
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

    private void setValue2Variable()
    {
        value2Var.gameObject.SetActive(true);
        value2Num.gameObject.SetActive(false);
        dropdownSetValue2Var();
    }

    private void setValue2Number()
    {
        value2Var.gameObject.SetActive(false);
        value2Num.gameObject.SetActive(true);
        dropdownSetValue2Num(value2Num.value);
    }

}
