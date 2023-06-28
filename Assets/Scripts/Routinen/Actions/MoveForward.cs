using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : Action
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
        go.transform.position += go.transform.forward * 0.05f;
        i++;
        if (i == 100) done = true;
    }
}
