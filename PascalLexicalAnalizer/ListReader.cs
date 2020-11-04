using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PascalLexicalAnalizer
{
    public class ListReader
    {
        public static List<string>ReadList(string filename)
        {
            var list = new List<string>();
            list.AddRange(File.ReadAllLines(filename));
            return list;
        }
    }
}
