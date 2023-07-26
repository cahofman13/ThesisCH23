using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPlate : PressurePlate
{

    public Material requiredColorMaterial;

    internal override void exOnTriggerEnter(Collider other)
    {
        if (other.tag != "Grabable" || other.GetComponent<Renderer>().material.color != requiredColorMaterial.color) return;
        if (!activated) activate(); 
        count++;
    }

    internal override void exOnTriggerExit(Collider other)
    {
        if (other.tag != "Grabable" || other.GetComponent<Renderer>().material.color != requiredColorMaterial.color) return;
        count--;
        if (count <= 0) deactivate();
    }
}
