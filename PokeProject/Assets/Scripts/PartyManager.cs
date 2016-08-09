using UnityEngine;
using System.Collections;

public class PartyManager : MonoBehaviour {

    public static PartyManager instance;

    public Party party;

    public int selection;

    void Awake()
    {
        instance = this;
        selection = 0;
    }

	// Use this for initialization
	void Start ()
    {
        party = Game.Instance.player.trainer.party;
        enabled = false;
	}
	
    void OnEnable()
    {
        print(GameManager.instance);
        //    GameManager.instance.inPartyMenu = true;
    }

	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selection < 5 && (selection < party.nbInParty))
            {
                selection++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selection > 0)
            {
                selection--;
            }
        }
	}
}
