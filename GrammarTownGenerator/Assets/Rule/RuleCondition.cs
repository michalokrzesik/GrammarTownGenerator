using UnityEngine;
using System.Collections.Generic;

namespace Rule
{
    public class RuleCondition
    {
        List<RuleCondition> conditions;

        public bool Evaluate()
        {
            foreach (RuleCondition condition in conditions)
                if (!condition.Evaluate()) return false;
            return true;
        }
    }
}