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

    internal void setUIactive() 
    {
        if(node) node.setBlockActive();
    }

    internal void setUIinactive()
    {
        if (node) node.setBlockInactive();
    }

    internal void setUIinternal()
    {
        if (node) node.setBlockInternal();
    }

    public void setUIblock(Node node)
    {
        this.node = node;
    }
}
