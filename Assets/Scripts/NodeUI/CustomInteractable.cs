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

    Coroutine velCoroutine;
    Vector3 releaseVelocity;
    Queue<Vector3> oldPos;

    protected void Start()
    {
        if (this.TryGetComponent(out Node n)) node = n;
        else Destroy(this);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        StopAllCoroutines();
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        ControlsManager.instance.startLineDrag(args);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs call)
    {
        base.OnSelectEntered(call);

        //new LeftRight
        if (call.interactorObject.transform.name.StartsWith("Left"))
            ControlsManager.instance.addNode(node, true);
        else ControlsManager.instance.addNode(node, false);

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

        velCoroutine = StartCoroutine(trackVelocity());
    }

    protected override void OnSelectExited(SelectExitEventArgs call)
    {
        base.OnSelectExited(call);

        //new LeftRight
        if (call.interactorObject.transform.name.StartsWith("Left"))
            ControlsManager.instance.remNode(node, true);
        else ControlsManager.instance.remNode(node, false);

        //old Drag Actions
        Destroy(positionConstraint);
        Destroy(rotationConstraint);
        node.setDrag(false);
        if (transform.position.y < transform.lossyScale.y / 2) transform.position =
            new Vector3(transform.position.x, transform.lossyScale.y / 2, transform.position.z);
        node.rigidbody.velocity = Vector3.zero;
        node.rigidbody.AddForce(releaseVelocity * 6, ForceMode.VelocityChange);
        StopCoroutine(velCoroutine);
    }

    IEnumerator trackVelocity() 
    {
        releaseVelocity = Vector3.zero;
        oldPos = new Queue<Vector3>();
        oldPos.Enqueue(transform.position);
        oldPos.Enqueue(transform.position);
        oldPos.Enqueue(transform.position);
        oldPos.Enqueue(transform.position);
        oldPos.Enqueue(transform.position);
        while (true)
        {
            yield return new WaitForSeconds(0.02f);
            releaseVelocity = transform.position - oldPos.Dequeue();
            oldPos.Enqueue(transform.position);
        }
    }

}
