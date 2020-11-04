using System;
using System.Collections.Generic;
using System.Text;

namespace PascalLexicalAnalizer
{
    public class Lex
    {
        public readonly int id;
        public readonly LexType type;
        public readonly string value;

        public Lex(int id, LexType type, string value)
        {
            this.id = id;
            this.type = type;
            this.value = value;
        }
    }
}
