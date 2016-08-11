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
            Item potion = new Item("Potion");
            Item pokeBall = new Item("PokeBall");
            trainer.bag.addItem(potion);
            trainer.bag.addItem(pokeBall);
            testItems();
          if (GameManager.instance.inMenu)
            {
                BagManager.instance.enabled = true;
            }
        }
	}

    private void testItems()
    {
            Item superPotion = new Item("Super Potion");
            Item hyperPotion = new Item("Hyper Potion");
            Item maxPotion = new Item("Max Potion");
            Item fullRestore = new Item("Full Restore");
            Item elixir = new Item("Elixir");
            Item maxElixir = new Item("Max Elixir");
            Item freshWater = new Item("Fresh Water");
            Item sodaPop = new Item("Soda Pop");
            Item lemonade = new Item("Lemonade");
            Item revive = new Item("Revive");
            Item maxRevive = new Item("Max Revive");
            Item antidote = new Item("Antidote");
            Item paralyzeHeal = new Item("Paralyze Heal");
            Item awakening = new Item("Awakening");
            Item burnHeal = new Item("Burn Heal");
            Item iceHeal = new Item("Ice Heal");
            Item fullHeal = new Item("Full Heal");
            Item hpUp = new Item("Hp Up");
            Item protein = new Item("Protein");
            Item iron = new Item("Iron");
            Item calcium = new Item("Calcium");
            Item zinc = new Item("Zinc");
            Item carbos = new Item("Carbos");
            Item rareCandy = new Item("Rare Candy");
            Item xAttack = new Item("X Attack");
            Item xDefense = new Item("X Defense");
            Item xSpAtk = new Item("X Sp. Atk");
            Item xSpDef = new Item("X Sp. Def");
            Item xSpeed = new Item("X Speed");
            Item greatBall = new Item("GreatBall");
            Item ultraBall = new Item("UltraBall");
            Item masterBall = new Item("MasterBall");
            trainer.bag.addItem(superPotion);
            trainer.bag.addItem(hyperPotion);
            trainer.bag.addItem(maxPotion);
            trainer.bag.addItem(fullRestore);
            trainer.bag.addItem(elixir);
            trainer.bag.addItem(maxElixir);
            trainer.bag.addItem(freshWater);
            trainer.bag.addItem(sodaPop);
            trainer.bag.addItem(lemonade);
            trainer.bag.addItem(revive);
            trainer.bag.addItem(maxRevive);
            trainer.bag.addItem(antidote);
            trainer.bag.addItem(paralyzeHeal);
            trainer.bag.addItem(awakening);
            trainer.bag.addItem(burnHeal);
            trainer.bag.addItem(iceHeal);
            trainer.bag.addItem(fullHeal);
            trainer.bag.addItem(hpUp);
            trainer.bag.addItem(protein);
            trainer.bag.addItem(iron);
            trainer.bag.addItem(calcium);
            trainer.bag.addItem(zinc);
            trainer.bag.addItem(carbos);
            trainer.bag.addItem(rareCandy);
            trainer.bag.addItem(xAttack);
            trainer.bag.addItem(xDefense);
            trainer.bag.addItem(xSpAtk);
            trainer.bag.addItem(xSpDef);
            trainer.bag.addItem(xSpeed);
            trainer.bag.addItem(greatBall);
            trainer.bag.addItem(ultraBall);
            trainer.bag.addItem(masterBall);
 
    }
}
