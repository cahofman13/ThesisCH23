using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPlate : MonoBehaviour
{
    int count = 0;
    internal bool activated = false;

    public Material requiredColorMaterial;

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
        if (other.tag != "Grabable" || other.GetComponent<Renderer>().material.color != requiredColorMaterial.color) return;
        if (!activated) activate(); 
        count++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Grabable" || other.GetComponent<Renderer>().material.color != requiredColorMaterial.color) return;
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
