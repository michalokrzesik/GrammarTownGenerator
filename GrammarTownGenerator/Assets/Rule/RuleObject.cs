using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Rule
{
    public class RuleObject
    {
        List<Rule> rules;
        GeneratorObject generatorObject;

        List<RuleRelation> relations;
        Dictionary<string, string> parameters;
    }
}