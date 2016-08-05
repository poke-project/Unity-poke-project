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
    private Player playerData;
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

        playerData = FindObjectOfType<Player>();

        loadNumbersInDic();
        loadStatusInDic();
        blank = Resources.Load<Sprite>("Sprites/blank");

        // For test purpose
        player = playerData.party.getFirstPokemonReady();

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
            print(player.Evs.ToString());
            player.receiveEvs(pokemon.lootEvs);
            print(player.Evs.ToString());
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

    // Restore stats when fight ends
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
                player.restoreCurrentStats();
                Application.LoadLevel("ImplementBasicAction");
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
