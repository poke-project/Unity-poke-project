using UnityEngine;
using System.Collections;
using System;

public enum eStat
{
    HP,
    ATT,
    DEF,
    ATTSPE,
    DEFSPE,
    SPEED
}

public enum eStatus
{
    BURNED,
    FROZEN,
    PARALIZED,
    POISONED,
    SLEEPING,
    CONFUSED,
    NORMAL,
    NULL
}


abstract public class APokemon : IMySerializable
{
    // Not in gen I
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

    public Statistics Ivs { get; private set; }

    // Readonly ?
    // Pokedex data (same for one given Pokemon species)
    public abstract int BaseLootExp { get; }
    protected abstract eGrowthRate growthRate { get; }
    protected abstract Statistics baseStats { get; }
    public abstract Statistics lootEvs { get; }

    public abstract int numberEntry { get; }
    public abstract string speciesName { get; }
    public abstract string species { get; }
    public abstract float height { get; }
    public abstract float weight { get; }
    public abstract string description { get; }
    public abstract AType type1 { get; }
    public abstract AType type2 { get; }
    public abstract eAbility[] possibleAbilities { get; }
    public abstract int catchRate { get; }
    public abstract bool canEvolve { get; }
    public abstract float genderRepartition { get; }
    public abstract int evolutionNumber { get; }


    // Statistics (different for each instance)
    public eStatus status;
    public Statistics stats;
    public Statistics Evs { get; private set; }
    public eAbility ability
    {
        get
        {
            return (possibleAbilities[new System.Random().Next(0, possibleAbilities.Length)]);
        }
    }
    public Move[] moves;
    public Move move1;// { get; set; }
    public Move move2;// { get; set; }
    public Move move3;// { get; set; }
    public Move move4;//{ get; set; }

    // 0 == female 1 == genderless 2 == male
    public int gender;
    public int exp = 0;
    public int lvl = 5;
    public int expThreshold;
    public string name;    

    public Statistics currentStats;

    // In Fight only
    public Statistics statsStages;
    public int confusionTurns = 0;
    public int damageReceived = 0;
    public bool isEnemy = false;

    public APokemon()
    {
        setIv();
        Evs = new Statistics(0, 0, 0, 0, 0, 0, 0, 0);
        stats = new Statistics(baseStats);
        updateAllStats();
        updateThreshold();
        currentStats = new Statistics(stats);
        status = eStatus.NORMAL;
        moves = new Move[4] { move1, move2, move3, move4 };
        gender = 2;
        name = speciesName;
    }

    public APokemon(PokemonData data)
    {
        loadFromData(data);
    }

    public void loadFromData(IData data)
    {
        PokemonData pokemonData = (PokemonData)data;
        stats = new Statistics(0, 0, 0, 0, 0, 0, 0, 0);
        name = pokemonData.name;
        Ivs = pokemonData.ivs;
        Evs = pokemonData.evs;
        currentStats = pokemonData.currentStats;
        exp = pokemonData.exp;
        lvl = pokemonData.lvl;
        status = pokemonData.status;
        updateAllStats();
        updateThreshold();
        move1 = createMoveFromData(pokemonData.moves[0]);
        move2 = createMoveFromData(pokemonData.moves[1]);
        move3 = createMoveFromData(pokemonData.moves[2]);
        move4 = createMoveFromData(pokemonData.moves[3]);
        moves = new Move[4] { move1, move2, move3, move4 };
    }

    private Move createMoveFromData(MoveData data)
    {
        if (data != null)
        {
            Move ret = new Move();
            ret.loadFromData(data);
            return (ret);
        }
        else
        {
            return (null);
        }
    }

