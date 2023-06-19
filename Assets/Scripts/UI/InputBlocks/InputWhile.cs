using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputWhile : InputIf
{
    internal override void startup()
    {
        base.startup();
        blockName = "WhileBlock";
    }
}
