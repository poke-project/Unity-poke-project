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
	}
	
    void OnEnable()
    {
        print("on enable");
        selection = 0;
        cursorPos = 0;
        cancelSelected = false;
    }

	// Update is called once per frame
	void Update ()
    {
        nbItems = bag.items.Count;
        updateSelection();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!cancelSelected)
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
            //  print(selection);
            //  print(nbItems - 1);
            print("update in manager");
            if (selection < (nbItems - 1))
            {
                selection++;
            }
            else
            {
                print("cancel selected");
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
