using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    enum eDirection
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    };

    private Vector3 targetPosition;
    private Vector3 originPosition;
    private bool inMovement;
    private eDirection direction;
    private Animator animator;

	void Start ()
    {
        targetPosition = transform.position;
        inMovement = false;
        direction = eDirection.DOWN;
        animator = GetComponent<Animator>();
	}
	
	void Update ()
    {
        // Get a new movement input
        if (!inMovement)
        {
            if (Input.GetKey(Inputs.instance.left_key))
            {
                targetPosition = new Vector2(transform.position.x - 1, transform.position.y);
                direction = eDirection.LEFT;
            }
            if (Input.GetKey(Inputs.instance.right_key))
            {
                targetPosition = new Vector2(transform.position.x + 1, transform.position.y);
                direction = eDirection.RIGHT;
            }
            if (Input.GetKey(Inputs.instance.down_key))
            {
                targetPosition = new Vector2(transform.position.x, transform.position.y - 1);
                direction = eDirection.DOWN;
            }
            if (Input.GetKey(Inputs.instance.up_key))
            {
                targetPosition = new Vector2(transform.position.x, transform.position.y + 1);
                direction = eDirection.UP;
            }
            if (targetPosition != transform.position && AZone.instance.isPositionValid(targetPosition))
            {
                originPosition = transform.position;
                inMovement = true;
            }
        }
        if (inMovement)
        {
            move();
        }
        if (targetPosition == transform.position)
        {
            animator.SetBool("isWalking", false);
            inMovement = false;
            AZone.instance.updatePlayerPos(originPosition, targetPosition);
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
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, 2 * Time.deltaTime);
    }
}
