﻿using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour, IInteractable {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void action()
    {
        // Animation here
        Destroy(gameObject);
    }
}