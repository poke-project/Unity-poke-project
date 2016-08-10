using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PartyUI : MonoBehaviour {

    public GameObject[] slots;

    private Image[] arrowSlots;
    private Party party;
    private PartyManager manager;
    private Sprite arrowSprite;
    private int lastSelection;

    void Awake()
    {
        int i = -1;
        slots = new GameObject[6];
        arrowSlots = new Image[6];
        foreach (Transform child in transform)
        {
            if (i >= 0)
            {
                slots[i] = child.gameObject;
                arrowSlots[i] = child.Find("Arrow").GetComponent<Image>();
            }
            ++i;
        }
        arrowSprite = Resources.Load<Sprite>("Sprites/arrow");
        lastSelection = 0;
    }

	// Use this for initialization
	void Start ()
    {
        party = Game.Instance.player.trainer.party;
        manager = PartyManager.instance;
        setUI();
	}
	
    void OnEnable()
    {
        if (party != null)
        {
            setUI();
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (manager.enabled == false)
        {
            GameManager.instance.inPartyMenu = false;
            gameObject.SetActive(false);
        }
        arrowSlots[lastSelection].sprite = null;
        arrowSlots[manager.selection].sprite = arrowSprite;
        if (manager.needUIUpdate)
        {
            setSelectedSlot();
        }
        lastSelection = manager.selection;
	}

    private void setSlot(GameObject slot, APokemon pokemon)
    {
        // Changer le sprite
        slot.SetActive(true);
        slot.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Pokemons/Front/" + pokemon.speciesName);
        slot.transform.Find("Name").GetComponent<Text>().text = pokemon.name;
        slot.transform.Find("Level").GetComponent<Text>().text = pokemon.lvl.ToString();
        slot.transform.Find("HpText").GetComponent<Text>().text = pokemon.currentStats.hp.ToString() + "/    " + pokemon.stats.hp.ToString();
        slot.transform.Find("HpBar").GetComponent<Image>();
    }

    private void setUI()
    {
        for (int i = 0; i < 6; ++i)
        {
            if (i < party.nbInParty)
            {
                setSlot(slots[i], party.pokemons[i]);
            }
            else
            {
                slots[i].SetActive(false);
            }
        }
    }

    private void setSelectedSlot()
    {
        setSlot(slots[manager.selection], party.pokemons[manager.selection]);
    }
}
