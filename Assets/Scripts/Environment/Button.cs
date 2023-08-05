using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    AudioSource audioSource;

    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void press()
    {
        StartCoroutine(pressAnimation());
        if (audioSource != null) audioSource.Play();
    }

    protected virtual void activate()
    {

    }

    IEnumerator pressAnimation()
    {
        for(int i = 25; i > 0; i--)
        { //target 0.05f
            transform.position += new Vector3(0, -0.002f, 0);
            yield return null;
        }
        activate();
        for (int i = 25; i > 0; i--)
        { //target 0.05f
            transform.position += new Vector3(0, 0.002f, 0);
            yield return null;
        }
    }
}
