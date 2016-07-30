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
    // 8 for certains moves else 1
    public int criticalChanceModifier { get; private set; }

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
            CurrentPP--;
        }
        else
        {
            Debug.Log("not enough pp");
        }
        Debug.Log(MoveName);
    }
}
