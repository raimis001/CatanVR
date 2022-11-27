using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = 0; i < 150; i++)
        {
            int j = Random.Range(0, list.Count);
            int k = Random.Range(0, list.Count);
            T kind = list[k];
            list[k] = list[j];
            list[j] = kind;
        }
    }
}


public class Board : MonoBehaviour
{
    public Transform buildParent;
    public Transform roadParent;
    readonly List<Piece> pieces = new List<Piece>();
    readonly List<PieceKind> kinds = new List<PieceKind>()
    {
        PieceKind.Clay,
        PieceKind.Clay,
        PieceKind.Clay,
        PieceKind.Mountains,
        PieceKind.Mountains,
        PieceKind.Mountains,
        PieceKind.Fields,
        PieceKind.Fields,
        PieceKind.Fields,
        PieceKind.Fields,
        PieceKind.Forest,
        PieceKind.Forest,
        PieceKind.Forest,
        PieceKind.Forest,
        PieceKind.Meadows,
        PieceKind.Meadows,
        PieceKind.Meadows,
        PieceKind.Meadows,
        PieceKind.Desert,
    };

    internal static readonly List<BuildPoint> points = new List<BuildPoint>();
    internal static readonly List<RoadPoint> roads = new List<RoadPoint>();

    void Awake()
    {
        pieces.AddRange(GetComponentsInChildren<Piece>());
        points.AddRange(buildParent.GetComponentsInChildren<BuildPoint>());
        roads.AddRange(roadParent.GetComponentsInChildren<RoadPoint>());

        kinds.Shuffle();

        List<int> numbers = new List<int>();
        numbers.Add(2);
        numbers.Add(12);
        for (int i = 3; i < 12; i++)
        {
            if (i == 7)
                continue;

            numbers.Add(i);
            numbers.Add(i);
        }

        numbers.Shuffle();

        int ii = 0;
        int jj = 0;
        foreach (Piece piece in pieces)
        {
            if (!piece.isRandom)
                continue;


            piece.SetKind(kinds[ii++]);

            piece.number = piece.kind != PieceKind.Desert ? numbers[jj++] : 7;
        }
    }

    public void SwitchPoint(int pieceID, bool visible)
    {
        foreach (BuildPoint point in points)
        {
            if (!point.pieces.Contains(pieceID))
                continue;

            point.SwitchVisible(visible);
        }
        foreach (RoadPoint road in roads)
        {
            if (!road.pieces.Contains(pieceID))
                continue;

            road.SwitchVisible(visible);
        }
    }

    public IEnumerable<BuildPoint> GetPieces(int pieceID)
    {
        foreach (BuildPoint point in points)
        {
            if (!point.pieces.Contains(pieceID))
                continue;

            yield return point;
        }
    }
}
