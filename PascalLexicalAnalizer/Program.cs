using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PascalLexicalAnalizer
{
    class Program
    {
        static void Main(string[] args)
        {
            //var words = ListReader.ReadList("words.txt");
            //var delimiters = ListReader.ReadList("delimiters.txt");
            //string text = File.ReadAllText("code.txt");

            var words = ListReader.ReadList("../../../words.txt");
            var delimiters = ListReader.ReadList("../../../delimiters.txt");
            string text = File.ReadAllText("../../../Ifcode.txt");

            var lexicalAnalizer = new LexicalAnalyzer(words,delimiters);
            lexicalAnalizer.Analyze(text);
            //LexAnalyzerResult.Print(lexicalAnalizer);

            var syntaxAnalyzer = new SyntaxAnalyzer(lexicalAnalizer.Lexemes);
            syntaxAnalyzer.Analyze();
            SyntAnalyzerResult.Print(syntaxAnalyzer);

            Console.ReadKey();
        }
    }
}
