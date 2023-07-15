using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DroneInterface : MonoBehaviour
{
    [SerializeField] Node start;
    public Process getProcess()
    {
        if (start.next != null) return getProcess(start.next.end.GetComponent<Node>());
        else return new Process();
    }

    private Node getFirstNodeFromIntern(Node controlNode)
    {
        if (controlNode.intern)
            if(controlNode.intern.end.TryGetComponent(out Node n)) return n;
        return null;
    }

    private Process getProcess(Node startNode)
    {
        if(!startNode) { Debug.Log("Empty Start Node, Creating Empty Process."); return new Process(); }

        Process process = new Process();
        Node node = startNode;

        while (true)
        {
            //REGISTER ALL BLOCKS HERE
            switch (node.blockName)
            {
                //-----------------------ACTIONS-----------------------------
                case "MoveForward": process.addBlock(new MoveForward(), node); break;
                case "TurnRight": process.addBlock(new TurnRight(), node); break;
                case "TurnLeft": process.addBlock(new TurnLeft(), node); break;
                case "Grab": process.addBlock(new Grab(), node); break;
                case "Release": process.addBlock(new Release(), node); break;
                case "SetMarker":
                    SetMarker setMarker = new SetMarker();
                    setMarker.color = ((MarkerNode)node).getColorName();
                    process.addBlock(setMarker, node);
                    break;
                case "FlyToMarker":
                    FlyToMarker flyToMarker = new FlyToMarker();
                    flyToMarker.color = ((MarkerNode)node).getColorName();
                    process.addBlock(flyToMarker, node);
                    break;
                    
                //-----------------------CONTROLS----------------------------
                case "ForBlock":
                    ForBlock forBlock = new ForBlock();
                    forBlock.process = getProcess(getFirstNodeFromIntern(node));
                    forBlock.executionTarget = ((ForNode)node).repeatNr;
                    process.addBlock(forBlock, node);
                    break;
                case "IfBlock":
                    IfBlock ifBlock = new IfBlock();
                    ifBlock.process = getProcess(getFirstNodeFromIntern(node));
                    ifBlock.condition = new Condition(((IfNode)node).condition);
                    process.addBlock(ifBlock, node);
                    break;
                case "WhileBlock":
                    WhileBlock whileBlock = new WhileBlock();
                    whileBlock.process = getProcess(getFirstNodeFromIntern(node));
                    whileBlock.condition = new Condition(((IfNode)node).condition);
                    process.addBlock(whileBlock, node);
                    break;
                    /*
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

            if (node.next != null && node.next.end.TryGetComponent(out Node n) && n.blockName != "Start") node = n;
            else break;

        }   //End of While

        return process;
    }
}
