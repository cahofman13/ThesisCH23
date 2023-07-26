using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Release : Action
{
    GrabberModule grabberModule;
    int waitCount = 0;

    public override void act(GameObject go)
    {
        //RESET
        if (done)
        {
            grabberModule = null;
            waitCount = 0;
            done = false;

            grabberModule = go.GetComponentInChildren<GrabberModule>();
            valid = grabberModule.release();
        }

        //ACT
        if (!valid)
        {
            waitCount++;

            //GOAL
            if (waitCount >= 100) done = true;
        }
        else if (!grabberModule.inProgress) done = true;
    }
}
