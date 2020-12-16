using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PascalLexicalAnalizer
{
    public class LexicalAnalyzer
    {
        private string _buf = ""; // buffer for lexeme caching
        private char _currentChar;
        private StringReader _stringReader;

        private List<string> _words; 
        private List<string> _delimiters;
        private enum States { START, NUMBER, STRING, DELIMITER, FINISH, ID, ERROR, ASGN , NOTEQ } // states of state machine
        private States _state; // current state 
        public List<string> TableIdentifiers { get; private set; }
        public List<string> TableLiterals { get; private set; }
        public List<string> TableKeywords { get; private set; }
        public List<Lex> Lexemes { get; private set; }
        public string Message { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public LexicalAnalyzer(List<string> words , List<string> delimiters)
        {
            _words = words;
            _delimiters = delimiters;
            TableKeywords = new List<string>();
            TableKeywords.AddRange(_words);
            TableKeywords.AddRange(_delimiters);
            Lexemes = new List<Lex>();
            TableIdentifiers = new List<string>();
            TableLiterals = new List<string>();
        }

        private void Log(string message) => Message += message;
        private bool IsKeyword(string buffer) => TableKeywords.Contains(buffer);
        private void ClearBuf() => _buf = "";
        private void AddBuf(char symb) => _buf += symb;
        private void GetNextChar() 
        {
            int next = _stringReader.Read();
            if (next != -1)
            {
                _currentChar = (char)next;
            }
            else
            {
                _state = States.FINISH;
                Log("Program finish");
            }
        }

        /// <summary>
        /// Get id for new lexeme in table specified by its type.
        /// </summary>
        private int GetId(string buf, LexType type)
        {
            if (type == LexType.KEY)
            {
                return TableKeywords.IndexOf(buf);
            }
            else
            {
                var table = type == LexType.IDN ? TableIdentifiers : TableLiterals;
                if (table.IndexOf(buf) == -1)
                {
                    table.Add(buf);
                }
                return table.IndexOf(buf);
            }
        }

        /// <summary>
        /// Add new lexem to Lexemes list.
        /// </summary>
        private void AddLexem(string buf, LexType type)
        {
            var id = GetId(buf, type);
            var lex = new Lex(id, type, buf);
            Lexemes.Add(lex);
        } 

        public void Analyze(string text)
        {
            Log("Initializing..");
            _stringReader = new StringReader(text);
            while (_state != States.FINISH)
            {
                switch (_state)
                {
                    case States.START:
                        StartVertex();
                        break;
                    case States.STRING:
                        StringVertex();
                        break;
                    case States.ID:
                        IdVertex();
                        break;
                    case States.NUMBER:
                        NumberVertex();
                        break;
                    case States.DELIMITER:
                        DelimiterVertex();
                        break;
                    case States.ASGN:
                        AsignVertex();
                        break;
                    case States.NOTEQ:
                        NotEqVertex();
                        break;
                    case States.ERROR:
                        ErrorVertex();
                        break;
                }
            }
        }

        private void ErrorVertex()
        {
            Log("Program error....");
            _state = States.FINISH;
        }

        private void NotEqVertex()
        {
            if (_currentChar == '>')
            {
                AddBuf(_currentChar);
                AddLexem(_buf, LexType.KEY);
                ClearBuf();
                GetNextChar();
            }
            else
            {
                AddLexem(_buf, LexType.KEY);
            }
            _state = States.START;
        }

        private void AsignVertex()
        {
            if (_currentChar == '=')
            {
                AddBuf(_currentChar);
                AddLexem(_buf, LexType.KEY);
                ClearBuf();
                GetNextChar();
            }
            else
            {
                AddLexem(_buf, LexType.KEY);
            }
            _state = States.START;
        }

        private void DelimiterVertex()
        {
            ClearBuf();
            AddBuf(_currentChar);

            if (GetId(_buf, LexType.KEY) != -1)
            {
                AddLexem(_buf, LexType.KEY);
                _state = States.START;
                GetNextChar();
            }
            else
            {
                _state = States.ERROR;
            }
        }

        private void NumberVertex()
        {
            if (char.IsDigit(_currentChar))
            {
                AddBuf(_currentChar);
                GetNextChar();
            }
            else
            {
                AddLexem(_buf, LexType.LIT);
                _state = States.START;
            }
        }

        private void IdVertex()
        {
            if (char.IsLetterOrDigit(_currentChar))
            {
                AddBuf(_currentChar);
                GetNextChar();
            }
            else
            {
                var type = IsKeyword(_buf) ? LexType.KEY : LexType.IDN;
                AddLexem(_buf, type);
                _state = States.START;
            }
        }

        private void StringVertex()
        {
            if (_currentChar == '\'')
            {
                if (_buf.Length == 0)
                {
                    _state = States.ERROR;
                }
                else
                {
                    AddLexem(_buf, LexType.LIT);
                    _state = States.START;
                    GetNextChar();
                }
            }
            else
            {
                AddBuf(_currentChar);
                GetNextChar();
            }
        }

        private void StartVertex()
        {
            ClearBuf();
            if (_currentChar == ' ' || _currentChar == '\n' || _currentChar == '\t' || _currentChar == '\0' || _currentChar == '\r')
                GetNextChar();
            else if (char.IsLetter(_currentChar))
            {
                AddBuf(_currentChar);
                _state = States.ID;
                GetNextChar();
            }
            else if (char.IsDigit(_currentChar))
            {
                AddBuf(_currentChar);
                GetNextChar();
                _state = States.NUMBER;

            }
            else if (_currentChar == ':')
            {
                _state = States.ASGN;
                AddBuf(_currentChar);
                GetNextChar();
            }
            else if (_currentChar == '<')
            {
                _state = States.NOTEQ;
                AddBuf(_currentChar);
                GetNextChar();
            }
            else if (_currentChar == '\'')
            {
                _state = States.STRING;
                GetNextChar();
            }
            else
            {
                _state = States.DELIMITER;
            }
        }
    }
}
