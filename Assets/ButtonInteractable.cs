using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonInteractable : XRSimpleInteractable
{
    Button button;

    protected void Start()
    {
        button = GetComponent<Button>();
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        button.press();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        button.press();
    }
}
