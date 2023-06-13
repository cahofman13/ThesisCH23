using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockSlot : MonoBehaviour, IDropHandler
{
    public int index = 1;

    //BlockUI
    public GameObject droppedBlock;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDrop(PointerEventData eventData)  //ISSUESS FIXXXXXX PLSSSS NOWWWWN :-(((((((((((((((((((((
    {
        //noChange!
        if (droppedBlock == eventData.pointerDrag) return;

        GameObject oldBlock = droppedBlock;

        droppedBlock = eventData.pointerDrag;
        //THIS ORDER IS IMPORTANT IT INTERACTS DIRECTLY WITH THE BLOCK_UI
        droppedBlock.GetComponent<RectTransform>().position = this.GetComponent<RectTransform>().position;
        droppedBlock.GetComponent<BlockUI>().registerDrop(this);

        {
            transform.parent.GetComponent<RoutineUI>().blockAdded(oldBlock, index);
        }
    }

    public void unsetBlock()
    {
        droppedBlock = null;
        transform.parent.GetComponent<RoutineUI>().blockRemoved();
    }

    public void updateBlockPosition()
    {
        if (droppedBlock == null) return;
        droppedBlock.GetComponent<RectTransform>().position = this.GetComponent<RectTransform>().position;
        droppedBlock.GetComponent<BlockUI>().updateSlot(this);
    }

}
