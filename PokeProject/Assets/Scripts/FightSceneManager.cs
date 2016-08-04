using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

public enum eMode
{
    MENU = 0,
    FIGHT,
    BAG,
    POKEMON,
    RUN
}

public class FightSceneManager : MonoBehaviour {


    public static FightSceneManager instance;

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
    public APokemon player;
    public APokemon enemy;
    public Dictionary<string, Sprite> numbers;
    public Dictionary<string, Sprite> status;
    [HideInInspector]
    public Sprite blank;
    [HideInInspector]
    public int currentSelection;
    [HideInInspector]
    public eMode currentMode;
    public string dialogueText;
    private List<string> texts;
    public bool isTrainerBattle;

    private APokemon faintedPokemon;
    private float internalTime;
    private bool inDialogue;
    private float nbParticipatedPokemon;
    private string prefix;

    void Awake()
    {
        instance = this;

        loadNumbersInDic();
        loadStatusInDic();
        blank = Resources.Load<Sprite>("Sprites/blank");

        // For test purpose
        player = new Bulbasaur();

        // REMOVE
        player.currentStats.speed *= 2;
        player.stats.speed *= 2;
        // END REMOVE

        player.initInBattleStats();
        enemy = new Bulbasaur();
        enemy.initInBattleStats();
        enemy.isEnemy = true;
        currentSelection = 1;
        currentMode = eMode.MENU;
        internalTime = 0.0f;
        inDialogue = false;
        nbParticipatedPokemon = 1f;
        texts = new List<string>();

    }

	// Use this for initialization
	void Start () {
	
	}
	
    private void endTurn()
    {
        dialogueText = "";
        texts.Clear();
        currentMode = eMode.MENU;
        inDialogue = false;
    }

    IEnumerator updateHp(APokemon pokemon)
    {
        while (pokemon.damageReceived > 0)
        {
            pokemon.currentStats.hp--;
            if (pokemon.currentStats.hp == 0)
            {
                StopAllCoroutines();
                faintedPokemon = pokemon;
                StartCoroutine(pokemonFaintProcess(pokemon));
                break;
            }
            pokemon.damageReceived--;
            yield return null;
        }
    }

    IEnumerator updateXp(int expGain)
    {
        int expChunk = (expGain / 200);
        if (expChunk == 0)
            expChunk = 1;
        while (expGain > 0)
        {
            if (expGain >= expChunk)
            {
                player.receiveExp(expChunk);
            }
            else
            {
                player.receiveExp(expGain);
            }
            expGain -= expChunk;
            yield return null;
        }
    }

    IEnumerator waitForEndDialogue()
    {
        bool negateFirstInput = true;
        while (negateFirstInput || !Input.GetKeyDown(KeyCode.Space))
        {
            negateFirstInput = false;
            yield return null;
        }
    }

    IEnumerator waitForEndDialogue(string text)
    {
        dialogueText = text;
        yield return waitForEndDialogue();
    }

    IEnumerator startDialogue()
    {
        foreach (string text in texts)
        {
            yield return StartCoroutine(waitForEndDialogue(text));
        }
        texts.Clear();
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

    IEnumerator runTurn(APokemon first, APokemon second, int moveSelected)
    {
        inDialogue = true;
        yield return StartCoroutine(moveWrapper(first, second, moveSelected));
        yield return StartCoroutine(moveWrapper(second, first, moveSelected));
        endTurn();
    }

    // Globalize to give exp to all participating pokemon
    IEnumerator pokemonFaintProcess(APokemon pokemon)
    {
        yield return StartCoroutine(waitForEndDialogue((pokemon.isEnemy ? "Foe " : "") + pokemon.name + " fainted!"));
        if (pokemon.isEnemy)
        {
            int expGain = findExpGain(pokemon);
            expGain = 5000;
            print(expGain);
            texts.Add(player.name + " gained " + expGain.ToString() + " EXP. Points!");
            yield return StartCoroutine(startDialogue());
            yield return StartCoroutine(updateXp(expGain));
        }
        else
        {
            // Choose other pokemon
        }
        endTurn();
        yield return null;
    }

    private int findExpGain(APokemon fainted)
    {
        float trainerBattleModifier;
        int exp;

        trainerBattleModifier = (isTrainerBattle ? 1.5f : 1f);
        // Different formula if Object "Exp. All" in bag
        // For participating pokemons :
        // exp = (trainerBattleModifier * fainted.lootExp * fainted.lvl) / (7 * (nbParticipatedPokemon * 2))
        // FOr other :
        // exp = (trainerBattleModifier * fainted.lootExp * fainted.lvl) / (7 * (nbParticipatedPokemon * 2 * nbPokemonParty))
        exp = (int)(((trainerBattleModifier * fainted.BaseLootExp * fainted.lvl)) / (7 * nbParticipatedPokemon));
        return (exp);
    }

    // Remove after test
    private void controlStatus(APokemon pokemon)
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            pokemon.status = eStatus.BURNED;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            pokemon.status = eStatus.FROZEN;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            pokemon.status = eStatus.PARALIZED;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            pokemon.status = eStatus.POISONED;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            pokemon.status = eStatus.SLEEPING;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            pokemon.status = eStatus.NORMAL;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            pokemon.status = eStatus.CONFUSED;
            pokemon.confusionTurns = 1;
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
        texts.Add(prefix + user.name + " used " + move.MoveName.ToUpper() + "!");
        target.damageReceived = dmgs;
    }

