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
        objsTransform = GameObject.Find("objs").transform;
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
            if (prefabDic.ContainsKey(objData.prefabName))
            {
                GameObject go;
                if (objData.tag != "Player")
                {
                    go = (GameObject)GameObject.Instantiate(prefabDic[objData.prefabName]);
                    go.transform.parent = objsTransform;
                    go.transform.position = objData.pos;
                    go.transform.eulerAngles = objData.rot;
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