using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class MenuUI : MonoBehaviour {

    private GameObject[] boxes;
    private eMode lastMode;
    private Text dialogue;

    void Awake()
    {
        dialogue = transform.Find("Dialogue").Find("Text").GetComponent<Text>();
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
            if ((int)FightSceneManager.instance.currentMode < 4)
            {
                boxes[(int)lastMode + 1].SetActive(false);
                lastMode = FightSceneManager.instance.currentMode;
                boxes[(int)lastMode + 1].SetActive(true);
            }
        }
        dialogue.text = FightSceneManager.instance.dialogueText;
        if (dialogue.text != "")
        {
            boxes[(int)lastMode + 1].SetActive(false);
        }
    }
}
