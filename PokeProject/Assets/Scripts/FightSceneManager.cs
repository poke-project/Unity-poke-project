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
    public Trainer enemy;
    public APokemon enemyPkmn;
    public Dictionary<string, Sprite> numbers;
    public Dictionary<string, Sprite> status;
    [HideInInspector]
    public Sprite blank;
    [HideInInspector]
    public int currentSelection;
    public int shakeNb;
    public bool enemyCaught;
    public bool enemyPkmnChange;
    public bool playerPkmnChange;
    
    //[HideInInspector]
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
        enemy = new Trainer();
        applyEffect toto = target => target.damageReceived = -20;
        Item potion = new Item("Potion", toto, true, false);
        enemy.bag.addItem(potion);
        APokemon newEnemyPkmn = new Bulbasaur();
        newEnemyPkmn.name = "deuxieme";
        enemy.party.addPokemonInParty(newEnemyPkmn);

        APokemon newPlayerPkmn = new Bulbasaur();
        newPlayerPkmn.name = "meinPokemon";
        player.trainer.party.addPokemonInParty(newPlayerPkmn);
        // END REMOVE

        playerPkmn.initInBattleStats();

        enemyPkmn = enemy.party.getFirstPokemonReady();
        enemyPkmn.initInBattleStats();
        enemyPkmn.isEnemy = true;

        //enemy = null;

        if (enemy == null)
        {
            nbEnemyLeft = 1;
        }
        else
        {
            nbEnemyLeft = enemy.party.nbInParty;
        }
        currentSelection = 1;
        currentMode = eMode.MENU;
        internalTime = 0.0f;
        inDialogue = false;
        nbParticipatedPokemon = 1f;
        texts = new List<string>();
        shakeNb = 0;
        enemyCaught = false;
        enemyPkmnChange = true;
        playerPkmnChange = true;
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
            if (pokemon.currentStats.hp == pokemon.stats.hp)
            {
                break;
            }

            pokemon.currentStats.hp++;
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

    IEnumerator updateShake()
    {
        while (shakeNb > 0)
        {
            yield return new WaitForSeconds(1.8f);
            shakeNb--;
        }
        yield return null;
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
        inDialogue = true;
        foreach (string text in texts)
        {
            yield return StartCoroutine(waitForEndDialogue(text));
        }
        texts.Clear();
    }

    IEnumerator setShakes(APokemon pokemon, Item pokeball, float f, int s)
    {
        if (f == 0)
        {
            f = (pokemon.stats.hp * 255 * 4) / (pokemon.currentStats.hp * pokeball.ballValue);
        }
        int d = (pokemon.catchRate * 100) / pokeball.ballMod;
        if (d >= 256)
        {
            shakeNb = 3;
        }
        else
        {
            int x = (int)(d * f) / (255 + s);
            if (x < 10)
            {
                texts.Add("You missed the " + pokemon.name + "!");
                shakeNb = 0;
            }
            else if (x < 30)
            {
                texts.Add("Darn! The " + pokemon.name + " broke free!");
                shakeNb = 1;
            }
            else if (x < 70)
            {
                texts.Add("Aww! It appeared to be caught!");
                shakeNb = 2;
            }
            else
            {
                texts.Add("Shoot! It was close too!");
                shakeNb = 3;
            }
        }
        yield return null;
    }

    IEnumerator itemWrapper(Trainer user, APokemon first, APokemon second, int itemSelected)
    {
        Item item = user.bag.items[itemSelected];
        if (item.isPokeball)
        {
            user.bag.useItem(item, first);
            bool success = false;
            if (item.name == "MasterBall")
            {
                success = true;
            }
            else
            {
                int rand = Random.Range(0, item.ballMod + 1);
                if (second.status == eStatus.SLEEPING || second.status == eStatus.FROZEN)
                {
                    if (rand < 25)
                    {
                        success = true;
                    }
                    else if (rand - 25 > second.catchRate)
                    {
                        yield return StartCoroutine(setShakes(second, item, 0, 10));
                    }
                }
                else if (second.status == eStatus.PARALIZED || second.status == eStatus.BURNED || second.status == eStatus.POISONED)
                {
                    if (rand < 12)
                    {
                        success = true;
                    }
                    else if (rand - 12 > second.catchRate)
                    {
                        yield return StartCoroutine(setShakes(second, item, 0, 5));
                    }
                }
                else
                {
                    rand = Random.Range(0, 255);
                    float f = (second.stats.hp * 255 * 4) / (second.currentStats.hp * item.ballValue);
                    if (f >= rand)
                    {
                        success = true;
                    }
                    else
                    {
                        yield return StartCoroutine(setShakes(second, item, f, 0));
                    }
                }
            }
            if (success)
            {
                shakeNb = 3;
                texts.Add("All right! " + second.name + " was caught!");
            }
            enemyCaught = success;
            yield return StartCoroutine(updateShake());
            yield return StartCoroutine(startDialogue());
        }
        else
        {
            texts.Add(user.name + " used " + item.name);
            yield return StartCoroutine(startDialogue());
            yield return StartCoroutine(updateHp(first));
        }
    }

    IEnumerator processPokemonCaught(APokemon pokemon)
    {
        if (!player.pokedex.isPokemonKnown(pokemon))
        {
            // Add pokemon in pokedex
            texts.Add(pokemon.name + " added in Pokedex!");
        }

        // Renommer pokemon
        player.trainer.party.addPokemonInParty(pokemon);

        if (player.trainer.party.nbInParty >= 6)
        {
            texts.Add("No more slot in party\n" + pokemon.name + " sent to storage!");
        }
        else
        {
            texts.Add(pokemon.name + " added in the party!");
        }
        yield return StartCoroutine(startDialogue());
        nbEnemyLeft--;
    }

    IEnumerator runTurn(APokemon first, APokemon second, int playerSelection, int enemySelection, bool useItem)
    {
        inDialogue = true;
        if (!first.isEnemy)
        {
            // Joueur plus rapide ou objet utilise
            if (useItem)
            {
                // Objet utilise par joueur !!!!! pas forcement !!!!!
                yield return StartCoroutine(itemWrapper(player.trainer, first, second, playerSelection));
                if (enemyCaught)
                {
                    yield return StartCoroutine(processPokemonCaught(second));
                    endTurn();
                    yield break;
                }
                // Determiner si enemy utilise objet
                if (checkForItemUse())
                {
                    // enemy utilise objet
                    print("enemy item 1");
                    yield return StartCoroutine(itemWrapper(enemy, second, first, 0));
                }
                else
                {
                    // enemy attaque
                    yield return StartCoroutine(moveWrapper(second, first, enemySelection));
                }
                // Fin du tour
                endTurn();
            }
            else
            {
                // Pas d'objet utilise PAR LE JOUEUR
                // Determiner si enemy utilise objet 
                if (checkForItemUse())
                {
                    // Enemy utilise objet
                    print("enemy item 2");
                    yield return StartCoroutine(itemWrapper(enemy, second, first, 0));
                }
                else
                {
                    // Enemy utilise pas objet, priorite au joueur
                    yield return StartCoroutine(moveWrapper(first, second, playerSelection));
                    // Puis enemy attaque
                    yield return StartCoroutine(moveWrapper(second, first, enemySelection));
                }
                // Fin du tour
                endTurn();
            }
        }
        else
        {
            // Enemy plus rapide
            if (useItem)
            {
                print("enemy item 3");
                // Enemy utilise objet
                yield return StartCoroutine(itemWrapper(enemy, first, second, 0));
                // Puis joueur attaque
                yield return StartCoroutine(moveWrapper(second, first, playerSelection));
                // Fin du tour
                endTurn();
            }
            else
            {
                // Enemy attaque
                yield return StartCoroutine(moveWrapper(first, second, enemySelection));
                // Puis joueur attaque
                yield return StartCoroutine(moveWrapper(second, first, playerSelection));
                // Fin du tour
                endTurn();
            }
        }
    }

    // Globalize to give exp to all participating pokemon
    IEnumerator pokemonFaintProcess(APokemon pokemon)
    {
        yield return StartCoroutine(waitForEndDialogue((pokemon.isEnemy ? "Foe " : "") + pokemon.name + " fainted!"));
        if (pokemon.isEnemy)
        {
            playerPkmn.receiveEvs(pokemon.lootEvs);
            int expGain = findExpGain(pokemon);
            expGain = 5000;
            texts.Add(playerPkmn.name + " gained " + expGain.ToString() + " EXP. Points!");
            yield return StartCoroutine(startDialogue());
            yield return StartCoroutine(updateXp(expGain));
            nbEnemyLeft--;
            enemyPkmn = null;
        }
        else
        {
            if (player.trainer.party.getFirstPokemonReady() == null)
            {
                texts.Add(player.trainer.name + " blacks out!");
                yield return StartCoroutine(startDialogue());
                if (enemy != null)
                    enemy.money += player.trainer.money / 2;
                player.trainer.money /= 2;
                // Go back to last pokeCenter used
                exitFight();
            }
            else
            {
                // Choose other pokemon
                // will not stay like this
                playerPkmn = player.trainer.party.getFirstPokemonReady();
                playerPkmn.initInBattleStats();
                playerPkmnChange = true;
            }
        }
        endTurn();
        yield return null;
    }

    private int findExpGain(APokemon fainted)
    {
        float trainerBattleModifier;
        int exp;

        trainerBattleModifier = (enemy != null ? 1.5f : 1f);
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
            if (enemyPkmn.moves[i] == null)
                break;
            float effectiveness = enemyPkmn.moves[i].Type.dmgsModifier(playerPkmn.type1)
                * enemyPkmn.moves[i].Type.dmgsModifier(playerPkmn.type2);
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

    IEnumerator singleDialogue(string message)
    {
        inDialogue = true;
        yield return StartCoroutine(waitForEndDialogue(message));
        endTurn();
    }

    private bool checkForItemUse()
    {
        return ((enemy != null) && !enemy.bag.isEmpty && (enemyPkmn.currentStats.hp < (enemyPkmn.stats.hp / 5)));
    }

    // Restore stats when fight ends
	void Update () {
        // block user input during dialogue
        if (inDialogue)
            return;
        if (enemyPkmn == null)
        {
            if (nbEnemyLeft == 0)
            {
                exitFight();
            }
            else
            {
                enemyPkmn = enemy.party.getFirstPokemonReady();
                enemyPkmn.initInBattleStats();
                enemyPkmn.isEnemy = true;
                enemyPkmnChange = true;
            }
        }
        updateSelection();
        controlStatus(playerPkmn);
        controlStatus(enemyPkmn);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int enemyMove = enemyChoice();
            bool enemyUseItem = checkForItemUse();
            switch (currentMode)
            {
                case eMode.MENU:
                    StartCoroutine(menuActions());
                    break;

                case eMode.FIGHT:
                    if (!enemyUseItem && (playerPkmn.currentStats.speed > enemyPkmn.currentStats.speed
                        || (playerPkmn.currentStats.speed == enemyPkmn.currentStats.speed
                            && Random.Range(0, 100) < 50)))
                    {
                        StartCoroutine(runTurn(playerPkmn, enemyPkmn, currentSelection, enemyMove, false));
                    }
                    else
                    {
                        StartCoroutine(runTurn(enemyPkmn, playerPkmn, currentSelection, enemyMove, enemyUseItem));
                    }
                    break;

                case eMode.BAG:
                    if (!BagManager.instance.cancelSelected)
                    {
                        Item item = player.trainer.bag.items[BagManager.instance.selection];
                        if (!item.usableInFight || (item.isPokeball && (enemy != null)))
                        {
                            StartCoroutine(singleDialogue("Cannot use this item now!"));
                        }
                        else
                        {
                            StartCoroutine(runTurn(playerPkmn, enemyPkmn, BagManager.instance.selection, enemyMove, true));
                        }
                    }
                    else
                    {
                        currentMode = eMode.MENU;
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
            BagManager.instance.enabled = false;
        }
        internalTime += Time.deltaTime;
	}


    private IEnumerator menuActions()
    {
        switch ((eMode)currentSelection)
        {
            case eMode.MENU:
                break;

            case eMode.FIGHT:
                    print("in fight");
                break;

            case eMode.BAG:
                BagManager.instance.enabled = true;
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
        yield return null;
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
