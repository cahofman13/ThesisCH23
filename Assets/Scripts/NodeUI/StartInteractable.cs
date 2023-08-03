using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StartInteractable : XRSimpleInteractable
{
    protected override void OnActivated(ActivateEventArgs args)
    {
        ControlsManager.instance.startLineDrag(args);
    }
}
