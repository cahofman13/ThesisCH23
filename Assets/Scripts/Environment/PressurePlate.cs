using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    internal int count = 0;
    internal bool activated = false;

    new internal Renderer renderer;

    private void Start()
    {
        exStart();
    }

    internal virtual void exStart()
    {
        renderer = this.GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        exOnTriggerEnter(other);
    }

    internal virtual void exOnTriggerEnter(Collider other)
    {
        if (other.tag != "Grabable" && other.tag != "Drone") return;
        if (!activated) activate();
        count++;
    }

    private void OnTriggerExit(Collider other)
    {
        exOnTriggerExit(other);
    }

    internal virtual void exOnTriggerExit(Collider other)
    {
        if (other.tag != "Grabable" && other.tag != "Drone") return;
        count--;
        if (count <= 0) deactivate();
    }

    internal virtual void activate()
    {
        activated = true;
        renderer.material = MaterialManager.instance.mPressureActive;
    }

    internal virtual void deactivate()
    {
        activated = false;
        renderer.material = MaterialManager.instance.mPressureInactive;
    }
}
