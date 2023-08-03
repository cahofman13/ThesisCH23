using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : Button
{
    public GameObject drone;
    public Transform resetPosition;

    protected override void activate()
    {
        base.activate();
        StartCoroutine(resetDrone());
    }

    IEnumerator resetDrone()
    {
        DroneCommand droneCommand = drone.GetComponent<DroneCommand>();
        droneCommand.pauseRoutine(true);
        droneCommand.readProcess(false);
        droneCommand.tryDeactivateHUD();
        droneCommand.enabled = false;
        yield return new WaitForSeconds(2);
        GrabberModule grabber = drone.GetComponentInChildren<GrabberModule>();
        grabber.resetGrab();
        drone.transform.position = resetPosition.position;
        drone.transform.rotation = resetPosition.rotation;
        yield return new WaitForSeconds(2);
        droneCommand.enabled = true;
        droneCommand.tryActivateHUD();
    }

}
