using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRControllers : MonoBehaviour
{

    static Controllers controller;

    static internal Vector2 LeftJoystick => controller.Left.Joystick.ReadValue<Vector2>();
    static internal Vector2 RightJoystick => controller.Right.Joystick.ReadValue<Vector2>();

    static internal bool LeftTriggerPress => controller.Left.Trigger.IsPressed();
    static internal bool RightTriggerPress => controller.Right.Trigger.IsPressed();
    
    static internal bool LeftGripPress => controller.Left.Grip.IsPressed();
    static internal bool RightGripPress => controller.Right.Grip.IsPressed();
    static internal bool AnyGripPress => LeftGripPress || RightGripPress;

    private void Awake()
    {
        controller = new Controllers();
    }

    private void OnEnable()
    {
        controller.Enable();
    }
    private void OnDisable()
    {
        controller.Disable();
    }
}
