using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [HideInInspector] public float leftGripValue;
    [HideInInspector] public float rightGripValue;
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
    [HideInInspector] public float leftSecondaryValue;
    [HideInInspector] public float rightSecondaryValue;
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

    private void Update()
    {
        //Read Inputs -----------------------------------------------------
        leftGripValue = leftGripBtn.action.ReadValue<float>();
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

        leftSecondaryValue = leftSecondaryBtn.action.ReadValue<float>();
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
    }
    private void rightTriggerDown()
    {
        foreach (Node node in rightHandNodes) node.changeInput();
    }

    private void leftPrimaryDown()
    {
        foreach (Node node in leftHandNodes) node.changeValType();
    }
    private void rightPrimaryDown()
    {
        foreach (Node node in rightHandNodes) node.changeValType();
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
