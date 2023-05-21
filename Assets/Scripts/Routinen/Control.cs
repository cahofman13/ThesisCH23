using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : Block
{
    internal bool finished = false;
    internal Action currentAction = new Action();

    /// <summary>
    /// delegates process
    /// </summary>
    /// <param name="go"></param>
    /// <returns>bool: IsControlFinished</returns>
    public (bool, Action) appoint()
    {
        calculate();
        if (!finished)
        {
            return (false, currentAction);
        }
        else
        {
            reset();
            return (true, currentAction);
        }
    }

    internal virtual void calculate()
    {
        finished = true;
    }

    internal virtual void reset()
    {
        finished = false;
        currentAction = new Action();
    }
}
