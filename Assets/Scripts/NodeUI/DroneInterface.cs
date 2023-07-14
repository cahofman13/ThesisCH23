using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneInterface : MonoBehaviour
{
    [SerializeField] Node start; 

    public Process getProcess()
    {
        Process process = new Process();
        Node node = start;

        while (true)
        {
            if (node.next != null && node.next.end.TryGetComponent(out Node n) && n.blockName != "Start") node = n;
            else break;

            //REGISTER ALL BLOCKS HERE
            switch (node.blockName)
            {
                //-----------------------ACTIONS-----------------------------
                case "MoveForward": process.addBlock(new MoveForward(), node); break;
                case "TurnRight": process.addBlock(new TurnRight(), node); break;
                case "TurnLeft": process.addBlock(new TurnLeft(), node); break;
                case "Grab": process.addBlock(new Grab(), node); break;
                case "Release": process.addBlock(new Release(), node); break;
                /*case "SetMarker":
                    SetMarker setMarker = new SetMarker();
                    setMarker.color = "Red";
                    process.addBlock(setMarker, blockUI);
                    break;
                case "FlyToMarker":
                    FlyToMarker flyToMarker = new FlyToMarker();
                    flyToMarker.color = "Red";
                    process.addBlock(flyToMarker, blockUI);
                    break;

                //-----------------------CONTROLS----------------------------
                case "ForBlock":
                    ForBlock forBlock = new ForBlock();
                    forBlock.process = getProcessFromBlock(blockGO);
                    forBlock.executionTarget = blockGO.GetComponent<InputFor>().executionTarget;//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! 
                    process.addBlock(forBlock);
                    break;
                case "IfBlock":
                    IfBlock ifBlock = new IfBlock();
                    ifBlock.process = getProcessFromBlock(blockGO);
                    ifBlock.condition = new Condition(blockGO.GetComponent<InputIf>().condition);
                    process.addBlock(ifBlock);
                    break;
                case "WhileBlock":
                    WhileBlock whileBlock = new WhileBlock();
                    whileBlock.process = getProcessFromBlock(blockGO);
                    whileBlock.condition = new Condition(blockGO.GetComponent<InputWhile>().condition);
                    process.addBlock(whileBlock);
                    break;

                //-----------------------OPERATIONS--------------------------
                case "WriteBlock":
                    Operation writeBlock = new Operation();
                    InputWrite inputWrite = blockGO.GetComponent<InputWrite>();
                    writeBlock.setOpNone(inputWrite.key, inputWrite.name1, inputWrite.value1);
                    process.addBlock(writeBlock);
                    break;
                case "CalcBlock":
                    Operation calcBlock = new Operation();
                    InputCalc inputCalc = blockGO.GetComponent<InputCalc>();
                    if (inputCalc.Op == "/") calcBlock.setOpDiv(inputCalc.key, inputCalc.name1, inputCalc.value1, inputCalc.name2, inputCalc.value2);
                    else if (inputCalc.Op == "x") calcBlock.setOpMult(inputCalc.key, inputCalc.name1, inputCalc.value1, inputCalc.name2, inputCalc.value2);
                    else if (inputCalc.Op == "-") calcBlock.setOpSub(inputCalc.key, inputCalc.name1, inputCalc.value1, inputCalc.name2, inputCalc.value2);
                    else calcBlock.setOpAdd(inputCalc.key, inputCalc.name1, inputCalc.value1, inputCalc.name2, inputCalc.value2);
                    process.addBlock(calcBlock);
                    break;*/

                default: Debug.LogWarning("Reading Unidentified Block in UI"); break;
            }


        }   //End of While

        return process;
    }
}
