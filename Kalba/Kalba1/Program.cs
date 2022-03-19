using System;
using System.Collections.Generic;
using System.IO;
namespace Kalba1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] scriptText = File.ReadAllLines(@"C:\Users\andri\OneDrive - Kaunas University of Technology\Dokumentai\GitHub\Kalba\Kalba\Kalba1\script.txt");
            List<Token> tokens = Token.TXTtoTokens(scriptText);
            tokens = Token.Compress(tokens);
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