    private void levelUp(int expGain)
    {
        if (lvl >= 100)
            return;
        lvl++;
        exp = 0;
        int hpBefore = stats.hp;
        updateAllStats();
        currentStats.hp += (stats.hp - hpBefore);
        applyStagesMultipliers(); 
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

    public void restoreCurrentStats()
    {
        currentStats.att = stats.att;
        currentStats.def = stats.def;
        currentStats.attSpe = stats.attSpe;
        currentStats.defSpe = stats.defSpe;
        currentStats.speed = stats.speed;
    }

    public void applyStagesMultipliers()
    {
        currentStats.att = (int)(stats.att * FightSceneManager.stageMultipliers[statsStages.att]);
        currentStats.def = (int)(stats.def * FightSceneManager.stageMultipliers[statsStages.def]);
        currentStats.attSpe = (int)(stats.attSpe * FightSceneManager.stageMultipliers[statsStages.attSpe]);
        currentStats.defSpe = (int)(stats.defSpe * FightSceneManager.stageMultipliers[statsStages.defSpe]);
        currentStats.speed = (int)(stats.speed * FightSceneManager.stageMultipliers[statsStages.speed]);
        currentStats.evasion = (int)(100f * FightSceneManager.stageMultipliers[-(int)statsStages.evasion]);
        currentStats.accuracy = (int)(100f * FightSceneManager.stageMultipliers[(int)statsStages.accuracy]);
    }

    private void updateAllStats()
    {
        updateStat(ref stats.hp, eStat.HP);
        updateStat(ref stats.att, eStat.ATT);
        updateStat(ref stats.def, eStat.DEF);
        updateStat(ref stats.attSpe, eStat.ATTSPE);
        updateStat(ref stats.defSpe, eStat.DEFSPE);
        updateStat(ref stats.speed, eStat.SPEED);
    }

    // Use formula from generation III
    private void updateStat(ref int stat, eStat statType)
    {
        int baseStat = getBaseStat(statType);
        int iv = getIvForStat(statType);
        int ep = getEvForStat(statType) / 4;

        if (statType != eStat.HP)
        {
            stat = (((((2 * baseStat) + iv + ep) * lvl) / 100) + 5);
        }
        else
        {
            stat = ((((2 * baseStat) + iv + ep) * lvl) / 100) + lvl + 10;
        }
    }

    public void receiveExp(int expGain)
    {
        exp += expGain;
        if (exp >= expThreshold)
        {
            levelUp(exp - expThreshold);
        }
    }

    public void receiveEvs(Statistics evsGain, int cap)
    {
        int diff = 510 - getTotalEvs();
        if (diff > 0)
        {
            addEvWithCap(ref Evs.hp, evsGain.hp, diff, cap);
            addEvWithCap(ref Evs.att, evsGain.att, diff, cap);
            addEvWithCap(ref Evs.def, evsGain.def, diff, cap);
            addEvWithCap(ref Evs.attSpe, evsGain.attSpe, diff, cap);
            addEvWithCap(ref Evs.defSpe, evsGain.defSpe, diff, cap);
            addEvWithCap(ref Evs.speed, evsGain.speed, diff, cap);
        }
    }

    private void addEvWithCap(ref int ev, int gain, int diff, int cap)
    {
        if (gain > diff)
        {
            gain = diff;
        }
        int sum = ev + gain;
        if (ev < cap)
        {
            ev = sum > cap ? cap : sum;
        }
    }

    private int getTotalEvs()
    {
        return (Evs.hp + Evs.att + Evs.def + Evs.attSpe + Evs.defSpe + Evs.speed);
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
        Ivs = new Statistics(UnityEngine.Random.Range(0, 32),
            UnityEngine.Random.Range(0, 32),
            UnityEngine.Random.Range(0, 32),
            UnityEngine.Random.Range(0, 32),
            UnityEngine.Random.Range(0, 32),
            UnityEngine.Random.Range(0, 32),
            0,
            0);
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

    public void initInBattleStats()
    {
        currentStats.evasion = 100f;
        currentStats.accuracy = 100f;
        statsStages = new Statistics(0, 0, 0, 0, 0, 0, 0, 0);
    }
}
