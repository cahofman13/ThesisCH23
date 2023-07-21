using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    // IM EMPTY :)  
    //// Not for Long...
    // :O 
    // RIP empty 15.05.2023 - 30.06.2023

    Node node;

    internal void setUIactiveAction() 
    {
        if(node) node.setActionActive();
    }

    internal void setUIinactiveAction()
    {
        if (node) node.setActionInactive();
    }
    internal void setUIinvalidAction()
    {
        if (node) node.setActionInvalid();
    }

    internal void setUIactiveControl()
    {
        if (node) node.setControlActive();
    }

    internal void setUIinactiveControl()
    {
        if (node) node.setControlInactive();
    }

    public void setUINode(Node node)
    {
        this.node = node;
    }
}
