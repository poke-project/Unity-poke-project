using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FightSceneManager : MonoBehaviour {

    public static FightSceneManager instance;

    [SerializeField]
    public Dictionary<string, Sprite> numbers;

    public Sprite[] sprites;

    void Awake()
    {
        instance = this;

        loadNumbersInDic();
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
        sprites = tmp;
        foreach (Sprite sprite in tmp)
        {
            numbers.Add(sprite.name, sprite);
        }
    }
}
