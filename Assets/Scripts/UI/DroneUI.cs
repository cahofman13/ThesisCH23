using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DroneUI : MonoBehaviour
{
    Canvas canvas;

    public GameObject pauseToggle;

    // Start is called before the first frame update
    void Start()
    {
        canvas = this.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        //Canvas Forward goes toward its back, so instead of (t-t)+c it's t+t-c
        transform.LookAt(2*transform.position - this.GetComponent<Canvas>().worldCamera.transform.position);

        //Pause Drone with Key <V>
        if (pauseToggle && Input.GetKeyDown(KeyCode.V)) pauseToggle.GetComponent<Toggle>().isOn = !pauseToggle.GetComponent<Toggle>().isOn;

        if (Input.GetKeyDown(KeyCode.X)) canvas.enabled = !canvas.enabled;

    }

    public void resetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
