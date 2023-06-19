using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputWrite : BlockUI
{

    internal Routine routine;

    public string key { get; private set; } = "";
    public string name1 { get; private set; } = null;
    public System.Object value1 { get; private set; } = 0f;

    [SerializeField] private TMP_Dropdown value1Var;
    [SerializeField] private TMP_Dropdown value2Type;
    [SerializeField] private TMP_Dropdown value2Var;
    [SerializeField] private Slider value2Num;

    internal override void startup()
    {
        base.startup();
        blockName = "WriteBlock";
        routine = canvas.GetComponentInParent<Routine>();
        dropdownSetValue1Var();
        dropdownSetType2();
    }

    internal override void UpdateExtend()
    {
        base.UpdateExtend();
        if (value1Var.gameObject.activeInHierarchy) updateVarDropdown(value1Var);
        if (value2Var.gameObject.activeInHierarchy) updateVarDropdown(value2Var);
    }

    internal void updateVarDropdown(TMP_Dropdown dropdown)
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

    public void dropdownSetValue1Var()
    {
        string text = value1Var.options[value1Var.value].text;
        key = text;
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
        name1 = text;
        ref Storage storage = ref routine.getStorage();
        if (!storage.listNames().Contains(text)) storage.writeValue(text, null);
    }

    public void dropdownSetValue2Num(float f)
    {
        name1 = null;
        value1 = f;
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
