using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml; 
using System.Xml.Serialization;

// Class holding the game's data
public class Game
{
	private static Game	instance;

	[XmlArray(ElementName = "GameObjects")]
	public List<ObjectData> objects;
    public Party party;

    [XmlIgnore]
    public Dictionary<string, Object> prefabDic;
    [XmlIgnore]
    private Transform objsTransform;



    public static Game Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new Game();
			}
			return instance;
		}
	}


	private Game()
	{
        prefabDic = new Dictionary<string, Object>();
        objects = new List<ObjectData>();
		// Store prefabs with name as key
		Object[] objs = Resources.LoadAll("Prefabs");
		foreach (Object o in objs)
		{
			prefabDic.Add(o.name, o);
		}
        objsTransform = GameObject.Find("map").transform;
        party = Object.FindObjectOfType<Player>().party;
	}

	
	public void	saveCurrentObjects()
	{
        // Save all GameObjects in scene before serialization
        objects.Clear();
        GameObject[] objs = (GameObject[])GameObject.FindObjectsOfType<GameObject>();
		foreach (GameObject go in objs)
		{
            if (go.GetComponent<IMySerializable>() != null)
            {
                ObjectData objData = new ObjectData(go);
                objects.Add(objData);
            }
		}
	}

	public void createObjects()
	{
		// Instantiate and place objects according to xml save
		foreach (ObjectData objData in objects)
		{
            Debug.Log(objData.pos);
            if (prefabDic.ContainsKey(objData.prefabName))
            {
                GameObject go;
                // May be useless
                if (objData.tag != "Player")
                {
                    go = (GameObject)GameObject.Instantiate(prefabDic[objData.prefabName]);
                    go.transform.parent = objsTransform;
                    go.transform.position = objData.pos;
                    go.transform.eulerAngles = objData.rot;
                    go.name = removeBrackets(go.name);
                }
                else
                {
                }
            }
		}
	}

    private string removeBrackets(string s)
    {
        int index = s.LastIndexOf('(');
        if (index != -1)
        {
            return (s.Substring(0, index));
        }
        return (s);
    }

	public void restoreManagers()
	{
	}
}