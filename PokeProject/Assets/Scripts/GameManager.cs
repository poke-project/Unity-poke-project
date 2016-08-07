using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [HideInInspector]
    public Game game;
    [HideInInspector]
    public PersistentData persistentData;
    [HideInInspector]
    public bool inMenu;
    //[HideInInspector]
    public bool inBagMenu;

    [SerializeField]
    public bool inFight;
    [SerializeField]
    private GameObject UIManager;

    void Awake()
    {
        instance = this;
        persistentData = GameObject.Find("PersistentData").GetComponent<PersistentData>();
        inMenu = false;
        inBagMenu = false;
    }
    
	// Use this for initialization
	void Start ()
    {
        game = Game.Instance;
  	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            inMenu = !inMenu;
        }
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
