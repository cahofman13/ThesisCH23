using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Release : Action
{
    GrabberModule grabberModule;
    bool done = false;

    public override bool checkDone()
    {
        if (!done) return false;
        else
        {
            grabberModule = null;
            done = false;
            return true;
        }
    }

    public override void act(GameObject go)
    {
        if(!grabberModule) 
        {
            //Might ERROR !
            grabberModule = go.GetComponentInChildren<GrabberModule>();
            bool valid = grabberModule.release();
            if (!valid) done = true;
        }
        else
        {
            if (!grabberModule.inProgress) done = true;
        }
    }
}
