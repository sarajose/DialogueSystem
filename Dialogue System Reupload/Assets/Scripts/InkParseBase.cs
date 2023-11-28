using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;
using UnityEngine.EventSystems;

public class InkParseBase : MonoBehaviour
{
    [SerializeField]
    private TextAsset _InkJsonFile;
    private Story story;

    public Text textPrefab;
    public Button buttonPrefab;

    public Dropdown dropdownPrefab;

    [SerializeField]
    private GameObject inputCanvas;
    // Get component by name ?
    //public GetSentences getSentences;

    [SerializeField]
    private GameObject embeddings;

    private bool choices;
    private bool insideDropdown;

    private List<int> indexes;

    void Start()
    {
        story = new Story(_InkJsonFile.text);
        DisplayNextLine();
        //int trustlevel = (int)story.variablesState["trustlevel"];
        //Debug.Log(trustlevel);
        inputCanvas.SetActive(false);

        choices = true;
        //new
        insideDropdown = false;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // fix choices
            if (choices)
            {
                eraseUI();
            }
            DisplayNextLine();
        }

        //new
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (insideDropdown)
            {
                eraseUI();
                inputCanvas.SetActive(true);
                activateDropdown();
                indexes.Clear();
            }
        }
    }

    public void DisplayNextLine()
    {
        //new
        insideDropdown = false;
        if (story.canContinue) // Checking if there is content to go through
        {
            Text storyText = Instantiate(textPrefab) as Text;
            string text = "";

            text = story.Continue(); //Gets Next Line
            text = text?.Trim(); //Removes White space from the text
            text = displayTags(text);

            storyText.text = text;
            storyText.transform.SetParent(this.transform, false);
        }

        else
        {
            if (choices)
            {
                displayChoices();
                choices = false;
            }
        }
    }

    public string displayTags(string text)
    {
        List<string> tags = story.currentTags;
        if (tags.Count > 0)
        {
            //Create a new text prefab for that
            text = "<b>" + tags[0] + "</b> - " + text;
        }
        return text;
    }

    public void displayChoices()
    {
        //if((bool)story.variablesState["isMain"]) or if currentchoices > 10 per exemple
        // declared panel and set panel to parent as well
        
        if (story.currentChoices.Count > 5)
        {
            inputCanvas.SetActive(true);

            Debug.Log("Dropdown activated");
            activateDropdown();
        }
        else
        {
            foreach (Choice choice in story.currentChoices)
            {
                Button choiceButton = Instantiate(buttonPrefab) as Button;
                choiceButton.transform.SetParent(this.transform, false);

                //Text choiceText = buttonPrefab.GetComponentInChildren<Text>();
                Text choiceText = choiceButton.GetComponentInChildren<Text>();
                choiceText.text = choice.text;

                // delegate let us send a method as a parameter for another method.
                // Click a button and moves story fwd
                choiceButton.onClick.AddListener(delegate
                {
                    story.ChooseChoiceIndex(choice.index);
                    // Get variables from ink file
                    // Update variables and show them in FL panel

                    eraseUI();
                    choices = true;
                });
            }
        }
    }

    public void activateDropdown()
    {
        insideDropdown = true;
        // Put options inside a dropdown
        Dropdown dropown = Instantiate(dropdownPrefab, gameObject.transform);
        dropown.options.Clear();
 
        dropown.options.Add(new Dropdown.OptionData() { text = "Anything you would like to ask?" });

        /*foreach (Choice choice in story.currentChoices)
        {
            dropown.options.Add(new Dropdown.OptionData() { text = choice.text });
            dropown.onValueChanged.AddListener(delegate
            {
                story.ChooseChoiceIndex(choice.index);
                eraseUI();
            });
        }*/
        embeddings = GameObject.Find("Embeddings");
        GetSentences getSentences = embeddings.GetComponent<GetSentences>();
        indexes = getSentences.listOfIndexes;

        Debug.Log("Number of indexes iterations: " + getSentences.listOfIndexes.Count);
        for (int i = 0; i < getSentences.listOfIndexes.Count; i++)
        {
            Choice choice = story.currentChoices[getSentences.listOfIndexes[i]];
            Debug.Log(choice.text);
            dropown.options.Add(new Dropdown.OptionData() { text = choice.text });
            dropown.onValueChanged.AddListener(delegate
            {
                story.ChooseChoiceIndex(choice.index);
                eraseUI();
            });
        }
    }

    void eraseUI()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }

        inputCanvas.SetActive(false);
        DisplayNextLine();
    }
}