using UnityEngine;
using System.Collections;
using System;
using System.Xml.Serialization;

public class Party : IMySerializable
{
    [XmlIgnore]
    public APokemon[] pokemons;
    [XmlIgnore]
    public int nbInParty;

    public Party()
    {
        pokemons = new APokemon[6];
        nbInParty = 0;
        addPokemonInParty(new Bulbasaur());
    }

    public void loadFromData(IData data)
    {
        PartyData partyData = (PartyData)data;
        Debug.Log("Load from data");
        nbInParty = partyData.pokemons.Length;
        pokemons = new APokemon[nbInParty];
        for (int i = 0; i < nbInParty; ++i)
        {
            pokemons[i] = new Bulbasaur(partyData.pokemons[i]);
        }
    }

    public Party(PartyData data)
    {
        loadFromData(data);
    }
    
    public void addPokemonInParty(APokemon newPokemon)
    {
        if (nbInParty < 6)
        {
            pokemons[nbInParty] = newPokemon;
            nbInParty++;
        }
        else
        {
            // Send to storage
        }
    }

    public APokemon getFirstPokemonReady()
    {
        for (int i = 0; i < nbInParty; ++i)
        {
            if (pokemons[i].currentStats.hp > 0)
            {
                return (pokemons[i]);
            }
        }
        Debug.Log("No pokemon ready for battle");
        return (null);
    }

    public override string ToString()
    {
        string ret = "";
        for (int i = 0; i < nbInParty; ++i)
        {
            ret += pokemons[i].name + " " + pokemons[i].currentStats.ToString() + "\n";
        }
        return (ret);
    }
}
