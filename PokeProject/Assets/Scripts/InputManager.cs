using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    public static InputManager instance;

    public KeyCode left_key;
    public KeyCode right_key;
    public KeyCode up_key;
    public KeyCode down_key;
    public KeyCode menu_key;
    public KeyCode back_key;

    void Awake()
    {
        instance = this;
    }

	void Start ()
    {
        left_key = KeyCode.A;
        right_key = KeyCode.D;
        up_key = KeyCode.W;
        down_key = KeyCode.S;
	}
	
	void Update () {
	
	}
}
