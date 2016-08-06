using UnityEngine;
using System.Collections;

public class BagManager : MonoBehaviour {

    public static BagManager instance;

    [HideInInspector]
    public int selection;
    [HideInInspector]
    public int nbItems;

    public Bag bag;

    private bool cancelSelected;

    void Awake()
    {
        selection = 0;
        instance = this;
        cancelSelected = false;
    }

	// Use this for initialization
	void Start ()
    {
        bag = Game.Instance.player.trainer.bag;
	}
	
	// Update is called once per frame
	void Update ()
    {
        nbItems = bag.items.Count;
        updateSelection();
	}

    private void updateSelection()
    {
        if (nbItems == 0)
        {
            cancelSelected = true;
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
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
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
