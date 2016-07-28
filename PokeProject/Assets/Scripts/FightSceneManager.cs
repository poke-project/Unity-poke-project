using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FightSceneManager : MonoBehaviour {

    private enum eMode
    {
        MENU = 0,
        FIGHT,
        BAG,
        POKEMON,
        RUN
    }

    public static FightSceneManager instance;

    public APokemon player;
    public APokemon enemy;
    public Dictionary<string, Sprite> numbers;
    public Dictionary<string, Sprite> status;
    public Sprite blank;

    public int currentSelection;
    private eMode currentMode;

    void Awake()
    {
        instance = this;

        loadNumbersInDic();
        loadStatusInDic();
        blank = Resources.Load<Sprite>("Sprites/blank");

        // For test purpose
        player = new Bulbasaur();
        enemy = new Bulbasaur();

        currentSelection = 0;
        currentMode = eMode.FIGHT;
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
                    print(player.moves[currentSelection].getMoveName());
                    break;

                default:
                    print("Should not be here");
                    break;
            }
        }
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
        int newSelection = 0;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            newSelection = -2;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            newSelection = 2;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            newSelection = -1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            newSelection = 1;
        }
        if ((newSelection + currentSelection) >= 0 && (newSelection + currentSelection < 4))
        {
            currentSelection += newSelection;
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
