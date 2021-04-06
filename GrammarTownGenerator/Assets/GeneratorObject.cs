using UnityEngine;
using System.Collections.Generic;

public class GeneratorObject
{
    public Dictionary<string, string> parameters;
    public List<Relation> relations;
    bool drawn = false;


    public GeneratorObject()
    {
        parameters = new Dictionary<string, string>();
        relations = new List<Relation>();
    }

    public string getType()
    {
        return parameters["type"];
    }

    public bool isDrawn()
    {
        return drawn;
    }

    public void setAsDrawn()
    {
        drawn = true;
    }

    public string getName()
    {
        if(parameters.ContainsKey("name")) return parameters["name"];
        return getType();
    }

    public string toString()
    {
        string ret = getType() + "\n";
        if(parameters != null)
            foreach (KeyValuePair<string, string> parameter in parameters) 
               if(parameter.Key != "type")
                    ret += parameter.Key + ": " + parameter.Value + "\n";
        if (relations != null)
        {
            ret += "relations:\n";
            foreach (Relation relation in relations) ret += relation.fullString(this);
        }
        return ret;
    }
}
