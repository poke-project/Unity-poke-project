using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MovesUI : MonoBehaviour {

    private Text[] movesName;
    private Text currentPP;
    private Text maxPP;
    private Text type;
    private Transform moveDescription;

    private int selected;
    private APokemon playerPkmn;

    void Awake()
    {
        movesName = GetComponentsInChildren<Text>();
        moveDescription = transform.Find("Move description");
        currentPP = moveDescription.Find("Current PP").GetComponent<Text>();
        maxPP = moveDescription.Find("Max PP").GetComponent<Text>();
        type = moveDescription.Find("Type").GetComponent<Text>();
        selected = -1;
    }

	// Use this for initialization
	void Start ()
    {
        playerPkmn = FightSceneManager.instance.playerPkmn;
        for (int i = 0; i < 4; ++i)
        {
            if (playerPkmn.moves[i] != null)
            {
                movesName[i].text = playerPkmn.moves[i].MoveName;
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
        selected = FightSceneManager.instance.currentSelection;
        currentPP.text = playerPkmn.moves[selected - 1].CurrentPP.ToString();
        maxPP.text = playerPkmn.moves[selected - 1].MaxPP.ToString();
        type.text = playerPkmn.moves[selected - 1].Type.ToString();
	}
}
