using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DroneCommand : MonoBehaviour
{
    Routine routine;
    [SerializeField] DroneInterface droneInterface;

    // Start is called before the first frame update
    void Start()
    {
        routine = this.GetComponent<Routine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) resetScene();
        if (Input.GetKeyDown(KeyCode.C)) routine.paused = !routine.paused;
        if (Input.GetKeyDown(KeyCode.X)) droneInterface.gameObject.SetActive(!droneInterface.gameObject.activeSelf);
        if (Input.GetKeyDown(KeyCode.V)) routine.setProcess(droneInterface.getProcess());
    }

    public void resetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
