using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml; 
using System.Xml.Serialization;

public class Game
{
	public string	fileName;
	public string	test;

	[XmlArray(ElementName = "Objects")]
	public List<GameObjectData> objects = new List<GameObjectData>();
	public Dictionary<string, Object>	dic = new Dictionary<string, Object>();

	public Game()
	{
		Object[] objs = Resources.LoadAll("Prefabs");
            Debug.Log("add : " + objs.Length);
		foreach (Object o in objs)
		{
			dic.Add(o.name, o);
            Debug.Log("add : " + o.name);
		}
	}

	public void	saveCurrentObjects()
	{
		object[] obj = GameObject.FindObjectsOfType(typeof (GameObject));
		foreach (object o in obj)
		{
			GameObjectData objData = new GameObjectData((GameObject)o);
			objects.Add(objData);
		}
	}

	public void createObjects()
	{
		foreach (GameObjectData objData in objects)
		{
            if (dic.ContainsKey(objData.data.name))
            {
                GameObject go;
                if (objData.data.tag != "Player")
                {
                    go = (GameObject)GameObject.Instantiate(dic[objData.data.name]);
                    go.transform.position = new Vector3(objData.data.x, objData.data.y, objData.data.z);
                }
                else
                {
                    // If gameObject is the player
                }
                //go.transform.position = new Vector3(objData.data.x, objData.data.y, objData.data.z);
            }
		}
	}
}