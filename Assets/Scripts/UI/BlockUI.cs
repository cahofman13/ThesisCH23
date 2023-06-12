using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockUI : MonoBehaviour, IInitializePotentialDragHandler, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    RectTransform rTransform;
    CanvasGroup blocksGroup;
    CanvasGroup canvasGroup;

    //Dragging
    internal bool firstDrag = true;
    bool dropped = false;
    //Anchor
    Vector2 startPoint;
    public BlockSlot slot = null;

    //Identifier that determines the created block
    public string blockName = "";


    // Start is called before the first frame update
    void Start()
    {
        startup();
    }

    //Extendable Start Method
    internal virtual void startup()
    {
        rTransform = this.GetComponent<RectTransform>();
        blocksGroup = transform.parent.GetComponent<CanvasGroup>();
        canvasGroup = this.GetComponent<CanvasGroup>();

        startPoint = rTransform.anchoredPosition;
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        blocksGroup.blocksRaycasts = false;
        try
        {
            slot.unsetBlock();
            slot = null;
        } catch (NullReferenceException)
        {   //THAT IS FINE
            Debug.Log("NoSlotYet");
        }
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector3 hitPos = eventData.pointerCurrentRaycast.worldPosition;
        if (hitPos == new Vector3(0, 0, 0))
            canvasGroup.alpha = 0;
        else
        {
            rTransform.position = hitPos;
            canvasGroup.alpha = 1;
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        //Reset Drag Effects
        canvasGroup.alpha = 1;
        blocksGroup.blocksRaycasts = true;

        if (!dropped)
        {
            if (firstDrag) resetPosition();
            else Destroy(gameObject, 0.005f);
        }
        else dropped = false;
    }

    public virtual void registerDrop(BlockSlot newSlot)
    {
        //Reset Drag Effects early for Clone
        canvasGroup.alpha = 1;
        blocksGroup.blocksRaycasts = true;

        //Clone if new
        if (firstDrag)
        {
            GameObject clone = Instantiate(gameObject, transform.parent);
            clone.GetComponent<RectTransform>().anchoredPosition = startPoint;
            firstDrag = false;
        }

        //Set new DropLocation
        dropped = true;
        startPoint = rTransform.anchoredPosition;
        slot = newSlot;
    }

    public void updateLocationSlot(BlockSlot bs)
    {
        startPoint = rTransform.anchoredPosition;
        slot = bs;
    }

    private void resetPosition()
    {
        rTransform.anchoredPosition = startPoint;
    }
}
