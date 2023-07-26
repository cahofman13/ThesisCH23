using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyToMarker : Action
{
    public string color = "Red";
    GameObject marker;

    public override void act(GameObject go)
    {
        //Act means it is supposed to be this block
        //If it were not it would have been swapped out
        //RESET
        if(done)
        {
            done = false;
            marker = GameObject.Find("Marker" + color);
        }

        //ACT
        Vector3 distance = marker.transform.position - go.transform.position;
        if (distance.magnitude >= 0.05f) distance = distance.normalized * 0.05f;
        go.transform.position += distance;

        //GOAL
        if (go.transform.position == marker.transform.position) done = true;
    }
}
