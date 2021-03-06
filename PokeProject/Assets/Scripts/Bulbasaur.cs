﻿using UnityEngine;
using System.Collections;
using System;

public class Bulbasaur : APokemon {

    public Bulbasaur()
    {
        move1 = new Move("Tackle");
        move2 = new Move("blabla");
        moves = new Move[4] { move1, move2, move3, move4 };
    }

    public Bulbasaur(PokemonData data) : base(data)
    {
    }

    public override int BaseLootExp { get { return (64); }}
    protected override eGrowthRate growthRate
    {
        get
        {
            return (eGrowthRate.mediumSlow);
        }
    }
    protected override Statistics baseStats
    {
       get
        {
            return (new Statistics(45, 49, 49, 65, 65, 45, 100, 100));
        }
    }
    public override Statistics lootEvs
    {
        get
        {
            return (new Statistics(0, 0, 0, 1, 0, 0, 0, 0));
        }
    }
    public override int numberEntry
    {
        get
        {
            return (1);
        }
    }
    public override string speciesName
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
    public override int catchRate
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

    /*public override Move move1
    {
        get
        {
            return new Move("Tackle");
        }
        set
        {
        }
    }

    public override Move move2
    {
        get
        {
            return new Move("blabla");
        }
        set
        {
        }
    }

    public override Move move3
    {
        get
        {
            return (null);
        }
        set
        {
        }
    }

    public override Move move4
    {
        get
        {
            return (null);
        }
        set
        {
        }
    }*/
}
