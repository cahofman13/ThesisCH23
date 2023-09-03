using Unity.AI.Navigation.Samples;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DragLine : MonoBehaviour
{
    Node lineNode;
    Connection line;
    GameObject interactorGO;
    LineState lineState;

    public void Init(ActivateEventArgs call)
    {
        Debug.Log("startLine");
        lineNode = call.interactableObject.transform.GetComponent<Node>();
        interactorGO = call.interactorObject.transform.gameObject;
        if (lineNode.next) lineNode.next.lineRenderer.enabled = false;
        interactorGO.GetComponent<XRInteractorLineVisual>().enabled = false;
        interactorGO.GetComponent<XRRayInteractor>().enabled = false;
        interactorGO.GetComponent<LineRenderer>().enabled = false;
        line = this.GetComponent<Connection>();
        line.start = lineNode.gameObject;
        line.end = interactorGO;
        lineState = LineState.NEXT;
    }

    public void Toggle()
    {
        if (lineState != LineState.NONE)
        {
            switch (lineState)
            {
                case LineState.NEXT:
                    if (!lineNode.isControl)
                    {
                        lineState = LineState.PREV;
                        line.end = lineNode.gameObject;
                        line.start = interactorGO;
                        if (lineNode.next) lineNode.next.lineRenderer.enabled = true;
                        if (lineNode.prev) lineNode.prev.lineRenderer.enabled = false;
                    }
                    else
                    {
                        lineState = LineState.INTERN;
                        Gradient gradient1 = line.lineRenderer.colorGradient;
                        GradientColorKey[] colorKeys1 = gradient1.colorKeys;
                        colorKeys1[0].color = ColorManager.instance.cInternStart;
                        colorKeys1[1].color = ColorManager.instance.cInternEnd;
                        gradient1.SetKeys(colorKeys1, line.lineRenderer.colorGradient.alphaKeys);
                        line.lineRenderer.colorGradient = gradient1;
                        if (lineNode.next) lineNode.next.lineRenderer.enabled = true;
                        if (lineNode.intern) lineNode.intern.lineRenderer.enabled = false;
                    }
                    break;
                case LineState.PREV:
                    lineState = LineState.NEXT;
                    line.start = lineNode.gameObject;
                    line.end = interactorGO;
                    if (lineNode.next) lineNode.next.lineRenderer.enabled = false;
                    if (lineNode.prev) lineNode.prev.lineRenderer.enabled = true;
                    break;
                case LineState.INTERN:
                    lineState = LineState.PREV;
                    Gradient gradient2 = line.lineRenderer.colorGradient;
                    GradientColorKey[] colorKeys2 = gradient2.colorKeys;
                    colorKeys2[0].color = Color.green;
                    colorKeys2[1].color = Color.red;
                    gradient2.SetKeys(colorKeys2, line.lineRenderer.colorGradient.alphaKeys);
                    line.lineRenderer.colorGradient = gradient2;
                    line.end = lineNode.gameObject;
                    line.start = interactorGO;
                    if (lineNode.intern) lineNode.intern.lineRenderer.enabled = true;
                    if (lineNode.prev) lineNode.prev.lineRenderer.enabled = false;
                    break;
            }
        }
    }

    public void Erase()
    {
        if (lineState != LineState.NONE)
        {
            switch (lineState)
            {
                case LineState.NEXT: destroyLine(lineNode, LineState.NEXT); break;
                case LineState.PREV: destroyLine(lineNode, LineState.PREV); break;
                case LineState.INTERN: destroyLine(lineNode, LineState.INTERN); break;
            }

            cancelLineDrag();
        }
    }

    public void End()
    {
        if (lineState != LineState.NONE)
        {
            Node target = null;
            foreach (Collider collider in Physics.OverlapSphere(interactorGO.transform.position, 0.3f))
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Node")
                    && collider.gameObject != lineNode.gameObject
                    && collider.transform.parent == lineNode.transform.parent)
                {
                    target = collider.gameObject.GetComponent<Node>();
                }
            }

            if (target)
            {
                switch (lineState)
                {
                    case LineState.NEXT:
                        if (lineNode.prev && target.gameObject == lineNode.prev.start
                            || lineNode.intern && target.gameObject == lineNode.intern.end) { cancelLineDrag(); break; }

                        line.end = target.gameObject;
                        destroyLine(lineNode, LineState.NEXT);
                        destroyLine(target, LineState.PREV);
                        lineNode.next = line;
                        target.prev = line;
                        transform.parent.GetComponent<DroneInterface>().droneCommand.programChanged = true;
                        break;


                    case LineState.PREV:
                        if (lineNode.next && target.gameObject == lineNode.next.end
                            || lineNode.intern && target.gameObject == lineNode.intern.end) { cancelLineDrag(); break; }

                        line.start = target.gameObject;
                        destroyLine(lineNode, LineState.PREV);
                        destroyLine(target, LineState.NEXT);
                        lineNode.prev = line;
                        target.next = line;
                        transform.parent.GetComponent<DroneInterface>().droneCommand.programChanged = true;
                        break;


                    case LineState.INTERN:
                        if (lineNode.prev && target.gameObject == lineNode.prev.start
                            || lineNode.next && target.gameObject == lineNode.next.end) { cancelLineDrag(); break; }

                        line.end = target.gameObject;
                        destroyLine(lineNode, LineState.INTERN);
                        destroyLine(target, LineState.PREV);
                        lineNode.intern = line;
                        target.prev = line;
                        transform.parent.GetComponent<DroneInterface>().droneCommand.programChanged = true;
                        break;
                }
            }
            else
            {
                cancelLineDrag();
            }

            //UNSET
            interactorGO.GetComponent<LineRenderer>().enabled = true;
            interactorGO.GetComponent<XRRayInteractor>().enabled = true;
            interactorGO.GetComponent<XRInteractorLineVisual>().enabled = true;
            Destroy(this);
        }
    }


    void cancelLineDrag()
    {
        if (lineNode.next) lineNode.next.lineRenderer.enabled = true;
        if (lineNode.prev) lineNode.prev.lineRenderer.enabled = true;
        interactorGO.GetComponent<LineRenderer>().enabled = true;
        interactorGO.GetComponent<XRRayInteractor>().enabled = true;
        interactorGO.GetComponent<XRInteractorLineVisual>().enabled = true;
        Destroy(line.gameObject); //this.gameObject
    }

    void destroyLine(Node lineNode, LineState lineType)
    {
        if (lineType == LineState.NEXT)
        {
            if (lineNode.next)
            {
                Connection connection = lineNode.next;
                lineNode.next = null;
                connection.end.GetComponent<Node>().prev = null;
                transform.parent.GetComponent<DroneInterface>().droneCommand.programChanged = true;
                Destroy(connection.gameObject);
            }
        }
        else if (lineType == LineState.PREV)
        {
            if (lineNode.prev)
            {
                Connection connection = lineNode.prev;
                lineNode.prev = null;
                Node targetNode = connection.start.GetComponent<Node>();
                if (targetNode.next == connection) targetNode.next = null;
                if (targetNode.intern == connection) targetNode.intern = null;
                transform.parent.GetComponent<DroneInterface>().droneCommand.programChanged = true;
                Destroy(connection.gameObject);
            }
        }
        else if (lineType == LineState.INTERN)
        {
            if (lineNode.intern)
            {
                Connection connection = lineNode.intern;
                lineNode.intern = null;
                connection.end.GetComponent<Node>().prev = null;
                transform.parent.GetComponent<DroneInterface>().droneCommand.programChanged = true;
                Destroy(connection.gameObject);
            }
        }
    }

}
