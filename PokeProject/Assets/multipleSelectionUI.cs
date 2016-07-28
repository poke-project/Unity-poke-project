using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class multipleSelectionUI : MonoBehaviour {

    private Image[] arrowHolders;
    private Sprite arrow;
    private Sprite blank;

    private int lastSelected;

    void Awake()
    {
        arrowHolders = GetComponentsInChildren<Image>();
        arrow = arrowHolders[1].sprite;
        lastSelected = 1;
    }

	// Use this for initialization
	void Start () {
        blank = FightSceneManager.instance.blank;
	}
	
	// Update is called once per frame
	void Update () {
        if (lastSelected != FightSceneManager.instance.currentSelection + 1)
        {
            arrowHolders[FightSceneManager.instance.currentSelection + 1].sprite = arrow;
            arrowHolders[lastSelected].sprite = blank;
            lastSelected = FightSceneManager.instance.currentSelection + 1;
        }
	}
}
