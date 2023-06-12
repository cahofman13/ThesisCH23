using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

public class DroneUI : MonoBehaviour
{
    Process process;  //useless???

    public GameObject owner;
    Routine ownerRoutine;  //useless???

    public GameObject pauseToggle;

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
        //Canvas Forward goes toward its back, so instead of (t-t)+c it's t+t-c
        transform.LookAt(2*transform.position - this.GetComponent<Canvas>().worldCamera.transform.position);

        //Pause Drone with Key <V>
        if (pauseToggle && Input.GetKeyDown(KeyCode.V)) pauseToggle.GetComponent<Toggle>().isOn = !pauseToggle.GetComponent<Toggle>().isOn;
    }

}
