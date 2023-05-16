using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Process : MonoBehaviour
{
    Action currentAction = null;

    List<Block> blocks;
    Block currentBlock;

    public Action compute()
    {
        if(currentAction && !currentAction.checkDone()) 
            return currentAction;

        blocks[currentBlock++].ev();
    }



}
