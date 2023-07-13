using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : Action
{
    GrabberModule grabberModule;

    public override void act(GameObject go)
    {
        //RESET
        if (done)
        {
            grabberModule = null;
            done = false;
        }

        //ACT
        if (!grabberModule) 
        {
            //Might ERROR !
            grabberModule = go.GetComponentInChildren<GrabberModule>();
            bool valid = grabberModule.grab();
            if (!valid) done = true;
        }
        else
        {
            if (!grabberModule.inProgress) done = true;
        }
    }
}
