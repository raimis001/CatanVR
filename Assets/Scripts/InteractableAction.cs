using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableAction : MonoBehaviour, IInteractable
{
    public UnityEvent<XRHand> OnEnterAction;
    public UnityEvent<XRHand> OnExitAction;
    public UnityEvent<XRHand> OnStayAction;

    public void OnEnter(XRHand hand)
    {
        OnEnterAction.Invoke(hand);
    }

    public void OnExit(XRHand hand)
    {
        OnExitAction.Invoke(hand);
    }

    public void OnStay(XRHand hand)
    {
        OnStayAction.Invoke(hand);
    }

}
