using System;
using System.Collections.Generic;
using System.Text;

namespace Kalba1
{
    class Intepretator
    {

        List<Token> tokens;
        Dictionary<string, Token> values;
        Dictionary<string, List<Token>> methods;

        public Intepretator(List<Token> tokens) {
            this.tokens = tokens;
            values = new Dictionary<string, Token>();
            methods = new Dictionary<string, List<Token>>();
        }

        public void Run() {
            for (int i = 0; i < tokens.Count; i++) {
                object obj = executeCommand(tokens[i]);
            }
        }

        public Token executeCommand(Token token) {


            if (token.type == TokenType.Add) {
                Token a = executeCommand(token.connections[0]);
                Token b = executeCommand(token.connections[1]);
                if (a.type == TokenType.String || b.type == TokenType.String)
                {
                    string a1 = Convert.ToString(a);
                    string b1 = Convert.ToString(b);
                    return new Token(token.row, token.column, TokenType.String, a1 + b1);
                }
                if (a.type == TokenType.Double || b.type == TokenType.Double)
                {
                    double a1 = Convert.ToDouble(a);
                    double b1 = Convert.ToDouble(b);
                    return new Token(token.row, token.column, TokenType.Double, a1 + b1);
                }
                if (a.type == TokenType.Int || b.type == TokenType.Int)
                {
                    int a1 = Convert.ToInt32(a);
                    int b1 = Convert.ToInt32(b);
                    return new Token(token.row, token.column, TokenType.Int, a1 + b1);
                }
            }
            if(token.type == TokenType.Sub)
            {
                
            }
            //if(token.type == TokenType.)
            return new Token();
        }

    }
}
