using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBuild : MonoBehaviour, IInteractable
{
    public BuildState state;

    public Material[] materials;


    public void OnEnter(XRHand hand)
    {
        GetComponent<Renderer>().material = materials[1];
    }

    public void OnExit(XRHand hand)
    {
        GetComponent<Renderer>().material = materials[0];
    }

    public void OnStay(XRHand hand)
    {
        if (Game.state != BuildState.None)
            return;

        if (!hand.TrigerPress)
            return;

        Game.state = state;

    }
}
