using UnityEngine;
using System.Collections;

public class Trainer
{
    public Party party;
    public Bag bag;
    public int money;

    public Trainer()
    {
        party = new Party();
        bag = new Bag();
        money = 0;
    }
}
