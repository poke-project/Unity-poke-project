using UnityEngine;
using System.Collections;

public class Trainer
{
    public string name;
    public Party party;
    public Bag bag;
    public int money;

    public Trainer()
    {
        name = "Trainer";
        party = new Party();
        bag = new Bag();
        money = 0;
    }
}
