using UnityEngine;
using System.Collections;
using System;
using System.Xml.Serialization;

public class Party
{
    [XmlIgnore]
    public APokemon[] pokemons;
    [XmlIgnore]
    public int nbInParty;

    public int toto;
    public PartyData partyData
    {
        get { return (new PartyData(this)); }
        set { }
    }

    public Party()
    {
        pokemons = new APokemon[6];
        nbInParty = 0;
    }

    public Party(PartyData data)
    {
        Debug.Log("IIICCIII");
        nbInParty = data.pokemons.Length;
        pokemons = new APokemon[nbInParty];
        for (int i = 0; i < nbInParty; ++i)
        {
            pokemons[i] = new Bulbasaur(data.pokemons[i]);
            Debug.Log(pokemons[i].currentStats.hp);
        }
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
