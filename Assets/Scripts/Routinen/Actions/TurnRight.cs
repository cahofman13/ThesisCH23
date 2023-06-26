using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnRight : Action
{
    public override void act(GameObject go)
    {
        go.transform.Rotate(go.transform.up, 90);
    }
}
