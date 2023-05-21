using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Routine : MonoBehaviour
{
    bool paused = false;
    Process process;

    // Start is called before the first frame update
    void Start()
    {
        process = new Process();
        Process forProcess = new Process();
        forProcess.addBlock(new MoveForward());
        ForBlock forBlock = new ForBlock();
        forBlock.process = forProcess;
        forBlock.executionTarget = 100;
        process.addBlock(forBlock);
        //for (int i = 0; i < 100; i++)
        //    process.addBlock(new MoveForward());
        process.addBlock(new TurnRight());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (!process.IsUnityNull() && !paused) actProcess();
    }

    private void actProcess()
    {
        process.compute();

        Action action = null;
        try
        {
            action = process.currentAction;
        } catch (Exception e) {
            Debug.LogError(e);
        }
        action.act(gameObject);
    }
}

/*
 * [Process]
 * [Block] [Action] forward
 * [Block] increase
 * [Block] [Control] if
 *      [Process1] 
 *      [Block] [Action] jump
 *      [Block] [Action] jump
 * [Block] [Action] backwards
 * 
 * 
 * 
 * 1. Delegation or Link Blocks
 * 2. Interpret or Compile
 * 
 * Delegation! Bringe dem Process bei mit einem Process umzugehen <<<<--------
 * 
 * 
 * ODER 
 * Fake-Sprung-Block + Return-Methode
 * ODER
 * Einruecken + Return-Methode
 * 
*/
