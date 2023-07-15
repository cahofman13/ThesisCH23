using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class IfNode : Node
{
    ConditionDisplay[] conditionDisplays;

    public Condition condition = new Condition();
    int v1Key = 0;
    int v2Key = 0;
    float v2Num = 3;

    InputState inputState = InputState.VAL1;
    InputType v2Type = InputType.NUM;

    Routine routine;
    List<string> varNames;

    enum InputState
    {
        VAL1,
        COMP,
        VAL2
    }

    enum InputType
    {
        VAR,
        NUM
    }

    internal override void exStart()
    {
        base.exStart();
        isControl = true;
        conditionDisplays = this.GetComponentsInChildren<ConditionDisplay>();
        routine = this.GetComponentInParent<DroneInterface>().gameObject
            .GetComponent<PositionConstraint>().GetSource(0).sourceTransform.GetComponent<Routine>(); //rework Storage?
        updateVarNames();
        condition.setComp(Comp.GREATER, "posX", null, null, 3f);
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
        foreach (ConditionDisplay conditionDisplay in conditionDisplays) conditionDisplay.activeBg[(int)inputState].SetActive(dragged);
    }

    private void prevVal()
    {
        switch (inputState)
        { //rework Storage?
            case InputState.VAL1:
                v1Key--;
                if (v1Key < 0) v1Key = varNames.Count - 1;

                condition.key1 = varNames[v1Key];
                foreach (ConditionDisplay conditionDisplay in conditionDisplays) 
                    conditionDisplay.display[(int)inputState].text = varNames[v1Key];
                break;

            case InputState.COMP:
                // GREATER > EQUAL > LOWER > GREATER
                condition.comp = condition.comp == Comp.GREATER ? Comp.EQUAL : (condition.comp == Comp.EQUAL ? Comp.LOWER : Comp.GREATER);

                foreach (ConditionDisplay conditionDisplay in conditionDisplays) 
                    conditionDisplay.display[(int)inputState].text =
                        condition.comp == Comp.GREATER ? ">" : (condition.comp == Comp.EQUAL ? "=" : "<");
                break;

            case InputState.VAL2: 
                switch (v2Type)
                {
                    case InputType.NUM:
                        if (v2Num > 0)
                        {
                            v2Num--;
                            condition.value2 = v2Num;
                            foreach (ConditionDisplay conditionDisplay in conditionDisplays)
                                conditionDisplay.display[(int)inputState].text = v2Num.ToString();
                        }
                        break;

                    case InputType.VAR:
                        v2Key--;
                        if (v2Key < 0) v2Key = varNames.Count - 1;

                        condition.key2 = varNames[v2Key];
                        foreach (ConditionDisplay conditionDisplay in conditionDisplays)
                            conditionDisplay.display[(int)inputState].text = varNames[v2Key];
                        break;
                }
                break;
        }
    }

    private void nextVal()
    {
        switch (inputState)
        { //rework Storage?
            case InputState.VAL1:
                v1Key++;
                if (v1Key >= varNames.Count) v1Key = 0;

                condition.key1 = varNames[v1Key];
                foreach (ConditionDisplay conditionDisplay in conditionDisplays)
                    conditionDisplay.display[(int)inputState].text = varNames[v1Key];
                break;

            case InputState.COMP:
                // GREATER < EQUAL < LOWER < GREATER
                condition.comp = condition.comp == Comp.GREATER ? Comp.LOWER : (condition.comp == Comp.LOWER ? Comp.EQUAL : Comp.GREATER);

                foreach (ConditionDisplay conditionDisplay in conditionDisplays)
                    conditionDisplay.display[(int)inputState].text =
                        condition.comp == Comp.GREATER ? ">" : (condition.comp == Comp.EQUAL ? "=" : "<");
                break;

            case InputState.VAL2:
                switch (v2Type)
                {
                    case InputType.NUM:
                        if (v2Num < 999)
                        {
                            v2Num++;
                            condition.value2 = v2Num;
                            foreach (ConditionDisplay conditionDisplay in conditionDisplays)
                                conditionDisplay.display[(int)inputState].text = v2Num.ToString();
                        }
                        break;

                    case InputType.VAR:
                        v2Key++;
                        if (v2Key >= varNames.Count) v2Key = 0;

                        condition.key2 = varNames[v2Key];
                        foreach (ConditionDisplay conditionDisplay in conditionDisplays)
                            conditionDisplay.display[(int)inputState].text = varNames[v2Key];
                        break;
                }
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

    private void changeInput()
    {
        foreach (ConditionDisplay conditionDisplay in conditionDisplays) conditionDisplay.activeBg[(int)inputState].SetActive(false);
        inputState = inputState == InputState.VAL1 ? InputState.COMP : (inputState == InputState.COMP ? InputState.VAL2 : InputState.VAL1);
        foreach (ConditionDisplay conditionDisplay in conditionDisplays) conditionDisplay.activeBg[(int)inputState].SetActive(true);
        if(inputState != InputState.COMP) updateVarNames();
    }

    private void changeValType()
    {
        if (inputState != InputState.VAL2) return;
        if (v2Type == InputType.NUM)
        {
            condition.value2 = null;
            v2Type = InputType.VAR;
            foreach (ConditionDisplay conditionDisplay in conditionDisplays)
                conditionDisplay.typeDisplay3.text = "V";

            updateVarNames();
            condition.key2 = varNames[v2Key];
            foreach (ConditionDisplay conditionDisplay in conditionDisplays)
                conditionDisplay.display[(int)inputState].text = varNames[v2Key];
        }
        else if (v2Type == InputType.VAR)
        {
            condition.key2 = null;
            v2Type = InputType.NUM;
            foreach (ConditionDisplay conditionDisplay in conditionDisplays)
                conditionDisplay.typeDisplay3.text = "N";

            condition.value2 = v2Num;
            foreach (ConditionDisplay conditionDisplay in conditionDisplays)
                conditionDisplay.display[(int)inputState].text = v2Num.ToString();
        }
    }
}
