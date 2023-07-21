using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfBlock : Control
{
    public Process process = new Process();
    public Condition condition = new Condition();
    bool inProcess = false;
    bool firstEntry = true;

    internal override void calculate(ref Storage storage)
    {
        if (firstEntry)
        {
            if (condition.evalBool(ref storage)) inProcess = true;
            firstEntry = false;
        }
        if (!inProcess)
        {
            finished = true;
            return;
        }

        if(process.compute(ref storage)) inProcess = false;
        currentAction = process.currentAction;
    }

    internal override void reset()
    {
        firstEntry = true;
        inProcess = false;
        base.reset();
    }
}
