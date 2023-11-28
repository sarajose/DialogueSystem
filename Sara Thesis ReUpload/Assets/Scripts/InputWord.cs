using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Microsoft.ML;
using Microsoft.ML.Transforms.Text;

using Inflector;

public class InputWord : MonoBehaviour
{
    [HideInInspector]
    public string input;
    [HideInInspector]
    public TextFeatures embedding;

    private PredictionEngine<TextInput, TextFeatures> predictionEngine;
    // Start is called before the first frame update
    void Awake()
    {
        predictionEngine = CreateEmbeddingSetting();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReadStringInput(string word)
    {
        word = word.Trim();
        if (word.All(char.IsLetter) && System.Text.RegularExpressions.Regex.IsMatch(word, "^[a-zA-Z0-9\x20]+$"))
        {
            input = word;
            Debug.Log("Valid input: " + input);

            //Singularize the word
            if (Inflector.Inflector.Singularize(word) != null)
            {
                input = Inflector.Inflector.Singularize(word);
            }

            //Create the embedding
            embedding =  CreateWordEmbedding(input, predictionEngine);
        }
        else
        {
            input = null;
            Debug.Log("Invalid input: " + input);
        }
    }

    /*public void TextChanged(string word)
    {
        while (!(word.All(char.IsLetter) && System.Text.RegularExpressions.Regex.IsMatch(word, "^[a-zA-Z0-9\x20]+$")))
        {
            Debug.Log("Invalid input. Try again " + input);
            ReadStringInput(input);
        }

        input = word;
        Debug.Log("Valid input: " + input);
    }*/

    // Fix turn to singular later or lemm
    public void CleanWord(string word)
    {
        word = word.ToLower();
        //word = SingularOrStem.Singularize(word);
    }

    public PredictionEngine<TextInput, TextFeatures> CreateEmbeddingSetting()
    {
        var context = new MLContext();

        var emptyData = context.Data.LoadFromEnumerable(new List<TextInput>());

        var embeddingsPipline = context.Transforms.Text.NormalizeText("Text", null, keepDiacritics: false, keepPunctuations: false, keepNumbers: false)
            .Append(context.Transforms.Text.TokenizeIntoWords("Tokens", "Text"))
            .Append(context.Transforms.Text.ApplyWordEmbedding("Features", "Tokens", WordEmbeddingEstimator.PretrainedModelKind.GloVe100D));

        var embeddingTransformer = embeddingsPipline.Fit(emptyData);

        var predictionEngine = context.Model.CreatePredictionEngine<TextInput, TextFeatures>(embeddingTransformer);

        return predictionEngine;
    }
    public TextFeatures CreateWordEmbedding(string word, PredictionEngine<TextInput, TextFeatures> predictionEngine)
    {
        var wordData = new TextInput { Text = word };

        var prediction = predictionEngine.Predict(wordData);

        Debug.Log($"Number of Features of {word}: {prediction.Features.Length}");

        return prediction;
    }

    public class TextFeatures
    {
        public float[] Features { get; set; }
    }

    public class TextInput
    {
        public string Text { get; set; }
    }
}
