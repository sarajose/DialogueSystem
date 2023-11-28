using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Ink.Runtime;
using UnityEngine.UI;
using System.Xml.Serialization;
using System.Xml;
using System.Globalization;
using Accord.Math;
using Accord.Math.Distances;

public class GetSentences : MonoBehaviour
{
    [SerializeField]
    private TextAsset _InkJsonFile;
    private Story story;

    public InputWord inputword;
    private InputWord.TextFeatures embedding;

    public static string directory = "/serializedData/";
    public static string fileName = "SerializedWordData.xml";

    [SerializeField]
    public List<int> listOfIndexes = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
    }

    public void OnButtonClicked() 
    {
        //Compare wordembedding to words in sentenceembeddings
        //wordEmbedding = inputword.embedding.Features;
        List<MainSerialize.WordEmbedding> deserializedData = new List< MainSerialize.WordEmbedding> ();
        embedding = inputword.embedding;

        deserializedData = SaveManager.Deserialize();
        for (int i = 0; i < deserializedData.Count; i++)
        {
            float[] sentences = deserializedData[i].embedding.Features;
            float[] singleWord = embedding.Features;
            //int match = 0;

            if (deserializedData[i].word == inputword.input)
            {
                Debug.Log("New match: " + deserializedData[i].word);
                if (!listOfIndexes.Contains(deserializedData[i].sentenceindex)) listOfIndexes.Add(deserializedData[i].sentenceindex);
            }

            else
            {
                /*for (int j = 0; j < sentences.Length; j++)
                {
                    if (sentences[j] == singleWord[j]) match++;
                    //Debug.Log(deserializedData[i].word + sentences[j]);
                    //Debug.Log(singleWord[j]);
                    //Debug.Log(match);
                }*/
                Cosine cosine = new Cosine();
                double cosineSim = cosine.Similarity(sentences.ToDouble(), singleWord.ToDouble());
                // Set threshold of cosine ismilarity to 0.8
                if (cosineSim > 0.4)
                {
                    Debug.Log("It's a match: " + deserializedData[i].word);
                    if (!listOfIndexes.Contains(deserializedData[i].sentenceindex)) listOfIndexes.Add(deserializedData[i].sentenceindex);
                }
            }

            //Save indices of choices that match
            // Proper match is 300
            /*if (match >= 299)
            { 
                Debug.Log("It's a match: " + deserializedData[i].word);
                // Fix this
                if (!listOfIndexes.Contains(deserializedData[i].sentenceindex)) listOfIndexes.Add(deserializedData[i].sentenceindex);
            }
            else
            {
                Debug.Log("It's not a match: " + deserializedData[i].word);
            }*/

            //Apply cosine similarity
            /*double cosineDistance = Distance.Cosine(sentences.ToDouble(), singleWord.ToDouble());
            Debug.Log("Cosine distance between 2 words: " + cosineDistance);*/

            /*Cosine cos = new Cosine();
            double cosineSimilarity = cos.Similarity(sentences.ToDouble(), singleWord.ToDouble());
            Debug.Log("Cosine similarity between 2 words: " + cosineSimilarity);*/
        }

        foreach (var item in listOfIndexes)
        {
            Debug.Log("Question index: " + item);
        }     
    }

    public static List<MainSerialize.WordEmbedding> Deserialize()
    {
        List<MainSerialize.WordEmbedding> desData = null;
        XmlSerializer ser = new XmlSerializer(typeof(List<MainSerialize.WordEmbedding>));
        using (XmlReader reader2 = XmlReader.Create(Application.dataPath + directory + fileName))
        {
            desData = (List<MainSerialize.WordEmbedding>)ser.Deserialize(reader2);
        }

        return desData;
    }
    void Update()
        {

        }

    [System.Serializable]
    public class WordEmbedding
    {
        public string word { get; set; }
        public int sentenceindex { get; set; }
        public TextFeatures embedding { get; set; }
    }

    [System.Serializable]
    public class TextFeatures
    {
        public float[] Features { get; set; }
    }

    public class TextInput
    {
        public string Text { get; set; }
    }
}


