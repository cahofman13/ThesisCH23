using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager instance;
    public Color cInternStart;
    public Color cInternEnd;

    private void Start()
    {
        if (instance) Destroy(this);
        else instance = this;
    }
}
