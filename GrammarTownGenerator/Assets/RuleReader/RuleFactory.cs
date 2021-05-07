using UnityEngine;
using System.Collections.Generic;
using System;
using RuleReader.FSM;
using Rule;

namespace RuleReader
{
    public class RuleFactory
    {
        #region SINGLETON
        private RuleFactory() { }

        private static RuleFactory _instance;

        public static RuleFactory Factory
        {
            get
            {
                if (_instance == null)
                    _instance = new RuleFactory();

                return _instance;
            }
        }

        #endregion

        #region CONTEXT
        public class FactoryContext
        {
        }

        public class RuleContext : FactoryContext
        {
            Rule.Rule
        }

        #endregion

        FiniteStateMachine fsm;
        Dictionary<char, int> count;


        Rule.Rule rule;
        string tmp;

        Func<char, bool> alwaysTrue = _ => true;

        #region CREATE_RULE

        public static bool newRule()
        {
            Factory.rule = new Rule.Rule();
            return true;
        }

        Func<char, bool> createRule = _ => newRule();
        #endregion
        #region RULE_NAME

        public static bool setRuleName()
        {
            Factory.rule.setName(Factory.tmp);
            return true;
        }

        Func<char, bool> nameRule = _ => setRuleName();

        #endregion
        #region SAVE_STRING

        public static bool addToString(char c)
        {
            Factory.tmp += c;
            return true;
        }

        Func<char, bool> append = _ => addToString(_);

        #endregion
        #region VARIABLE_NAME

        public static bool createVariable()
        {
            Factory.rule.changeContext(Factory.tmp);
            return true;
        }

        Func<char, bool> nameVariable = _ => createVariable();

        #endregion

        Func<char, bool> isLetter = _ => char.IsLetter(_);
        Func<char, bool> isDigit = _ => char.IsDigit(_);
        Func<char, bool> isLetterOrDigit = _ => char.IsLetterOrDigit(_);
        Func<char, bool> isWhiteSpace = _ => char.IsWhiteSpace(_);

        public void buildFSM()
        {
            fsm = new FiniteStateMachine();
            count = new Dictionary<char, int>();
            State startState = fsm.getStartState();
            State state = startState;
            state = fsm.recognizeString(state, "RULE").addFunction(createRule);
            state = state.addState(isLetterOrDigit, appendRuleName);
            state = state.addState(isLetterOrDigit, state).addState(isWhiteSpace, resetContexts);

            fsm.recognizeString(state, "ENDRULE").addState(isWhiteSpace, startState);

            State rm = state;

            #region PRIORITY
            State p = fsm.recognizeString(state, "PRIORITY");
            state = p.addState(isDigit, appendPriority);
            state.addState(isDigit, state).addState(isWhiteSpace, rm);

            state = fsm.recognizeChar(p, '_').addFunction(appendPriority);
            state.addState(isLetterOrDigit, state).addState(isWhiteSpace, rm);

            state = fsm.recognizeString(p, "CHANGE").addState(isLetterOrDigit, appendPriorityRule);
            State alg = state.addState(isLetterOrDigit, state).addState(isMath, appendPriority);
            fsm.recognizeChar(alg, ';', rm);
            alg.addState(alwaysTrue, alg);

            state = fsm.recognizeChar(state, '(').addFunction(appendPriorityRule);
            State nameStarted = state.addState(isLetter, appendPriorityRule);
            nameStarted.addState(isLetterOrDigit, nameStarted);
            fsm.recognizeChar(nameStarted, ',', state);
            fsm.recognizeChar(nameStarted, ')').addFunction(appendPriorityRule).addState(isMath, alg);
            #endregion

            State ifState = fsm.recognizeString(rm, "IF").addFunction(setContextIf);
            State thenState = fsm.recognizeString(ifState, "THE").addState(
                fsm.mergeExpressions(fsm.recognizeCharExpression('N'), (char c) => counter('{', 0)),
                setContextThen);

            state = ifState;
            State rel = new State(fsm, appendRelationName);
            StateChange[] relnStateChanges =
            {
                
            };
            State param = new State(fsm, setParamContext);

            State childNameState = new State(fsm, fsm.ignore);
            StateChange childNameStateChange = new StateChange(fsm.recognizeCharExpression('"'), childNameState);
            childNameState = childNameState.addState(alwaysTrue, childNameState).
                addState(fsm.recognizeCharExpression('"'), setChildName, true);
            childNameState.addStateChange(childNameStateChange);

            State appendThenGo = new State(fsm, appendRelationName);
            appendThenGo.addState(alwaysTrue, childNameState);
            appendThenGo.addFunction(_ => changeState(_));


            StateChange[] ifStateChanges = {
                new StateChange(fsm.recognizeCharExpression('_'), appendThenGo),
                new StateChange(fsm.recognizeCharExpression('?'), new State(fsm, makeRelationToFind)),
                new StateChange(fsm.recognizeCharExpression(''))
            };
        }

        Func<char, int, bool> counter = (c, i) => Factory.count[c] == i;
    }
}