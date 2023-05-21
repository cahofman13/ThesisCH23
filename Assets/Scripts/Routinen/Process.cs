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

    public bool compute()
    {
        if(currentAction != null && !currentAction.checkDone()) 
            return false;

        switch(blocks[currentBlock])
        {
            case Action: computeAction(); break;
            case Operation: computeOperation(); break;
            case Control: computeControl(); break;

            default: break;
        }

        if (blocks.Count <= currentBlock)
        {
            resetIteration();
            return true;
        }

        return false;
    }

    private void computeAction()
    {
        try
        {
            currentAction = (Action)blocks[currentBlock];
        } catch (Exception e) {
            Debug.LogError("Encountered Error while trying to read Action, block may not be an Action:\n" + e);
        }

        currentBlock++;
    }

    private void computeOperation()
    {

    }

    private void computeControl()
    {
        (bool, Action) tFinishedAction = ((Control)blocks[currentBlock]).appoint();
        if(tFinishedAction.Item1) currentBlock++;
        currentAction = tFinishedAction.Item2;
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
