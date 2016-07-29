using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;


public class MenuUI : MonoBehaviour {

    private GameObject actionBox;
    private GameObject fightBox;
    private GameObject[] boxes;
    private eMode lastMode;

    void Awake()
    {
        boxes = new GameObject[transform.childCount];
        int i = 0;
        foreach (Transform child in transform)
        {
            boxes[i] = child.gameObject;
            ++i;
        }
        lastMode = eMode.MENU;
    }

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        // This way or add a UnityEvent in FightSceneManager and call
        // method here when needed
        if (FightSceneManager.instance.currentMode != lastMode)
        {
            boxes[(int)lastMode].SetActive(false);
            lastMode = FightSceneManager.instance.currentMode;
            boxes[(int)lastMode + 1].SetActive(true);
        }
	}
}
