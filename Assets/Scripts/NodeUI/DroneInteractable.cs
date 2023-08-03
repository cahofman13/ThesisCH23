using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DroneInteractable : XRSimpleInteractable
{
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        if (args.interactorObject.transform.name.StartsWith("Left")) 
            ControlsManager.instance.leftDroneHover = this.gameObject;
        else ControlsManager.instance.rightDroneHover = this.gameObject;
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        if (args.interactorObject.transform.name.StartsWith("Left"))
            ControlsManager.instance.leftDroneHover = null;
        else ControlsManager.instance.rightDroneHover = null;
    }
}
