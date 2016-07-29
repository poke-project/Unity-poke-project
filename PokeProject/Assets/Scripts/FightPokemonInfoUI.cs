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
    private Image currentFirstDigit;
    private Image currentSecondDigit;
    private Image currentThirdDigit;
    private Dictionary<string, Sprite> numbers;
    private Dictionary<string, Sprite> status;
    private Sprite blank;
    private APokemon pokemon;
    private Transform frame;

    [SerializeField]
    private Sprite lowHp;
    [SerializeField]
    private Sprite mediumHp;
    [SerializeField]
    private Sprite highHp;

    private float internalTime;

    void Awake()
    {
        frame = transform.Find("Frame");
        statusImage = frame.transform.Find("Status").GetComponent<Image>();
        hpBar = frame.transform.Find("Hp bar").GetComponent<Slider>();
        barImage = hpBar.transform.Find("HP").GetComponent<Image>();
        pokemonImage = transform.Find("Sprite").GetComponent<Image>();
        if (isPlayer)
        {
            Transform currentHpText = frame.transform.Find("Hp text").Find("Current hp");
            currentFirstDigit = currentHpText.Find("First digit").GetComponent<Image>();
            currentSecondDigit = currentHpText.Find("Second digit").GetComponent<Image>();
            currentThirdDigit = currentHpText.Find("Third digit").GetComponent<Image>();
        }
        internalTime = 0.0f;
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
            Transform maxHpText = frame.transform.Find("Hp text").Find("Max hp");
            // modify currentHp for pokemon's max hp
            setHpText(maxHpText.Find("First digit").GetComponent<Image>(), maxHpText.Find("Second digit").GetComponent<Image>(), maxHpText.Find("Third digit").GetComponent<Image>());
            pokemonImage.sprite = Resources.Load<Sprite>("Sprites/Pokemons/Back/" + pokemon.name);
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
        hpBar.maxValue = pokemon.stats.hp;
	}
	
	void Update ()
    {
        // Remove after testing
        if (pokemon.stats.hp > 0)
        {
            if (internalTime > 0.1f)
            {
                pokemon.stats.hp -= 1;
                internalTime = 0.0f;
            }

            updateHpBar();
            updateStatus();
            if (isPlayer)
            {
                setHpText(currentFirstDigit, currentSecondDigit, currentThirdDigit);
            }
            internalTime += Time.deltaTime;
        }
	}

    private int getDigit(int number, int digit)
    {
        return ((number / ((int)Mathf.Pow(10, (digit - 1)))) % 10);
    }

    private void updateHpBar()
    {
        hpBar.value = hpBar.maxValue - pokemon.stats.hp;
        if (pokemon.stats.hp < (hpBar.maxValue / 5))
        {
            barImage.sprite = lowHp;
        }
        else if (pokemon.stats.hp < (hpBar.maxValue / 2))
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

            default:
                Debug.Log("Should not be here");
                break;
        }
    }

    private void setHpText(Image first, Image second, Image third)
    {
        int thirdDigit = getDigit(pokemon.stats.hp, 3);
        int secondDigit = getDigit(pokemon.stats.hp, 2);
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
        first.sprite = numbers[getDigit(pokemon.stats.hp, 1).ToString()];
    }
}
