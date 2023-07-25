using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
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
    public bool retainIsCollidedHud = false;
    public Transform retainCollidedParent = null;

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

    public virtual void setNodeHUD(bool enter, Transform newParent)
    {
        if(enter)
        {
            gravity = false;
            transform.parent = newParent;
        }
        else
        {
            if (prev) destroyLine(prev);
            if (next) destroyLine(next);
            if (intern) destroyLine(intern);
            gravity = true;
            transform.parent = transform.parent.parent;
        }
    }

    public void registerInHud(bool enter, Transform newParent)
    {
        retainIsCollidedHud = enter;
        retainCollidedParent = newParent;
    }

    public void setRetainedHud()
    {
        setNodeHUD(retainIsCollidedHud, retainCollidedParent);
        retainIsCollidedHud = false;
        retainCollidedParent = null;
    }

    private void destroyLine(Connection connection)
    {
        Node startNode = connection.start.GetComponent<Node>();
        if (startNode.next == connection) startNode.next = null;
        if (startNode.intern == connection) startNode.intern = null;
        Node endNode = connection.end.GetComponent<Node>();
        if (endNode.prev == connection) endNode.prev = null;
        Destroy(connection.gameObject);
    }
}
