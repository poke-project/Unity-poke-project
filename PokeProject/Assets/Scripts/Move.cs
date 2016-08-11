using UnityEngine;
using System.Collections;

public partial class Move : IMySerializable
{
    public string MoveName { get; private set; }
    public int MaxPP { get; private set; }
    public int CurrentPP { get; set; }

    public Statistics EnemyEffect { get; private set; }
    public Statistics SelfEffect { get; private set; }
    public eStatus EnemyStatus { get; private set; }
    public eStatus SelfStatus { get; private set; }
    public AType Type;
    // 8 for certains moves else 1
    public float CriticalChanceModifier { get; private set; }
    public float Accuracy { get; private set; }

    public Move()
    {
    }

    // Adapt constructor
    public Move(string name, int pp, Statistics enemyEffect, Statistics selfEffect, eStatus enemyStatus, eStatus selfStatus, AType type, float critChance, float accuracy)
    {
        MoveName = name;
        MaxPP = pp;
        CurrentPP = MaxPP;
        EnemyEffect = enemyEffect;
        SelfEffect = selfEffect;
        EnemyStatus = enemyStatus;
        SelfStatus = selfStatus;
        Type = type;
        CriticalChanceModifier = critChance;
        Accuracy = accuracy;
    }
    
    public Move(string name)
    {
        MoveName = name;
        setAttributesFromName(MoveName);
        CurrentPP = MaxPP;
    }

    public Move(MoveData data)
    {
        MoveName = data.name;
        setAttributesFromName(data.name);
        CurrentPP = data.pp;
        throw new System.Exception("Do not use data constructor");
    }

    public void loadFromData(IData data)
    {
        MoveData moveData = (MoveData)data;
        MoveName = moveData.name;
        setAttributesFromName(MoveName);
        CurrentPP = moveData.pp;
    }

    public int use()
    {
        if (CurrentPP > 0)
        {
            CurrentPP--;
        }
        else
        {
            Debug.Log("not enough pp");
            return (0);
        }
        return (1);
    }
}
