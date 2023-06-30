using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    // IM EMPTY :)  
    //// Not for Long...
    // :O 
    // RIP empty 15.05.2023 - 30.06.2023

    BlockUI blockUI;

    internal void setUIactive() 
    {
        if(blockUI) blockUI.setBlockActive();
    }

    internal void setUIinactive()
    {
        if (blockUI) blockUI.setBlockInactive();
    }

    public void setUIblock(BlockUI blockUI)
    {
        this.blockUI = blockUI;
    }
}
