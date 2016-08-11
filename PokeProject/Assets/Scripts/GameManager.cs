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
    public bool inPartyMenu;

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
        inPartyMenu = false;
    }
    
	// Use this for initialization
	void Start ()
    {
        game = Game.Instance;
        inBagMenu = false;
        inPartyMenu = false;
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
        float rand = Random.Range(0f, 100f);
        if (rand <= 0.66f)
        {
            print("Very rare");
        }
        else if (rand <= 2.436f)
        {
            print("Rare");
        }
        else if (rand <= 6.036f)
        {
            print("Semi-rare");
        }
        else if (rand <= 10.57f)
        {
            print("Common");
        }
        else if (rand <= 15.9f)
        {
            print("Very common");
        }
    }
}
