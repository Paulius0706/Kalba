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
        public object value { get; set; }

        public List<Token> connections;

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
            return "[" + type + ":" + value + "]";
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
                        while (i < lines.Length && line[i] != '"'){newToken += lines[i++];}
                        i++;
                        tokens.Add(new Token(y, ox, TokenType.String, newToken));
                    }
                    else if (Char.IsDigit(line[i]))
                    {
                        int ox = i;
                        while (i < lines.Length && (Char.IsDigit(line[i]) || line[i] == '.')){ newToken += lines[i++]; }
                        tokens.Add(new Token(y, ox, TokenType.Number, newToken));
                    }
                    else if (Char.IsLetter(line[i]))
                    {
                        int ox = i;
                        while (i < lines.Length && Char.IsLetterOrDigit(line[i])){ newToken += lines[i++]; }
                        if (newToken == "bool") { tokens.Add(new Token(y, ox, TokenType.Bool, null)); }
                        else if (newToken == "int") { tokens.Add(new Token(y, ox, TokenType.Int, null)); }
                        else if (newToken == "double") { tokens.Add(new Token(y, ox, TokenType.Double, null)); }
                        else if (newToken == "string") { tokens.Add(new Token(y, ox, TokenType.String, null)); }
                        else { tokens.Add(new Token(i, y, TokenType.Unknow, newToken)); }
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
                    else if (line[i] == ')') { tokens.Add(new Token(y, i, TokenType.BraketR, null)); i++;}
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
            

            return tokens;
        }
    }
}
