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
            Item a = new Item("a");
            Item b = new Item("b");
            Item c = new Item("c");
            Item d = new Item("d");
            Item e = new Item("e");
            Item f = new Item("f");
            Item g = new Item("g");
            Item h = new Item("h");
            Item i = new Item("i");
            Item j = new Item("j");
            Item k = new Item("k");
            Item l = new Item("l");
            trainer.bag.addItem(a);
            trainer.bag.addItem(b);
            trainer.bag.addItem(c);
            trainer.bag.addItem(d);
            trainer.bag.addItem(e);
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
