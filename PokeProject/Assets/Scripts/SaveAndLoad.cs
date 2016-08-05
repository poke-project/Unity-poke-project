using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System;

public class SaveAndLoad : MonoBehaviour {

    private Game myGame;
    private XmlWriterSettings xmlSettings;
    private Type[] dataTypes;

    [HideInInspector]
    public string savesLocation;


    void Awake()
    {
        savesLocation = UnityEngine.Application.dataPath + "/Resources/Saves";
        Directory.CreateDirectory(savesLocation);

        // May uze to specify type of serialized data
        dataTypes = new Type[] { };
        xmlSettings = new XmlWriterSettings();
        xmlSettings.Indent = true;

        myGame = Game.Instance;
    }

    void Start()
    {
        if (PersistentData.instance.shouldLoadLevel)
        {
            readLevel();
        }
    }



    private byte[] StringToUTF8ByteArray(string xmlString)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        byte[] byteArray = encoding.GetBytes(xmlString);
        return (byteArray);
    }

    private void SerializeGame(Stream fs, Game obj)
    {
        XmlWriter xmlWriter = XmlWriter.Create(fs, xmlSettings);
        XmlSerializer xs = new XmlSerializer(typeof(GameData));
        xs.Serialize(xmlWriter, obj.gameData);
        xmlWriter.Close();
    }

    private object DeserializeString(string xmlizedString)
    {
        XmlSerializer xs = new XmlSerializer(typeof(GameData));
        MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(xmlizedString));
        return (xs.Deserialize(memoryStream));
    }

    private void readLevel()
    {
        StreamReader r = File.OpenText(savesLocation + "/" + PersistentData.instance.fileName);
        string data = r.ReadToEnd();
        r.Close();
        if (data != "")
        {
            print(myGame.GetHashCode());
            print(Game.Instance.GetHashCode());
            GameData gameData = ((GameData)DeserializeString(data));
            myGame.loadFromData(gameData);
            /*myGame.createObjects();
            myGame.restoreManagers();
            myGame.party.loadFromData();*/
        }
    }

    private void loadGame()
    {
        PersistentData.instance.shouldLoadLevel = true;
        UnityEngine.Application.LoadLevel("cleanScene");
    }

    public void saveGame()
    {
        myGame.saveCurrentObjects();
        myGame.setData();
        Stream myStream = new FileStream(savesLocation + "/" + PersistentData.instance.fileName, FileMode.Create);
        if (myStream != null && myStream.CanWrite)
        {
            print(myGame.party.ToString());
            SerializeGame(myStream, myGame);
            myStream.Close();
        }
    }

    public void openGame()
    {
        if (File.Exists(savesLocation + "/" + PersistentData.instance.fileName))
        {
            loadGame();
        }
    }
    
    void Update()
    {
        print(myGame.party.ToString());
        print(Game.Instance.party.ToString());
    }


    // Windows only
    /*public void openProject()
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.InitialDirectory = savesLocation;
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            if ((ofd.OpenFile() != null))
            {
                loadGame(ofd.FileName);
            }
        }
    }

    public void saveProject()
    {
        SaveFileDialog sfd = new SaveFileDialog();
        sfd.InitialDirectory = savesLocation;
        sfd.AddExtension = true;
        sfd.DefaultExt = "xml";
        if (sfd.ShowDialog() == DialogResult.OK)
        {
            Stream myStream;
            if ((myStream = sfd.OpenFile()) != null)
            {
                myGame.saveCurrentObjects();
                SerializeGame(myStream, myGame);
                myStream.Close();
            }
        }
    }*/
}
