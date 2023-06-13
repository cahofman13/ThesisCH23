using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockUI : MonoBehaviour, IInitializePotentialDragHandler, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static bool raycastEnabled = true;

    public static GameObject canvas;

    RectTransform rTransform;
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
        if (!canvas) canvas = GameObject.FindGameObjectWithTag("Canvas");
        startup();
    }

    //Extendable Start Method
    internal virtual void startup()
    {
        rTransform = this.GetComponent<RectTransform>();
        canvasGroup = this.GetComponent<CanvasGroup>();

        startPoint = rTransform.anchoredPosition;
    }

    void Update()
    {
        if (raycastEnabled) canvasGroup.blocksRaycasts = true;
        else canvasGroup.blocksRaycasts = false;
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
        raycastEnabled = false;

        try
        {
            slot.unsetBlock();
            slot = null;
        }
        catch (NullReferenceException)
        {   //THAT IS FINE
            Debug.Log("NoSlotYet");
        }

        transform.SetParent(canvas.transform);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector3 hitPos = eventData.pointerCurrentRaycast.worldPosition;
        if (hitPos == new Vector3(0, 0, 0))
            canvasGroup.alpha = 0;
        else
        {
            rTransform.position = hitPos + new Vector3(0, 0, 0.1f);
            canvasGroup.alpha = 1;
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        //Reset Drag Effects
        canvasGroup.alpha = 1;
        raycastEnabled = true;

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
        raycastEnabled = true;

        //Clone if new
        if (firstDrag)
        {
            GameObject clone = Instantiate(gameObject, transform.parent);
            clone.GetComponent<RectTransform>().anchoredPosition = startPoint;
            firstDrag = false;
        }

        //Set new DropLocation
        dropped = true;
        slot = newSlot;
        transform.SetParent(newSlot.transform);
    }

    public void updateSlot(BlockSlot bs)
    {
        slot = bs;
        transform.SetParent(bs.transform);
    }

    private void resetPosition()
    {
        rTransform.anchoredPosition = startPoint;
    }
}
