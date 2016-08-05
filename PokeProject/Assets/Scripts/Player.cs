using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public Party party;
    private static bool firstAwake = true;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        if (firstAwake)
        {
            // Should load party from file
            //party = new Party();
            //party = Party.Instance;
            firstAwake = false;
        }
        //party = Party.Instance;
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
       //     party = Game.Instance.party;
            //print(Game.Instance.party.ToString());
      //      print(party.ToString());
        }
	}
}
