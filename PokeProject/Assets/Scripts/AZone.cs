using UnityEngine;
using System.Collections;

abstract public class AZone : MonoBehaviour
{
    public static AZone instance;

    private GameObject saveCell;
    // Width and Height should be set for each zone
    protected abstract int width
    {
        get;
    }
    protected abstract int height
    {
        get;
    }
    // Map is computed on Start
    protected GameObject[,] map;

    void Start()
    {
        instance = this;
        loadMap();
    }

    public AZone getClass()
    {
        return (this);
    }

    public bool isPositionValid(Vector2 target)
    {
        int y = (int)target.y;
        int x = (int)target.x;
        if ((target.y >= 0 && target.y < height) && (target.x >= 0 && target.x < width)
             && ((map[y, x] == null) || (map[y, x].tag == "Tall grass")))
        {
            return (true);
        }
        else
            return (false);
    }

    public void updatePlayerPos(Vector2 origin, Vector2 target)
    {
        map[(int)origin.y, (int)origin.x] = saveCell;
        GameObject targetCell = map[(int)target.y, (int)target.x];
        if (targetCell != null && targetCell.CompareTag("Tall grass"))
        {
            saveCell = map[(int)target.y, (int)target.x];
            GameManager.instance.checkEncounter();
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
