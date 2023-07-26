using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrabberModule : MonoBehaviour
{
    [SerializeField] BoxCollider grabArea;

    GameObject grabbedObject = null;
    Transform originalParent = null;
    Vector3 originalPosition;

    public bool inProgress { get; private set; } = false;

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public bool hasGrabbed()
    {
        return grabbedObject != null;
    }

    public bool grab()
    {
        if (hasGrabbed()) return false;
        if (inProgress) return false;
        Collider[] colliders = Physics.OverlapBox(
            grabArea.transform.position,
            grabArea.bounds.extents);
        if (colliders.Count() <= 0) return false;

        foreach(Collider coll in colliders)
        {
            if(coll.tag == "Grabable")
                grabbedObject = coll.gameObject;
        }
        if (!hasGrabbed()) return false;

        inProgress = true;
        originalParent = grabbedObject.transform.parent;
        originalPosition = grabbedObject.transform.position;
        grabbedObject.transform.parent = gameObject.transform;

        StartCoroutine(GrabCoroutine());

        return true;
    }

    public bool release()
    {
        if (!hasGrabbed()) return false;
        if (inProgress) return false;

        inProgress = true;
        StartCoroutine(ReleaseCoroutine());

        return true;
    }

    public void resetGrab()
    {
        if(grabbedObject)
        {
            grabbedObject.transform.parent = originalParent;
            grabbedObject.transform.position = originalPosition;
            grabbedObject.transform.rotation = Quaternion.identity;
            grabbedObject = null;
            transform.localRotation = Quaternion.Euler(0, 0, -90);
        }
    }

    IEnumerator GrabCoroutine()
    {
        for(int i = 0; i < 1750; i++)
        {
            yield return null;
            transform.localRotation = Quaternion.Euler(
                transform.localEulerAngles.x - 0.05f, 
                transform.localEulerAngles.y, 
                transform.localEulerAngles.z);
        }
        inProgress = false;
    }

    IEnumerator ReleaseCoroutine()
    {
        for (int i = 0; i < 1750; i++)
        {
            yield return null;
            transform.localRotation = Quaternion.Euler(
                transform.localEulerAngles.x + 0.05f,
                transform.localEulerAngles.y,
                transform.localEulerAngles.z);
        }

        grabbedObject.transform.parent = originalParent;
        originalParent = null;
        grabbedObject = null;

        inProgress = false;
    }

}
