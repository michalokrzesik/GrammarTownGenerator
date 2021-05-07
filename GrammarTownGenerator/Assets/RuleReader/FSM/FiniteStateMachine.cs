using UnityEngine;
using System.Collections.Generic;
using System;


namespace RuleReader.FSM
{
    public class FiniteStateMachine
    {
        /// <summary>
        /// If state has no reaction on whitespace, the whitespace is ignored
        /// Probably no reason to change it to false, as probably will only throw more exceptions
        /// </summary>
        public bool IGNORE_WHITESPACES = true;

        /// <summary>
        /// Ignore character case in stateChanges created by recognizeChar and recognizeString
        /// Custom stateChanges will remain unchanged
        /// </summary>
        public bool IGNORE_CASE = true;

        State start;
        State currentState;

        /// <summary>
        /// Lambda expression that does nothing
        /// </summary>
        public Func<char, bool> ignore = _ => true;

        public FiniteStateMachine()
        {
            start = new State(this, ignore);
            currentState = start;
        }

        /// <summary>
        /// Creates new blank FiniteStateMachine
        /// To add states get starter state by getStartState and add them manually:
        /// - recognizeChar(state, char) and recognizeString(state, string)
        /// - state.addState for custom checks
        /// To add actions on entering state use state.addFunction or state.addFunctions
        /// </summary>
        /// <returns></returns>
        public static FiniteStateMachine NewMachine()
        {
            return new FiniteStateMachine();
        }

        public static FiniteStateMachine NewMachine(State start)
        {
            FiniteStateMachine fsm = new FiniteStateMachine();
            fsm.start = start;
            return fsm;
        }


        public Func<char, bool> mergeExpressions(Func<char, bool> func1, Func<char, bool> func2)
        {
            return _ => func1(_) && func2(_);
        }

        public Func<char, bool> recognizeCharExpression(char c)
        {
            if (IGNORE_CASE)
                return _ => char.ToUpper(_) == c;
            return _ => _ == c;
        }

        public State recognizeChar(State state, char c, State nextState = null)
        {
            if (IGNORE_CASE) c = char.ToUpper(c);
            try
            {
                State temp = state.changeState(c);
                return temp;
            }
            catch
            {
                if(nextState != null)
                    return state.addState(recognizeCharExpression(c), nextState);
                
                return state.addState(recognizeCharExpression(c), ignore);
            }
        }

        public State recognizeString(State start, string s, State nextState = null)
        {
            State currentState = start;
            foreach (char c in s)
                currentState = recognizeChar(currentState, c, nextState);
            return currentState;
        }

        /// <summary>
        /// Returns starter state, but does NOT restart machine. To restart machine, use thisFiniteStateMachine.restart
        /// thisFiniteStateMachine.changeState will run thisFiniteMachine.currentState.changeState
        /// startState.changeState will NOT change thisFiniteMachine.currentState or thisFiniteMachine.startState
        /// Can be used to run pararel FiniteStateMachine manually or by FiniteStateMachine.NewMachine
        /// </summary>
        /// <returns></returns>
        public State getStartState() { return start; }

        /// <summary>
        /// Returns current state.
        /// To run this FiniteStateMachine, use thisFiniteStateMachine.changeState
        /// currentState.changeState will NOT change thisFiniteStateMachine.currentState
        /// Can be used to run pararel FiniteStateMachine manually or by FiniteStateMachine.NewMachine
        /// </summary>
        /// <returns></returns>
        public State getCurrentState() { return currentState; }

        public FiniteStateMachine restartMachine()
        {
            currentState = start;
            return this;
        }


        public FiniteStateMachine changeState(char c)
        {
            currentState = currentState.changeState(c);
            return this;
        }

        public FiniteStateMachine changeState(string s)
        {
            foreach (char c in s)
                currentState = currentState.changeState(c);
            return this;
        }
    }
}
