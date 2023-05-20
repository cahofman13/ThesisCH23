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
