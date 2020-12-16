using System;
using System.Collections.Generic;
using System.Text;

namespace PascalLexicalAnalizer
{
    public class LexAnalyzerResult
    {
        public static void Print(LexicalAnalyzer analizer)
        {
            PrintAllLexems(analizer.Lexemes);
            Console.WriteLine(analizer.Message);
            PrintTable("IDENTIFIERS TABLE", analizer.TableIdentifiers);
            PrintTable("LITERALS TABLE", analizer.TableLiterals);
        }

        private static void PrintTable(string name, List<string> table)
        {
            Console.WriteLine(name);
            foreach(var lex in table)
            {
                Console.WriteLine( $"[{lex}]");
            }
        }

        private static void PrintAllLexems(List<Lex> lexemes)
        {
            foreach (Lex lex in lexemes)
            {
                Console.WriteLine($"{lex.type} id:{lex.id} val:[{lex.value}]");
            }
        }
    }
}
