using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class CalcNode : Node
{
    CalcDisplay[] calcDisplays;

#pragma warning disable CS8632
    int v1Key = 0;
    float v1Num = 3;
    public (string?,float?) getVal1() { return (v1Type == InputType.VAR ? new(varNames[v1Key], null) : new(null, v1Num)); }

    int v2Key = 0;
    float v2Num = 3;
    public (string?, float?) getVal2() { return (v2Type == InputType.VAR ? new(varNames[v2Key], null) : new(null, v2Num)); }
#pragma warning restore CS8632

    int targKey = 3;
    public string getTarget() { return varNames[targKey]; }

    InputState inputState = InputState.VAL1;
    InputType v1Type = InputType.NUM;
    InputType v2Type = InputType.NUM;
    public OpType opType { get; internal set; } = OpType.ADD;

    Routine routine;
    List<string> varNames;

    enum InputState
    {
        VAL1,
        OP,
        VAL2,
        TARG
    }

    enum InputType
    {
        VAR,
        NUM
    }

    public enum OpType
    {
        ADD,
        SUB,
        MUL,
        DIV
    }

    internal override void exStart()
    {
        base.exStart();
        calcDisplays = this.GetComponentsInChildren<CalcDisplay>();
        routine = this.GetComponentInParent<DroneInterface>().gameObject
            .GetComponent<PositionConstraint>().GetSource(0).sourceTransform.GetComponent<Routine>(); //rework Storage?
        updateVarNames();
    }

    internal override void exUpdate()
    {
        base.exUpdate();
        if (isDragged && Input.GetKeyDown(KeyCode.Q)) { prevVal(); StartCoroutine(delayedHold(KeyCode.Q)); }
        if (isDragged && Input.GetKeyDown(KeyCode.E)) { nextVal(); StartCoroutine(delayedHold(KeyCode.E)); }
        if (isDragged && Input.GetKeyDown(KeyCode.Tab)) { changeInput(); }
        if (isDragged && Input.GetKeyDown(KeyCode.F)) { changeValType(); }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public override void setDrag(bool dragged)
    {
        base.setDrag(dragged);
        foreach (CalcDisplay calcDisplay in calcDisplays) calcDisplay.activeBg[(int)inputState].SetActive(dragged);
    }

    public override void prevVal()
    {
        switch (inputState)
        { //rework Storage?
            case InputState.VAL1:
                switch (v1Type)
                {
                    case InputType.NUM:
                        if (v1Num > -999)
                        {
                            v1Num--;
                            foreach (CalcDisplay calcDisplay in calcDisplays)
                                calcDisplay.display[(int)inputState].text = v1Num.ToString();
                        }
                        break;

                    case InputType.VAR:
                        v1Key--;
                        if (v1Key < 0) v1Key = varNames.Count - 1;

                        foreach (CalcDisplay calcDisplay in calcDisplays)
                            calcDisplay.display[(int)inputState].text = varNames[v1Key];
                        break;
                }
                break;

            case InputState.OP:
                // + > / > * > -
                opType = opType == OpType.ADD ? OpType.DIV :
                    (opType == OpType.DIV ? OpType.MUL :
                    (opType == OpType.MUL ? OpType.SUB : OpType.ADD));

                foreach (CalcDisplay calcDisplay in calcDisplays)
                    calcDisplay.display[(int)inputState].text =
                        opType == OpType.ADD ? "+" :
                        (opType == OpType.SUB ? "-" :
                        (opType == OpType.MUL ? "*" : "/"));
                break;

            case InputState.VAL2:
                switch (v2Type)
                {
                    case InputType.NUM:
                        if (v2Num > -999)
                        {
                            v2Num--;
                            foreach (CalcDisplay calcDisplay in calcDisplays)
                                calcDisplay.display[(int)inputState].text = v2Num.ToString();
                        }
                        break;

                    case InputType.VAR:
                        v2Key--;
                        if (v2Key < 0) v2Key = varNames.Count - 1;

                        foreach (CalcDisplay calcDisplay in calcDisplays)
                            calcDisplay.display[(int)inputState].text = varNames[v2Key];
                        break;
                }
                break;


            case InputState.TARG:
                targKey--;
                if (targKey < 0) targKey = varNames.Count - 1;

                foreach (CalcDisplay calcDisplay in calcDisplays)
                    calcDisplay.display[(int)inputState].text = varNames[targKey];
                break;
        }
    }

    public override void nextVal()
    {
        switch (inputState)
        { //rework Storage?
            case InputState.VAL1:
                switch (v1Type)
                {
                    case InputType.NUM:
                        if (v1Num < 999)
                        {
                            v1Num++;
                            foreach (CalcDisplay calcDisplay in calcDisplays)
                                calcDisplay.display[(int)inputState].text = v1Num.ToString();
                        }
                        break;

                    case InputType.VAR:
                        v1Key++;
                        if (v1Key >= varNames.Count) v1Key = 0;

                        foreach (CalcDisplay calcDisplay in calcDisplays)
                            calcDisplay.display[(int)inputState].text = varNames[v1Key];
                        break;
                }
                break;

            case InputState.OP:
                // + > - > * > /
                opType = opType == OpType.ADD ? OpType.SUB :
                    (opType == OpType.SUB ? OpType.MUL :
                    (opType == OpType.MUL ? OpType.DIV : OpType.ADD));

                foreach (CalcDisplay calcDisplay in calcDisplays)
                    calcDisplay.display[(int)inputState].text =
                        opType == OpType.ADD ? "+" :
                        (opType == OpType.SUB ? "-" :
                        (opType == OpType.MUL ? "*" : "/"));
                break;

            case InputState.VAL2:
                switch (v2Type)
                {
                    case InputType.NUM:
                        if (v2Num < 999)
                        {
                            v2Num++;
                            foreach (CalcDisplay calcDisplay in calcDisplays)
                                calcDisplay.display[(int)inputState].text = v2Num.ToString();
                        }
                        break;

                    case InputType.VAR:
                        v2Key++;
                        if (v2Key >= varNames.Count) v2Key = 0;

                        foreach (CalcDisplay calcDisplay in calcDisplays)
                            calcDisplay.display[(int)inputState].text = varNames[v2Key];
                        break;
                }
                break;

            case InputState.TARG:
                targKey++;
                if (targKey >= varNames.Count) targKey = 0;

                foreach (CalcDisplay calcDisplay in calcDisplays)
                    calcDisplay.display[(int)inputState].text = varNames[targKey];
                break;
        }
    }

    IEnumerator delayedHold(KeyCode keyCode)
    {
        bool cancel = false;
        for(int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.15f);
            if (!Input.GetKey(keyCode)) cancel = true;
        }
        yield return new WaitForSeconds(0.15f);
        while(!cancel)
        {
            if (Input.GetKey(keyCode))
            {
                if (keyCode == KeyCode.Q) prevVal();
                else nextVal();
            }
            else break;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void updateVarNames()
    {
        ref Storage storage = ref routine.getStorage();
        varNames = new List<string>();
        foreach (string key in storage.listNames())
        {
            varNames.Add(key);
        }
        foreach (string s in Storage.varNameArray)
        {
            if (!varNames.Contains(s))
            {
                varNames.Add(s);
                break;
            }
        }
    }

    public override void changeInput()
    {
        foreach (CalcDisplay calcDisplay in calcDisplays) calcDisplay.activeBg[(int)inputState].SetActive(false);
        inputState = inputState == InputState.VAL1 ? InputState.OP : 
            (inputState == InputState.OP ? InputState.VAL2 :
            (inputState == InputState.VAL2 ? InputState.TARG : InputState.VAL1));
        foreach (CalcDisplay calcDisplay in calcDisplays) calcDisplay.activeBg[(int)inputState].SetActive(true);
        updateVarNames();
    }

    public override void changeValType()
    {
        if (inputState == InputState.TARG || inputState == InputState.OP) return;
        else if (inputState == InputState.VAL1)
        {
            if (v1Type == InputType.NUM)
            {
                v1Type = InputType.VAR;
                foreach (CalcDisplay calcDisplay in calcDisplays)
                    calcDisplay.typeDisplay1.text = "V";

                updateVarNames();
                foreach (CalcDisplay calcDisplay in calcDisplays)
                    calcDisplay.display[(int)inputState].text = varNames[v1Key];
            }
            else if (v1Type == InputType.VAR)
            {
                v1Type = InputType.NUM;
                foreach (CalcDisplay calcDisplay in calcDisplays)
                    calcDisplay.typeDisplay1.text = "N";

                foreach (CalcDisplay calcDisplay in calcDisplays)
                    calcDisplay.display[(int)inputState].text = v1Num.ToString();
            }
        }
        else if (inputState == InputState.VAL2)
        {
            if (v2Type == InputType.NUM)
            {
                v2Type = InputType.VAR;
                foreach (CalcDisplay calcDisplay in calcDisplays)
                    calcDisplay.typeDisplay2.text = "V";

                updateVarNames();
                foreach (CalcDisplay calcDisplay in calcDisplays)
                    calcDisplay.display[(int)inputState].text = varNames[v2Key];
            }
            else if (v1Type == InputType.VAR)
            {
                v1Type = InputType.NUM;
                foreach (CalcDisplay calcDisplay in calcDisplays)
                    calcDisplay.typeDisplay2.text = "N";

                foreach (CalcDisplay calcDisplay in calcDisplays)
                    calcDisplay.display[(int)inputState].text = v2Num.ToString();
            }
        }
    }
}
