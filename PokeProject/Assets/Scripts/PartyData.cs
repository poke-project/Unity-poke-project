using UnityEngine;
using System.Collections;

public class PartyData
{
    public PokemonData[] pokemons;

    public PartyData()
    {

    }

    public PartyData(Party party)
    {
        pokemons = new PokemonData[party.nbInParty];
        for (int i = 0; i < pokemons.Length; ++i)
        {
            pokemons[i] = new PokemonData(party.pokemons[i]);
        }
    }
}
