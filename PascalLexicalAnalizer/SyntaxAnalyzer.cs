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
        private Dictionary<string, List<Action>> _syntax;
        public SyntaxAnalyzer(List<Lex> lexemes)
        {
            _lexemes = lexemes;
            _syntax = new Dictionary<string, List<Action>>
            {
                {
                    "IfStatement",
                    new List<Action>{ () => Word("if") ,  LogicalRelation, ()=> Word("then"), FunctionCall, () => Word(";") , LexemListisEmpty }
                },
                {
                    "LogicalRelation",
                    new List<Action> { Property, RelationOperator , LiteralOrIdentifier}
                },
                {
                    "FunctionCall",
                    new List<Action> { Identifier, () => Word("("), LiteralOrIdentifier, () => Word(")") }
                },
                {
                    "Property",
                    new List<Action> { Identifier, () => Word("."), Identifier }
                }
            };
        }

        private void ParseSyntaxSequence(List<Action> parsersList) => parsersList.ForEach(p => p.Invoke());
        private void IfStatement() => ParseSyntaxSequence(_syntax["IfStatement"]);
        private void LogicalRelation() => ParseSyntaxSequence(_syntax["LogicalRelation"]);
        private void Property() => ParseSyntaxSequence(_syntax["Property"]);
        private void FunctionCall()
        {
            try
            {
                ParseSyntaxSequence(_syntax["FunctionCall"]);
            }
            catch
            {
                throw new Exception($"Function call expected");
            }
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
            
            return false;
        }
    }
}
