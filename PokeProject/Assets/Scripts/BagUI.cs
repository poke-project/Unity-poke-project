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
    private int maxDisplay;

    void Awake()
    {
        texts = GetComponentsInChildren<Text>(true);
        arrow = transform.Find("Arrow").GetComponent<Image>();
        cursorPos = 0;
    }

	// Use this for initialization
	void Start ()
    {
        bagManager = BagManager.instance;
        bag = bagManager.bag;
        maxDisplay = texts.Length - 1;
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
        int nbItems = bagManager.nbItems;
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (bagManager.selection >= (nbItems - 2))
            {
                if (cursorPos < maxDisplay && cursorPos < nbItems)
                {
                    cursorPos++;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (cursorPos > 0)
            {
                cursorPos--;
            }
        }

        int itemIndex = bagManager.selection;
        if (nbItems < 2)
        {
            texts[0].text = bag.items[itemIndex].name;
            texts[1].text = "Cancel";
        }
        else if (nbItems < 3 && itemIndex < nbItems - 1)
        {
            texts[0].text = bag.items[itemIndex].name;
            texts[1].text = bag.items[itemIndex + 1].name;
            texts[2].text = "Cancel";
        }
        else if (itemIndex < (nbItems - 2))
        {
            texts[0].text = bag.items[itemIndex].name;
            texts[1].text = bag.items[itemIndex + 1].name;
            texts[2].text = bag.items[itemIndex + 2].name;
            texts[3].text = "Cancel";
        }
        arrow.rectTransform.anchorMin = new Vector2(0.05f, texts[cursorPos].rectTransform.anchorMin.y);
        arrow.rectTransform.anchorMax = new Vector2(0.15f, texts[cursorPos].rectTransform.anchorMax.y);
	}
}
