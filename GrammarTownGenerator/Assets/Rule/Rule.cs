using UnityEngine;
using System.Collections.Generic;

namespace Rule
{
    public class Rule
    {
        string name;
        float priority;
        List<Rule> copies;
        Dictionary<Rule, string> priorityChange;
        Dictionary<string, string> variables;
        List<RuleObject> objects;
        List<RuleRelation> relations;
        List<RuleObject> objectsToCreate;
        List<RuleCondition> conditions;
    }
}
