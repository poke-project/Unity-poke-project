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

    public void checkEncounter()
    {
        // Should get type of area in parameters
        int rand = Random.Range(1, 1880);
        if (rand <= 100)
        {
            print("Very common");
        }
        else if (rand <= 185)
        {
            print("Common");
        }
        else if (rand <= 252)
        {
            print("Semi-rare");
        }
        else if (rand <= 285)
        {
            print("Rare");
        }
        else if (rand <= 297)
        {
            print("Very rare");
        }
    }
}
