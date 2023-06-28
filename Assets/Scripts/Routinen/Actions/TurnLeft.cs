using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLeft : Action
{
    int i = 0;
    bool done = false;

    public override bool checkDone()
    {
        if (!done) return false;
        else
        {
            i = 0;
            done = false;
            return true;
        }
    }

    public override void act(GameObject go)
    {
        go.transform.Rotate(go.transform.up, -1);
        i++;
        if (i == 90) done = true;
    }
}
