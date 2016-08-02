using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FightPokemonInfoUI : MonoBehaviour {

    public bool isPlayer;

    private Image pokemonImage;
    private Image statusImage;
    private Image barImage;
    private Slider hpBar;
    private Slider xpBar;
    private Image currentFirstDigit;
    private Image currentSecondDigit;
    private Image currentThirdDigit;
    private Image maxFirstDigit;
    private Image maxSecondDigit;
    private Image maxThirdDigit;
    private Text pokemonName;
    private Dictionary<string, Sprite> numbers;
    private Dictionary<string, Sprite> status;
    private Sprite blank;
    private APokemon pokemon;
    private Transform frame;
    private Sprite[] resourcesFrame;
    private int goalHp;

    [SerializeField]
    private Sprite lowHp;
    [SerializeField]
    private Sprite mediumHp;
    [SerializeField]
    private Sprite highHp;


    void Awake()
    {
        frame = transform.Find("Frame");
        statusImage = frame.transform.Find("Status").GetComponent<Image>();
        hpBar = frame.transform.Find("Hp bar").GetComponent<Slider>();
        barImage = hpBar.transform.Find("HP").GetComponent<Image>();
        pokemonImage = transform.Find("Sprite").GetComponent<Image>();
        pokemonName = frame.transform.Find("Name").GetComponent<Text>();
        if (isPlayer)
        {
            Transform currentHpText = frame.transform.Find("Hp text").Find("Current hp");
            currentFirstDigit = currentHpText.Find("First digit").GetComponent<Image>();
            currentSecondDigit = currentHpText.Find("Second digit").GetComponent<Image>();
            currentThirdDigit = currentHpText.Find("Third digit").GetComponent<Image>();
            Transform maxHpText = frame.transform.Find("Hp text").Find("Max hp");
            maxFirstDigit = maxHpText.Find("First digit").GetComponent<Image>();
            maxSecondDigit = maxHpText.Find("Second digit").GetComponent<Image>();
            maxThirdDigit = maxHpText.Find("Third digit").GetComponent<Image>();

            resourcesFrame = Resources.LoadAll<Sprite>("Sprites/Combat Scene/Player");
            xpBar = frame.transform.Find("Xp bar").GetComponent<Slider>();
        }
        else
        {
            resourcesFrame = Resources.LoadAll<Sprite>("Sprites/Combat Scene/Enemy");
            xpBar = null;
        }
    }

	void Start ()
    {
        numbers = new Dictionary<string, Sprite>(FightSceneManager.instance.numbers);
        status = new Dictionary<string, Sprite>(FightSceneManager.instance.status);
        blank = FightSceneManager.instance.blank;
        if (isPlayer)
        {
            pokemon = FightSceneManager.instance.player;
            // Set text for max hp
            pokemonImage.sprite = Resources.Load<Sprite>("Sprites/Pokemons/Back/" + pokemon.name);
            xpBar.maxValue = pokemon.expThreshold;
        }
        // Check in pokedex if pokemon caught
        else
        {
            pokemon = FightSceneManager.instance.enemy;
            Sprite caughtSprite = Resources.Load<Sprite>("Sprites/pokemonCaught");
            if (true)
            {
                frame.transform.Find("Caught").GetComponent<Image>().sprite = caughtSprite;
            }
            else
            {
                frame.transform.Find("Caught").GetComponent<Image>().sprite = blank;
            }
            pokemonImage.sprite = Resources.Load<Sprite>("Sprites/Pokemons/Front/" + pokemon.name);
        }
        updateFrame();
        updateName();
	}
	
	void Update ()
    {
        updateHpBar();
        updateStatus();
        if (isPlayer)
        {
            updateExp();
            setHpText(currentFirstDigit, currentSecondDigit, currentThirdDigit, false);
            setHpText(maxFirstDigit, maxSecondDigit, maxThirdDigit, true);
        }
	}
    
    private void updateFrame()
    {
        frame.GetComponent<Image>().sprite = resourcesFrame[pokemon.gender];
    }

    private void updateName()
    {
        pokemonName.text = pokemon.name;
    }

    private int getDigit(int number, int digit)
    {
        return ((number / ((int)Mathf.Pow(10, (digit - 1)))) % 10);
    }

    private void updateExp()
    {
        xpBar.maxValue = pokemon.expThreshold;
        xpBar.value = pokemon.exp;
    }

    private void updateHpBar()
    {
        hpBar.maxValue = pokemon.stats.hp;
        hpBar.value = pokemon.stats.hp - pokemon.currentStats.hp;
        if (pokemon.currentStats.hp < (hpBar.maxValue / 5))
        {
            barImage.sprite = lowHp;
        }
        else if (pokemon.currentStats.hp < (hpBar.maxValue / 2))
        {
            barImage.sprite = mediumHp;
        }
        else
        {
            barImage.sprite = highHp;
        }
    }

    private void updateStatus()
    {
        switch (pokemon.status)
        {
            case eStatus.BURNED:
                statusImage.sprite = status["burned"]; 
                break;

            case eStatus.FROZEN:
                statusImage.sprite = status["frozen"]; 
                break;

            case eStatus.PARALIZED:
                statusImage.sprite = status["paralized"]; 
                break;

            case eStatus.POISONED:
                statusImage.sprite = status["poisoned"]; 
                break;

            case eStatus.SLEEPING:
                statusImage.sprite = status["sleeping"]; 
                break;

            case eStatus.NORMAL:
                statusImage.sprite = blank;
                break;

            case eStatus.CONFUSED:
                break;

            default:
                Debug.Log("Should not be here");
                break;
        }
    }

    private void setHpText(Image first, Image second, Image third, bool isMax)
    {
        int thirdDigit;
        int secondDigit;
        int firstDigit;
        if (isMax)
        {
            thirdDigit = getDigit(pokemon.stats.hp, 3);
            secondDigit = getDigit(pokemon.stats.hp, 2);
            firstDigit = getDigit(pokemon.stats.hp, 1);
        }
        else
        {
            thirdDigit = getDigit(pokemon.currentStats.hp, 3);
            secondDigit = getDigit(pokemon.currentStats.hp, 2);
            firstDigit = getDigit(pokemon.currentStats.hp, 1);
        }
        if (thirdDigit == 0)
        {
            third.enabled = false;
            if (secondDigit == 0)
            {
                second.enabled = false;
            }
            else
            {
                second.enabled = true;
                second.sprite = numbers[secondDigit.ToString()];
            }
        }
        else
        {
            third.enabled = true;
            second.enabled = true;
            third.sprite = numbers[thirdDigit.ToString()];
            second.sprite = numbers[secondDigit.ToString()];
        }
        first.sprite = numbers[firstDigit.ToString()];
    }
}
