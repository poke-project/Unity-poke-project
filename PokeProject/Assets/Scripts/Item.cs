using UnityEngine;
using System.Collections;

public class Item
{
    public string name { get; private set; }
    public int nbHeld;
    public bool usableInFight;

    public Item() { }

    public Item(string name)
    {
        this.name = name;
        nbHeld = 1;
        usableInFight = true;
    }

    public Item(Item source)
    {
        name = source.name;
        nbHeld = 1;
        usableInFight = source.usableInFight;
    }

    public void use()
    {
        Debug.Log("Do stuff");
    }
}
