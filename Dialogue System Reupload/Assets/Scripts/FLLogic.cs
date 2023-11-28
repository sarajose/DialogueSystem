using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FLS;
using FLS.Rules;
using UnityEngine.UI;
using System; // for convert
using Ink.Runtime;

public class FLLogic : MonoBehaviour
{
    public Text domText;
    public Text conText;
    public Text infText;

    //[HideInInspector]
    //public double currentTrust;

    [SerializeField]
    private TextAsset _InkJsonFile;
    private Story story;

    private float happiness;
    private float stable;
    private float trustlevel;
    private float sensitivity;

    // Start is called before the first frame update
    void Start()
    {
        // Fuzzy sets
        // happiness -> sad - joy
        var happiness = new LinguisticVariable("Happiness");
        var joy = happiness.MembershipFunctions.AddTriangle("Joy", 0, 25, 50);
        var neutral = happiness.MembershipFunctions.AddTriangle("Sad", 25, 50, 75);
        var sad = happiness.MembershipFunctions.AddTriangle("Sad", 50, 75, 100);

        // stable -> fear - anger
        var stable = new LinguisticVariable("stable");
        var fear = stable.MembershipFunctions.AddTriangle("Fear", 0, 25, 50);
        var calm = stable.MembershipFunctions.AddTriangle("Anger", 25, 50, 75);
        var anger = stable.MembershipFunctions.AddTriangle("Anger", 50, 75, 100);

        // trustlevel -> trust - disgust
        var trustlevel = new LinguisticVariable("trustlevel");
        var trust = trustlevel.MembershipFunctions.AddTriangle("Trust", 0, 25, 50);
        var indiference = trustlevel.MembershipFunctions.AddTriangle("Indiference", 25, 50, 75);
        var disgust = trustlevel.MembershipFunctions.AddTriangle("Disgust", 50, 75, 100);

        // Sensitivity -> High -low (Opcional)
        var sensitivity = new LinguisticVariable("Sensitivity");
        var high = sensitivity.MembershipFunctions.AddTriangle("High", 0, 33.33, 60);
        var med = sensitivity.MembershipFunctions.AddTriangle("High", 0, 33.33, 60);
        var low = sensitivity.MembershipFunctions.AddTriangle("Low", 33.33, 66.66, 100);

        //Next state -> output
        /*var happiness2 = happiness;
        var dependency2 = dependency;
        var stable2 = stable;*/

        // Dominance rules if s- high if s- low
        var d1 = Rule.If(happiness.Is(joy)).Then(trustlevel.Is(trust)); 
        var d2 = Rule.If(happiness.Is(neutral).Or(stable.Is(fear)).Or(stable.Is(anger))).Then(trustlevel.Is(disgust));
        var d3 = Rule.If(happiness.Is(sad).Or(stable.Is(calm))).Then(trustlevel.Is(indiference));

        //Conciousness - Steadyness
        var c1 = Rule.If(happiness.Is(joy).Or(happiness.Is(neutral))).Then(trustlevel.Is(indiference));
        var c2 = Rule.If(happiness.Is(sad).Or(stable.Is(calm))).Then(trustlevel.Is(trust));
        var c3 = Rule.If(stable.Is(anger).Or(stable.Is(fear))).Then(trustlevel.Is(indiference));

        //Influence 
        var i1 = Rule.If(happiness.Is(joy).Or(happiness.Is(neutral))).Then(trustlevel.Is(trust));
        var i2 = Rule.If(happiness.Is(sad).Or(stable.Is(calm))).Then(trustlevel.Is(indiference));
        var i3 = Rule.If(stable.Is(anger).Or(stable.Is(fear))).Then(trustlevel.Is(disgust));

        IFuzzyEngine fuzzyEngineD = new FuzzyEngineFactory().Default();
        fuzzyEngineD.Rules.Add(d1, d2, d3);
        double resultD = fuzzyEngineD.Defuzzify(new { happiness = 35, stable = 1 }); //45 in
        //currentTrust = Convert.ToDouble(resultD);
        //domText.text = resultD.ToString();

        IFuzzyEngine fuzzyEngineC = new FuzzyEngineFactory().Default();
        fuzzyEngineC.Rules.Add(c1, c2, c3);
        var resultC = fuzzyEngineC.Defuzzify(new { happiness = 35, stable = 1 }); //50 in
        //conText.text = resultC.ToString();

        IFuzzyEngine fuzzyEngineI = new FuzzyEngineFactory().Default();
        fuzzyEngineI.Rules.Add(i1, i2, i3);
        var resultI = fuzzyEngineI.Defuzzify(new { happiness = 35, stable = 1 }); //28.1 trust
        //infText.text = resultI.ToString();

        //Debug.Log(result.ToString());

        // Update state rules -- theory not now
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void GetVariables()
    {
        happiness = (int)story.variablesState["happiness"];
        stable = (int)story.variablesState["stable"];
        trustlevel = (int)story.variablesState["trustlevel"];
        sensitivity = (int)story.variablesState["sensitivity"];
    }
}