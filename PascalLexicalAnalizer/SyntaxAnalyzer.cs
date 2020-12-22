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
        /// if (Status = csError) and(Key <> 'C') then Key:= ' ';
        /// </summary>
        public void Analyze()
        {
            bool ifound = false;
            //SHIT CODE BELOW
            try
            {
                while (GetNextLex())
                {
                    if (_currentLex.value == "if")
                    {
                        ifound = true;
                        IfStatement();
                    }
                }
                if (ifound)
                {
                    Result = "NO SYNTAX ERROR";
                }
                else
                {
                    Result = "ERROR: NO IF STATEMENT";
                }
            }
            catch(Exception ex)
            {
                Result = "SYNTAX ERROR: " +  ex.Message;
            }
        }

        /// <summary>
        /// Parse If statement 
        /// example: If {condition} then {Statement};
        /// </summary>
        private void IfStatement()
        {
            if (GetNextLex() && (_currentLex.value == "(" || _currentLex.value == "not"))
            {
                LogicalExpr();
            }
            else
            {
                throw new Exception("Logical expretion should start with ( or NOT");
            }

            if (GetNextLex() && _currentLex.value == "then")
            {
                StateCorrect();
            }
            else
            {
                throw new Exception("Then expected");
            }
        }

        /// <summary>
        /// Parse statement. 
        /// IDN := {IDN|LIT}
        /// </summary>
        private void StateCorrect()
        {
            if (!GetNextLex() || _currentLex.type != LexType.IDN)
            {
                throw new Exception(" Identificator expected in <state>");
            }

            if (!GetNextLex() || _currentLex.value != ":=")
            {
                throw new Exception(" := expected in <state>");
            }

            if (!GetNextLex() || !(_currentLex.type == LexType.IDN || _currentLex.type == LexType.LIT))
            {
                throw new Exception(" identificator or litteral expected in <state>");
            }

            if (!GetNextLex() || _currentLex.value != ";")
            {
                throw new Exception(" := expected in <state>");
            }

            if (GetNextLex())
            {
                throw new Exception("  Additional lexem after <state> not expected");
            }
        }

        /// <summary>
        /// Parse logical expression. 
        /// Structure:  (LogicalMember) {and | or}  (LogicalMember) .
        /// LogicalMember is {LogicalReleation | LogicalExpression}
        /// NOT operator is not implemented
        /// </summary>
        private void LogicalExpr()
        {
            //1-st member
            LogicalExprMember();
            LogicOperator();

            //2-nd member
            if (GetNextLex() && _currentLex.value == "(")
            {
                LogicalExprMember();
            }
            else
            {
                throw new Exception(" ( - expected");
            }
        }

        private void LogicOperator()
        {
            if (!GetNextLex() || !(_currentLex.value == "and" || _currentLex.value == "or"))
            {
                throw new Exception("logical operator expected");
            }
        }

        /// <summary>
        /// LogicalMember is LogicalReleation | LogicalExpression
        /// </summary>
        private void LogicalExprMember()
        {
            if (GetNextLex() && _currentLex.value == "(")
            {
                LogicalExpr();
                GetNextLex();
            }
            else
            {
                LogicalRelationExpr();
            }
        }

        /// <summary>
        /// Parse logical relation. 
        /// Example: (foo > bar)
        /// </summary>
        private void LogicalRelationExpr()
        {
            LiteralOrIdentifier();
            
            GetNextLex();
            RelationOperator();
            
            GetNextLex();
            LiteralOrIdentifier();
            
            GetNextLex();
            ClosingParenthesis();
        }

        /// <summary>
        /// Current lexem is relation operator (<> , > , = etc) 
        /// if not - throw Exception
        /// </summary>
        private void RelationOperator()
        {
            if (!_relationOperatorsList.Contains(_currentLex.value))
            {
                throw new Exception($"unknown relation operator {_currentLex.value}");
            }
        }

        /// <summary>
        /// Current lexem is '(', if not - throw Exception
        /// </summary>
        private void ClosingParenthesis()
        {
            if ( !(_currentLex.value == ")"))
            {
                throw new Exception(" ) expected");
            }
        }

        /// <summary>
        /// Current lexem is litteral or identifier
        /// </summary>
        private void LiteralOrIdentifier()
        {
            if (!(_currentLex.type == LexType.IDN || _currentLex.type == LexType.LIT))
            {
                throw new Exception("memeber of logical relation should be identificator or litteral");
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
