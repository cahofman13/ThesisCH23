using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLeft : Action
{
    int i = 0;

    public override void act(GameObject go)
    {
        //Act means it is supposed to be this block
        //If it were not it would have been swapped out
        //RESET
        if (done)
        {
            i = 0;
            done = false;
        }

        //ACT
        go.transform.Rotate(go.transform.up, -1);
        i++;

        //GOAL
        if (i == 90) done = true;
    }
}
