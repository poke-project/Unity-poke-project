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

public partial class FightSceneManager : MonoBehaviour {


    public static FightSceneManager instance;

    private Player player;
    public APokemon playerPkmn;
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
    int nbEnemyLeft;

    void Awake()
    {
        instance = this;

        player = FindObjectOfType<Player>();

        loadNumbersInDic();
        loadStatusInDic();
        blank = Resources.Load<Sprite>("Sprites/blank");

        // For test purpose
        playerPkmn = player.trainer.party.getFirstPokemonReady();

        // REMOVE
        playerPkmn.currentStats.speed *= 2;
        playerPkmn.stats.speed *= 2;
        // END REMOVE

        playerPkmn.initInBattleStats();
        enemy = new Bulbasaur();
        enemy.initInBattleStats();
        enemy.isEnemy = true;
        nbEnemyLeft = 1;
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
        while (pokemon.damageReceived < 0)
        {
            pokemon.currentStats.hp++;
            if (pokemon.currentStats.hp == pokemon.stats.hp)
            {
                break;
            }
            pokemon.damageReceived++;
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
                playerPkmn.receiveExp(expChunk);
            }
            else
            {
                playerPkmn.receiveExp(expGain);
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

    IEnumerator itemWrapper(APokemon first, APokemon second, int itemSelected)
    {
        Item item = player.trainer.bag.items[itemSelected];
        item.useItem(first);
        player.trainer.bag.useItem(item);
        texts.Add(item.name + " used!");
        yield return StartCoroutine(startDialogue());
        yield return StartCoroutine(updateHp(first));
    }

    IEnumerator runTurn(APokemon first, APokemon second, int firstSelection, int secondSelection, bool useItem)
    {
        inDialogue = true;
        if (useItem)
        {
            yield return StartCoroutine(itemWrapper(first, second, firstSelection));
        }
        else
        {
            yield return StartCoroutine(moveWrapper(first, second, firstSelection));
        }
        // Check hp enemy for item use ? (poison, player use item)
        yield return StartCoroutine(moveWrapper(second, first, secondSelection));
        endTurn();
    }

    // Globalize to give exp to all participating pokemon
    IEnumerator pokemonFaintProcess(APokemon pokemon)
    {
        yield return StartCoroutine(waitForEndDialogue((pokemon.isEnemy ? "Foe " : "") + pokemon.name + " fainted!"));
        if (pokemon.isEnemy)
        {
            print(playerPkmn.Evs.ToString());
            playerPkmn.receiveEvs(pokemon.lootEvs);
            print(playerPkmn.Evs.ToString());
            int expGain = findExpGain(pokemon);
            expGain = 5000;
            print(expGain);
            texts.Add(playerPkmn.name + " gained " + expGain.ToString() + " EXP. Points!");
            yield return StartCoroutine(startDialogue());
            yield return StartCoroutine(updateXp(expGain));
            nbEnemyLeft--;
        }
        else
        {
            if (player.trainer.party.getFirstPokemonReady() == null)
            {
                //TAPETTE
                player.trainer.money /= 2;
            }
            else
            {
                // Choose other pokemon
            }
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

    private int enemyChoice()
    {
        float max = 0;
        int indexMove = 0;
        int i;
        for (i = 0; i < 4; ++i)
        {
            if (enemy.moves[i] == null)
                break;
            float effectiveness = enemy.moves[i].Type.dmgsModifier(playerPkmn.type1)
                * enemy.moves[i].Type.dmgsModifier(playerPkmn.type2);
            if (effectiveness > max)
            {
                max = effectiveness;
                indexMove = i;
            }
        }
        if (max < 2)
        {
            indexMove = Random.Range(0, i);
        }
        // index is decremented later
        return (indexMove + 1);
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

    // Restore stats when fight ends
	void Update () {
        // block user input during dialogue
        if (inDialogue)
            return;
        updateSelection();
        controlStatus(playerPkmn);
        controlStatus(enemy);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int enemyMove = enemyChoice();
            // Use if pokemon hp in red
            bool enemyUseItem = false;
            switch (currentMode)
            {
                case eMode.MENU:
                    menuActions();
                    break;

                case eMode.FIGHT:
                    if (playerPkmn.currentStats.speed > enemy.currentStats.speed
                        || (playerPkmn.currentStats.speed == enemy.currentStats.speed
                            && Random.Range(0, 100) < 50))
                    {
                        StartCoroutine(runTurn(playerPkmn, enemy, currentSelection, enemyMove, enemyUseItem));
                    }
                    else
                    {
                        StartCoroutine(runTurn(enemy, playerPkmn, currentSelection, enemyMove, enemyUseItem));
                    }
                    break;

                case eMode.BAG:
                    StartCoroutine(runTurn(playerPkmn, enemy, BagManager.instance.selection, enemyMove, enemyUseItem));
                    break;

                default:
                    print("Should not be here");
                    break;
            }
            currentSelection = 1;
        }
        if (nbEnemyLeft == 0)
        {
            exitFight();
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
                BagManager.instance.enabled = true;
                print("BAG");
                break;

            case eMode.POKEMON:
                print("POKEMON");
                break;

            case eMode.RUN:
                exitFight();
                break;

            default:
                print("Should not be here");
                break;
        }
        currentMode = (eMode)currentSelection;
    }

    private void exitFight()
    {
        playerPkmn.restoreCurrentStats();
        Application.LoadLevel("ImplementBasicAction");
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
            if (!(currentMode == eMode.FIGHT && playerPkmn.moves[nextSelection - 1] == null))
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
