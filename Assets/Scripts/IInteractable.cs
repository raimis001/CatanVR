using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void OnEnter(XRHand hand);
    void OnExit(XRHand hand);
    void OnStay(XRHand hand);
}
