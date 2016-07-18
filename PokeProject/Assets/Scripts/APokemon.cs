using UnityEngine;
using System.Collections;
using System;

abstract public class APokemon
{
    public enum eNature
    {
        bold,
        quirky,
        brave,
        calm,
        quiet,
        mild,
        rash,
        gentle,
        hardy,
        jolly,
        lax,
        impish,
        sassy,
        naughty,
        modest,
        naive,
        hasty,
        careful,
        bashful,
        relaxed,
        adamant,
        serious,
        lonely,
        timid
    };
    public enum eAbility
    {
        overgrow,
        chlorophyll
        // TODO
    }
    public enum eGrowthRate
    {
        erratic,
        fast,
        mediumFast,
        mediumSlow,
        slow,
        fluctuating
    }
    public enum eStat
    {
        HP,
        ATT,
        DEF,
        ATTSPE,
        DEFSPE,
        SPEED
    }
    public struct sStat
    {
        public sStat(int hp, int att, int def, int attSpe, int defSpe, int speed)
        {
            this.hp = hp;
            this.att = att;
            this.def = def;
            this.attSpe = attSpe;
            this.defSpe = defSpe;
            this.speed = speed;
        }
        public int hp;
        public int att;
        public int def;
        public int attSpe;
        public int defSpe;
        public int speed;
    }

    // UI
    protected abstract string frontSprite { get; }
    protected abstract string backSprite { get; }

    // Base stats, constants
    protected abstract int baseLootExp { get; }
    protected abstract eGrowthRate growthRate { get; }
    protected abstract sStat baseStats { get; }
    private sStat Ivs;
    protected abstract sStat lootEvs { get; }


    // Readonly ?
    // Pokedex data (same for one given Pokemon species)
    public abstract int numberEntry { get; }
    public abstract string name { get; }
    public abstract string species { get; }
    public abstract float height { get; }
    public abstract float weight { get; }
    public abstract string description { get; }
    public abstract AType type1 { get; }
    public abstract AType type2 { get; }
    public abstract eAbility[] possibleAbilities { get; }
    public abstract float catchRate { get; }
    public abstract bool canEvolve { get; }
    public abstract float genderRepartition { get; }
    public abstract int evolutionNumber { get; }


    // Statistics (different for each instance)
    public sStat stats;
    private sStat Evs;
    public int lootExp;
    public eNature nature;
    public eAbility ability
    {
        get
        {
            return (possibleAbilities[new System.Random().Next(0, possibleAbilities.Length)]);
        }
    }
    public Move[] moves;

    // 0 == male 1 == female
    public int gender;
    public int exp = 0;
    public int lvl = 1;
    public int expThreshold;

    private void levelUp(int expGain)
    {
        if (lvl >= 100)
            return;
        lvl++;
        exp = 0;
        updateStat(ref stats.hp, eStat.HP);
        updateStat(ref stats.att, eStat.ATT);
        updateStat(ref stats.def, eStat.DEF);
        updateStat(ref stats.attSpe, eStat.ATTSPE);
        updateStat(ref stats.defSpe, eStat.DEFSPE);
        updateStat(ref stats.speed, eStat.SPEED);
        //updateStat(lootExp);
        updateThreshold();
        receiveExp(expGain);
    }

