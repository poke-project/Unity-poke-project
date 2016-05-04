using UnityEngine; 
using System.Collections; 
using System.Collections.Generic;
using System.Xml; 
using System.Xml.Serialization; 
using System.IO; 
using System.Text; 

public class XmlSaveLoad : MonoBehaviour {

	private Rect save, load; 
	private string fileLocation; 
	private Game myGame; 
	private string data; 
	private XmlWriterSettings settings;

	void Start () { 
		save = new Rect(10,80,100,20); 
		load = new Rect(10,100,100,20); 

		fileLocation = Application.dataPath + "/Resources/Saves"; 
		Directory.CreateDirectory(fileLocation);

		myGame = new Game();

		settings = new XmlWriterSettings();
		settings.Indent = true;

		if (PersistentData.instance.shouldLoadLevel)
		{
			loadLevel();
		}
	} 
	
	void Update () {} 
	
	void OnGUI() 
	{    
		if (GUI.Button(load, "Load"))
		{ 
			PersistentData.instance.shouldLoadLevel = true;
			PersistentData.instance.fileName = "mdrfile";
            Application.LoadLevel("cleanScene");
		} 
		
		if (GUI.Button(save, "Save"))
		{ 
			myGame.saveCurrentObjects();
			SerializeObject("mdrfile", myGame.objects);
		} 
	} 

	public void saveGame(string fileName)
	{
		myGame.saveCurrentObjects ();
		SerializeObject (fileName, myGame.objects);
	}

	private byte[] StringToUTF8ByteArray(string xmlString) 
	{ 
		UTF8Encoding encoding = new UTF8Encoding(); 
		byte[] byteArray = encoding.GetBytes(xmlString); 
		return (byteArray); 
	} 
	
	private void SerializeObject(string fileName, object obj) 
	{
		Stream fs = new FileStream(fileLocation + "/" + fileName, FileMode.Create);
		XmlWriter xmlWriter = XmlWriter.Create (fs, settings);
		XmlSerializer xs = new XmlSerializer(typeof(List<GameObjectData>)); 
		xs.Serialize(xmlWriter, obj); 
		xmlWriter.Close();
	} 
	
	private object DeserializeObject(string xmlizedString) 
	{ 
		XmlSerializer xs = new XmlSerializer(typeof(List<GameObjectData>)); 
		MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(xmlizedString)); 
		return (xs.Deserialize(memoryStream)); 
	} 
	
	private void loadLevel()
	{
		StreamReader r = File.OpenText(fileLocation + "/" + PersistentData.instance.fileName); 
		data = r.ReadToEnd(); 
		r.Close(); 
		if (data != "") 
		{ 
			myGame.objects = (List<GameObjectData>)DeserializeObject(data); 
			myGame.createObjects();
		} 
	}

	public void loadGame(string toLoad)
	{
		// VERIFIER VALIDITE DU NOM ICI
		PersistentData.instance.fileName = toLoad;
		PersistentData.instance.shouldLoadLevel = true;
		Application.LoadLevel ("cleanScene");
	}
} 