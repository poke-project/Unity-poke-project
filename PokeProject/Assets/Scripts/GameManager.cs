using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [HideInInspector]
    public Game game;
    [HideInInspector]
    public PersistentData persistentData;

	// Use this for initialization
	void Start ()
    {
        instance = this;
        game = Game.Instance;
        persistentData = GameObject.Find("PersistentData").GetComponent<PersistentData>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
