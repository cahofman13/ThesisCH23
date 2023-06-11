using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoutineUI : MonoBehaviour
{
    [SerializeField]
    private GameObject blockSlotPrefab;

    bool needReorder = false;

    // Start is called before the first frame update
    void Start()
    {
        
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
            GameObject blockUI = child.GetComponent<BlockSlot>().droppedBlock;
            if (!blockUI) Debug.Log("ReadEmpty :)");
            else
            {
                string blockType = blockUI.GetComponent<BlockUI>().blockName;

                //REGISTER ALL BLOCKS HERE
                switch (blockType)
                {
                    //-----------------------ACTIONS-----------------------------
                    case "MoveForward": process.addBlock(new MoveForward()); break;
                    case "TurnRight": process.addBlock(new TurnRight()); break;

                    //-----------------------CONTROLS----------------------------
                    case "ForBlock":
                        ForBlock block = new ForBlock();
                        block.process = getProcessFromBlock(blockUI);
                        block.executionTarget = 5;//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! 
                        process.addBlock(block);
                        break;

                    //-----------------------OPERATIONS--------------------------
                    case "Operation": Debug.LogWarning("OPERATION not implemented"); break; //splitted here in READ + WRITE later
                    case "Condition": Debug.LogWarning("CONDITION not implemented"); break;

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
            GameObject child = blockUI.transform.GetChild(0).gameObject;
            bool suc = child.TryGetComponent(out RoutineUI routineUI);
            if (suc) process = routineUI.gather();
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
        GameObject newSlot = Instantiate(blockSlotPrefab, transform);
        newSlot.GetComponent<BlockSlot>().index = transform.childCount;
        newSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(150, -80) + (new Vector2(0, -120) * (transform.childCount - 1));
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
        Vector2 pos = new Vector2(150, -80);
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
                child.GetComponent<RectTransform>().anchoredPosition = pos;
                child.GetComponent<BlockSlot>().updateBlockPosition();
                child.GetComponent<BlockSlot>().index = i;
                pos += new Vector2(0, -120);
                i++;
            }
        }
        GameObject newSlot = Instantiate(blockSlotPrefab, transform);
        newSlot.GetComponent<BlockSlot>().index = transform.childCount - disabledChilds;
        newSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(150, -80) + (new Vector2(0, -120) * (transform.childCount - 1 - disabledChilds));

        needReorder = false;
    }

}