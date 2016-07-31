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

    private float internalTime;

    void Awake()
    {
        instance = this;

        loadNumbersInDic();
        loadStatusInDic();
        blank = Resources.Load<Sprite>("Sprites/blank");

        // For test purpose
        player = new Bulbasaur();
        player.initInBattleStats();
        enemy = new Bulbasaur();
        enemy.initInBattleStats();

        currentSelection = 1;
        currentMode = eMode.MENU;
        internalTime = 0.0f;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
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
                    moveProcess(player, enemy);
                    break;

                default:
                    print("Should not be here");
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            currentSelection = 1;
            currentMode = eMode.MENU;
        }
        if (internalTime > 0.1f)
        {
            player.stats.hp -= 1;
            enemy.stats.hp -= 1;
            internalTime = 0.0f;
        }
        internalTime += Time.deltaTime;
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
    }

    private void moveProcess(APokemon user, APokemon target)
    {
        Move usedMove = user.moves[currentSelection - 1];
        usedMove.use();

        int hitProbability = (int)(usedMove.Accuracy * (user.accuracyRate / target.evasionRate));
        if (!(hitProbability >= 100 || Random.Range(0, 100) < hitProbability))
        {
            print(usedMove.MoveName + " missed !");
            return;
        }
        sStat userTurnStat = user.stats;
        sStat targetTurnStat = target.stats;
        // user attack or attackSpe stat
        float userAttack;
        // target defense or defenseSpe stat
        float targetDefense;
        bool isCritical = false;
        // final dmgs
        int dmgs;

        float modifier = findDmgsModifier(user, target, usedMove, userTurnStat.speed, ref isCritical);
        // On critical hit unchanged stats are used 
        if (!isCritical)
        {
            // TODO : stats modification
            if (user.status == eStatus.BURNED)
            {
                userTurnStat.att /= 2;
            }
        }
        if (usedMove.Type.isPhysical())
        {
            userAttack = userTurnStat.att;
            targetDefense = targetTurnStat.def;
        }
        else
        {
            userAttack = userTurnStat.attSpe;
            targetDefense = targetTurnStat.defSpe;
        }
        dmgs = (int)(((((2 * (float)user.lvl) + 10) / 250) * (userAttack / targetDefense) * usedMove.EnemyEffect.hp + 2) * modifier);
        print(dmgs);
        target.stats.hp -= dmgs;
        if (target.stats.hp <= 0)
        {
            target.stats.hp = 0;
            print(target.name + " is K.O.");
        }
    }

    private float findDmgsModifier(APokemon user, APokemon target, Move move, float userSpeed, ref bool isCritical)
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
