using UnityEngine;
using System.Collections;

public class PersistentData : MonoBehaviour {

    public static PersistentData instance;

    [HideInInspector]
	public bool shouldLoadLevel;
    [HideInInspector]
	public string fileName;

    void Awake()
    {
        instance = this;
        shouldLoadLevel = false;
        fileName = "save.xml";
		DontDestroyOnLoad(gameObject);
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

	void Start () {
	}
	
	void Update () {
	
	}
}
