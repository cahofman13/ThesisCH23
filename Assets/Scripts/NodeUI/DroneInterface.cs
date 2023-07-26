using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DroneInterface : MonoBehaviour
{
    [SerializeField] Node start;
    [SerializeField] public InterfaceCollider interfaceCollider;

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
                    
                //-----------------------OPERATIONS--------------------------
                case "WriteBlock":
                    Operation writeBlock = new Operation();
                    WriteNode writeNode = (WriteNode)node;
                    writeBlock.setOpNone(writeNode.getTarget(), writeNode.getVal1().Item1, writeNode.getVal1().Item2);
                    process.addBlock(writeBlock);
                    break;
                case "CalcBlock":
                    Operation calcBlock = new Operation();
                    CalcNode calcNode = (CalcNode)node;
                    if (calcNode.opType == CalcNode.OpType.ADD) calcBlock.setOpAdd(calcNode.getTarget(), calcNode.getVal1().Item1, calcNode.getVal1().Item2, calcNode.getVal2().Item1, calcNode.getVal2().Item2);
                    else if (calcNode.opType == CalcNode.OpType.SUB) calcBlock.setOpSub(calcNode.getTarget(), calcNode.getVal1().Item1, calcNode.getVal1().Item2, calcNode.getVal2().Item1, calcNode.getVal2().Item2);
                    else if (calcNode.opType == CalcNode.OpType.MUL) calcBlock.setOpMult(calcNode.getTarget(), calcNode.getVal1().Item1, calcNode.getVal1().Item2, calcNode.getVal2().Item1, calcNode.getVal2().Item2);
                    else calcBlock.setOpDiv(calcNode.getTarget(), calcNode.getVal1().Item1, calcNode.getVal1().Item2, calcNode.getVal2().Item1, calcNode.getVal2().Item2);
                    process.addBlock(calcBlock);
                    break;

                default: Debug.LogWarning("Reading Unidentified Block in UI"); break;
            }

            if (node.next != null && node.next.end.TryGetComponent(out Node n) && n.blockName != "Start") node = n;
            else break;

        }   //End of While

        return process;
    }
}
