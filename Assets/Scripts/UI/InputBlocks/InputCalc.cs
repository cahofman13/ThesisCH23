using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputCalc : InputWrite
{

    public string Op { get; private set; } = "";
    public string name2 { get; private set; } = null;
    public System.Object value2 { get; private set; } = 0f;

    [SerializeField] private TMP_Dropdown OpDrop;
    [SerializeField] private TMP_Dropdown value3Type;
    [SerializeField] private TMP_Dropdown value3Var;
    [SerializeField] private Slider value3Num;

    internal override void startup()
    {
        base.startup();
        blockName = "CalcBlock";
        dropdownSetOp();
        dropdownSetType3();
    }

    internal override void UpdateExtend()
    {
        base.UpdateExtend();
        if (value3Var.gameObject.activeInHierarchy) updateVarDropdown(value3Var);
    }

    public void dropdownSetOp()
    {
        if (OpDrop.options[OpDrop.value].text != "") Op = OpDrop.options[OpDrop.value].text;
        else Op = "-";
    }

    public void dropdownSetType3()
    {
        string text = value3Type.options[value3Type.value].text;
        switch (text)
        {
            case "Variable": setValue3Variable(); break;
            case "Number": setValue3Number(); break;

            default: setValue3Variable(); break;
        }
    }

    public void dropdownSetValue3Var()
    {
        string text = value3Var.options[value3Var.value].text;
        name2 = text;
        ref Storage storage = ref routine.getStorage();
        if (!storage.listNames().Contains(text)) storage.writeValue(text, null);
    }

    public void dropdownSetValue3Num(float f)
    {
        name2 = null;
        value2 = f;
    }

    private void setValue3Variable()
    {
        value3Var.gameObject.SetActive(true);
        value3Num.gameObject.SetActive(false);
        dropdownSetValue3Var();
    }

    private void setValue3Number()
    {
        value3Var.gameObject.SetActive(false);
        value3Num.gameObject.SetActive(true);
        dropdownSetValue3Num(value3Num.value);
    }

}
