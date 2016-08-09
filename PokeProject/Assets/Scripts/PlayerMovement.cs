using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public enum eDirection
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    };

    private Vector3 targetPosition;
    private Vector3 originPosition;
    private bool inMovement;
    private bool freeTarget;
    private bool refresh;
    private bool needUpdate;
    private eDirection direction;
    private Animator animator;

	void Start ()
    {
        targetPosition = transform.position;
        inMovement = false;
        freeTarget = false;
        refresh = true;
        needUpdate = true;
        direction = eDirection.DOWN;
        animator = GetComponent<Animator>();
	}

    void Update()
    {
        if (GameManager.instance.inMenu)
        {
            return;
        }
        // Get a new movement input every 0.4 seconds
        if (!inMovement && refresh)
        {
            bool isInput = false; 
            eDirection oldDirection = direction;
            if (Input.GetKey(InputManager.instance.left_key))
            {
                targetPosition = new Vector2(transform.position.x - 1, transform.position.y);
                direction = eDirection.LEFT;
                isInput = true;
            }
            if (Input.GetKey(InputManager.instance.right_key))
            {
                targetPosition = new Vector2(transform.position.x + 1, transform.position.y);
                direction = eDirection.RIGHT;
                isInput = true;
            }
            if (Input.GetKey(InputManager.instance.down_key))
            {
                targetPosition = new Vector2(transform.position.x, transform.position.y - 1);
                direction = eDirection.DOWN;
                isInput = true;
            }
            if (Input.GetKey(InputManager.instance.up_key))
            {
                targetPosition = new Vector2(transform.position.x, transform.position.y + 1);
                direction = eDirection.UP;
                isInput = true;
            }
            // Process only if new input
            if (isInput)
            {
                refresh = false;
                Invoke("refreshInput", 0.4f);
                needUpdate = true;
                if (targetPosition != transform.position)
                {
                    if (AZone.instance.isPositionValid(targetPosition))
                    {
                        originPosition = transform.position;
                        freeTarget = true;
                    }
                    else
                    {
                        if (oldDirection != direction)
                        {
                            targetPosition = transform.position;
                        }
                        else
                        {
                            Invoke("stopMovement", 1);
                        }
                        freeTarget = false;
                    }
                    inMovement = true;
                }
            }
        }
        if (inMovement)
        {
            move();
        }
        if (targetPosition == transform.position && needUpdate)
        {
            animator.SetBool("isWalking", false);
            inMovement = false;
            AZone.instance.updatePlayerPos(originPosition, targetPosition);
            needUpdate = false;
        }
	}


    private void move()
    {
        switch (direction)
        {
            case eDirection.UP:
                animator.SetFloat("x", 0);
                animator.SetFloat("y", 1);
                break;

            case eDirection.DOWN:
                animator.SetFloat("x", 0);
                animator.SetFloat("y", -1);
                break;

            case eDirection.LEFT:
                animator.SetFloat("x", -1);
                animator.SetFloat("y", 0);
               break;

            case eDirection.RIGHT:
                animator.SetFloat("x", 1);
                animator.SetFloat("y", 0);
              break;

            default:
                break;
        }
        animator.SetBool("isWalking", true);
        if (freeTarget)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, 2 * Time.deltaTime);
        }
    }

    private void stopMovement()
    {
        inMovement = false;
        targetPosition = transform.position;
    }

    private void refreshInput()
    {
        refresh = true;
    }

    public eDirection getDirection()
    {
        return (direction);
    }

    public bool getInMovement()
    {
        return (inMovement);
    }
}
