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
            //SHIT CODE BELOW
            try
            {
                IfStatement();
                Result = "No errors";
            }
            catch(Exception ex)
            {
                Result = "error! " +  ex.Message;
            }
        }

        private void IfStatement()
        {
            Word("if");
            LogicalRelation();
            Word("then");
            FunctionCall();
            Word(";");
            LexemListisEmpty();
        }

        private void LexemListisEmpty()
        {
            if (GetNextLex())
            {
                throw new Exception("No lexems after ; expected");
            }
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

        private void CheckNextLex(Func<bool> predicate, Func<string> ErrorMessage)
        {
            if (!GetNextLex() || !predicate())
            {
                throw new Exception(ErrorMessage());
            }
        }

        private void Word(string word) =>
            CheckNextLex(() => (_currentLex.value == word), 
                         () => $"{word} expected");

        private void Identifier() =>
             CheckNextLex(() => (_currentLex.type == LexType.IDN), 
                          ()=> "identifier expected");

        private void RelationOperator() =>
            CheckNextLex(() => (_relationOperatorsList.Contains(_currentLex.value)), 
                         () => $"unknown relation operator {_currentLex.value}");

        private void LiteralOrIdentifier() =>
            CheckNextLex(() => (_currentLex.type == LexType.IDN || _currentLex.type == LexType.LIT), 
                         () => "Literal or identifier expected");

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
