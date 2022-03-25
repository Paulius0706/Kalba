using System;
using System.Collections.Generic;
using System.Text;

namespace Kalba1
{
    class Intepretator
    {

        List<Token> tokens;
        Dictionary<string, Token> values;
        Dictionary<string, Token> methods;

        public Intepretator(List<Token> tokens) {
            this.tokens = tokens;
            values = new Dictionary<string, Token>();
            methods = new Dictionary<string, Token>();
        }

        public void Run() {
            for (int i = 0; i < tokens.Count; i++) {
                object obj = executeCommand(tokens[i]);
            }
        }

        public Token executeCommand(Token token) {

            if (token.type == TokenType.Double || token.type == TokenType.Int || token.type == TokenType.Bool || token.type == TokenType.String) {
                return token;
            }
            if (token.type == TokenType.Add) {
                Token a = executeCommand(token.connections[0]);
                Token b = executeCommand(token.connections[1]);
                if (a.type == TokenType.String || b.type == TokenType.String)
                {
                    string a1 = Convert.ToString(a);
                    string b1 = Convert.ToString(b);
                    return new Token(token.row, token.column, TokenType.String, a1 + b1);
                }
                if ((a.type == TokenType.Double && a.type == TokenType.Int) || (b.type == TokenType.Double && b.type == TokenType.Int))
                {
                    double a1 = Convert.ToDouble(a);
                    double b1 = Convert.ToDouble(b);
                    double c = a1 + b1;
                    if (c % 1 == 0) return new Token(token.row, token.column, TokenType.Int, c);
                    return new Token(token.row, token.column, TokenType.Double, c);
                }
                else {

                }
            }
            if(token.type == TokenType.Sub)
            {
                Token a = executeCommand(token.connections[0]);
                Token b = executeCommand(token.connections[1]);
                if (a.type == TokenType.Double || b.type == TokenType.Double)
                {
                    double a1 = Convert.ToDouble(a);
                    double b1 = Convert.ToDouble(b);
                    return new Token(token.row, token.column, TokenType.Double, a1 - b1);
                }
                if (a.type == TokenType.Int && b.type == TokenType.Int)
                {
                    int a1 = Convert.ToInt32(a);
                    int b1 = Convert.ToInt32(b);
                    return new Token(token.row, token.column, TokenType.Int, a1 - b1);
                }
            }
            if (token.type == TokenType.Mul)
            {
                Token a = executeCommand(token.connections[0]);
                Token b = executeCommand(token.connections[1]);
                if ((a.type == TokenType.Double && a.type == TokenType.Int) || (b.type == TokenType.Double && b.type == TokenType.Int))
                {
                    double a1 = Convert.ToDouble(a);
                    double b1 = Convert.ToDouble(b);
                    double c = a1 / b1;
                    if (c % 1 == 0) return new Token(token.row, token.column, TokenType.Int, c);
                    return new Token(token.row, token.column, TokenType.Double, c);
                }
            }
            if (token.type == TokenType.Div)
            {
                Token a = executeCommand(token.connections[0]);
                Token b = executeCommand(token.connections[1]);
                if ((a.type == TokenType.Double && a.type == TokenType.Int) || (b.type == TokenType.Double && b.type == TokenType.Int))
                {
                    double a1 = Convert.ToDouble(a);
                    double b1 = Convert.ToDouble(b);
                    double c = a1 / b1;
                    if (c % 1 == 0) return new Token(token.row, token.column, TokenType.Int, c);
                    return new Token(token.row, token.column, TokenType.Double, c);
                }
            }
            if (token.type == TokenType.Ass)
            {
                Token b = executeCommand(token.connections[1]);
                if (token.connections[0].type == TokenType.Value)
                {
                    string a1 = Convert.ToString(token.connections[0].value);
                    values.Add(a1, new Token(b.row,b.column,token.assignType,b.value));
                    return new Token();
                }
            }
            if (token.type == TokenType.Value)
            {
                string a1 = Convert.ToString(token.value);
                return new Token(token.row, token.column, values[a1].type, values[a1].value);
            }
            if(token.type == TokenType.Method)
            {
                string a1 = Convert.ToString(token.value);
                Intepretator intepretator = new Intepretator(methods[a1].connections);
                foreach(Token t in token.connections){
                    Token value = new Token();
                }

            }



            return new Token();
        }

    }
}
