using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// Desactiver autres input quand actif
public class BagUI : MonoBehaviour {

    private Text[] texts;
    private Image arrow;
    private BagManager bagManager;
    private Bag bag;
    private int cursorPos;
    //private int maxDisplay;
    private int nbItems;
    private int itemIndex;

    void Awake()
    {
        texts = GetComponentsInChildren<Text>(true);
        arrow = transform.Find("Arrow").GetComponent<Image>();
    }

	// Use this for initialization
	void Start ()
    {
        bagManager = BagManager.instance;
        bag = bagManager.bag;
        //maxDisplay = texts.Length - 1;
	}

    void OnEnable()
    {
        cursorPos = 0;
        itemIndex = 0;
        nbItems = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // this way or whole UI manager
        if (bagManager.enabled == false)
        {
            gameObject.SetActive(false);
        }

        if (bag.items.Count == 0)
        {
            texts[0].text = "Cancel";
            return;
        }
        nbItems = bagManager.nbItems;
        cursorPos = bagManager.cursorPos;
        itemIndex = bagManager.selection;
        if (nbItems < 2)
        {
            texts[0].text = bag.items[itemIndex].name + "\t\t\t\tx" + bag.items[itemIndex].nbHeld;
            texts[1].text = "Cancel";
        }
        else if (nbItems < 3 && itemIndex < nbItems - 1)
        {
            texts[0].text = bag.items[itemIndex].name + "\t\t\t\tx" + bag.items[itemIndex].nbHeld;
            texts[1].text = bag.items[itemIndex + 1].name + "\t\t\t\tx" + bag.items[itemIndex + 1].nbHeld;
            texts[2].text = "Cancel";
        }
        else if (itemIndex < (nbItems - 2))
        {
            texts[0].text = bag.items[itemIndex].name + "\t\t\t\tx" + bag.items[itemIndex].nbHeld;
            texts[1].text = bag.items[itemIndex + 1].name + "\t\t\t\tx" + bag.items[itemIndex + 1].nbHeld;
            texts[2].text = bag.items[itemIndex + 2].name + "\t\t\t\tx" + bag.items[itemIndex + 2].nbHeld;
            texts[3].text = "Cancel";
        }
        arrow.rectTransform.anchorMin = new Vector2(0.05f, texts[cursorPos].rectTransform.anchorMin.y);
        arrow.rectTransform.anchorMax = new Vector2(0.15f, texts[cursorPos].rectTransform.anchorMax.y);
	}
}
