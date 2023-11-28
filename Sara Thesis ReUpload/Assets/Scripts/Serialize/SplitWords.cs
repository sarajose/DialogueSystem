using System.Collections;
using System.Collections.Generic;
internal class SplitWords
{
    public static string[][] Split(List<string> questions)
    {
        string[][] splitQuestions = new string[questions.Count][];
        for (int i = 0; i < questions.Count; i++)
        {
            string[] words = questions[i].Split(' ');
            splitQuestions[i] = words;
        }

        return splitQuestions;
    }
}