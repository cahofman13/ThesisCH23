using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockGateColored : ColorPlate
{
    public GameObject gate;

    bool running = false;
    Coroutine currentCoroutine;

    internal override void activate()
    {
        base.activate();
        if(running) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(dropGate());
    }

    internal override void deactivate()
    {
        base.deactivate();
        if (running) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(raiseGate());
    }

    IEnumerator dropGate()
    {
        running = true;
        while(gate.transform.localPosition.y > -0.9)
        {
            gate.transform.position += new Vector3(0, -0.005f, 0);
            yield return null;
        }
        gate.GetComponent<Collider>().enabled = false;
        running = false;
    }

    IEnumerator raiseGate()
    {
        running = true;
        while (gate.transform.localPosition.y < 0)
        {
            gate.transform.position += new Vector3(0, 0.005f, 0);
            yield return null;
        }
        gate.GetComponent<Collider>().enabled = true;
        running = false;
    }
}
