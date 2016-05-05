using UnityEngine;
using System.Collections;

public class PersistentData : MonoBehaviour {

    [HideInInspector]
	public bool shouldLoadLevel;
    [HideInInspector]
	public string fileName;

	void Start () {
		shouldLoadLevel = false;
		fileName = "";
		Object.DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
