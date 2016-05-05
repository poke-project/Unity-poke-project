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
	public List<ObjectData> objects = new List<ObjectData>();

	[XmlIgnore]
	public Dictionary<string, Object>	dic = new Dictionary<string, Object>();
	[XmlIgnore]
	public string	fileName;

	private Game()
	{
		// Store prefabs with name as key
		Object[] objs = Resources.LoadAll("Prefabs");
		foreach (Object o in objs)
		{
			dic.Add(o.name, o);
		}
	}

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

	public void	saveCurrentObjects()
	{
		// Save all GameObjects in scene before serialization
		object[] obj = GameObject.FindObjectsOfType(typeof (GameObject));
		foreach (object o in obj)
		{
			ObjectData objData = new ObjectData((GameObject)o);
			objects.Add(objData);
		}
	}

	public void createObjects()
	{
		// Instantiate and place objects according to xml save
		Transform	mapTransform = GameObject.Find ("map").transform;
		foreach (ObjectData objData in objects)
		{
            if (dic.ContainsKey(objData.name))
            {
                GameObject go;
                if (objData.tag != "Player")
                {
                    go = (GameObject)GameObject.Instantiate(dic[objData.name]);
					go.transform.parent = mapTransform;
                    go.transform.position = objData.pos;
                    int index = go.name.LastIndexOf('(');
                    if (index != -1)
                    {
                        go.name = go.name.Substring(0, index);
                    }
                }
                else
                {
                }
            }
		}
	}

	public void restoreManagers()
	{
	}
}