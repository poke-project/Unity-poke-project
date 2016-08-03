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
}
