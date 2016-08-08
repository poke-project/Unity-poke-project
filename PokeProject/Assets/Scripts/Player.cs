using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
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
    }

	void Start ()
    {
	}
	
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.E))
        {
            Application.LoadLevel("fightScene");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            print(trainer.party.ToString());
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            applyEffect toto = target => target.damageReceived = -20;
            Item potion = new Item("Potion", toto, true, false);
            Item pokeBall = new Item("PokeBall", null, true, true);
            Item greatBall = new Item("GreatBall", null, true, true);
            Item ultraBall = new Item("UltraBall", null, true, true);
            Item masterBall = new Item("MasterBall", null, true, true);
            Item f = new Item("f");
            Item g = new Item("g");
            Item h = new Item("h");
            Item i = new Item("i");
            Item j = new Item("j");
            Item k = new Item("k");
            Item l = new Item("l");
            trainer.bag.addItem(potion);
            trainer.bag.addItem(pokeBall);
            trainer.bag.addItem(greatBall);
            trainer.bag.addItem(ultraBall);
            trainer.bag.addItem(masterBall);
            trainer.bag.addItem(f);
            trainer.bag.addItem(g);
            trainer.bag.addItem(h);
            trainer.bag.addItem(i);
            trainer.bag.addItem(j);
            trainer.bag.addItem(k);
            trainer.bag.addItem(l);
            BagManager.instance.enabled = true;
        }
	}
}
