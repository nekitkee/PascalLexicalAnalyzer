using System;
using System.Collections.Generic;
using System.Text;

namespace PascalLexicalAnalizer
{
    public class SyntaxAnalyzer
    {
        private List<Lex> _lexemes;
        private int _currentLexIndex = 0;
        private Lex _currentLex = null; 
        private List<string> _relationOperatorsList = new List<string> { "<>", "=", ">", ">", "<=", ">=" };
        public string Result { get; set;} = "";

        public SyntaxAnalyzer(List<Lex> lexemes)
        {
            _lexemes = lexemes;
        }

        public void Analyze()
        {
            try
            {
                IfStatement();
                Result = "no errors";
            }
            catch(Exception ex)
            {
                Result = "error! " +  ex.Message;
            }
        }

        // if Event.Command = cmCalcButton then ClearEvent(Event);
        private void IfStatement()
        {
            Word("if");
            LogicalRelation();
            Word("then");
            FunctionCall();
            Word(";");
            LexemListisEmpty();
        }

        private void LogicalRelation()
        {
            Property();
            RelationOperator();
            LiteralOrIdentifier();
        }

        private void FunctionCall()
        {
            try
            {
                Identifier();
                Word("(");
                LiteralOrIdentifier();
                Word(")");
            }
            catch
            {
                throw new Exception($"Function call expected");
            }
        }

        private void Property()
        {
            Identifier();
            Word(".");
            Identifier();
        }

        private void LexemListisEmpty()
        {
            if (GetNextLex())
            {
                throw new Exception("No lexems after ; expected");
            }
        }

        private void ParseNextLexem(Func<bool> predicate, Func<string> errorMessage)
        {
            if (!GetNextLex() || !predicate())
            {
                throw new Exception(errorMessage());
            }
        }

        private void Word(string word) =>
            ParseNextLexem(predicate:() => (_currentLex.value == word), 
                        errorMessage:() => $"{word} expected");

        private void Identifier() =>
             ParseNextLexem(predicate:() => (_currentLex.type == LexType.IDN), 
                         errorMessage:() => "Identificator expected");

        private void RelationOperator() =>
            ParseNextLexem(predicate:() => (_relationOperatorsList.Contains(_currentLex.value)), 
                        errorMessage:() => $"unknown relation operator {_currentLex.value}");

        private void LiteralOrIdentifier() =>
            ParseNextLexem(predicate:() => (_currentLex.type == LexType.IDN || _currentLex.type == LexType.LIT), 
                        errorMessage:() => "Literal or identifier expected");

        private bool GetNextLex()
        {
            if (_currentLexIndex < _lexemes.Count)
            {
                _currentLex = _lexemes[_currentLexIndex++];
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
