using UnityEngine;
using System.Collections;

public class MoveData
{
    public string name;
    public int pp;

    public MoveData()
    {

    }

    public MoveData(Move move)
    {
        name = move.MoveName;
        pp = move.CurrentPP;
    }
}
