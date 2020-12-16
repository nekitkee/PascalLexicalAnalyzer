using System;
using System.Collections.Generic;
using System.Text;

namespace PascalLexicalAnalizer
{
    public class SyntAnalyzerResult
    {
        public static void Print(SyntaxAnalyzer syntaxAnalyzer)
        {
            Console.WriteLine(syntaxAnalyzer.Result);
        }
    }
}
