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
    bool firstDrop = true;
    bool dropped = false;
    Vector2 startPoint;

    //Identifier that determines the created block
    public string blockName = "";

    BlockSlot slot = null;

    // Start is called before the first frame update
    void Start()
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        blocksGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
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

    public void OnEndDrag(PointerEventData eventData)
    {
        //Reset Drag Effects
        canvasGroup.alpha = 1;
        blocksGroup.blocksRaycasts = true;

        if (!dropped) resetPosition();
        else dropped = false;
    }

    public void registerDrop(BlockSlot newSlot)
    {
        //Reset Drag Effects early for Clone
        canvasGroup.alpha = 1;
        blocksGroup.blocksRaycasts = true;

        //Clone if new
        if (firstDrop)
        {
            GameObject clone = Instantiate(gameObject, transform.parent);
            clone.GetComponent<RectTransform>().anchoredPosition = startPoint;
            firstDrop = false;
        }
        else slot.unsetBlock(); //Notify Slot otherwise

        //Set new DropLocation
        dropped = true;
        startPoint = rTransform.anchoredPosition;
        slot = newSlot;
    }

    public void updateLocation()
    {
        startPoint = rTransform.anchoredPosition;
    }

    private void resetPosition()
    {
        rTransform.anchoredPosition = startPoint;
    }
}
