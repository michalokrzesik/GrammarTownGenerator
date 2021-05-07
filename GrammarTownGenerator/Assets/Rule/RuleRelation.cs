using UnityEngine;
using System.Collections.Generic;

namespace Rule
{
    public class RuleRelation
    {
        Rule rule;
        Relation relation;

        Dictionary<string, string> parameters;
        Dictionary<string, RuleObject> relationObjects;
    }
}