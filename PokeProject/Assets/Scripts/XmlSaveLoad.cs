using UnityEngine; 
using System.Collections; 
using System.Collections.Generic;
using System.Xml; 
using System.Xml.Serialization; 
using System.IO; 
using System.Text; 
using System;

public class XmlSaveLoad : MonoBehaviour {

    public string saveName;

    private Game myGame;
	private Rect save, load; 
	private string fileLocation; 
	private XmlWriterSettings settings;
	private Type[] dataTypes;

    void Awake()
    {
        fileLocation = Application.dataPath + "/Resources/Saves";
		Directory.CreateDirectory(fileLocation);

        // Peut etre utilise pour preciser les types des data serializees
        dataTypes = new Type[] { };
        settings = new XmlWriterSettings();
        settings.Indent = true;

        myGame = Game.Instance;

		save = new Rect(10,80,100,20); 
		load = new Rect(10,100,100,20);

        saveName = "pokemonSave"; 
    }

    void Start () { 

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
            loadGame();
		} 
		if (GUI.Button(save, "Save"))
		{
            saveGame();
		} 
	} 

	public void saveGame()
	{
        myGame.saveCurrentObjects();
		SerializeObject (saveName + ".xml", myGame);
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
		string data = r.ReadToEnd(); 
		r.Close(); 
		if (data != "") 
		{
            myGame = (Game)DeserializeObject(data);
            myGame.createObjects();
            myGame.restoreManagers();
		} 
	}

	public void loadGame()
	{
		// VERIFIER VALIDITE DU NOM ICI
		GameManager.instance.persistentData.fileName = saveName;
		GameManager.instance.persistentData.shouldLoadLevel = true;
		Application.LoadLevel ("cleanScene");
	}
} 