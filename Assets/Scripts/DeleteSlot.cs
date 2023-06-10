using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeleteSlot : MonoBehaviour, IDropHandler
{

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
        eventData.pointerDrag.GetComponent<BlockUI>().registerDrop(null);
        Destroy(eventData.pointerDrag, 0.01f);
    }

}
