using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class DroneUI : MonoBehaviour
{
    Process process = null;

    public GameObject owner;
    Routine ownerRoutine;

    // Start is called before the first frame update
    void Start()
    {
        if (!owner || !owner.TryGetComponent(out Routine routine))
        {
            Destroy(this.gameObject, 0.1f);
            throw new UnassignedReferenceException("Missing DroneOwner!!!");
        }
        else ownerRoutine = routine;
    }

    // Update is called once per frame
    void Update()
    {
        //Canvas Forward goes toward its back
        transform.LookAt(this.transform.position - this.GetComponent<Canvas>().worldCamera.transform.position);
    }

    void CommitProcess()
    {
        ownerRoutine.process = process;
    }
}
