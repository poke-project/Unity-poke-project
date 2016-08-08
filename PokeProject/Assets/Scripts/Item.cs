using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void applyEffect(APokemon target);

public class Item
{
    public string name { get; private set; }
    public int nbHeld;
    public bool usableInFight { get; private set; }
    public applyEffect use;

    // May do an inherited class for pokeball
    public bool isPokeball { get; private set; }
    public float ballValue { get; private set; }
    public int ballMod { get; private set; }

    public Item() { }

    public Item(string name)
    {
        this.name = name;
        nbHeld = 1;
        usableInFight = false;
    }

    public Item(string name, applyEffect effect, bool usableInFight, bool isPokeball)
    {
        this.name = name;
        use = effect;
        nbHeld = 1;
        this.usableInFight = true;
        this.isPokeball = isPokeball;
        setCatchRate();
    }

    public Item(Item source)
    {
        name = source.name;
        nbHeld = 1;
        usableInFight = source.usableInFight;
        use = source.use;
        isPokeball = source.isPokeball;
        setCatchRate();
    }

    private void setCatchRate()
    {
        if (isPokeball)
        {
            switch (name)
            {
                case "PokeBall":
                    ballValue = 12;
                    ballMod = 255;
                    break;

                case "GreatBall":
                    ballValue = 8;
                    ballMod = 200;
                    break;

                case "UltaBall":
                    ballValue = 12;
                    ballMod = 150;
                    break;

                case "MasterBall":
                    break;

                default:
                    Debug.Log("Should not be here");
                    break;
            }
        }
        else
        {
        }
    }

}
