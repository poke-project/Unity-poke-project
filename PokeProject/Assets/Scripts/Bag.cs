using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bag
{
    public List<Item> items;
    public bool isEmpty;

    public Bag()
    {
        items = new List<Item>();
        isEmpty = true;
    }

    public void addItem(Item newItem)
    {
        isEmpty = false;
        if (items.Count > 20)
        {
            Debug.Log("Not enough slots in bag");
            return;
        }
        if (items.Count == 0)
        {
            items.Add(newItem);
            return;
        }
        for (int i = 0; i < items.Count; ++i)
        {
            if (items[i].name == newItem.name && items[i].nbHeld < 99)
            {
                items[i].nbHeld++;
                break;
            }
            else if ((i == (items.Count - 1)))
            {
                items.Add(newItem);
                break;
            }
        }
    }

    public void removeItem(Item item)
    {
        int indexLessHeld = 0;
        int min = 100;
        for (int i = 0; i < items.Count; ++i)
        {
            if (items[i].name == item.name)
            {
                if (items[i].nbHeld <= min)
                {
                    min = items[i].nbHeld;
                    indexLessHeld = i;
                }
            }
        }
        items[indexLessHeld].nbHeld--;
        if (items[indexLessHeld].nbHeld == 0)
        {
            items.RemoveAt(indexLessHeld);
        }
        if (items.Count == 0)
        {
            isEmpty = true;
        }
    }

    public void useItem(Item item)
    {
        item.use();
        removeItem(item);
    }

    public void useItem(int index)
    {
        useItem(items[index]);
    }
}
