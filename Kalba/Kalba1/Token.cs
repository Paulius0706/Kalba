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

        List<Token> connections;

        public Token(int row, int column, TokenType type, object value)
        {
            connections = new List<Token>();
            this.row = row;
            this.column = column;
            this.type = type;
            this.value = value;
        }

        public override string ToString()
        {
            return "[" + type + ":" + value + "]";
        }

        public static List<Token> TXTtoTokens(string lines)
        {
            List<Token> tokens = new List<Token>();
            
            int i = 0;
            int x = 0;
            int y = 0;
            while (i < lines.Length)
            {
                string newToken = "";

                
                if(lines[i] == '\n')
                {
                    y++;
                    x = 0;
                }

                if (lines[i] == '"') {
                    int ox = x;
                    i++;
                    x++;
                    while (i < lines.Length && lines[i] !='"')
                    {
                        newToken += lines[i++];
                        x++;
                    }
                    i++;
                    x++;
                    tokens.Add(new Token(y, ox, TokenType.String, newToken));
                }
                else if (Char.IsDigit(lines[i]))
                {
                    int ox = x;
                    while (i < lines.Length && (Char.IsDigit(lines[i]) || lines[i] == '.'))
                    {
                        newToken += lines[i++];
                        x++;
                    }
                    tokens.Add(new Token(y, ox, TokenType.Number, newToken));
                }
                else if (Char.IsLetter(lines[i]))
                {
                    int ox = x;
                    while (i < lines.Length && Char.IsLetterOrDigit(lines[i]))
                    {
                        newToken += lines[i++];
                        x++;
                    }
                    if (newToken == "bool") { tokens.Add(new Token(y, ox, TokenType.Bool, null)); }
                    else if (newToken == "int") { tokens.Add(new Token(y, ox, TokenType.Int, null)); }
                    else if (newToken == "double") { tokens.Add(new Token(y, ox, TokenType.Double, null)); }
                    else if (newToken == "string") { tokens.Add(new Token(y, ox, TokenType.String, null)); }
                    else { tokens.Add(new Token(x, y, TokenType.Unknow, newToken)); }
                }
                else if (lines[i] == '+') { tokens.Add(new Token(y, x, TokenType.Add       , null)); i++; x++; }
                else if (lines[i] == '-') { tokens.Add(new Token(y, x, TokenType.Sub       , null)); i++; x++; }
                else if (lines[i] == '*') { tokens.Add(new Token(y, x, TokenType.Mul       , null)); i++; x++; }
                else if (lines[i] == '/') {
                    if (lines[i + 1] == '/') { tokens.Add(new Token(y, x, TokenType.Comment, null)); i += 2; x += 2; }
                    else                     { tokens.Add(new Token(y, x, TokenType.Div    , null)); i++; x++; }
                }
                else if (lines[i] == '.') { tokens.Add(new Token(y, x, TokenType.Dot       , null)); i++; x++; }
                else if (lines[i] == ',') { tokens.Add(new Token(y, x, TokenType.Comma     , null)); i++; x++; }
                else if (lines[i] == ';') { tokens.Add(new Token(y, x, TokenType.SemiComma , null)); i++; x++; }
                else if (lines[i] == '(') { tokens.Add(new Token(y, x, TokenType.BracketL  , null)); i++; x++; }
                else if (lines[i] == ')') { tokens.Add(new Token(y, x, TokenType.BraketR   , null)); i++; x++; }
                else if (lines[i] == '[') { tokens.Add(new Token(y, x, TokenType.BoxL      , null)); i++; x++; }
                else if (lines[i] == ']') { tokens.Add(new Token(y, x, TokenType.BoxR      , null)); i++; x++; }
                else if (lines[i] == '{') { tokens.Add(new Token(y, x, TokenType.CurlyL    , null)); i++; x++; }
                else if (lines[i] == '}') { tokens.Add(new Token(y, x, TokenType.CurlyR    , null)); i++; x++; }
                else if (lines[i] == '=') { tokens.Add(new Token(y, x, TokenType.Ass       , null)); i++; x++; }
                else if (lines[i] == '!') {
                    if (lines[i + 1] == '=') { tokens.Add(new Token(y, x, TokenType.NotEqual   , null)); i += 2; x += 2; }
                }
                else if (lines[i] == '<'){
                    if (lines[i + 1] == '=') { tokens.Add(new Token(y, x, TokenType.LessOrEqual, null)); i += 2; x += 2; }
                    else                     { tokens.Add(new Token(y, x, TokenType.Less       , null)); i++; x++; }
                }
                else if (lines[i] == '>'){
                    if (lines[i + 1] == '=') { tokens.Add(new Token(y, x, TokenType.MoreOrEqual, null)); i += 2; x += 2; }
                    else                     { tokens.Add(new Token(y, x, TokenType.Less       , null)); i++; x++; }
                }
                else { i++; x++; }


            }

            return tokens;
        }

        public static List<Token> Compress(List<Token> tokens) {
            

            return tokens;
        }
    }
}
