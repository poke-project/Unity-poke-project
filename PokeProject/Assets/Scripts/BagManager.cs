using UnityEngine;
using System.Collections;

public class BagManager : MonoBehaviour {

    public static BagManager instance;

    [HideInInspector]
    public int selection;
    [HideInInspector]
    public int cursorPos;
    [HideInInspector]
    public int nbItems;

    public Bag bag;

    private bool cancelSelected;
    private int maxDisplay;

    void Awake()
    {
        instance = this;
        maxDisplay = 3;
    }

	// Use this for initialization
	void Start ()
    {
        bag = Game.Instance.player.trainer.bag;
        enabled = false;
	}
	
    void OnEnable()
    {
        selection = 0;
        cursorPos = 0;
        cancelSelected = false;
        GameManager.instance.inBagMenu = true;
    }

	// Update is called once per frame
	void Update ()
    {
        nbItems = bag.items.Count;
        updateSelection();
        if (GameManager.instance.inBagMenu && Input.GetKeyDown(KeyCode.Space))
        {
            if (!cancelSelected && GameManager.instance.inFight)
            {
                bag.useItem(selection);
            }
            enabled = false;
        }
	}

    private void updateSelection()
    {
        if (nbItems == 0)
        {
            cancelSelected = true;
            return;
        }
        if ((selection != (nbItems - 1)) && cancelSelected)
        {
            cancelSelected = false;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selection < (nbItems - 1))
            {
                selection++;
            }
            else
            {
                cancelSelected = true;
            }
            if (selection > (nbItems - 3) && cursorPos < maxDisplay && cursorPos < nbItems)
            {
                cursorPos++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (cursorPos > 0)
            {
                cursorPos--;
            }
            if (selection > 0 && !cancelSelected)
            {
                selection--;
            }
            if (cancelSelected)
            {
                cancelSelected = false;
            }
        }
    }
}
