using UnityEngine;
using System.Collections.Generic;
using System;

namespace RuleReader.FSM
{
    public class StateChange
    {
        Func<char, bool> _expression;
        State _state;

        public StateChange(Func<char, bool> expression, State state) => (_expression, _state) = (expression, state);

        public State getState() { return _state; }
        public bool checkExpression(char c) { return _expression(c); }

    }
}
