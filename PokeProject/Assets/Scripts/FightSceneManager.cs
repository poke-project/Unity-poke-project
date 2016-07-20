using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FightSceneManager : MonoBehaviour {

    public static FightSceneManager instance;

    public APokemon player;
    public APokemon enemy;
    public Dictionary<string, Sprite> numbers;
    public Dictionary<string, Sprite> status;
    public Sprite blank;

    void Awake()
    {
        instance = this;

        loadNumbersInDic();
        loadStatusInDic();
        blank = Resources.Load<Sprite>("Sprites/blank");

        // For test purpose
        player = new Bulbasaur();
        enemy = new Bulbasaur();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void loadNumbersInDic()
    {
        numbers = new Dictionary<string, Sprite>();
        Sprite[] tmp = Resources.LoadAll<Sprite>("Sprites/Numbers");
        foreach (Sprite sprite in tmp)
        {
            numbers.Add(sprite.name, sprite);
        }
    }

    private void loadStatusInDic()
    {
        status = new Dictionary<string, Sprite>();
        Sprite[] tmp = Resources.LoadAll<Sprite>("Sprites/Status");
        foreach (Sprite sprite in tmp)
        {
            status.Add(sprite.name, sprite);
        }
    }
}
