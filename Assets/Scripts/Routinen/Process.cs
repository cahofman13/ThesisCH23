using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Process
{
    public Action currentAction { get; private set; }

    List<Block> blocks = new List<Block>();
    int currentBlock = 0;

    public void compute()
    {
        if(currentAction != null && !currentAction.checkDone()) 
            return ;

        currentBlock++;
        if (blocks.Count <= currentBlock) resetIteration();

        switch(blocks[currentBlock])
        {
            case Action: computeAction(); break;
            case Operation: computeOperation(); break;
            case Control: computeControl(); break;

            default: break;
        }
    }

    private void computeAction()
    {
        try
        {
            currentAction = (Action)blocks[currentBlock];
        } catch (Exception e) {
            Debug.LogError("Encountered Error while trying to read Action, block may not be an Action:\n" + e);
        }
    }

    private void computeOperation()
    {

    }

    private void computeControl()
    {

    }

    public void resetIteration()
    {
        currentBlock = 0;
    }

    public void addBlock(Block block)
    {
        blocks.Add(block);
    }



}
