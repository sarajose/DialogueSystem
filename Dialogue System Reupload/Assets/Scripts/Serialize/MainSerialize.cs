using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.ML;
using Microsoft.ML.Transforms.Text;
//using Newtonsoft.Json;
using System;
using System.Linq;

//using System.IO;
using Inflector;

public class MainSerialize : MonoBehaviour
{
        void Start()
    {
        // We assume we already are in knott questions
        // if there are multichoices, saves all the multichoices in a list
        /*story = new Story(_InkJsonFile.text);
        story.ContinueMaximally();
        foreach (Choice choice in story.currentChoices)
        {
            Debug.Log(choice.text);
        }*/

        List<string> questions = new List<string>();
        questions.Add("Did you know any of the people involved in the robbery?");
        questions.Add("Does you or your friends have an alibi?");
        questions.Add("Did you know any of the people?");
        questions.Add("Did you know there was a robbery?");
        questions.Add("Did you see the person who did it?");

        //Remove stopwords
        for (int i = 0; i < questions.Count; i++)
        {
            questions[i].ToLower();
            if (questions[i].EndsWith("?")) questions[i] = questions[i].Remove(questions[i].Length - 1);
            questions[i] = StopWords.RemoveStopwords(questions[i]);
        }

        // Split sentences
        string[][] splitQuestions = new string[questions.Count][];
        splitQuestions = SplitWords.Split(questions);

        //Make the words in plural be singular
        for (int i = 0; i < splitQuestions.Length; i++)
        {
            for (int j = 0; j < splitQuestions[i].Length; j++)
            {
                splitQuestions[i][j] = SingularOrStem.SingularOrStemWord(splitQuestions[i][j]);
            }
        }

        //Create word embeddings
        List<TextFeatures[]> embeddedWords = new List<TextFeatures[]> ();
        WordEmbedding current = new WordEmbedding();

        List<WordEmbedding> alldata = new List<WordEmbedding>();
        for (int i = 0; i < splitQuestions.Length; i++)
        {

            for (int j = 0; j < splitQuestions[i].Length; j++)
            {
                current.word = splitQuestions[i][j];
               //Debug.Log(current.word);
                current.sentenceindex = i;
               //Debug.Log(current.sentenceindex);
                current.embedding = WordEmbeddings(splitQuestions[i][j]);
                WordEmbedding currentTemp =  new WordEmbedding { word = current.word, sentenceindex = current.sentenceindex, embedding = current.embedding };
                alldata.Add(currentTemp);
            }
        }
        /*for (int i = 0; i < alldata.Count; i++)
        {
            Debug.Log(alldata[i].word);
            Debug.Log(alldata[i].sentenceindex);
            Debug.Log(alldata[i].embedding);
        }*/

        //Save serialized class
        //SaveManager.Save(alldata);
        SaveManager.Serialize(alldata);

        //Deserialize the class
        List<WordEmbedding> deserializedData = new List<WordEmbedding>();
        //deserializedData = SaveManager.Load();
        deserializedData = SaveManager.Deserialize();

        /*for (int i = 0; i < deserializedData.Count; i++)
        {
            Debug.Log(deserializedData[i].word);
        }*/
    }
    void Update()
    {
        
    }

    [System.Serializable]
    public class WordEmbedding
    {
        public string word { get; set; }
        public int sentenceindex { get; set; }
        public TextFeatures embedding { get; set;}
        //public [] embedding { get; set; }
        /*public WordEmbedding(string stringword, int index, TextFeatures embeddings)
        {
            word = stringword;
            sentenceindex = index;
            embedding = embeddings;
        }*/
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

    static private TextFeatures WordEmbeddings(string fullWord)
    {
        var context = new MLContext();
        var emptyData = context.Data.LoadFromEnumerable(new List<TextInput>());

        var embeddingsPipline = context.Transforms.Text.NormalizeText("Text", null, keepDiacritics: false, keepPunctuations: false, keepNumbers: false)
            .Append(context.Transforms.Text.TokenizeIntoWords("Tokens", "Text"))
            .Append(context.Transforms.Text.ApplyWordEmbedding("Features", "Tokens", WordEmbeddingEstimator.PretrainedModelKind.GloVe100D));

        var embeddingTransformer = embeddingsPipline.Fit(emptyData);
        var predictionEngine = context.Model.CreatePredictionEngine<TextInput, TextFeatures>(embeddingTransformer);

        //Embedd words
        var word = new TextInput { Text = fullWord };
        var prediction = predictionEngine.Predict(word);

        return prediction;
    }
}
