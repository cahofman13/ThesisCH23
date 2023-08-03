using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class WriteNode : Node
{
    WriteDisplay[] writeDisplays;

    int v1Key = 0;
    float v1Num = 3;

#pragma warning disable CS8632
    public (string?,float?) getVal1() { return (v1Type == InputType.VAR ? new(varNames[v1Key], null) : new(null, v1Num)); }
#pragma warning restore CS8632

    int targKey = 3;
    public string getTarget() { return varNames[targKey]; }

    InputState inputState = InputState.VAL1;
    InputType v1Type = InputType.NUM;

    Routine routine;
    List<string> varNames;

    enum InputState
    {
        VAL1,
        TARG
    }

    enum InputType
    {
        VAR,
        NUM
    }

    public override void resetAfterClone()
    {
        base.resetAfterClone();
        v1Key = 0;
        v1Num = 3;
        targKey = 3;
        inputState = InputState.VAL1;
        v1Type = InputType.NUM;
        routine = null;
        varNames = null;
        try
        {
            routine = this.GetComponentInParent<DroneInterface>().gameObject
                .GetComponent<PositionConstraint>().GetSource(0).sourceTransform.GetComponent<Routine>(); //rework Storage?!
        }
        catch (NullReferenceException e) { Debug.LogWarning(e.Message); }
        updateVarNames();
        //Toggle to auto-sync
        //1
        nextVal(); prevVal();
        changeValType();
        nextVal(); prevVal();
        changeValType();
        changeInput();
        //2
        nextVal(); prevVal();
        changeInput();
    }

    internal override void exStart()
    {
        base.exStart();
        writeDisplays = this.GetComponentsInChildren<WriteDisplay>();
        try
        {
            routine = this.GetComponentInParent<DroneInterface>().gameObject
                .GetComponent<PositionConstraint>().GetSource(0).sourceTransform.GetComponent<Routine>(); //rework Storage?
            updateVarNames();
        }
        catch (Exception e) { Debug.Log("Error when getting storage\n" + e); }
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
        try
        {
            foreach (WriteDisplay writeDisplay in writeDisplays) writeDisplay.activeBg[(int)inputState].SetActive(dragged);
        }
        catch (NullReferenceException)
        {
            writeDisplays = this.GetComponentsInChildren<WriteDisplay>();
            foreach (WriteDisplay writeDisplay in writeDisplays) foreach(GameObject bg in writeDisplay.activeBg) bg.SetActive(dragged);
        }
    }

    public override void setNodeHUD(bool enter, Transform newParent)
    {
        base.setNodeHUD(enter, newParent);
        try
        {
            routine = this.GetComponentInParent<DroneInterface>().gameObject
                .GetComponent<PositionConstraint>().GetSource(0).sourceTransform.GetComponent<Routine>(); //rework Storage?
            updateVarNames();
        }
        catch (Exception e)
        {
            Debug.Log("Error when getting storage\n" + e);
        }
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
                            foreach (WriteDisplay writeDisplay in writeDisplays)
                                writeDisplay.display[(int)inputState].text = v1Num.ToString();
                        }
                        break;

                    case InputType.VAR:
                        v1Key--;
                        if (v1Key < 0) v1Key = varNames.Count - 1;

                        foreach (WriteDisplay writeDisplay in writeDisplays)
                            writeDisplay.display[(int)inputState].text = varNames[v1Key];
                        break;
                }
                break;

            case InputState.TARG:
                targKey--;
                if (targKey < 0) targKey = varNames.Count - 1;

                foreach (WriteDisplay writeDisplay in writeDisplays)
                    writeDisplay.display[(int)inputState].text = varNames[targKey];
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
                            foreach (WriteDisplay writeDisplay in writeDisplays)
                                writeDisplay.display[(int)inputState].text = v1Num.ToString();
                        }
                        break;

                    case InputType.VAR:
                        v1Key++;
                        if (v1Key >= varNames.Count) v1Key = 0;

                        foreach (WriteDisplay writeDisplay in writeDisplays)
                            writeDisplay.display[(int)inputState].text = varNames[v1Key];
                        break;
                }
                break;

            case InputState.TARG:
                targKey++;
                if (targKey >= varNames.Count) targKey = 0;

                foreach (WriteDisplay writeDisplay in writeDisplays)
                    writeDisplay.display[(int)inputState].text = varNames[targKey];
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
        if (!routine) return;
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
        foreach (WriteDisplay writeDisplay in writeDisplays) writeDisplay.activeBg[(int)inputState].SetActive(false);
        inputState = inputState == InputState.VAL1 ? InputState.TARG : InputState.VAL1;
        foreach (WriteDisplay writeDisplay in writeDisplays) writeDisplay.activeBg[(int)inputState].SetActive(true);
        updateVarNames();
    }

    public override void changeValType()
    {
        if (inputState != InputState.VAL1) return;
        if (v1Type == InputType.NUM)
        {
            v1Type = InputType.VAR;
            foreach (WriteDisplay writeDisplay in writeDisplays)
                writeDisplay.typeDisplay1.text = "V";

            updateVarNames();
            foreach (WriteDisplay writeDisplay in writeDisplays)
                writeDisplay.display[(int)inputState].text = varNames[v1Key];
        }
        else if (v1Type == InputType.VAR)
        {
            v1Type = InputType.NUM;
            foreach (WriteDisplay writeDisplay in writeDisplays)
                writeDisplay.typeDisplay1.text = "N";

            foreach (WriteDisplay writeDisplay in writeDisplays)
                writeDisplay.display[(int)inputState].text = v1Num.ToString();
        }
    }
}
