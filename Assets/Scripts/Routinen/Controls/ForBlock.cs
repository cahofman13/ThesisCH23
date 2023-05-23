using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForBlock : Control
{
    public Process process = new Process();
    public int executionTarget = 10;
    int executionCount = 0;

    internal override void calculate(ref Storage storage)
    {
        if(executionCount >= executionTarget)
        {
            finished = true;
            return;
        }

        if(process.compute(ref storage)) executionCount++;
        currentAction = process.currentAction;
    }

    internal override void reset()
    {
        executionCount = 0;
        base.reset();
    }
}
