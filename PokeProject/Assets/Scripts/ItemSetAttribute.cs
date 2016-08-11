using UnityEngine;
using System.Collections;

public partial class Item : IMySerializable
{
    private void setAttributesFromName(string name)
    {
        applyEffect effect = target => Debug.Log(target.name);
        // Could nest function in default case of previous switch
        if (!switchPotion(name, ref effect))
        {
            if (!switchDrink(name, ref effect))
            {
                if (!switchRevive(name, ref effect))
                {
                    if (!switchEther(name, ref effect))
                    {
                        if (!switchPokeball(name))
                        {
                            if (!switchStatusHealer(name, ref effect))
                            {
                                if (!switchVitamin(name, ref effect))
                                {
                                    if (!switchBattleItem(name, ref effect))
                                    {
                                        // TODO
                                        // Key item or held item
                                    }
                                }
                            }
                        }
                    }
                }
            }
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

    private bool switchPotion(string name, ref applyEffect effect)
    {
        switch (name)
        {
            case "Potion":
                effect = target => target.damageReceived = -20;
                usableInFight = true;
                isPokeball = false;
                return (true);

            case "Super Potion":
                effect = target => target.damageReceived = -50;
                usableInFight = true;
                isPokeball = false;
                return (true);

            case "Hyper Potion":
                effect = target => target.damageReceived = -200;
                usableInFight = true;
                isPokeball = false;
                return (true);

            case "Max Potion":
                effect = target => target.damageReceived = -(target.stats.hp);
                usableInFight = true;
                isPokeball = false;
                return (true);

            case "Full Restore":
                effect = target =>
                {
                    target.damageReceived = -(target.stats.hp);
                    target.status = eStatus.NORMAL;
                };
                usableInFight = true;
                isPokeball = false;
                return (true);

            default:
                return (false);
        }
    }
    
    private bool switchEther(string name, ref applyEffect effect)
    {
        switch (name)
        {
            case "Ether":
                // TODO
                // Restore 10 pp of one move
                return (true);

            case "Max Ether":
                // TODO
                // Restore all pp of one move
                return (true);

            case "Elixir":
                effect = target =>
                {
                    foreach (Move move in target.moves)
                    {
                        if (move != null)
                        {
                            move.CurrentPP += 10;
                            if (move.CurrentPP > move.MaxPP)
                            {
                                move.CurrentPP = move.MaxPP;
                            }
                        }
                    }
                };
                usableInFight = true;
                isPokeball = false;
                return (true);

            case "Max Elixir":
                effect = target =>
                {
                    foreach (Move move in target.moves)
                    {
                        if (move != null)
                        {
                            Debug.Log("lalala");
                            move.CurrentPP = move.MaxPP;
                        }
                    }
                };
                usableInFight = true;
                isPokeball = false;
                return (true);

            default:
                return (false);
        }
    }

    private bool switchDrink(string name, ref applyEffect effect)
    {
        switch (name)
        {
            case "Fresh Water":
                effect = target => target.damageReceived = -50;
                usableInFight = true;
                isPokeball = false;
                return (true);

            case "Soda Pop":
                effect = target => target.damageReceived = -60;
                usableInFight = true;
                isPokeball = false;
                return (true);

            case "Lemonade":
                effect = target => target.damageReceived = -80;
                usableInFight = true;
                isPokeball = false;
                return (true);

            default:
                return (false);
        }
    }

    private bool switchRevive(string name, ref applyEffect effect)
    {
        switch (name)
        {
            case "Revive":
                effect = target =>
                {
                    if (target.currentStats.hp == 0)
                    {
                        target.damageReceived = -(target.stats.hp / 2);
                    }
                };
                usableInFight = true;
                isPokeball = false;
                return (true);

            case "Max Revive":
                effect = target =>
                {
                    if (target.currentStats.hp == 0)
                    {
                        target.damageReceived = -(target.stats.hp);
                    }
                };
                usableInFight = true;
                isPokeball = false;
                return (true);

            default:
                return (false);
        }
    }

    private bool switchPokeball(string name)
    {
        switch (name)
        {
            case "PokeBall":
            case "GreatBall":
            case "UltraBall":
            case "MasterBall":
                usableInFight = true;
                isPokeball = true;
                use = null;
                return (true);

            default:
                return (false);
        }
    }

    private bool switchStatusHealer(string name, ref applyEffect effect)
    {
        switch (name)
        {
            case "Antidote":
                effect = target =>
                {
                    if (target.status == eStatus.POISONED)
                    {
                        target.status = eStatus.NORMAL;
                    }
                };
                usableInFight = true;
                isPokeball = false;
                return (true);

            case "Paralyze Heal":
                effect = target =>
                {
                    if (target.status == eStatus.PARALIZED)
                    {
                        target.status = eStatus.NORMAL;
                    }
                };
                usableInFight = true;
                isPokeball = false;
                return (true);

            case "Awakening":
                effect = target =>
                {
                    if (target.status == eStatus.SLEEPING)
                    {
                        target.status = eStatus.NORMAL;
                    }
                };
                usableInFight = true;
                isPokeball = false;
                return (true);

            case "Burn Heal":
                effect = target =>
                {
                    if (target.status == eStatus.BURNED)
                    {
                        target.status = eStatus.NORMAL;
                    }
                };
                usableInFight = true;
                isPokeball = false;
                return (true);

            case "Ice Heal":
                effect = target =>
                {
                    if (target.status == eStatus.FROZEN)
                    {
                        target.status = eStatus.NORMAL;
                    }
                };
                usableInFight = true;
                isPokeball = false;
                return (true);
                
            case "Full Heal":
                effect = target =>
                {
                    if (target.status != eStatus.CONFUSED)
                    {
                        target.status = eStatus.NORMAL;
                    }
                };
                usableInFight = true;
                isPokeball = false;
                return (true);

            default:
                return (false);
        }
    }

    private bool switchVitamin(string name, ref applyEffect effect)
    {
        Statistics evGain = new Statistics(0, 0, 0, 0, 0, 0, 0, 0);
        switch (name)
        {
            case "HP Up":
                evGain.hp = 10;
                break;

            case "Protein":
                evGain.att = 10;
                break;
            case "Iron":
                evGain.def = 10;
                break;
            case "Calcium":
                evGain.attSpe = 10;
                break;
            case "Zinc":
                evGain.defSpe = 10;
                break;
            case "Carbos":
                evGain.speed = 10;
                break;

            case "PP Up":
                //TODO
                // Raises pp of a move by 20% of base
                // Up to 3 uses
                return (true);

            case "Rare Candy":
                effect = target =>
                {
                    if (target.lvl < 100)
                    {
                        target.lvl++;
                        target.damageReceived -= 50;
                    }
                };
                usableInFight = false;
                isPokeball = false;
                return (true);

            default:
                return (false);
        }
        usableInFight = false;
        isPokeball = false;
        effect = target => target.receiveEvs(evGain, 100);
        return (true);
    }

    private bool switchBattleItem(string name, ref applyEffect effect)
    {
        Statistics stats = new Statistics(0, 0, 0, 0, 0, 0, 0, 0);
        switch (name)
        {
            case "X Attack":
                stats.att = 1; ;
                break;

            case "X Defense":
                stats.def = 1;
                break;

            case "X Sp. Atk":
                stats.attSpe = 1;
                break;

            case "X Sp. Def":
                stats.defSpe = 1;
                break;

            case "X Speed":
                stats.speed = 1;
                break;

            case "X Accuracy":
                // TODO
                // target ignore accuracy checks
                return (true);

            case "Dire Hit":
                // TODO
                // raise critical hit ratio
                return (true);

            case "Guard Spec":
                // TODO
                // prevent stats reduction on party for 5 turns
                return (true);

            default:
                return (false);
        }
        usableInFight = true;
        isPokeball = false;
        effect = target => FightSceneManager.instance.moveStatsProcess(stats, target);
        return (true);
    }
}