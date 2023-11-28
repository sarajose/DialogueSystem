internal class SingularOrStem
{
    public static string SingularOrStemWord(string word)
    {
        string wordback = "";

        //Singularize
        if (Inflector.Inflector.Singularize(word) != null)
        {
            wordback = Inflector.Inflector.Singularize(word);
        }
        else
        {
            wordback = word;
        }
        return wordback;
    }
}