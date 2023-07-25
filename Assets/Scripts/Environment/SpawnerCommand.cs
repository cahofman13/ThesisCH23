using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerCommand : MonoBehaviour
{
    [SerializeField] GameObject spawnPrefab;
    [SerializeField] PressurePlate[] pressurePlates;

    GameObject spawnedNode;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(checkIfBlockWasTaken());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    IEnumerator checkIfBlockWasTaken()
    {
        while (true)
        {
            bool activated = true;
            foreach (PressurePlate pressure in pressurePlates)
            {
                if (!pressure.activated)
                {
                    activated = false;
                    break;
                }
            }
            if (activated &&
                (spawnedNode == null
                || Mathf.Abs(spawnedNode.transform.position.x - this.transform.position.x) > 2
                || Mathf.Abs(spawnedNode.transform.position.z - this.transform.position.z) > 2))
                StartCoroutine(spawnNew());

            yield return new WaitForSeconds(2);
        }
    }

    IEnumerator spawnNew()
    {
        spawnedNode = null;
        yield return new WaitForSeconds(0.5f);
        spawnedNode = Instantiate(spawnPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity, transform.parent);
        spawnedNode.GetComponent<Node>().gravity = true;
    }

}
