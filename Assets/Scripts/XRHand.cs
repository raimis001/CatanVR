using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class XRHand : MonoBehaviour
{
    public XRNode node;
    public Transform rayPoint;
    public float rayLenght = 1;
    public LayerMask rayMask;

    public GameObject[] visuals;

    public Vector2 Joystick => node == XRNode.LeftHand ? XRControllers.LeftJoystick : XRControllers.RightJoystick;
    public bool TrigerPress => node == XRNode.LeftHand ? XRControllers.LeftTriggerPress : XRControllers.RightTriggerPress;
    public bool GripPress => node == XRNode.LeftHand ? XRControllers.LeftGripPress : XRControllers.RightGripPress;


    IInteractable selected = null;

    private void OnEnable()
    {
        Game.OnStateChange += OnStateChange;
    }
    private void OnDisable()
    {
        Game.OnStateChange -= OnStateChange;
    }

    void OnStateChange()
    {
        //Debug.LogFormat("{0} - {1}", Game.state, (int)Game.state);
        for (int i = 0; i < visuals.Length; i++)
        {
            visuals[i].SetActive(i == (int)Game.state);
        }
    }

    private void Update()
    {
        if (!Physics.Raycast(rayPoint.position, rayPoint.forward, out RaycastHit hit, rayLenght, rayMask))
        {
            if (selected != null)
                selected.OnExit(this);
            selected = null;
            return;
        }
        IInteractable find = hit.collider.GetComponentInParent<IInteractable>();
        if (find == null)
        {
            if (selected != null)
                selected.OnExit(this);
            selected = null;
            return;
        }

        if (selected == null)
        {
            selected = find;
            selected.OnEnter(this);
            return;
        }
        if (selected == find)
        {
            selected.OnStay(this);
            return;
        }

        selected.OnExit(this);
        selected = find;
        selected.OnEnter(this);

    }
}
