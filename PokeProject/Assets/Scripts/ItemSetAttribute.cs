using UnityEngine;
using System.Collections;

public partial class Item : IMySerializable
{
    private void setAttributesFromName(string name)
    {
        applyEffect effect = target => Debug.Log(target.name);
        switch (name)
        {
            case "Potion":
                effect = target => target.damageReceived = -20;
                usableInFight = true;
                isPokeball = false;
                break;

            case "Super Potion":
                effect = target => target.damageReceived = -50;
                usableInFight = true;
                isPokeball = false;
                break;

            case "Hyper Potion":
                effect = target => target.damageReceived = -200;
                usableInFight = true;
                isPokeball = false;
                break;

            case "Max Potion":
                effect = target => target.damageReceived = -(target.stats.hp);
                usableInFight = true;
                isPokeball = false;
                break;

            case "Full Restore":
                effect = target =>
                {
                    target.damageReceived = -(target.stats.hp);
                    target.status = eStatus.NORMAL;
                };
                usableInFight = true;
                isPokeball = false;
                break;

            case "PokeBall":
            case "GreatBall":
            case "UltraBall":
            case "MasterBall":
                usableInFight = true;
                isPokeball = true;
                use = null;
                break;

            default:
                Debug.Log("Should not be here");
                break;
        }

        if (!isPokeball)
        {
            ballValue = 0;
            ballMod = 0;
            use = effect;
        }
        else
        {
            setCatchRate();
        }
    }
}
