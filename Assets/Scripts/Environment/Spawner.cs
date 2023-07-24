using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject spawnPrefab;

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
            if (spawnedNode == null
                || Mathf.Abs(spawnedNode.transform.position.x - this.transform.position.x) > 2
                || Mathf.Abs(spawnedNode.transform.position.z - this.transform.position.z) > 2)
                StartCoroutine(spawnNew());

            yield return new WaitForSeconds(10);
        }
    }

    IEnumerator spawnNew()
    {
        spawnedNode = null;
        yield return new WaitForSeconds(3);
        spawnedNode = Instantiate(spawnPrefab, transform.position + new Vector3(0, 5, 0), Quaternion.identity, transform.parent);
        spawnedNode.GetComponent<Node>().gravity = true;
    }

}
