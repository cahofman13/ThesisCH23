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


    public bool compute(ref Storage storage)
    {
        if (blocks.Count == 0) return true;

        if(currentAction != null && !currentAction.checkDone()) 
            return false;

        switch(blocks[currentBlock])
        {
            case Action: computeAction(); break;
            case Operation: computeOperation(ref storage); break;
            case Control: computeControl(ref storage); break;

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

    private void computeOperation(ref Storage storage)
    {
        ((Operation)blocks[currentBlock]).calculate(ref storage);
        currentAction = new Action();
        currentBlock++;
    }

    private void computeControl(ref Storage storage)
    {
        (bool, Action) tFinishedAction = ((Control)blocks[currentBlock]).appoint(ref storage);
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

    public void addBlock(Block block, Node node)
    {
        addBlock(block);
        block.setUIblock(node);
    }

}
