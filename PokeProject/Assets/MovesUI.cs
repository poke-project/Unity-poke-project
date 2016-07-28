using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MovesUI : MonoBehaviour {

    private Text[] movesName;
    private Text currentPP;
    private Text maxPP;
    private Text type;
    private Transform moveDescription;

    private int lastSelected;
    private APokemon player;

    void Awake()
    {
        movesName = GetComponentsInChildren<Text>();
        moveDescription = transform.Find("Move description");
        currentPP = moveDescription.Find("Current PP").GetComponent<Text>();
        maxPP = moveDescription.Find("Max PP").GetComponent<Text>();
        type = moveDescription.Find("Type").GetComponent<Text>();
        lastSelected = -1;
    }

	// Use this for initialization
	void Start ()
    {
        player = FightSceneManager.instance.player;
        for (int i = 0; i < 4; ++i)
        {
            if (player.moves[i] != null)
            {
                movesName[i].text = player.moves[i].getMoveName();
            }
            else
            {
                movesName[i].text = "-";
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (lastSelected != FightSceneManager.instance.currentSelection)
        {
            lastSelected = FightSceneManager.instance.currentSelection;
            currentPP.text = player.moves[lastSelected].getCurrentPP().ToString();
            maxPP.text = player.moves[lastSelected].getMaxPP().ToString();
        }   
	}
}
