using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public Trainer trainer;
    private static bool firstAwake = true;

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
            Item potion = new Item("Potion");
            Item pokeball = new Item("Pokeball");
            Item tgtgtg = new Item("TGTGTG");
            for (int i = 0; i < 300; ++i)
            {
                trainer.bag.addItem(potion);
                trainer.bag.addItem(pokeball);
                trainer.bag.addItem(tgtgtg);
            }
        }
	}
}
