using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager instance;
    public Material mNodeInactive;
    public Material mNodeActive;
    public Material mNodeInternal;

    private void Start()
    {
        if (instance) Destroy(this);
        else instance = this;
    }
}
