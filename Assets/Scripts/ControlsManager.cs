using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TreeEditor;
using Unity.AI.Navigation.Samples;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ControlsManager : MonoBehaviour
{
    #region Singleton and Control-Enum
    //ControlStates
    public enum ControlState
    {
        Default, Grab, Line
    }
    [HideInInspector] public ControlState crtlStateLeft = ControlState.Default;
    [HideInInspector] public ControlState crtlStateRight = ControlState.Default;

    // singleton
    public static ControlsManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(this);
        else instance = this;
    }
    #endregion

    #region Inputs
    [SerializeField] InputActionProperty leftGripBtn;
    [SerializeField] InputActionProperty rightGripBtn;
    [HideInInspector] public float oldLeftGripValue;
    [HideInInspector] public float leftGripValue = 0;
    [HideInInspector] public float oldRightGripValue;
    [HideInInspector] public float rightGripValue = 0;
    [HideInInspector] public float GripValue;

    [SerializeField] InputActionProperty leftTriggerBtn;
    [SerializeField] InputActionProperty rightTriggerBtn;
    [HideInInspector] public float leftTriggerValue = 0;
    [HideInInspector] public float oldLeftTriggerValue;
    [HideInInspector] public float rightTriggerValue = 0;
    [HideInInspector] public float oldRightTriggerValue;
    [HideInInspector] public float triggerValue;

    [SerializeField] InputActionProperty leftPrimaryBtn;
    [SerializeField] InputActionProperty rightPrimaryBtn;
    [HideInInspector] public float leftPrimaryValue = 0;
    [HideInInspector] public float oldLeftPrimaryValue;
    [HideInInspector] public float rightPrimaryValue = 0;
    [HideInInspector] public float oldRightPrimaryValue;
    [HideInInspector] public float primaryValue;

    [SerializeField] InputActionProperty leftSecondaryBtn;
    [SerializeField] InputActionProperty rightSecondaryBtn;
    [HideInInspector] public float leftSecondaryValue = 0;
    [HideInInspector] public float oldLeftSecondaryValue;
    [HideInInspector] public float rightSecondaryValue = 0;
    [HideInInspector] public float oldRightSecondaryValue;
    [HideInInspector] public float secondaryValue;

    [SerializeField] InputActionProperty leftStick;
    [SerializeField] InputActionProperty rightStick;
    [HideInInspector] public Vector2 leftStickValue = Vector2.zero;
    [HideInInspector] public Vector2 oldLeftStickValue;
    [HideInInspector] public Vector2 rightStickValue = Vector2.zero;
    [HideInInspector] public Vector2 oldRightStickValue;
    #endregion

    List<Node> leftHandNodes = new List<Node>();
    List<Node> rightHandNodes = new List<Node>();

    public GameObject dragLinePrefab;
    DragLine leftLine = null;
    DragLine rightLine = null;

    public GameObject leftDroneHover = null;
    public GameObject rightDroneHover = null;

    [SerializeField] ActionBasedSnapTurnProvider actionBasedSnapTurnProvider;
    [SerializeField] ActionBasedContinuousMoveProvider actionBasedContinuousMoveProvider ;

    public void addNode(Node n, bool left)
    {
        if (left) { leftHandNodes.Add(n); actionBasedContinuousMoveProvider.enabled = false; }
        else { rightHandNodes.Add(n); actionBasedSnapTurnProvider.enabled = false ; }
    }
    public void remNode(Node n, bool left)
    {
        if (left) { leftHandNodes.Remove(n); if(leftHandNodes.Count <= 0) actionBasedContinuousMoveProvider.enabled = true; }
        else { rightHandNodes.Remove(n); if (rightHandNodes.Count <= 0) actionBasedSnapTurnProvider.enabled = true; }
    }
    public List<Node> getNodes(bool left)
    {
        if (left) return leftHandNodes;
        else return rightHandNodes;
    }

    public void startLineDrag(ActivateEventArgs call)
    {
        if (call.interactorObject.transform.name.StartsWith("Left"))
        { if (leftHandNodes.Count > 0) return; }
        else { if (rightHandNodes.Count > 0) return; }

        DragLine dragLine = Instantiate(dragLinePrefab, call.interactableObject.transform.parent).GetComponent<DragLine>();
        dragLine.Init(call);

        if (call.interactorObject.transform.name.StartsWith("Left")) 
            leftLine = dragLine;
        else rightLine = dragLine;

    }
    public void endLineDrag(bool left)
    {
        if (left) leftLine.End();
        else rightLine.End();
    }

    // PRIVATE UPDATES AND FUNCTIONS --------------------------------------------------------------------------------------------
    private void Update()
    {
        //Read Inputs -----------------------------------------------------
        oldLeftGripValue = leftGripValue;
        leftGripValue = leftGripBtn.action.ReadValue<float>();
        oldRightGripValue = rightGripValue;
        rightGripValue = rightGripBtn.action.ReadValue<float>();
        GripValue = Mathf.Max(leftGripValue, rightGripValue);

        oldLeftTriggerValue = leftTriggerValue;
        leftTriggerValue = leftTriggerBtn.action.ReadValue<float>();
        oldRightTriggerValue = rightTriggerValue;
        rightTriggerValue = rightTriggerBtn.action.ReadValue<float>();
        triggerValue = Mathf.Max(leftTriggerValue, rightTriggerValue);

        oldLeftPrimaryValue = leftPrimaryValue;
        leftPrimaryValue = leftPrimaryBtn.action.ReadValue<float>();
        oldRightPrimaryValue = rightPrimaryValue;
        rightPrimaryValue = rightPrimaryBtn.action.ReadValue<float>();
        primaryValue = Mathf.Max(leftPrimaryValue, rightPrimaryValue);

        oldLeftSecondaryValue = leftSecondaryValue;
        leftSecondaryValue = leftSecondaryBtn.action.ReadValue<float>();
        oldRightSecondaryValue = rightSecondaryValue;
        rightSecondaryValue = rightSecondaryBtn.action.ReadValue<float>();
        secondaryValue = Mathf.Max(leftSecondaryValue, rightSecondaryValue);

        oldLeftStickValue = leftStickValue;
        leftStickValue = leftStick.action.ReadValue<Vector2>();
        oldRightStickValue = rightStickValue;
        rightStickValue = rightStick.action.ReadValue<Vector2>();

        //Take Actions -----------------------------------------------------

        if (leftTriggerValue > 0.9f && oldLeftTriggerValue <= 0.9f)
        {
            leftTriggerDown();
        }
        //--
        if (rightTriggerValue > 0.9f && oldRightTriggerValue <= 0.9f)
        {
            rightTriggerDown();
        }
        //--
        if (leftTriggerValue <= 0.9f && oldLeftTriggerValue > 0.9f)
        {
            leftTriggerUp();
        }
        //--
        if (rightTriggerValue <= 0.9f && oldRightTriggerValue > 0.9f)
        {
            rightTriggerUp();
        }
        //--
        if (leftGripValue > 0.9f && oldLeftGripValue <= 0.9f)
        {
            leftGripDown();
        }
        //--
        if (rightGripValue > 0.9f && oldRightGripValue <= 0.9f)
        {
            rightGripDown();
        }
        //--
        if (leftPrimaryValue > 0.9f && oldLeftPrimaryValue <= 0.9f)
        {
            leftPrimaryDown();
        }
        //--
        if (rightPrimaryValue > 0.9f && oldRightPrimaryValue <= 0.9f)
        {
            rightPrimaryDown();
        }
        //--
        if (leftSecondaryValue > 0.9f && oldLeftSecondaryValue <= 0.9f)
        {
            leftSecondaryDown();
        }
        //--
        if (rightSecondaryValue > 0.9f && oldRightSecondaryValue <= 0.9f)
        {
            rightSecondaryDown();
        }
        //--
        if (rightStickValue.x > 0.9f && oldRightStickValue.x <= 0.9f)
        {
            rightStickToRight();
        }
        else if (rightStickValue.x < -0.9f && oldRightStickValue.x >= -0.9f)
        {
            rightStickToLeft();
        }
        //--
        if (leftStickValue.x > 0.9f && oldLeftStickValue.x <= 0.9f)
        {
            leftStickToRight();
        }
        else if (leftStickValue.x < -0.9f && oldLeftStickValue.x >= -0.9f)
        {
            leftStickToLeft();
        }
        //--

    }

    private void leftTriggerDown() 
    {
        foreach (Node node in leftHandNodes) node.changeInput();
        if (leftDroneHover) leftDroneHover.GetComponent<DroneCommand>().toggleHUD();
    }
    private void rightTriggerDown()
    {
        foreach (Node node in rightHandNodes) node.changeInput();
        if (rightDroneHover) rightDroneHover.GetComponent<DroneCommand>().toggleHUD();
    }
    private void leftTriggerUp()
    {
        if(leftLine) endLineDrag(true);
    }
    private void rightTriggerUp()
    {
        if (rightLine) endLineDrag(false);
    }

    private void leftGripDown()
    {
        if (leftDroneHover) leftDroneHover.GetComponent<DroneCommand>().pauseRoutineToggle();
        if (leftLine) leftLine.Toggle();
    }
    private void rightGripDown()
    {
        if (rightDroneHover) rightDroneHover.GetComponent<DroneCommand>().pauseRoutineToggle();
        if (rightLine) rightLine.Toggle();
    }

    private void leftPrimaryDown()
    {
        foreach (Node node in leftHandNodes) node.changeValType();
        if (leftLine) leftLine.Erase();
        else if (leftDroneHover) leftDroneHover.GetComponent<DroneCommand>().readProcess(true);
    }
    private void rightPrimaryDown()
    {
        foreach (Node node in rightHandNodes) node.changeValType();
        if (rightLine) rightLine.Erase();
        else if (rightDroneHover) rightDroneHover.GetComponent<DroneCommand>().readProcess(true);
    }

    private void leftSecondaryDown()
    {
        foreach (Node node in leftHandNodes) cloneNode(node); //??
    }
    private void rightSecondaryDown()
    {
        foreach (Node node in rightHandNodes) cloneNode(node); //??
    }
    private void cloneNode(Node node)
    {
        node.Clone();
    }

    private void leftStickToRight()
    {
        foreach (Node node in leftHandNodes) node.nextVal();
        StartCoroutine(delayedHoldStickLeftX());
    }
    private void leftStickToLeft()
    {
        foreach (Node node in leftHandNodes) node.prevVal();
        StartCoroutine(delayedHoldStickLeftX());
    }
    private void rightStickToRight()
    {
        foreach (Node node in rightHandNodes) node.nextVal();
        StartCoroutine(delayedHoldStickRightX());
    }
    private void rightStickToLeft()
    {
        foreach (Node node in rightHandNodes) node.prevVal();
        StartCoroutine(delayedHoldStickRightX());
    }


    IEnumerator delayedHoldStickRightX()
    {
        bool cancel = false;
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.15f);
            if (rightStickValue.x > -0.9f && rightStickValue.x < 0.9f) cancel = true;
        }
        yield return new WaitForSeconds(0.15f);
        while (!cancel)
        {
            if (rightStickValue.x > 0.9f) { foreach(Node n in rightHandNodes) n.nextVal(); }
            else if (rightStickValue.x < -0.9f) { foreach (Node n in rightHandNodes) n.prevVal(); }
            else break;
            yield return new WaitForSeconds(0.05f);
        }
    }
    IEnumerator delayedHoldStickLeftX()
    {
        bool cancel = false;
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.15f);
            if (leftStickValue.x > -0.9f && leftStickValue.x < 0.9f) cancel = true;
        }
        yield return new WaitForSeconds(0.15f);
        while (!cancel)
        {
            if (leftStickValue.x > 0.9f) { foreach (Node n in leftHandNodes) n.nextVal(); }
            else if (leftStickValue.x < -0.9f) { foreach (Node n in leftHandNodes) n.prevVal(); }
            else break;
            yield return new WaitForSeconds(0.05f);
        }
    }

}
