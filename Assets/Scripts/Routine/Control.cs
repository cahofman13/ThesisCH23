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
    public (bool, Action) appoint(ref Storage storage)
    {
        calculate(ref storage);
        if (!finished)
        {
            setUIactiveControl();
            return (false, currentAction);
        }
        else
        {
            setUIinactiveControl();
            reset();
            return (true, currentAction);
        }
    }

    internal virtual void calculate(ref Storage storage)
    {
        finished = true;
    }

    internal virtual void reset()
    {
        finished = false;
        currentAction = new Action();
    }
}
