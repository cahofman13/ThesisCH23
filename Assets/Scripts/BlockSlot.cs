using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockSlot : MonoBehaviour, IDropHandler
{
    public int index = 1;

    //BlockUI
    public GameObject droppedBlock { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        //if(droppedBlock != null)
        {
            transform.parent.GetComponent<RoutineUI>().blockAdded(droppedBlock, index);
        }

        droppedBlock = eventData.pointerDrag;
        //THIS ORDER IS IMPORTANT IT INTERACTS DIRECTLY WITH THE BLOCK_UI
        droppedBlock.GetComponent<RectTransform>().position = this.GetComponent<RectTransform>().position;
        droppedBlock.GetComponent<BlockUI>().registerDrop(this);
    }

    public void unsetBlock()
    {
        droppedBlock = null;
        transform.parent.GetComponent<RoutineUI>().blockRemove(index); //??????
    }

    public void updateBlockPosition()
    {
        droppedBlock.GetComponent<RectTransform>().position = this.GetComponent<RectTransform>().position;
        droppedBlock.GetComponent<BlockUI>().updateLocation();
    }

}
