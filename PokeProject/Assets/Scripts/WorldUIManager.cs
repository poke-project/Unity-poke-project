using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorldUIManager : MonoBehaviour
{
    public static WorldUIManager instance;

    public bool inFight;

    private Transform canvas;
    private GameObject menu;
    private GameObject bagUI;
    private GameObject partyUI;

    void Awake()
    {
        instance = this;
        canvas = GameObject.Find("Canvas").transform;
        if (!inFight)
        {
            menu = canvas.Find("Menu").gameObject;
            bagUI = canvas.Find("Bag").gameObject;
        }
        partyUI = canvas.Find("Party").gameObject;
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!inFight)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                menu.SetActive(!menu.activeInHierarchy);
            }
            if (GameManager.instance.inMenu)
            {
                if (Input.GetKeyDown(KeyCode.B))
                {
                    bagUI.SetActive(true);
                }
                if (Input.GetKeyDown(KeyCode.P))
                {
                    partyUI.SetActive(true);
                }
            }
        }	
        if (BagManager.instance.setPartyUIActive)
        {
            partyUI.SetActive(true);
            BagManager.instance.setPartyUIActive = false;
        }
	}
}
