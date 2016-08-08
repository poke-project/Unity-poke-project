using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void applyEffect(APokemon target);

public class Item
{
    public string name { get; private set; }
    public int nbHeld;
    public bool usableInFight;

    public applyEffect useItem;

    public Item() { }

    public Item(string name)
    {
        this.name = name;
        nbHeld = 1;
        usableInFight = false;
    }

    public Item(string name, applyEffect effect, bool usableInFight)
    {
        this.name = name;
        useItem = effect;
        nbHeld = 1;
        this.usableInFight = true;
    }

    public Item(Item source)
    {
        name = source.name;
        nbHeld = 1;
        usableInFight = source.usableInFight;
    }

    public void use()
    {
    }
}
