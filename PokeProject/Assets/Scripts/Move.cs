using UnityEngine;
using System.Collections;

public class Move
{
    private string moveName;
    private int maxPP;
    private int currentPP;

    public Move(string name, int pp)
    {
        moveName = name;
        maxPP = pp;
        currentPP = maxPP;
    }

    public string getMoveName()
    {
        return (moveName);
    }

    public int getCurrentPP()
    {
        return (currentPP);
    }

    public int getMaxPP()
    {
        return (maxPP);
    }
}
