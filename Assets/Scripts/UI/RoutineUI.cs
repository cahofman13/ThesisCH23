using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoutineUI : MonoBehaviour
{
    [SerializeField]
    private GameObject blockSlotPrefab;

    bool needReorder = false;

    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (needReorder) reorder();
    }

    public void submit(GameObject drone)
    {
        drone.GetComponent<Routine>().setProcess(gather());
    }

    public Process gather()
    {
        Process process = new Process();

        foreach (Transform child in transform)
        {
            GameObject blockGO = child.GetComponent<BlockSlot>().droppedBlock;
            if (!blockGO) Debug.Log("ReadEmpty :)");
            else
            {
                BlockUI blockUI = blockGO.GetComponent<BlockUI>();

                //REGISTER ALL BLOCKS HERE
                switch (blockUI.blockName)
                {
                    //-----------------------ACTIONS-----------------------------
                    case "MoveForward": process.addBlock(new MoveForward(), blockUI); break;
                    case "TurnRight": process.addBlock(new TurnRight(), blockUI); break;
                    case "TurnLeft": process.addBlock(new TurnLeft(), blockUI); break;
                    case "Grab": process.addBlock(new Grab(), blockUI); break;
                    case "Release": process.addBlock(new Release(), blockUI); break;

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
                        if(inputCalc.Op == "/") calcBlock.setOpDiv(inputCalc.key, inputCalc.name1, inputCalc.value1, inputCalc.name2, inputCalc.value2);
                        else if(inputCalc.Op == "x") calcBlock.setOpMult(inputCalc.key, inputCalc.name1, inputCalc.value1, inputCalc.name2, inputCalc.value2);
                        else if(inputCalc.Op == "-") calcBlock.setOpSub(inputCalc.key, inputCalc.name1, inputCalc.value1, inputCalc.name2, inputCalc.value2);
                        else calcBlock.setOpAdd(inputCalc.key, inputCalc.name1, inputCalc.value1, inputCalc.name2, inputCalc.value2);
                        process.addBlock(calcBlock);
                        break;

                    default: Debug.LogWarning("Reading Unidentified Block in UI"); break;
                }
            }
        }

        return process;
    }

    private Process getProcessFromBlock(GameObject blockUI)
    {
        Process process = new Process();

        //try - catch for Child ?
        {
            RoutineUI routineUI = blockUI.GetComponentInChildren<RoutineUI>();
            if (routineUI != null) process = routineUI.gather();
            else Debug.LogError("No Routine found in Block " + blockUI.name);
        }

        return process;
    }

    public void blockAdded(GameObject shiftBlock, int index)
    {
        for(int i = index; i < transform.childCount; i++)
        {
            BlockSlot slot = transform.GetChild(i).GetComponent<BlockSlot>();
            GameObject temp = slot.droppedBlock;
            slot.droppedBlock = shiftBlock;
            shiftBlock = temp;
            slot.updateBlockPosition();
        }
        //unnecessary because reorder erases?
        GameObject newSlot = Instantiate(blockSlotPrefab, transform);
        newSlot.GetComponent<BlockSlot>().index = transform.childCount;
        //newSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(150, -80) + (new Vector2(0, -120) * (transform.childCount - 1));
        
        needReorder = true;
    }

    public void blockRemoved()
    {
        needReorder = true;
    }

    private void reorder()
    {
        int disabledChilds = 0;
        int i = 1;
        //Vector2 pos = new Vector2(150, -80);
        foreach (Transform child in transform)
        {
            if(child.GetComponent<BlockSlot>().droppedBlock == null)
            {
                child.gameObject.SetActive(false);
                Destroy(child.gameObject, 0.01f);
                disabledChilds++;
            }
            else
            {
                //child.GetComponent<RectTransform>().anchoredPosition = pos;
                BlockSlot slot = child.GetComponent<BlockSlot>();
                slot.updateBlockPosition();
                slot.index = i;
                //pos += new Vector2(0, -120);
                i++;
            }
        }
        GameObject newSlot = Instantiate(blockSlotPrefab, transform);
        int actualChildCount = transform.childCount - disabledChilds;
        newSlot.GetComponent<BlockSlot>().index = actualChildCount;
        //newSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(150, -80) + (new Vector2(0, -120) * (actualChildCount - 1));
        
        needReorder = false;
    }

    public void updatePosition()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<BlockSlot>().updateBlockPosition();
        }
    }
}
