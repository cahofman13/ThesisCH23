using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomInteractable : XRSimpleInteractable
{
    Node node;
    PositionConstraint positionConstraint;
    RotationConstraint rotationConstraint;

    protected void Start()
    {
        if (this.TryGetComponent(out Node n)) node = n;
        else Destroy(this);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs call)
    {
        base.OnSelectEntered(call);

        //new LeftRight
        if (call.interactorObject.transform.name.StartsWith("Left"))
            ControlsManager.instance.leftHandNodes.Add(node);
        else ControlsManager.instance.rightHandNodes.Add(node);

        //old Drag Actions
        Debug.Log("startDrag");
        node.setDrag(true);
        if (node.retainIsCollidedHud) node.setRetainedHud();

        positionConstraint = gameObject.AddComponent<PositionConstraint>();
        positionConstraint.constraintActive = true;
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = call.interactorObject.transform;
        source.weight = 1;
        positionConstraint.AddSource(source);
        positionConstraint.weight = 1;

        rotationConstraint = gameObject.AddComponent<RotationConstraint>();
        rotationConstraint.constraintActive = true;
        rotationConstraint.AddSource(source);
        rotationConstraint.weight = 1;
    }

    protected override void OnSelectExited(SelectExitEventArgs call)
    {
        base.OnSelectExited(call);

        //new LeftRight
        if (call.interactorObject.transform.name.StartsWith("Left"))
            ControlsManager.instance.leftHandNodes.Remove(node);
        else ControlsManager.instance.rightHandNodes.Remove(node);

        //old Drag Actions
        Destroy(positionConstraint);
        node.setDrag(false);
        if (transform.position.y < transform.lossyScale.y / 2) transform.position =
            new Vector3(transform.position.x, transform.lossyScale.y / 2, transform.position.z);
    }

}
