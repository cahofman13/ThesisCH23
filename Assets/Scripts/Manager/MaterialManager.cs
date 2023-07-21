using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager instance;
    public Material mNodeActionInactive;
    public Material mNodeActionActive;
    public Material mNodeActionInvalid;
    public Material mNodeControlInactive;
    public Material mNodeControlActive;

    private void Start()
    {
        if (instance) Destroy(this);
        else instance = this;
    }
}
