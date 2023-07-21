using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Action : Block
{

    internal bool done = true;
    internal bool valid = true;

    public bool checkDone()
    {
        if (!done)
        {
            if (valid) setUIactiveAction();
            else setUIinvalidAction();
            return false;
        }
        else
        {
            setUIinactiveAction();
            return true;
        }
    }

    public virtual void act(GameObject go)
    {
        return;
    }
}
// Sollen Actions nur auf bestimmte Dronen angewandt werden (aka FlyActions & DriveActions?)
// Oder sollen spezifische Actions mit go.GetComponent<Executor>() bestimmt werden.
