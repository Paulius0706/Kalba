using System;
using System.Collections.Generic;
using System.Text;

namespace Kalba1
{
    class Token
    {
        public static string letters = "";
        public static string numbers = "0123456789.";


        public int row { get; set; }
        public int column { get; set; }
        public TokenType type;
        public TokenType assignType;
        public object value { get; set; }
        public bool none { get; set; }

        public List<Token> connections;
        public List<Token> bonusConnetions;


        public Token(int row, int column, TokenType type, object value)
        {
            connections = new List<Token>();
            this.row = row;
            this.column = column;
            this.type = type;
            this.value = value;
        }
        public Token() { }

        public override string ToString()
        {
            string output = "[" + type + ":" + value + " :";
            foreach (Token token in connections) {
                output += token;
            }
            output += "]";
            return output;
        }

        public static List<Token> TXTtoTokens(string[] lines)
        {
            List<Token> tokens = new List<Token>();
            for (int y = 0; y < lines.Length; y++) {
                string line = lines[y];
                int i = 0;
                while (i < line.Length)
                {
                    string newToken = "";

                    if (line[i] == '"')
                    {
                        int ox = i;
                        i++;
                        while (i < line.Length && line[i] != '"'){newToken += line[i++];}
                        i++;
                        tokens.Add(new Token(y, ox, TokenType.String, newToken));
                    }
                    else if (Char.IsDigit(line[i]))
                    {
                        int ox = i;
                        while (i < line.Length && (Char.IsDigit(line[i]) || line[i] == '.')){ newToken += line[i++]; }
                        tokens.Add(new Token(y, ox, TokenType.Number, newToken));
                    }
                    else if (Char.IsLetter(line[i]))
                    {
                        int ox = i;
                        while (i < line.Length && Char.IsLetterOrDigit(line[i])){ newToken += line[i++]; }
                        if (newToken == "bool") { tokens.Add(new Token(y, ox, TokenType.Bool, null)); }
                        else if (newToken == "int") { tokens.Add(new Token(y, ox, TokenType.Int, null)); }
                        else if (newToken == "double") { tokens.Add(new Token(y, ox, TokenType.Double, null)); }
                        else if (newToken == "string") { tokens.Add(new Token(y, ox, TokenType.String, null)); }
                        else { tokens.Add(new Token(y, ox, TokenType.Unknow, newToken)); }
                    }
                    else if (line[i] == '+') { tokens.Add(new Token(y, i, TokenType.Add, null)); i++; }
                    else if (line[i] == '-') { tokens.Add(new Token(y, i, TokenType.Sub, null)); i++; }
                    else if (line[i] == '*') { tokens.Add(new Token(y, i, TokenType.Mul, null)); i++; }
                    else if (line[i] == '/')
                    {
                        if (line[i + 1] == '/') { tokens.Add(new Token(y, i, TokenType.Comment, null)); i += 2; }
                        else { tokens.Add(new Token(y, i, TokenType.Div, null)); i++; }
                    }
                    else if (line[i] == '.') { tokens.Add(new Token(y, i, TokenType.Dot, null)); i++;}
                    else if (line[i] == ',') { tokens.Add(new Token(y, i, TokenType.Comma, null)); i++;}
                    else if (line[i] == ';') { tokens.Add(new Token(y, i, TokenType.SemiComma, null)); i++;}
                    else if (line[i] == '(') { tokens.Add(new Token(y, i, TokenType.BracketL, null)); i++;}
                    else if (line[i] == ')') { tokens.Add(new Token(y, i, TokenType.BracketR, null)); i++;}
                    else if (line[i] == '[') { tokens.Add(new Token(y, i, TokenType.BoxL, null)); i++;}
                    else if (line[i] == ']') { tokens.Add(new Token(y, i, TokenType.BoxR, null)); i++;}
                    else if (line[i] == '{') { tokens.Add(new Token(y, i, TokenType.CurlyL, null)); i++;}
                    else if (line[i] == '}') { tokens.Add(new Token(y, i, TokenType.CurlyR, null)); i++;}
                    else if (line[i] == '=') { tokens.Add(new Token(y, i, TokenType.Ass, null)); i++;}
                    else if (line[i] == '!')
                    {
                        if (line[i + 1] == '=') { tokens.Add(new Token(y, i, TokenType.NotEqual, null)); i += 2;}
                    }
                    else if (line[i] == '<')
                    {
                        if (line[i + 1] == '=') { tokens.Add(new Token(y, i, TokenType.LessOrEqual, null)); i += 2;}
                        else { tokens.Add(new Token(y, i, TokenType.Less, null)); i++;}
                    }
                    else if (line[i] == '>')
                    {
                        if (line[i + 1] == '=') { tokens.Add(new Token(y, i, TokenType.MoreOrEqual, null)); i += 2;}
                        else { tokens.Add(new Token(y, i, TokenType.Less, null)); i++; }
                    }
                    else { i++; }

                }
            }

            return tokens;
        }

        public static List<Token> Compress(List<Token> tokens) {
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].type == TokenType.Unknow)
                {
                    if (tokens[i + 1].type == TokenType.BracketL)
                    {
                        tokens[i].type = TokenType.Method;
                        tokens.RemoveAt(i + 1);
                        int tIndex = i + 1;
                        while (tokens[tIndex].type != TokenType.BracketR)
                        {
                            tokens[i].connections.Add(tokens[tIndex]);
                            tokens.RemoveAt(tIndex);
                        }
                        tokens.RemoveAt(tIndex);
                        //tokens[i].connections = Token.Compress(tokens[i].connections);
                    }
                    else
                    {
                        tokens[i].type = TokenType.Value;
                    }
                }
                /*else if (tokens[i].type == TokenType.Add || tokens[i].type == TokenType.Mul || tokens[i].type == TokenType.Sub)
                {
                    int tmpIndex = i - 1;
                    Token tmp = tokens[tmpIndex];
                    tokens[tmpIndex] = tokens[i];
                    tokens[i] = tmp;
                }*/
            }
            return tokens;
        }
    }
}