    private void updateThreshold()
    {
        int poweredLvl = lvl * lvl * lvl;

        switch (growthRate)
        {
            case eGrowthRate.erratic:
                if (lvl <= 50)
                {
                    expThreshold = (poweredLvl * (100 - lvl)) / 50;
                }
                else if (lvl <= 68)
                {
                    expThreshold = (poweredLvl * (150 - lvl)) / 100;
                }
                else if (lvl <= 98)
                {
                    expThreshold = (poweredLvl * ((1911 - (10 * lvl)) / 3)) / 500;
                }
                else
                {
                    expThreshold = (poweredLvl * (160 - lvl)) / 100;
                }
                break;
            case eGrowthRate.fast:
                expThreshold = (int)(0.8f * poweredLvl);
                break;
            case eGrowthRate.mediumFast:
                expThreshold = poweredLvl;
                break;
            case eGrowthRate.mediumSlow:
                if (lvl != 1)
                {
                    expThreshold = (int)(1.2f * poweredLvl - (15 * lvl * lvl) + (100 * lvl) - 140);
                }
                else
                {
                    expThreshold = 5;
                }
                break;
            case eGrowthRate.slow:
                expThreshold = (int)(1.25f * poweredLvl);
                break;
            case eGrowthRate.fluctuating:
                if (lvl <= 15)
                {
                    expThreshold = poweredLvl * ((((lvl + 1) / 3) + 24) / 50);
                }
                else if (lvl <= 36)
                {
                    expThreshold = poweredLvl * ((lvl + 14) / 50);
                }
                else
                {
                    expThreshold = poweredLvl * (((lvl / 2) + 32) / 50);
                }
                break;
            default:
                Debug.Log("Should not be here");
                break;
        }
    }

    private void updateStat(ref int stat, eStat statType)
    {
        int baseStat = getBaseStat(statType);
        int iv = getIvForStat(statType);
        int ep = getEvForStat(statType) / 4;
        // TODO
        int natureModifier = 1;

        if (statType != eStat.HP)
        {
            stat = (((((2 * baseStat) + iv + ep) * lvl) / 100) + 5) * natureModifier;
        }
        else
        {
            stat = ((((2 * baseStat) + iv + ep) * lvl) / 100) + lvl + 10;
        }
    }

    private void receiveExp(int expGain)
    {
        // Apply modifier here ?
        exp += expGain;
        if (exp >= expThreshold)
        {
            levelUp(exp - expThreshold);
        }
    }

    private int getEvForStat(eStat statType)
    {
        switch (statType)
        {
            case eStat.HP:
                return (Evs.hp);
            case eStat.ATT:
                return (Evs.att);
            case eStat.DEF:
                return (Evs.def);
            case eStat.ATTSPE:
                return (Evs.attSpe);
            case eStat.DEFSPE:
                return (Evs.defSpe);
            case eStat.SPEED:
                return (Evs.speed);
            default:
                Debug.Log("Should not be here");
                return (-1);
        }
    }

    private int getIvForStat(eStat statType)
    {
        switch (statType)
        {
            case eStat.HP:
                return (Ivs.hp);
            case eStat.ATT:
                return (Ivs.att);
            case eStat.DEF:
                return (Ivs.def);
            case eStat.ATTSPE:
                return (Ivs.attSpe);
            case eStat.DEFSPE:
                return (Ivs.defSpe);
            case eStat.SPEED:
                return (Ivs.speed);
            default:
                Debug.Log("Should not be here");
                return (-1);
        }
    }

    private void setIv()
    {
        Ivs.hp = UnityEngine.Random.Range(0, 32);
        Ivs.att = UnityEngine.Random.Range(0, 32);
        Ivs.def = UnityEngine.Random.Range(0, 32);
        Ivs.attSpe = UnityEngine.Random.Range(0, 32);
        Ivs.defSpe = UnityEngine.Random.Range(0, 32);
        Ivs.speed = UnityEngine.Random.Range(0, 32);
    }

    private int getBaseStat(eStat statType)
    {
        switch (statType)
        {
            case eStat.HP:
                return (baseStats.hp);
            case eStat.ATT:
                return (baseStats.att);
            case eStat.DEF:
                return (baseStats.def);
            case eStat.ATTSPE:
                return (baseStats.attSpe);
            case eStat.DEFSPE:
                return (baseStats.defSpe);
            case eStat.SPEED:
                return (baseStats.speed);
            default:
                Debug.Log("Should not be here");
                return (-1);
        }
    }
}
