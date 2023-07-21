using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMarker : Action
{
    public string color = "Red";

    public override void act(GameObject go)
    {
        //Act means it is supposed to be this block
        //If it were not it would have been swapped out
        //RESET

        //ACT
        GameObject.Find("Marker" + color).transform.position = go.transform.position;

        //GOAL
        done = true;
    }
}
