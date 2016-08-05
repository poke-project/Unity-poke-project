using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Desactiver autres input quand actif
public class BagUI : MonoBehaviour {

    private Text[] itemsNames;
    private Image arrow;
    private int cursorPos;
    private Bag bag;

    void Awake()
    {
        itemsNames = GetComponentsInChildren<Text>(true);
        arrow = transform.Find("Arrow").GetComponent<Image>();
        cursorPos = 0;
    }

	// Use this for initialization
	void Start ()
    {
        bag = Game.Instance.player.trainer.bag;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            cursorPos++;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            cursorPos--;
        }
        if (bag.items.Count < 4)
        {
            arrow.rectTransform.anchorMin = new Vector2(0.05f, itemsNames[cursorPos].rectTransform.anchorMin.y);
            arrow.rectTransform.anchorMax = new Vector2(0.15f, itemsNames[cursorPos].rectTransform.anchorMax.y);
        }
        else
        {
            itemsNames[0].text = bag.items[cursorPos].name;
            itemsNames[1].text = bag.items[cursorPos + 1].name;
            itemsNames[2].text = bag.items[cursorPos + 2].name;
        }
	}
}
