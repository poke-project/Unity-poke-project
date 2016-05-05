using UnityEngine; 
using System.Collections; 
using System.Collections.Generic;
using System.Xml; 
using System.Xml.Serialization; 
using System.IO; 
using System.Text; 
using System;

public class XmlSaveLoad : MonoBehaviour {

	private Rect save, load; 
	private string fileLocation; 
	private string data; 
	private XmlWriterSettings settings;
	private Type[] dataTypes;

	void Start () { 
		save = new Rect(10,80,100,20); 
		load = new Rect(10,100,100,20); 

		fileLocation = Application.dataPath + "/Resources/Saves"; 
		Directory.CreateDirectory(fileLocation);

		// Peut etre utilise pour preciser les types des data serializees
		dataTypes = new Type[] { };

		settings = new XmlWriterSettings();
		settings.Indent = true;

		if (GameManager.instance.persistentData.shouldLoadLevel)
		{
			loadLevel();
		}
	} 
	
	void Update () {} 
	
	void OnGUI() 
	{    
		if (GUI.Button(load, "Load"))
		{ 
			GameManager.instance.persistentData.shouldLoadLevel = true;
			GameManager.instance.persistentData.fileName = "gameSave";
            Application.LoadLevel("cleanScene");
		} 
		
		if (GUI.Button(save, "Save"))
		{ 
			GameManager.instance.game.saveCurrentObjects();
			SerializeObject("gameSave", GameManager.instance.game);
		} 
	} 

	public void saveGame(string fileName)
	{
		GameManager.instance.game.saveCurrentObjects ();
		SerializeObject (fileName, GameManager.instance.game);
	}

	private byte[] StringToUTF8ByteArray(string xmlString) 
	{ 
		UTF8Encoding encoding = new UTF8Encoding(); 
		byte[] byteArray = encoding.GetBytes(xmlString); 
		return (byteArray); 
	} 
	
	private void SerializeObject(string fileName, Game obj) 
	{
		Stream fs = new FileStream(fileLocation + "/" + fileName, FileMode.Create);
		XmlWriter xmlWriter = XmlWriter.Create (fs, settings);
		XmlSerializer xs = new XmlSerializer(typeof(Game)); 
		xs.Serialize(xmlWriter, obj); 
		xmlWriter.Close();
	} 
	
	private object DeserializeObject(string xmlizedString) 
	{ 
		XmlSerializer xs = new XmlSerializer(typeof(Game)); 
		MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(xmlizedString)); 
		return (xs.Deserialize(memoryStream)); 
	} 
	
	private void loadLevel()
	{
		StreamReader r = File.OpenText(fileLocation + "/" + GameManager.instance.persistentData.fileName); 
		data = r.ReadToEnd(); 
		r.Close(); 
		if (data != "") 
		{ 
			// Verifier que l'on recupere bien tout
			GameManager.instance.game = (Game)DeserializeObject(data); 
			GameManager.instance.game.createObjects();
            GameManager.instance.game.restoreManagers();
		} 
	}

	public void loadGame(string toLoad)
	{
		// VERIFIER VALIDITE DU NOM ICI
		GameManager.instance.persistentData.fileName = toLoad;
		GameManager.instance.persistentData.shouldLoadLevel = true;
		Application.LoadLevel ("cleanScene");
	}
} 