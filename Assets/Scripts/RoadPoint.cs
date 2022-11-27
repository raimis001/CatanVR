using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPoint : MonoBehaviour, IInteractable
{
    public int id;

    public Transform road;
    public Transform roadCollider;

    public Material[] materials;

    public List<int> pieces;
    public List<int> roads;

    internal bool ready = false;
    internal string owner;

    bool visible = false;
    Renderer roadRenderer => road.GetComponent<Renderer>();

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
        if (Game.state != BuildState.Road)
        {
            road.gameObject.SetActive(false);
            roadCollider.gameObject.SetActive(false);
            return;
        }

        road.gameObject.SetActive(visible);
        roadCollider.gameObject.SetActive(visible);
        if (!visible)
            return;


        roadRenderer.material = materials[1];
    }

    public void OnEnter(XRHand hand)
    {
        if (ready)
            return;

        if (Game.state != BuildState.Road)
            return;

        roadRenderer.material = materials[CheckAllow() ? 2 : 3];
    }

    public void OnExit(XRHand hand)
    {
        if (ready)
            return;
        if (Game.state != BuildState.Road)
            return;

        roadRenderer.material = materials[1];
    }

    public void OnStay(XRHand hand)
    {
        if (ready)
            return;

        if (Game.state != BuildState.Road)
            return;

        if (!hand.TrigerPress)
            return;

        if (!CheckAllow())
            return;

        ready = true;
        roadRenderer.material = materials[0];
        roadCollider.gameObject.SetActive(false);
        owner = "Player 1";
    }

    private void Start()
    {
        road.gameObject.SetActive(false);
        roadCollider.gameObject.SetActive(false);
    }

    public void SwitchVisible(bool visible)
    {
        if (ready)
            return;

        this.visible = visible;

        if (Game.state != BuildState.Road)
            return;

        road.gameObject.SetActive(visible);
        roadCollider.gameObject.SetActive(visible);
        if (!visible)
            return;


        roadRenderer.material = materials[1];
    }

    bool CheckAllow()
    {
        bool allow = false;

        foreach (BuildPoint point in Board.points)
        {
            if (point.state == BuildState.None)
                continue;

            int i = 0;
            foreach (int p in pieces)
            {
                if (point.pieces.Contains(p))
                    i++;
            }
            if (i < 2)
                continue;

            allow = true;
        }
        if (allow)
            return true;

        //Check roads
        foreach (RoadPoint road in Board.roads)
        {
            if (road.id == id)
                continue;

            if (!road.ready)
                continue;

            if (!road.roads.Contains(id))
                continue;

            allow = true;
            break;
        }
        
        return allow;
    }
}
