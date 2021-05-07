using UnityEngine;
using System.Collections.Generic;
using System;


namespace RuleReader.FSM
{
    public class State
    {
        FiniteStateMachine fsm;

        List<Func<char, bool>> _functions;

        List<StateChange> nextStates;

        public State(FiniteStateMachine finiteStateMachine, List<Func<char, bool>> functions) =>
            (fsm, _functions, nextStates) = (finiteStateMachine, functions, new List<StateChange>()); 

        public State(FiniteStateMachine finiteStateMachine, Func<char, bool> function) 
        {
            fsm = finiteStateMachine;
            _functions = new List<Func<char, bool>>();
            _functions.Add(function);
            nextStates = new List<StateChange>();
        }

        public State addFunction(Func<char, bool> function)
        {
            _functions.Add(function);
            return this;
        }

        public State addFunctions(List<Func<char, bool>> functions)
        {
            _functions.AddRange(functions);
            return this;
        }

        public State addState(Func<char, bool> expression, List<Func<char, bool>> functions, bool topPriority = false)
        {
            State newState = new State(fsm, functions);
            return addState(expression, newState, topPriority);
        }

        public State addState(Func<char, bool> expression, Func<char, bool> function, bool topPriority = false)
        {
            List<Func<char, bool>> functions = new List<Func<char, bool>>();
            functions.Add(function);

            return addState(expression, functions, topPriority);
        }

        public State addState(Func<char, bool> expression, State state, bool topPriority = false)
        {
            StateChange stateChange = new StateChange(expression, state);
            if (topPriority)
                nextStates.Insert(0, stateChange);
            else
                nextStates.Add(stateChange);
            return state;
        }


        public State changeState(char c)
        {
            foreach (StateChange stateChange in nextStates)
                if (stateChange.checkExpression(c))
                {
                    State state = stateChange.getState();
                    foreach(Func<char, bool> function in state._functions)
                        function(c);
                    return state;
                }

            if (fsm.IGNORE_WHITESPACES && char.IsWhiteSpace(c))
                return this;

            throw new Exception("Syntax error: Unexpected character: " + c);
        }

        public void addStateChange(StateChange stateChange)
        {
            nextStates.Add(stateChange);
        }
        public void addStateChanges(StateChange[] stateChanges)
        {
            nextStates.AddRange(stateChanges);
        }

    }
}
