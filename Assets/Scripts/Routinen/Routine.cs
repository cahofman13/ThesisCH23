using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Routine : MonoBehaviour
{
    public bool paused = false;
    public Process process;
    Storage storage = new Storage();

    void Test1()
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
    void Test2()
    {
        process = new Process();

        Process whileProcess = new Process();
        whileProcess.addBlock(new MoveForward());
        WhileBlock whileBlock = new WhileBlock();
        whileBlock.process = whileProcess;
        whileBlock.condition = new Condition(-1, "posZ", null, null, 10f);
        process.addBlock(whileBlock);
        process.addBlock(new TurnRight());
        process.addBlock(new TurnRight()); 

        Process whileProcess2 = new Process();
        whileProcess2.addBlock(new MoveForward());
        WhileBlock whileBlock2 = new WhileBlock();
        whileBlock2.process = whileProcess2;
        whileBlock2.condition = new Condition(1, "posZ", null, null, -1f);
        process.addBlock(whileBlock2);
        process.addBlock(new TurnRight());
        process.addBlock(new TurnRight());
    }
    void Test3()
    {
        process = new Process();

        Process whileProcess = new Process();
        whileProcess.addBlock(new MoveForward());
        WhileBlock whileBlock = new WhileBlock();
        whileBlock.process = whileProcess;
        whileBlock.condition = new Condition(-1, "posZ", null, null, 10f);
        process.addBlock(whileBlock);
        process.addBlock(new TurnRight());
        process.addBlock(new TurnRight());

        Operation operation = new Operation();
        operation.setOpNone("a", null, "forward");
        process.addBlock(operation);

        Process whileProcess2 = new Process();
        whileProcess2.addBlock(new MoveForward());
        IfBlock ifBlock = new IfBlock();
        Process ifProcess = new Process();
        Operation operation2 = new Operation();
        operation2.setOpNone("a", null, "");
        ifProcess.addBlock(operation2);
        ifBlock.process = ifProcess;
        ifBlock.condition = new Condition(-1, "posZ", null, null, -1f);
        whileProcess2.addBlock(ifBlock);
        WhileBlock whileBlock2 = new WhileBlock();
        whileBlock2.process = whileProcess2;
        whileBlock2.condition = new Condition(0, "a", null, null, "forward");
        process.addBlock(whileBlock2);
        process.addBlock(new TurnRight());
        process.addBlock(new TurnRight());
    }

    // Start is called before the first frame update
    void Start()
    {
        Test3();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        updateStorage();
        if (!process.IsUnityNull() && !paused) actProcess();
    }

    public ref Storage getStorage()
    {
        return ref storage;
    }

    //Auto-Updating Variables
    private void updateStorage()
    {
        storage.writeValue("posX", transform.position.x);
        storage.writeValue("posY", transform.position.y);
        storage.writeValue("posZ", transform.position.z);

    }

    private void actProcess()
    {
        process.compute(ref storage);

        try
        {
            process.currentAction.act(gameObject);
        } catch (NullReferenceException) {
            //Debug.Log("NoActionTaken");
        }
    }

    public void setProcess(Process proc)
    {
        storage = new Storage();  //??? KEEP HERE ???
        process = proc;
    }


    public void togglePaused()
    {
        paused = !paused;
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
