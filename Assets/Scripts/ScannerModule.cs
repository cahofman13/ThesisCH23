using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScannerModule : MonoBehaviour
{
    [SerializeField] BoxCollider grabArea;

    public bool scanObstacle()
    {
        Collider[] colliders = Physics.OverlapBox(
            grabArea.transform.position,
            grabArea.bounds.extents);
        if (colliders.Count() <= 0) return false;

        bool validObstacle = false;
        foreach(Collider coll in colliders)
        {
            if (coll.gameObject.layer == LayerMask.NameToLayer("Maze")
                || coll.tag == "Grabable")
            {
                validObstacle = true;
                break;
            }
        }
        return validObstacle;
    }

    public string scanColor()
    {
        string colorName = "None";
        Collider[] colliders = Physics.OverlapBox(
            grabArea.transform.position,
            grabArea.bounds.extents);
        if (colliders.Count() <= 0) return colorName;

        foreach (Collider coll in colliders)
        {
            if (coll.tag == "Grabable")
            {
                Color color = coll.GetComponent<Renderer>().material.color;
                if (color == Color.red) colorName = "Red";
                else if (color == Color.green) colorName = "Green";
                else if (color == Color.blue) colorName = "Blue";
                break;
            }
        }

        return colorName;
    }

}
