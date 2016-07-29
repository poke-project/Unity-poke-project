using UnityEngine;
using System.Collections;

public class Move
{
    public string MoveName { get; private set; }
    public int MaxPP { get; private set; }
    public int CurrentPP { get; private set; }

    public sStat EnemyEffect { get; private set; }
    public sStat SelfEffect { get; private set; }
    public eStatus enemyStatus { get; private set; }
    public eStatus selfStatus { get; private set; }
    public AType type;

    // Adapt constructor
    public Move(string name, int pp)
    {
        MoveName = name;
        MaxPP = pp;
        CurrentPP = MaxPP;
    }

    public void use()
    {
        if (CurrentPP > 0)
        {
            Debug.Log("ici");
            CurrentPP--;
        Debug.Log(CurrentPP);
        }
        else
        {
            Debug.Log("not enough pp");
        }
        Debug.Log(MoveName);
    }
}
