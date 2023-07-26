using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [HideInInspector] public float leftTriggerValue;
    [HideInInspector] public float rightTriggerValue;
    [HideInInspector] public float triggerValue;

    [SerializeField] InputActionProperty leftPrimaryBtn;
    [SerializeField] InputActionProperty rightPrimaryBtn;
    [HideInInspector] public float leftPrimaryValue;
    [HideInInspector] public float rightPrimaryValue;
    [HideInInspector] public float primaryValue;

    [SerializeField] InputActionProperty leftSecundaryBtn;
    [SerializeField] InputActionProperty rightSecundaryBtn;
    [HideInInspector] public float leftSecondaryValue;
    [HideInInspector] public float rightSecondaryValue;
    [HideInInspector] public float secondaryValue;
    #endregion

    private void Start()
    {
    }

    private void Update()
    {
        leftGripValue = leftGripBtn.action.ReadValue<float>();
        rightGripValue = rightGripBtn.action.ReadValue<float>();
        GripValue = Mathf.Max(leftGripValue, rightGripValue);

        leftTriggerValue = leftTriggerBtn.action.ReadValue<float>();
        rightTriggerValue = rightTriggerBtn.action.ReadValue<float>();
        triggerValue = Mathf.Max(leftTriggerValue, rightTriggerValue);

        leftPrimaryValue = leftPrimaryBtn.action.ReadValue<float>();
        rightPrimaryValue = rightPrimaryBtn.action.ReadValue<float>();
        primaryValue = Mathf.Max(leftPrimaryValue, rightPrimaryValue);

        leftSecondaryValue = leftSecundaryBtn.action.ReadValue<float>();
        rightSecondaryValue = rightSecundaryBtn.action.ReadValue<float>();
        secondaryValue = Mathf.Max(leftSecondaryValue, rightSecondaryValue);

    }
}
