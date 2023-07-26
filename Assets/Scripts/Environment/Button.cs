using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void press()
    {
        StartCoroutine(pressAnimation());
    }

    internal virtual void activation()
    {

    }

    IEnumerator pressAnimation()
    {
        for(int i = 25; i > 0; i--)
        { //target 0.05f
            transform.position += new Vector3(0, -0.002f, 0);
            yield return null;
        }
        activation();
        for (int i = 25; i > 0; i--)
        { //target 0.05f
            transform.position += new Vector3(0, 0.002f, 0);
            yield return null;
        }
    }
}
