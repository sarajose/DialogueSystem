using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;

public static class SaveManager
{
    public static string directory = "/serializedData/";
    public static string fileName = "SerializedWordData_v2.xml";

    public static void Save(List<MainSerialize.WordEmbedding> we)
    {
        string m_Path = Application.dataPath + directory;
        if (!Directory.Exists(m_Path))
        {
            Directory.CreateDirectory(m_Path);
        }

        //Check jsonUtility
        string json = JsonUtility.ToJson(m_Path);
        File.WriteAllText(m_Path + fileName, json);
        Debug.Log(json);
    }

    public static void Serialize(List<MainSerialize.WordEmbedding> we)
    {
        string m_Path = Application.dataPath + directory;
        if (!Directory.Exists(m_Path))
        {
            Directory.CreateDirectory(m_Path);
        }

       /* string json = JsonConvert.SerializeObject(we);
        Debug.Log(json);
        File.WriteAllText(m_Path + fileName, JsonConvert.SerializeObject(json));*/
        using (StreamWriter writer = new StreamWriter(Application.dataPath + directory + fileName))
        {
            XmlSerializer serializer = new XmlSerializer(we.GetType());
            serializer.Serialize(writer, we);
        }
    }
    public static List<MainSerialize.WordEmbedding> Deserialize()
    {
        //CreateRoot();
        List<MainSerialize.WordEmbedding> desData= null;
        /*using (StreamReader file = File.OpenText(Application.dataPath + directory + fileName))
        {
            JsonSerializer serializer = new JsonSerializer();
            desData = (List<MainSerialize.WordEmbedding>)serializer.Deserialize(file, typeof(List<MainSerialize.WordEmbedding>));
        }*/
        /*var json = System.IO.File.ReadAllText(Application.dataPath + directory + fileName);
        desData = JsonConvert.DeserializeObject<List<MainSerialize.WordEmbedding>>(json);*/

        /*XmlSerializer serializer = new XmlSerializer(typeof(List<MainSerialize.WordEmbedding>));
        StreamReader reader = new StreamReader(Application.dataPath + directory + fileName);
        reader.ReadToEnd();
        desData = (List<MainSerialize.WordEmbedding>)serializer.Deserialize(reader);
        reader.Close();*/
        XmlSerializer ser = new XmlSerializer(typeof(List<MainSerialize.WordEmbedding>));
        using (XmlReader reader2 = XmlReader.Create(Application.dataPath + directory + fileName))
        {
            desData = (List<MainSerialize.WordEmbedding>)ser.Deserialize(reader2);
        }

        return desData;
    }
    /*public static void CreateRoot()
    {
        var report = XDocument.Load(Application.dataPath + directory + fileName);
        var newdoc = new XDocument();
        newdoc.Add(new XElement("root"));
        newdoc.Root.Add(report.Root);

        newdoc.Save(Application.dataPath + directory + fileName);
    }*/
    public static List<MainSerialize.WordEmbedding> Load()
    {
        string full_Path = Application.dataPath + directory + fileName;
        List<MainSerialize.WordEmbedding> we = new List<MainSerialize.WordEmbedding>();

        if (File.Exists(full_Path))
        {
            string json = File.ReadAllText(full_Path);
            we = JsonUtility.FromJson<List<MainSerialize.WordEmbedding>>(json);
            Debug.Log("Object serialized sucessfully");
        }
        else
        {
            Debug.Log("Save file not found");
        }
        return we;
    }
}
