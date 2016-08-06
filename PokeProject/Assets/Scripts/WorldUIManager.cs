using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorldUIManager : MonoBehaviour
{
    public static WorldUIManager instance;

    private Transform canvas;
    private GameObject menu;
    private GameObject bagUI;

    void Awake()
    {
        instance = this;
        canvas = GameObject.Find("Canvas").transform;
        menu = canvas.Find("Menu").gameObject;
        bagUI = canvas.Find("Bag").gameObject;
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
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
        }	
	}
}
