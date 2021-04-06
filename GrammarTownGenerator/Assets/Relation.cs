using UnityEngine;
using System.Collections.Generic;

public class Relation
{
    public Dictionary<string, string> parameters;
    public Dictionary<string, GeneratorObject> relationObjects;

    public GeneratorObject getRelationObject(string name)
    {
        if(relationObjects.ContainsKey(name))
            return relationObjects[name];
        return null;
    }

    public string getParameter(string name)
    {
        if (parameters.ContainsKey(name))
            return parameters[name];
        return null;
    }

    public GeneratorObject parent()
    {
        return relationObjects["parent"];
    }

    public GeneratorObject child()
    {
        return getRelationObject("child");
    }


    public Relation()
    {
        parameters = new Dictionary<string, string>();
        relationObjects = new Dictionary<string, GeneratorObject>();
    }

    public string getType()
    {
        return parameters["type"];
    }

    public string toString()
    {
        return getRelationObject("parent").getName() + " is in relation " + getType() + " with " + getRelationObject("child").getName();
    }

    public string fullString(GeneratorObject from)
    {
        if (from == getRelationObject("parent"))
        {
            GridLayout layout = new GridLayout();
            
            string ret = "{\n";
            foreach (KeyValuePair<string, string> parameter in parameters) 
                ret += parameter.Key + ": " + parameter.Value + "\n";
            return ret + getRelationObject("child").toString() + "\n}\n";
        }

        return "";
    }

}
