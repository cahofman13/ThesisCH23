using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    int count = 0;
    internal bool activated = false;

    new Renderer renderer;

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
        if (other.tag != "Grabable" && other.tag != "Drone") return;
        if (!activated) activate(); 
        count++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Grabable" && other.tag != "Drone") return;
        count--;
        if (count <= 0) deactivate();
    }

    internal virtual void activate()
    {
        renderer.material = MaterialManager.instance.mPressureActive;
    }

    internal virtual void deactivate()
    {
        renderer.material = MaterialManager.instance.mPressureInactive;
    }
}
