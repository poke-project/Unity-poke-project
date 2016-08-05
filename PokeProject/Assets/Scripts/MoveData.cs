using UnityEngine;
using System.Collections;

public class MoveData : IData
{
    public string name;
    public int pp;

    public MoveData()
    {

    }

    public MoveData(Move move)
    {
        populate(move);
    }

    public void populate(IMySerializable source)
    {
        Move move = (Move)source;
        name = move.MoveName;
        pp = move.CurrentPP;
    }
}
