using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputFor : BlockUI
{
    public int executionTarget = 1;

    CanvasGroup routineCanvas;

    internal override void startup()
    {
        base.startup();
        blockName = "ForBlock";
        routineCanvas = transform.GetChild(1).GetChild(1).GetComponent<CanvasGroup>();
        image = transform.GetComponentInChildren<Image>();
    }

    public void dropdownSetExecutionTarget(GameObject dropdownGO)
    {
        TMP_Dropdown dropdown = dropdownGO.GetComponent<TMP_Dropdown>();
        string text = dropdown.options[dropdown.value].text;
        if (int.TryParse(text, out int i))
            executionTarget = i;
        else
            executionTarget = 1;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        routineCanvas.blocksRaycasts = false;
        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Vector3 hitPos = eventData.pointerCurrentRaycast.worldPosition;
        if (hitPos == new Vector3(0, 0, 0))
            routineCanvas.alpha = 0;
        else
        {
            routineCanvas.alpha = 1;
        }
        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        routineCanvas.blocksRaycasts = true;
        base.OnEndDrag(eventData);
    }

}
