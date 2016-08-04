using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PokemonData
{
    public string name;
    public string species;
    public Statistics ivs;
    public Statistics evs;
    public Statistics currentStats;
    public int exp;
    public int lvl;
    public eStatus status;
    public List<MoveData> moves;

    public PokemonData() { }

    public PokemonData(APokemon pokemon)
    {
        name = pokemon.name;
        species = pokemon.GetType().ToString();
        ivs = pokemon.Ivs;
        evs = pokemon.Evs;
        currentStats = pokemon.currentStats;

        exp = pokemon.exp;
        lvl = pokemon.lvl;

        status = pokemon.status;

        moves = new List<MoveData>();
        foreach (Move move in pokemon.moves)
        {
            if (move != null)
            {
                moves.Add(move.moveData);
            }
        }
    }
}
