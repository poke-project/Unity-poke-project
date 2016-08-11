using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public Pokedex pokedex;
    public Trainer trainer;
    private static bool firstAwake = true;

    private Item potion;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        trainer = new Trainer();
        // poor as fuck
        trainer.money = 0;
        // Prompt player for name
        trainer.name = "PGM";
        // Should load pokedex from save
        pokedex = new Pokedex();

        APokemon totor = new Bulbasaur();
        totor.name = "totor";
        APokemon bulbamazing = new Bulbasaur();
        bulbamazing.name = "bulbamazing";
        APokemon bulbattorney = new Bulbasaur();
        bulbattorney = new Bulbasaur();
        bulbattorney.name = "bulbattorney";
        APokemon bulbapocalypse = new Bulbasaur();
        bulbapocalypse = new Bulbasaur();
        bulbapocalypse.name = "bulbapocalypse";
        trainer.party.addPokemonInParty(totor);
        trainer.party.addPokemonInParty(bulbattorney);
        trainer.party.addPokemonInParty(bulbapocalypse);
    }

	void Start ()
    {
	}
	
	void Update ()
    {
        print(trainer.bag.ToString());
        print(trainer.party.getFirstPokemonReady().move1.CurrentPP);
	    if (Input.GetKeyDown(KeyCode.E))
        {
            Application.LoadLevel("fightScene");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            print(trainer.party.ToString());
            if (GameManager.instance.inMenu)
            {
                PartyManager.instance.enabled = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            applyEffect toto = target => target.damageReceived = -20;
            //Item potion = new Item("Potion", toto, true, false);
            Item potion = new Item("Potion");
            Item pokeBall = new Item("PokeBall");
            Item greatBall = new Item("GreatBall");
            Item ultraBall = new Item("UltraBall");
            Item masterBall = new Item("MasterBall");
            trainer.bag.addItem(potion);
            trainer.bag.addItem(pokeBall);
            trainer.bag.addItem(greatBall);
            trainer.bag.addItem(ultraBall);
            trainer.bag.addItem(masterBall);
           if (GameManager.instance.inMenu)
            {
                BagManager.instance.enabled = true;
            }
        }
	}
}
