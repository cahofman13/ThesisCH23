using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation.Samples;
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
        if (Input.GetKeyDown(KeyCode.X)) toggleHUD();
        if (Input.GetKeyDown(KeyCode.V)) readProcess();
    }

    public void resetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void pauseRoutine(bool paused)
    {
        routine.paused = paused;
    }

    public void readProcess()
    {
        routine.setProcess(droneInterface.getProcess());
    }

    private void toggleHUD()
    {
        if (droneInterface.gameObject.activeSelf) StartCoroutine(deactivateHUD());
        else StartCoroutine(activateHUD());
    }

    public void tryDeactivateHUD()
    {
        if (droneInterface.gameObject.activeSelf) StartCoroutine(deactivateHUD());
    }

    public void tryActivateHUD()
    {
        if (!droneInterface.gameObject.activeSelf) StartCoroutine(activateHUD());
    }

    private IEnumerator activateHUD()
    {
        droneInterface.gameObject.SetActive(true);
        for (int i = 0; i < 50; i++)
        {
            yield return null;
            droneInterface.transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
        }
        if (FreeCam.instance.draggedObject && FreeCam.instance.draggedObject.TryGetComponent(out Node node))
            node.setNodeHUD(true, droneInterface.transform);
        droneInterface.interfaceCollider.enable();
    }


    private IEnumerator deactivateHUD()
    {
        droneInterface.interfaceCollider.disable();
        if (FreeCam.instance.draggedObject && FreeCam.instance.draggedObject.TryGetComponent(out Node node)) 
            node.setNodeHUD(false, null);

        for (int i = 0; i < 50; i++)
        {
            yield return null;
            droneInterface.transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
        }
        droneInterface.gameObject.SetActive(false);
    }
}
