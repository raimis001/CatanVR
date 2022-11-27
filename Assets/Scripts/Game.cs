using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum BuildState
{
    None = 0, 
    Village = 1, 
    City = 2, 
    Road = 3
}

public class Game : MonoBehaviour
{
    public static Game instance;
    static BuildState _state = BuildState.None;
    public static BuildState state { 
        get => _state; 
        set {
            _state = value;
            OnStateChange.Invoke();
        } 
    }
    public static Action OnStateChange;

    public TMP_Text pieceName;
    public TMP_Text pieceBuildings;

    public TMP_Text[] diceTexts;


    public LayerMask pieceLayer;

    public GameObject villagePrefab;

    public Dice[] dices;
    public Animator diceAnim;

    Transform xrCamera;

    Piece curentPiece;

    int diceCount;
    int diceValue;
    bool diceSpinned;

    private void Awake()
    {
        instance = this;
        xrCamera = Camera.main.transform;

        foreach (TMP_Text txt in diceTexts)
            txt.text = "";
    }

    private void Update()
    {
        

        if (state != BuildState.None && XRControllers.AnyGripPress)
            state = BuildState.None;

        ProcessPiece();

    }

    void ProcessPiece()
    {
        pieceName.text = "";
        pieceBuildings.text = "";

        Ray ray = new Ray(xrCamera.position, Vector3.down);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, pieceLayer))
        {
            if (curentPiece)
                curentPiece.selected = false;
            curentPiece = null;
            return;
        }
        Piece piece = hit.collider.GetComponentInParent<Piece>();
        if (!piece)
        {
            if (curentPiece)
                curentPiece.selected = false;
            curentPiece = null;
            return;
        }

        pieceName.text = string.Format("{0} ({1})",piece.kind, piece.number);
        

        foreach (BuildPoint p in piece.Points())
        {
            if (p.state == BuildState.None)
                continue;

            pieceBuildings.text = pieceBuildings.text.Insert(0, string.Format("{0}: {1}\n", p.state, p.owner));
        }

        if (!curentPiece)
        {
            curentPiece = piece;
            curentPiece.selected = true;
            return;
        }

        if (curentPiece == piece)
        {
            return;
        }

        curentPiece.selected = false;
        curentPiece = piece;
        curentPiece.selected = true;

    }

    public static void OnBuildPress(Transform point)
    {
        if (state == BuildState.None)
            return;

        GameObject h = Instantiate(instance.villagePrefab, point.position, point.rotation);

    }

    public static void StartDices(XRHand hand)
    {
        if (instance.diceSpinned)
            return;

        if (!hand.TrigerPress)
            return;

        instance.diceCount = 0;
        instance.diceValue = 0;
        instance.diceSpinned = true;
        foreach (TMP_Text txt in instance.diceTexts)
            txt.text = "";
        foreach (Dice dice in instance.dices)
        {
            dice.Spin();
        }
        instance.diceAnim.SetBool("Open", true);
    }

    public void OnDiceEnd(int id, int number)
    {
        diceTexts[id].text = number.ToString();
        diceValue += number;
        diceCount++;
        if (diceCount > 1)
        {
            diceTexts[2].text = diceValue.ToString();
            diceSpinned = false;
            //diceAnim.SetBool("Open", false);
        }
    }
}
