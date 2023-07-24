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

    public Connection intern;
    public bool isControl { get; internal set; } = false;

    internal bool isDragged = false;

    float minimumHeight;
    public bool gravity = false;

    // Start is called before the first frame update
    void Start()
    {
        exStart();
    }

    //Extendable Start Method
    internal virtual void exStart()
    {
        minimumHeight = transform.lossyScale.y/2;
        renderer = this.GetComponent<Renderer>();
    }

    void Update()
    {
        exUpdate();
    }

    internal virtual void exUpdate()
    {
        
    }

    void FixedUpdate()
    {
        exFixedUpdate();
    }

    internal virtual void exFixedUpdate()
    {
        if(gravity && transform.position.y > minimumHeight)
        {
            if (transform.position.y - minimumHeight < 0.1f) transform.position = new Vector3(transform.position.x, minimumHeight, transform.position.z);
            else transform.position += new Vector3(0, -0.1f, 0);
        }

    }

    public virtual void setDrag(bool dragged)
    {
        isDragged = dragged;
    }

    public void setActionActive()
    {
        renderer.material = MaterialManager.instance.mNodeActionActive;
    }

    public void setActionInactive()
    {
        renderer.material = MaterialManager.instance.mNodeActionInactive;
    }
    public void setActionInvalid()
    {
        renderer.material = MaterialManager.instance.mNodeActionInvalid;
    }

    public void setControlActive()
    {
        renderer.material = MaterialManager.instance.mNodeControlActive;
    }

    public void setControlInactive()
    {
        renderer.material = MaterialManager.instance.mNodeControlInactive;
    }

}
