using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPoint : MonoBehaviour, IInteractable
{
    public Renderer point;
    public Material[] materials;
    public List<int> pieces;

    internal BuildState state = BuildState.None;
    internal string owner;
    bool visible = false;

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
        if (state == BuildState.Village || state == BuildState.City)
            return;

        if (Game.state != BuildState.Village)
        {
            point.gameObject.SetActive(false);
            return;
        }

        point.gameObject.SetActive(visible);
    }

    public void SwitchVisible(bool visible)
    {
        if (state == BuildState.Village || state == BuildState.City)
            return;

        this.visible = visible;
        if (Game.state != BuildState.Village)
            return;
        
        point.gameObject.SetActive(visible);
    }

    public void OnEnter(XRHand hand)
    {
        point.material = materials[1];
    }

    public void OnExit(XRHand hand)
    {
        point.material = materials[0];
    }

    public void OnStay(XRHand hand)
    {
        if (state != BuildState.None)
            return;

        if (!point.gameObject.activeInHierarchy)
            return;

        if (Game.state != BuildState.Village)
            return;

        if (!hand.TrigerPress)
            return;


        Game.OnBuildPress(transform);
        state = Game.state;
        
        owner = "Player1";//TODO: MP change

        point.gameObject.SetActive(false);

    }
}
