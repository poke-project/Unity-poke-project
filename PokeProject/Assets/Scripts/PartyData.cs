using UnityEngine;
using System.Collections;


public class PartyData : IData
{
    public PokemonData[] pokemons;

    public PartyData()
    {

    }

    public PartyData(Party party)
    {
        populate(party);
    }

    public void populate(IMySerializable source)
    {
        Party party = (Party)source;
        pokemons = new PokemonData[party.nbInParty];
        for (int i = 0; i < pokemons.Length; ++i)
        {
            pokemons[i] = new PokemonData(party.pokemons[i]);
        }
    }
}