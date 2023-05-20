using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : Block
{
    public bool checkDone()
    {
        return true;
    }

    public virtual void act(GameObject go)
    {
        return;
    }
}
// Sollen Actions nur auf bestimmte Dronen angewandt werden (aka FlyActions & DriveActions?)
// Oder sollen spezifische Actions mit go.GetComponent<Executor>() bestimmt werden.
