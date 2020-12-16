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
            string text = File.ReadAllText("../../../code.txt");

            LexicalAnalyzer analizer = new LexicalAnalyzer(words,delimiters);
            analizer.Analyze(text);
            ResultPrinter.Print(analizer);

            var syntaxAnalyzer = new SyntaxAnalyzer(analizer.Lexemes);
            syntaxAnalyzer.Analyze();

            Console.ReadKey();
        }
    }
}
