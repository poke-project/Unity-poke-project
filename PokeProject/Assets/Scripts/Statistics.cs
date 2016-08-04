using UnityEngine;
using System.Collections;

public class Statistics
{
    public int hp;
    public int att;
    public int def;
    public int attSpe;
    public int defSpe;
    public int speed;
    // Reset at each fight
    public float evasion;
    public float accuracy;

    public Statistics()
    {

    }

    public Statistics(int hp, int att, int def, int attSpe, int defSpe, int speed, float evasion, float accuracy)
    {
        this.hp = hp;
        this.att = att;
        this.def = def;
        this.attSpe = attSpe;
        this.defSpe = defSpe;
        this.speed = speed;
        this.evasion = evasion;
        this.accuracy = accuracy;
    }

    public Statistics(Statistics other)
    {
        hp = other.hp;
        att = other.att;
        def = other.def;
        attSpe = other.attSpe;
        defSpe = other.defSpe;
        speed = other.speed;
        evasion = other.evasion;
        accuracy = other.accuracy;
    }

    public override string ToString()
    {
        return ("hp : " + hp.ToString() +  " att : " + att.ToString() +  " def : "
            + def.ToString() +  " attSpe : " + attSpe.ToString()
            + " defSpe : " + defSpe.ToString() + " speed : " + speed.ToString()
            + " evasion : " + evasion.ToString() + " accuracy : " + accuracy.ToString());
    }
    public bool hasStatEffect()
    {
        return ((att != 0) || (def != 0) || (attSpe != 0) || (defSpe != 0) || (speed != 0));
    }

    public static Statistics operator +(Statistics s1, Statistics s2)
    {
        return (new Statistics(s1.hp + s2.hp,
            s1.att + s2.att,
            s1.def + s2.def,
            s1.attSpe + s2.attSpe,
            s1.defSpe + s2.defSpe,
            s1.speed + s2.speed,
            s1.evasion + s2.evasion,
            s1.accuracy + s2.accuracy));
    }

    public string stateStageModification()
    {
        string ret = "";
        int modif = 0;
        if (att != 0)
        {
            ret = " attack";
            modif = att;
        }
        else if (def != 0)
        {
            ret = " defense";
            modif = def;
        }
        else if (attSpe != 0)
        {
            ret = " attackSpe";
            modif = attSpe;
        }
        else if (defSpe != 0)
        {
            ret = " defenseSpe";
            modif = defSpe;
        }
        else if (speed != 0)
        {
            ret = " speed";
            modif = speed;
        }
        else if (evasion != 0)
        {
            ret = " evasion";
            modif = (int)evasion;
        }
        else if (accuracy != 0)
        {
            ret = " accuracy";
            modif = (int)accuracy;
        }
        switch (modif)
        {
            case -2:
                ret += " greatly fell!";
                break;
            case -1:
                ret += " fell!";
                break;
            case 1:
                ret += " rose!";
                break;
            case 2:
                ret += " greatly rose!";
                break;

            default:
                Debug.Log("Should not be here");
                break;
        }
        return (ret);
    }

    private int capStatHigh(ref int stat, int limit)
    {
        if (stat > limit)
        {
            stat = limit;
            return (1);
        }
        return (0);
    }

    private int capStatLow(ref int stat, int limit)
    {
        if (stat < limit)
        {
            stat = limit;
            return (1);
        }
        return (0);
    }



    private int capStatHigh(ref float stat, int limit)
    {
        if (stat > limit)
        {
            stat = limit;
            return (1);
        }
        return (0);
    }

    private int capStatLow(ref float stat, int limit)
    {
        if (stat < limit)
        {
            stat = limit;
            return (1);
        }
        return (0);
    }

    public bool capStatsStages()
    {
        int limitHit = 0;
        limitHit += capStatHigh(ref att, 6);
        limitHit += capStatHigh(ref def, 6);
        limitHit += capStatHigh(ref attSpe, 6);
        limitHit += capStatHigh(ref defSpe, 6);
        limitHit += capStatHigh(ref speed, 6);
        limitHit += capStatHigh(ref evasion, 6);
        limitHit += capStatHigh(ref accuracy, 6);

        limitHit += capStatLow(ref att, -6);
        limitHit += capStatLow(ref def, -6);
        limitHit += capStatLow(ref attSpe, -6);
        limitHit += capStatLow(ref defSpe, -6);
        limitHit += capStatLow(ref speed, -6);
        limitHit += capStatLow(ref evasion, -6);
        limitHit += capStatLow(ref accuracy, -6);
        return (limitHit != 0);
    }
}
