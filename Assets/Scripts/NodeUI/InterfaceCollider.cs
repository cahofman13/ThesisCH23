using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceCollider : MonoBehaviour
{
    new Collider collider;

    private void Start()
    {
        collider = this.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Node node))
        {
            if (node.isDragged) node.setNodeHUD(true, transform.parent);
            else node.registerInHud(true, transform.parent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Node node))
        {
            if (node.isDragged) node.setNodeHUD(false, null);
            else node.registerInHud(false, null);
        }
    }

    public void enable()
    {
        collider.enabled = true;
    }

    public void disable()
    {
        collider.enabled = false;
    }
}
