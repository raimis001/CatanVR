using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum PieceKind
{
    Ocean = 0,
    Mountains = 1,
    Forest = 2,
    Meadows = 3,
    Clay = 4,
    Fields = 5,
    Desert = 6,
}

[ExecuteInEditMode]
public class Piece : MonoBehaviour
{
    public PieceKind kind;
    public int id;
    public bool isRandom = true;

    internal List<string> owners;

    int _number;
    public int number { get => _number; set { _number = value; numberText.text = value.ToString(); } }
    public TMP_Text numberText;

    public List<GameObject> kindObjects;
    PieceKind _kind;
    internal Board board;


    bool _selected;
    internal bool selected { 
        get => _selected; 
        set { 
            _selected = value;
            numberText.gameObject.SetActive(value);
            board.SwitchPoint(id, value);
        } 
    }

    private void Awake()
    {
        board = GetComponentInParent<Board>();
    }

    private void Start()
    {
        if (kind == PieceKind.Ocean)
            numberText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!Application.isEditor)
            return;

        if (_kind != kind)
        {
            _kind = kind;
            SetKind(_kind);
        }
    }

    public void SetKind(PieceKind k)
    {
        kind = k;
        for (int i = 0; i < kindObjects.Count; i++)
        {
            kindObjects[i].SetActive(i == (int)kind);
        }
        if (kind == PieceKind.Ocean)
            numberText.gameObject.SetActive(false);

    }

    public IEnumerable<BuildPoint> Points()
    {
        foreach (BuildPoint p in board.GetPieces(id))
            yield return p;
    }
}
