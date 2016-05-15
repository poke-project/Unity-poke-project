using UnityEngine;
using System.Collections;

abstract public class AZone : MonoBehaviour
{
    public static AZone instance;

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
             && (map[y, x] == null))
        {
            return (true);
        }
        else
            return (false);
    }

    public void updatePlayerPos(Vector2 origin, Vector2 target)
    {
        map[(int)target.y, (int)target.x] = map[(int)origin.y, (int)origin.x];
        map[(int)origin.y, (int)origin.x] = null;
    }

    protected void loadMap()
    {
        map = new GameObject[height, width];
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(x + 0.5f, y + 0.5f), 0.1f);
                if (colliders.Length > 0)
                    map[y, x] = colliders[0].gameObject;
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
