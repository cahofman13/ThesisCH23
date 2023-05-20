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
