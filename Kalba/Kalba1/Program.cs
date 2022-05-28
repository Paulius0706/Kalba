using System;
using System.Collections.Generic;
using System.IO;
namespace Kalba1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] scriptText = File.ReadAllLines("../../../script1.txt");
            List<Token> tokens = Token.TXTtoTokens(scriptText);
            Sintax sintax = new Sintax();
            tokens = sintax.Compress(tokens);
            printTokens(tokens);
            Intepretator intepretator = new Intepretator(tokens, false,false);
            Console.WriteLine();
            intepretator.Run();
        }
        public static void printTokens(List<Token> tokens)
        {
            int i = 0;
            int y = 0;
            while (i < tokens.Count)
            {
                if (tokens[i].row != y)
                {
                    Console.Write("\n");
                    y = tokens[i].row;

                }
                Console.Write(tokens[i]);

                i++;
            }
        }
    }
}
