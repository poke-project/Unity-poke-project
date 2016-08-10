using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class FightSceneManager
{
    public static Dictionary<int, float> stageMultipliers
    {
        get
        {
            return (new Dictionary<int, float>()
            {
                { -6, 0.25f }, { -5, 0.28f }, { -4, 0.33f }, { -3, 0.4f }, { -2, 0.5f }, { -1, 0.6f },
                { 0, 1f },
                { 1, 1.5f }, { 2, 2f }, { 3, 2.5f }, { 4, 3f }, { 5, 3.5f }, { 6, 4f }
            });
        }
        private set { }
    }


    private bool hitCheck(Move move, APokemon user, APokemon target)
    {
        int hitProbability = (int)(move.Accuracy * (user.currentStats.accuracy / target.currentStats.evasion));
        if (!(hitProbability >= 100 || Random.Range(0, 100) < hitProbability))
        {
            texts.Add(prefix + user.name + " missed !");
            return (false);
        }
        return (true);
    }

    private void getAttackAndDefense(Statistics userStats, Statistics targetStats, out float userAttack, out float targetDefense, bool isMovePhysical)
    {
        if (isMovePhysical)
        {
            userAttack = userStats.att;
            targetDefense = targetStats.def;
        }
        else
        {
            userAttack = userStats.attSpe;
            targetDefense = targetStats.defSpe;
        }
    }

    private float findDmgsModifier(APokemon user, APokemon target, Move move, float userSpeed, out bool isCritical)
    {
        // same type attack bonus : 1.5 if same type as user
        float stab;
        // type effectiveness
        float typeModifier;
        // critical hit bonus : 2 if critical
        float critical;
        int critProbability;
        // items / abilities bonuses
        float other;

        if (move.Type.GetType() == user.type1.GetType() || move.Type.GetType() == user.type2.GetType())
        {
            stab = 1.5f;
        }
        else
        {
            stab = 1f;
        }
        typeModifier = move.Type.dmgsModifier(target.type1) * move.Type.dmgsModifier(target.type2);
        // Could be better with shuffle bag
        critProbability = (int)(userSpeed / (512f / move.CriticalChanceModifier));
        //
        // Critical hit should ignore modifier from burn and stat modifiers
        //
        if (Random.Range(0, 100) < critProbability)
        {
            critical = (2 * user.lvl + 5) / (user.lvl + 5);
            isCritical = true;
        }
        else
        {
            critical = 1f;
            isCritical = false;
        }

        // TODO
        other = 1f;

        return (stab * typeModifier * critical * other * Random.Range(0.85f, 1f));
    }


    private void inflictDamages(Move move, APokemon user, APokemon target)
    {
        bool isCritical;
        Statistics userTurnStat;
        Statistics targetTurnStat;

        float modifier = findDmgsModifier(user, target, move, user.currentStats.speed, out isCritical);
        if (isCritical)
        {
             userTurnStat = user.stats;
             targetTurnStat = target.stats;
        }
        else
        {
            userTurnStat = user.currentStats;
            targetTurnStat = target.currentStats;
            if (user.status == eStatus.BURNED)
                userTurnStat.att /= 2;
        }

        // user attack or attackSpe stat
        float userAttack;
        // target defense or defenseSpe stat
        float targetDefense;
        getAttackAndDefense(userTurnStat, targetTurnStat, out userAttack, out targetDefense,
            move.Type.isPhysical());

        // final dmgs
        int dmgs;
        dmgs = (int)(((((2 * (float)user.lvl) + 10) / 250) * (userAttack / targetDefense)
            * move.EnemyEffect.hp + 2) * modifier);
        target.damageReceived = dmgs;
    }

    private void moveDamagesProcess(Move move, APokemon user, APokemon target)
    {
        if (move.EnemyEffect.hp != 0)
        {
            inflictDamages(move, user, target);
        }
        if (move.SelfEffect.hp != 0)
        {
            inflictDamages(move, user, user);
        }
    }

    private void moveStatsProcess(Statistics effect, APokemon target)
    {
        if (effect.hasStatEffect())
        {
            target.statsStages += effect;
            if (target.statsStages.capStatsStages())
            {
                texts.Add("Nothing happened!");
                return;
            }
            target.applyStagesMultipliers();
            texts.Add((target.isEnemy ? "Foe " : "") + target.name + "'s" + effect.stateStageModification());
        }
    }

    private bool paralysisProcess(APokemon user)
    {
        if (user.status == eStatus.PARALIZED)
        {
            user.currentStats.speed = user.stats.speed / 4;
            if (Random.Range(0, 100) < 25)
            {
                texts.Add(user.name + " is fully paralyzed and cannot attack");
                return (false);
            }
        }
        else
        {
            user.currentStats.speed = user.stats.speed;
        }
        return (true);
    }

    private bool confusionProcess(APokemon user)
    {
        if (user.status == eStatus.CONFUSED)
        {
            if (user.confusionTurns == 0)
            {
                user.status = eStatus.NORMAL;
            }
            else
            {
                user.confusionTurns--;
                if (Random.Range(0, 100) < 50)
                {
                    int confusionDmgs = (int)(((((2 * (float)user.lvl) + 10) / 250) * (user.currentStats.att / user.currentStats.def) * 40 + 2) * Random.Range(0.85f, 1f));
                    texts.Add(prefix + user.name + " is confused !");
                    texts.Add(prefix + user.name + " hurts itself !");
                    user.damageReceived = confusionDmgs;
                    return (false);
                }
            }
        }
        return (true);
    }

    private void moveProcess(APokemon user, APokemon target, int moveSelected)
    {
        Move usedMove = user.moves[moveSelected - 1];

        if (usedMove.use() == 0)
        {
            return;
        }
        if (user.isEnemy)
        {
            prefix = "Foe ";
            usedMove.CurrentPP++;
        }
        else
        {
            prefix = "";
        }
        prefix = user.isEnemy ? "Foe " : "";

        // Add trapped and partially trapped
        if (!paralysisProcess(user)
            || !confusionProcess(user)
            || !hitCheck(usedMove, user, target))
        {
            return;
        }
        texts.Add(prefix + user.name + " used " + usedMove.MoveName.ToUpper() + "!");
        moveStatsProcess(usedMove.EnemyEffect, target);
        moveStatsProcess(usedMove.SelfEffect, user);
        moveDamagesProcess(usedMove, user, target);
    }

    private void statusEffect(APokemon pokemon)
    {
        // TODO Special case badly poisoned (toxic)
        if (pokemon.status == eStatus.BURNED || pokemon.status == eStatus.POISONED)
        {
            // Does not inflict dmgs if enemy KO
            pokemon.damageReceived = (pokemon.stats.hp / 16);
            texts.Add(prefix + pokemon.name + "'s hurt by the burn!");
        }
    }

    IEnumerator moveWrapper(APokemon user, APokemon target, int moveSelected)
    {
        user.damageReceived = 0;
        target.damageReceived = 0;
        // Move related effects
        moveProcess(user, target, moveSelected);
        yield return StartCoroutine(startDialogue());
        yield return StartCoroutine(updateHp(target));
        yield return StartCoroutine(updateHp(user));
        // Status related effects
        statusEffect(user);
        yield return StartCoroutine(startDialogue());
        yield return StartCoroutine(updateHp(user));
    }


}
