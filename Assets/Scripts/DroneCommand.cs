using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation.Samples;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DroneCommand : MonoBehaviour
{
    Routine routine;
    [SerializeField] DroneInterface droneInterface;
    [SerializeField] AudioSource audioSimplePress;
    [SerializeField] AudioSource audioConfirmProcess;
    [SerializeField] Renderer lampRenderer;

    bool hudInProgress = false;

    // Start is called before the first frame update
    void Start()
    {
        routine = this.GetComponent<Routine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) resetScene();
        if (Input.GetKeyDown(KeyCode.C)) pauseRoutineToggle();
        if (Input.GetKeyDown(KeyCode.X)) toggleHUD();
        if (Input.GetKeyDown(KeyCode.V)) readProcess(true);
    }

    public void resetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void pauseRoutine(bool paused)
    {
        routine.paused = paused;
        audioSimplePress.Play();
        if (routine.paused) lampRenderer.material = MaterialManager.instance.Red;
        else lampRenderer.material = MaterialManager.instance.Green;
    }
    public void pauseRoutineToggle()
    {
        routine.paused = !routine.paused;
        audioSimplePress.Play();
        if (routine.paused) lampRenderer.material = MaterialManager.instance.Red;
        else lampRenderer.material = MaterialManager.instance.Green;
    }

    public void readProcess(bool unpause)
    {
        if (!routine.paused) return;
        routine.setProcess(droneInterface.getProcess());
        audioConfirmProcess.Play();
        if(unpause) routine.paused = false;
    }

    public void toggleHUD()
    {
        if (hudInProgress) return;
        if (droneInterface.gameObject.activeSelf) StartCoroutine(deactivateHUD());
        else StartCoroutine(activateHUD());

        audioSimplePress.Play();
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
        hudInProgress = true;
        droneInterface.gameObject.SetActive(true);
        for (int i = 0; i < 50; i++)
        {
            yield return null;
            droneInterface.transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
        }
        if (FreeCam.instance && FreeCam.instance.draggedObject && FreeCam.instance.draggedObject.TryGetComponent(out Node node))
            node.setNodeHUD(true, droneInterface.transform);
        else if (ControlsManager.instance)
        {
            foreach (Node n in ControlsManager.instance.getNodes(false)) n.setNodeHUD(true, droneInterface.transform);
            foreach (Node n in ControlsManager.instance.getNodes(true)) n.setNodeHUD(true, droneInterface.transform);
        }

        droneInterface.interfaceCollider.enable();
        hudInProgress = false;
    }


    private IEnumerator deactivateHUD()
    {
        hudInProgress = true;
        droneInterface.interfaceCollider.disable();
        if (FreeCam.instance && FreeCam.instance.draggedObject && FreeCam.instance.draggedObject.TryGetComponent(out Node node)) 
            node.setNodeHUD(false, null);
        else if (ControlsManager.instance)
        {
            foreach (Node n in ControlsManager.instance.getNodes(false)) n.setNodeHUD(false, null);
            foreach (Node n in ControlsManager.instance.getNodes(true)) n.setNodeHUD(false, null);
        }

        for (int i = 0; i < 50; i++)
        {
            yield return null;
            droneInterface.transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
        }
        droneInterface.gameObject.SetActive(false);
        hudInProgress = false;
    }
}
