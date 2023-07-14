using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour
{
    public LineRenderer lineRenderer { get; private set; }
    public GameObject start;
    public GameObject end;

    public bool sync = true;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sync && end && start) setLine(start.transform.position, end.transform.position);
    }

    public void setLine(Vector3 pos1, Vector3 pos2)
    {
        Vector3 distance = pos2 - pos1;
        transform.position = pos1;
        lineRenderer.SetPositions(new Vector3[]{ 
            Vector3.zero, 0.4f * distance, 0.6f * distance, distance
        });
    }
}
