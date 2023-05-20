using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : Action
{
    public override void act(GameObject go)
    {
        go.transform.position += go.transform.forward * 0.05f;
    }
}
