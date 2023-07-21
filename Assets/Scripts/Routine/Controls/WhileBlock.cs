using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhileBlock : Control
{
    public Process process = new Process();
    public Condition condition = new Condition();
    bool inProcess = false;

    internal override void calculate(ref Storage storage)
    {
        if(!inProcess)
        {
            if (condition.evalBool(ref storage)) inProcess = true;
            else
            {
                finished = true;
                return;
            }
        }

        if(process.compute(ref storage)) inProcess = false;
        currentAction = process.currentAction;
    }

    internal override void reset()
    {
        inProcess = false;
        base.reset();
    }
}
