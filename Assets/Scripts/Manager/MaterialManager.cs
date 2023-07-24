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

    public Material mPressureInactive;
    public Material mPressureActive;

    private void Start()
    {
        instance = this;
    }
    private void Update()
    {
        instance = this;
    }

}
