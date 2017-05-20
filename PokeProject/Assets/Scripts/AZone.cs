using UnityEngine;
using System.Collections;

abstract public class AZone : MonoBehaviour
{
    public static AZone instance;

    private GameObject saveCell;
    // Width and Height should be set for each zone
    protected abstract int width { get; }
    protected abstract int height { get; }
    protected abstract Vector2 spawn { get;  }
    // Map is computed on Start
    protected GameObject[,] map;

    void Start()
    {
        instance = this;
        loadMap();
        print(saveCell);
    }

    public AZone getClass()
    {
        return (this);
    }

    public bool isPositionValid(ref Vector2 target, PlayerMovement.eDirection direction)
    {
        int y = (int)target.y;
        int x = (int)target.x;
        if ((target.y >= 0 && target.y < height) && (target.x >= 0 && target.x < width))
        {
            if ((map[y, x] == null) || (map[y, x].CompareTag("Tall grass")))
            {
                return (true);
            }
            else if (map[y, x].tag.StartsWith("Obstacle"))
            {
                switch (direction)
                {
                    case PlayerMovement.eDirection.UP:
                        if (map[y, x].tag.EndsWith("Upward")) {
                            target.y += 1;
                            return (true);
                        }
                        break;
                    case PlayerMovement.eDirection.DOWN:
                        if (map[y, x].tag.EndsWith("Downward")) {
                            target.y -= 1;
                            return (true);
                        }
                        break;
                    case PlayerMovement.eDirection.LEFT:
                        if (map[y, x].tag.EndsWith("Leftward")) {
                            target.x -= 1;
                            return (true);
                         }
                        break;
                    case PlayerMovement.eDirection.RIGHT:
                        if (map[y, x].tag.EndsWith("Rightward")) {
                            target.x += 1;
                            return (true);
                        }
                        break;
                }
                return (false);
            }
            return (false);
        }
        else
        {
            return (false);
        }
    }

    // Called once the movement is finished so target is where the player is currently standing
    public void updatePlayerPos(Vector2 origin, Vector2 target)
    {
        map[(int)origin.y, (int)origin.x] = saveCell;
        GameObject targetCell = map[(int)target.y, (int)target.x];
        print(targetCell);
        if (targetCell != null)
        {
            saveCell = map[(int)target.y, (int)target.x];
            if (targetCell.CompareTag("Tall grass"))
            {
                GameManager.instance.checkEncounter();
            }
        }
        else
        {
            saveCell = null;
        }
    }

    protected void loadMap()
    {
        map = new GameObject[height, width];
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(x + 0.5f, y + 0.5f), 0.1f);
                if (colliders.Length > 0 && !colliders[0].gameObject.CompareTag("Player"))
                {
                    if (colliders[0].gameObject.CompareTag("Player"))
                    {
                        if (colliders.Length > 1)
                        {
                            saveCell = colliders[1].gameObject;
                        }
                        else
                        {
                            saveCell = null;
                        }
                    }
                    map[y, x] = colliders[0].gameObject;
                }
                else
                    map[y, x] = null;
            }
        }
    }

    private void dispMap()
    {
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                string disp = y.ToString() + " " + x.ToString() + " " + map[y, x];
                print(disp);
            }
        }
    }
    
    public GameObject getCell(int y, int x)
    {
        if (y < 0 || x < 0)
            return (null);
        return (map[y, x]);
    }


}
