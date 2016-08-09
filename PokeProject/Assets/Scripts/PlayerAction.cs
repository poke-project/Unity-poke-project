using UnityEngine;
using System.Collections;

public class PlayerAction : MonoBehaviour {

    private PlayerMovement playerMovement;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

	// Update is called once per frame
	void Update ()
    {
        if (GameManager.instance.inMenu)
        {
            return;
        }
        if (Input.GetKeyDown(InputManager.instance.action_key) &&
           !playerMovement.getInMovement())
        {
            GameObject inFront;
            switch (playerMovement.getDirection())
            {
                case PlayerMovement.eDirection.LEFT:
                    inFront = AZone.instance.getCell((int)transform.position.y, (int)transform.position.x - 1);
                                        break;
                case PlayerMovement.eDirection.RIGHT:
                    inFront = AZone.instance.getCell((int)transform.position.y, (int)transform.position.x + 1);
                    break;
                case PlayerMovement.eDirection.DOWN:
                    inFront = AZone.instance.getCell((int)transform.position.y - 1, (int)transform.position.x);
                    break;
                case PlayerMovement.eDirection.UP:
                    inFront = AZone.instance.getCell((int)transform.position.y + 1, (int)transform.position.x);
                    break;

                default:
                    inFront = null;
                    break;
            }
            if ((inFront != null) && (inFront.GetComponent<IInteractable>() != null))
            {
                inFront.GetComponent<IInteractable>().action();
            }
        }	
	}
}
