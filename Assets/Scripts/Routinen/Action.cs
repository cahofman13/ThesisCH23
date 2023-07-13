using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Action : Block
{

    internal bool done = true;

    public bool checkDone()
    {
        if (!done)
        {
            setUIactive();
            return false;
        }
        else
        {
            setUIinactive();
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
