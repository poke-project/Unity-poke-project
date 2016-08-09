using UnityEngine;
using System.Collections;

public class PartyManager : MonoBehaviour {

    public static PartyManager instance;

    public Party party;
    public int selection;
    public Item selectedItem;
    public bool needUIUpdate;

    void Awake()
    {
        instance = this;
        selection = 0;
        selectedItem = null;
        needUIUpdate = false;
    }

	// Use this for initialization
	void Start ()
    {
        party = Game.Instance.player.trainer.party;
        enabled = false;
	}
	
    void OnEnable()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.inPartyMenu = true;
        }
    }

	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selection < 5 && (selection < party.nbInParty - 1))
            {
                selection++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selection > 0)
            {
                selection--;
            }
        }
        if (GameManager.instance.inPartyMenu && Input.GetKeyDown(KeyCode.Backspace))
        {
            enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selectedItem != null)
            {
                BagManager.instance.bag.useItem(selectedItem, party.pokemons[selection]);
                needUIUpdate = true;
                StartCoroutine(updateHp(party.pokemons[selection]));
                // Verifier que l'on modifie pas direct dans le sac (avec nbheld)
                selectedItem = null;
            }
        }
	}

    IEnumerator updateHp(APokemon pokemon)
    {
        while (pokemon.damageReceived > 0)
        {
            pokemon.currentStats.hp--;
            if (pokemon.currentStats.hp == 0)
            {
                // Pokemon faint
                needUIUpdate = false;
                StopAllCoroutines();
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
        needUIUpdate = false;
    }
}
