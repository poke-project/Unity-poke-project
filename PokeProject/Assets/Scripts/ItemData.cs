using UnityEngine;
using System.Collections;

public class ItemData : IData
{
    public string name;
    public int nbHeld;

    public ItemData()
    {

    }

    public ItemData(Item item)
    {
        populate(item);
    }

    public void populate(IMySerializable source)
    {
        Item item = (Item)source;
        name = item.name;
        nbHeld = item.nbHeld;
    }
}
