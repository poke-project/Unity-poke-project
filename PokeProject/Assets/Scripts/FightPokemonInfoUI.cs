using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FightPokemonInfoUI : MonoBehaviour {

    private Image status;
    private Image barSprite;
    private Slider hpBar;

    [SerializeField]
    private Sprite lowHp;
    [SerializeField]
    private Sprite mediumHp;
    [SerializeField]
    private Sprite highHp;

    private int currentHp;

    void Awake()
    {
        status = transform.Find("Status").GetComponent<Image>();
        hpBar = transform.Find("Hp Bar").GetComponent<Slider>();
        barSprite = hpBar.transform.Find("HP").GetComponent<Image>();

        currentHp = 100;
    }

	void Start ()
    {
        hpBar.maxValue = currentHp;
	}
	
	void Update ()
    {
	    // Remove after testing
        if (Input.GetKeyDown(KeyCode.Space) && currentHp >= 10)
        {
            currentHp -= 10;
        }

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
}