    private void moveProcess(APokemon user, APokemon target, int moveSelected)
    {
        Move usedMove = user.moves[moveSelected - 1];

        if (usedMove.use() == 0)
        {
            return;
        }
        prefix = user.isEnemy ? "Foe " : "";

        // Add trapped and partially trapped
        if (!paralysisProcess(user)
            || !confusionProcess(user)
            || !hitCheck(usedMove, user, target))
        {
            return;
        }
        moveStatsProcess(usedMove.EnemyEffect, target);
        moveStatsProcess(usedMove.SelfEffect, user);
        moveDamagesProcess(usedMove, user, target);
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

	void Update () {
        // block user input during dialogue
        if (inDialogue)
            return;
        updateSelection();
        controlStatus(player);
        controlStatus(enemy);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (currentMode)
            {
                case eMode.MENU:
                    menuActions();
                    break;

                case eMode.FIGHT:
                    print(player.currentStats.speed);
                    print(enemy.currentStats.speed);
                    if (player.currentStats.speed > enemy.currentStats.speed
                        || (player.currentStats.speed == enemy.currentStats.speed
                            && Random.Range(0, 100) < 50))
                    {
                        StartCoroutine(runTurn(player, enemy, currentSelection));
                    }
                    else
                    {
                        StartCoroutine(runTurn(enemy, player, currentSelection));
                    }
                    break;

                default:
                    print("Should not be here");
                    break;
            }
            currentSelection = 1;
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            currentSelection = 1;
            currentMode = eMode.MENU;
        }
        internalTime += Time.deltaTime;
	}


    private void menuActions()
    {
        switch ((eMode)currentSelection)
        {
            case eMode.MENU:
                break;

            case eMode.FIGHT:
                print("FIGHT");
                break;

            case eMode.BAG:
                print("BAG");
                break;

            case eMode.POKEMON:
                print("POKEMON");
                break;

            case eMode.RUN:
                Application.LoadLevel("ImplementBasicAction");
                print("RUN");
                break;

            default:
                print("Should not be here");
                break;
        }
        currentMode = (eMode)currentSelection;
    }

    private void updateSelection()
    {
        int nextSelection = currentSelection;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            nextSelection -= 2;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            nextSelection += 2;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            nextSelection -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            nextSelection += 1;
        }
        if (nextSelection > 0 && nextSelection <= 4)
        {
            if (!(currentMode == eMode.FIGHT && player.moves[nextSelection - 1] == null))
            {
                currentSelection = nextSelection;
            }
        }
    }

    private void loadNumbersInDic()
    {
        numbers = new Dictionary<string, Sprite>();
        Sprite[] tmp = Resources.LoadAll<Sprite>("Sprites/Numbers");
        foreach (Sprite sprite in tmp)
        {
            numbers.Add(sprite.name, sprite);
        }
    }

    private void loadStatusInDic()
    {
        status = new Dictionary<string, Sprite>();
        Sprite[] tmp = Resources.LoadAll<Sprite>("Sprites/Status");
        foreach (Sprite sprite in tmp)
        {
            status.Add(sprite.name, sprite);
        }
    }
}
