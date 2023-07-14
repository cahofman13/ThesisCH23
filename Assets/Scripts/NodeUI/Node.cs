using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    new Renderer renderer;

    //Identifier that determines the created block
    public string blockName = "";

    public Connection prev;
    public Connection next;

    // Start is called before the first frame update
    void Start()
    {
        startup();
    }

    //Extendable Start Method
    internal virtual void startup()
    {
        renderer = this.GetComponent<Renderer>();
    }

    void Update()
    {
        UpdateExtend();
    }

    internal virtual void UpdateExtend()
    {
        
    }

    public void setBlockActive()
    {
        renderer.material = MaterialManager.instance.mNodeActive;
    }

    public void setBlockInactive()
    {
        renderer.material = MaterialManager.instance.mNodeInactive;
    }

    public void setBlockInternal()
    {
        renderer.material = MaterialManager.instance.mNodeInternal;
    }

}
