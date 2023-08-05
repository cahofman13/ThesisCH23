using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class ForNode : Node
{
    NumberDisplay[] numberDisplays;

    public int repeatNr { get; internal set; } = 2;

    internal override void exStart()
    {
        base.exStart();
        isControl = true;
        numberDisplays = this.GetComponentsInChildren<NumberDisplay>();
    }

    internal override void exUpdate()
    {
        base.exUpdate();
        if (isDragged && Input.GetKeyDown(KeyCode.Q)) { prevVal(); StartCoroutine(delayedHold(KeyCode.Q)); }
        if (isDragged && Input.GetKeyDown(KeyCode.E)) { nextVal(); StartCoroutine(delayedHold(KeyCode.E)); }
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
            foreach (NumberDisplay numberDisplay in numberDisplays) numberDisplay.activeBg.SetActive(dragged);
        }
        catch (NullReferenceException)
        {
            numberDisplays = this.GetComponentsInChildren<NumberDisplay>();
            foreach (NumberDisplay numberDisplay in numberDisplays) numberDisplay.activeBg.SetActive(dragged);
        }
    }

    public override void prevVal()
    {
        if (repeatNr > 0) repeatNr--;
        foreach (NumberDisplay numberDisplay in numberDisplays) numberDisplay.text.text = repeatNr.ToString();
    }

    public override void nextVal()
    {
        if (repeatNr < 999) repeatNr++;
        foreach (NumberDisplay numberDisplay in numberDisplays) numberDisplay.text.text = repeatNr.ToString();
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
}
