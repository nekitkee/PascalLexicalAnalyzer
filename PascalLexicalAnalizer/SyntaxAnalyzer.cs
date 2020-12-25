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

        /// <summary>
        /// SOURCE STRING FOR ANALYS
        /// if Event.Command = cmCalcButton then ClearEvent(Event);
        /// </summary>
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

        /// <summary>
        /// Parse if statement
        /// </summary>
        private void IfStatement()
        {
            Word("if");
            Property();
            RelationOperator();
            LiteralOrIdentifier();
            Word("then");
            FunctionCall();
            Word(";");
        }

        /// <summary>
        /// Parse function call with one param (ex. ClearEvent(Event) )
        /// </summary>
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

        /// <summary>
        /// Parse word 
        /// </summary>
        private void Word(string keyword)
        {
            if (!GetNextLex() || _currentLex.value != keyword)
            {
                throw new Exception($"{keyword} expected");
            }
        }

        /// <summary>
        /// Parse property  (ex. Event.Command)
        /// </summary>
        private void Property()
        {
            Identifier();
            Word(".");
            Identifier();
        }

        /// <summary>
        /// Next lexem is identifier
        /// </summary>
        private void Identifier()
        {
            if (!GetNextLex() || _currentLex.type != LexType.IDN)
            {
                throw new Exception("identifier expected");
            }
        }

        /// <summary>
        /// Next lexem is relation operator (<> , > , = etc) 
        /// if not - throw Exception
        /// </summary>
        private void RelationOperator()
        {
            if (!GetNextLex() || !_relationOperatorsList.Contains(_currentLex.value))
            {
                throw new Exception($"unknown relation operator {_currentLex.value}");
            }
        }

        /// <summary>
        /// Next lexem is litteral or identifier
        /// </summary>
        private void LiteralOrIdentifier()
        {
            if (!GetNextLex() || !(_currentLex.type == LexType.IDN || _currentLex.type == LexType.LIT))
            {
                throw new Exception("identificator or litteral expected");
            }
        }

        /// <summary>
        /// Update _currentLexem
        /// </summary>
        /// <returns>false if end of list</returns>
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
