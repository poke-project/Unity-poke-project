using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FightPokemonInfoUI : MonoBehaviour {

    private Image status;
    private Image barSprite;
    private Slider hpBar;
    private Image currentFirstDigit;
    private Image currentSecondDigit;
    private Image currentThirdDigit;
    private Dictionary<string, Sprite> numbers;

    [SerializeField]
    private Sprite lowHp;
    [SerializeField]
    private Sprite mediumHp;
    [SerializeField]
    private Sprite highHp;

    private int currentHp;
    private float internalTime;

    void Awake()
    {
        status = transform.Find("Status").GetComponent<Image>();
        hpBar = transform.Find("Hp bar").GetComponent<Slider>();
        barSprite = hpBar.transform.Find("HP").GetComponent<Image>();

        Transform currentHpText = transform.Find("Hp text").Find("Current hp");
        currentFirstDigit = currentHpText.Find("First digit").GetComponent<Image>();
        currentSecondDigit = currentHpText.Find("Second digit").GetComponent<Image>();
        currentThirdDigit = currentHpText.Find("Third digit").GetComponent<Image>();

        currentHp = 105;
        internalTime = 0.0f;
    }

	void Start ()
    {
        numbers = new Dictionary<string, Sprite>(FightSceneManager.instance.numbers);
        hpBar.maxValue = currentHp;

        // Set text for max hp
        Transform maxHpText = transform.Find("Hp text").Find("Max hp");
        // modify currentHp for pokemon's max hp
        setHpText(currentHp, maxHpText.Find("First digit").GetComponent<Image>(), maxHpText.Find("Second digit").GetComponent<Image>(), maxHpText.Find("Third digit").GetComponent<Image>());
	}
	
	void Update ()
    {
        // Remove after testing
        if (currentHp > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) && currentHp >= 10)
            {
                currentHp -= 10;
            }

            if (internalTime > 0.1f)
            {
                currentHp -= 1;
                internalTime = 0.0f;
            }

            updateHpBar();
            setHpText(currentHp, currentFirstDigit, currentSecondDigit, currentThirdDigit);
            internalTime += Time.deltaTime;
        }
	}

    private int getDigit(int number, int digit)
    {
        return ((number / ((int)Mathf.Pow(10, (digit - 1)))) % 10);
    }

    private void updateHpBar()
    {
        hpBar.value = hpBar.maxValue - currentHp;
        if (currentHp < (hpBar.maxValue / 5))
        {
            barSprite.sprite = lowHp;
        }
        else if (currentHp < (hpBar.maxValue / 2))
        {
            barSprite.sprite = mediumHp;
        }
        else
        {
            barSprite.sprite = highHp;
        }
    }

    private void setHpText(int hp, Image first, Image second, Image third)
    {
        int thirdDigit = getDigit(hp, 3);
        int secondDigit = getDigit(hp, 2);
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
        first.sprite = numbers[getDigit(hp, 1).ToString()];
    }
}
