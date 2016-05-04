using UnityEngine;
using System.Collections;

public class PersistentData : MonoBehaviour {

	public static PersistentData	instance;

	public bool shouldLoadLevel;
	public string fileName;

	void Start () {
		instance = this;
		shouldLoadLevel = false;
		fileName = "";
		Object.DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
