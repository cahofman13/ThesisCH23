using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : Action
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
        go.transform.position += go.transform.forward * 0.05f;
        i++;

        //GOAL
        if (i == 100) done = true;
    }
}
