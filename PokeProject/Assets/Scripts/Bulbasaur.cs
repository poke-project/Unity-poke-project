using UnityEngine;
using System.Collections;
using System;

public class Bulbasaur : APokemon {

    protected override int baseLootExp { get { return (64); }}
    protected override eGrowthRate growthRate
    {
        get
        {
            return (eGrowthRate.mediumSlow);
        }
    }
    protected override sStat baseStats
    {
       get
        {
            return (new sStat(45, 49, 49, 65, 65, 45));
        }
    }
    protected override sStat lootEvs
    {
        get
        {
            return (new sStat(0, 0, 0, 1, 0, 0));
        }
    }
    public override int numberEntry
    {
        get
        {
            return (1);
        }
    }
    public override string name
    {
        get
        {
            return ("Bulbasaur");
        }
    }
    public override string species
    {
        get
        {
            return ("Seed pokemon");
        }
    }
    public override float height
    {
        get
        {
            return (0.71f);
        }
    }
    public override float weight
    {
        get
        {
            return (6.9f);
        }
    }
    public override string description
    {
        get
        {
            return ("A strange seed was planted on its back at birth. The plant sprouts and grows with this POKEMON.");
        }
    }
    public override AType type1
    {
        get
        {
            return (new Grass());
        }
    }
    public override AType type2
    {
        get
        {
            return (new Poison());
        }
    }
    public override eAbility[] possibleAbilities
    {
        get
        {
            return (new eAbility[2] { eAbility.overgrow, eAbility.chlorophyll });
        }
    }
    public override float catchRate
    {
        get
        {
            return (45);
        }
    }
    public override bool canEvolve
    {
        get
        {
            return (true);
        }
    }
    public override float genderRepartition
    {
        get
        {
            return (87.5f);
        }
    }
    public override int evolutionNumber
    {
        get
        {
            return (2);
        }
    }

    public override Move[] moves
    {
        get
        {
            Move move1 = new Move("Tackle", 10);
            Move move2 = new Move("Growl", 5);
            Move move3 = new Move("Flamethrower", 25);
            return (new Move[4] { move1, move2, move3, null });
        }
    }
}
