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

   // [HideInInspector]
    public int currentSelection;
   // [HideInInspector]
    public eMode currentMode;

    void Awake()
    {
        instance = this;

        loadNumbersInDic();
        loadStatusInDic();
        blank = Resources.Load<Sprite>("Sprites/blank");

        // For test purpose
        player = new Bulbasaur();
        enemy = new Bulbasaur();

        currentSelection = 1;
        currentMode = eMode.MENU;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        updateSelection();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (currentMode)
            {
                case eMode.MENU:
                    menuActions();
                    break;

                case eMode.FIGHT:
                    moveProcess();
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
	}

    private void moveProcess()
    {
        // IMPLEMENT ACCURACY AND EVASION
        //
        Move usedMove = player.moves[currentSelection - 1];
        usedMove.use();

        // same type attack bonus : 1.5 if same type as user
        float stab;
        // type effectiveness
        float typeModifier;
        // critical hit bonus : 2 if critical
        float critical;
        // items / abilities bonuses
        float other;
        // random modifier from 0.85 to 1
        float randModifier;
        // dmgs modifier from previous bonuses
        float modifier;
        // user attack or attackSpe stat
        float userAttack;
        // receiver defense or defenseSpe stat
        float receiverDefense;
        // final dmgs
        int dmgs;
        //
        // CHECK IF COPY IS OK
        //
        sStat turnUserStat = player.stats;
        sStat turnReceiverStat = enemy.stats;

        if (usedMove.Type.GetType() == player.type1.GetType() || usedMove.Type.GetType() == player.type2.GetType())
        {
            stab = 1.5f;
        }
        else
        {
            stab = 1f;
        }
        typeModifier = usedMove.Type.dmgsModifier(enemy.type1) * usedMove.Type.dmgsModifier(enemy.type2);
        // Could be better with shuffle bag
        int probability = (int)(turnUserStat.speed / (512f / usedMove.criticalChanceModifier));
        // Critical hit should ignore modifier from burn and stat modifiers
        if (Random.Range(0, 100) < probability)
        {
            critical = (2 * player.lvl + 5) / (player.lvl + 5);
        }
        else
        {
            critical = 1f;
        }
        if (player.status == eStatus.BURNED)
        {
            turnUserStat.att /= 2;
        }

        // TODO
        other = 1f;

        randModifier = Random.Range(0.85f, 1f);
        modifier = stab * typeModifier * critical * other * randModifier;
        if (usedMove.Type.isPhysical())
        {
            userAttack = turnUserStat.att;
            receiverDefense = turnReceiverStat.def;
        }
        else
        {
            userAttack = turnUserStat.attSpe;
            receiverDefense = turnReceiverStat.defSpe;
        }
        dmgs = (int)(((((2 * (float)player.lvl) + 10) / 250) * (userAttack / receiverDefense) * usedMove.EnemyEffect.hp + 2) * modifier);
        print(dmgs);
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
