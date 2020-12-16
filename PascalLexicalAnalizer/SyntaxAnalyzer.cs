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

    public SyntaxAnalyzer(List<Lex> lexemes)
        {
            _lexemes = lexemes;
        }

        /// <summary>
        /// SOURCE STRING FOR ANALYS
        /// if (Status = csError) and(Key <> 'C') then Key:= ' ';
        /// </summary>
        public void Analyze()
        {
            try
            {

                while (GetNextLex())
                {
                    if (_currentLex.value == "if")
                    {
                        IfStatement();
                    }
                }

                Console.WriteLine("NO SYNTAX ERROR");
            }
            catch(Exception ex)
            {
                Console.Write("SYNTAX ERROR: ");
                Console.WriteLine(ex.Message);
            }
        }

        private void IfStatement()
        {
            LogicalExpr();

            if (GetNextLex() && _currentLex.value == "then")
            {
                Statement();
            }
            else
            {
                throw new Exception("Then expected");
            }
        }

        private void Statement()
        {
            while (GetNextLex())
            {
                if (_currentLex.value == ";")
                {
                    return;
                }
            }

            throw new Exception(" ; expected");
        }

        //can be --- (logexp) AND | OR (logexpt)  --- NOT logexpr
        private void LogicalExpr()
        {
            if (GetNextLex() && _currentLex.value == "(")
            {
                //parse logic relation
                LogicalRelationExpr();
                if (!GetNextLex() || !(_currentLex.value == "and" || _currentLex.value == "or"))
                {
                    throw new Exception("logical operator expected");
                }
                if (GetNextLex() && _currentLex.value == "(")
                {
                    LogicalRelationExpr();
                }
                else
                {
                    throw new Exception(" ( - expected");
                }
            }
            else if (_currentLex.value == "not")
            {
                // check 
            }
            else
            {
                throw new Exception("logical expr should start with ( ");
            }
        }

        private void LogicalRelationExpr()
        {
            if (!GetNextLex() || !(_currentLex.type == LexType.IDN || _currentLex.type == LexType.LIT))
            {
                throw new Exception("first memeber of logical relation should be identificator or litteral");
            }

            if (!GetNextLex() || !_relationOperatorsList.Contains(_currentLex.value))
            {
                throw new Exception($"unknown relation operator {_currentLex.value}");
            }

            if (!GetNextLex() || !(_currentLex.type == LexType.IDN || _currentLex.type == LexType.LIT))
            {
                throw new Exception("second memeber of logical relation should be identificator or litteral");
            }

            if (!GetNextLex() || !(_currentLex.value == ")"))
            {
                throw new Exception(" ) expected");
            }
        }


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
