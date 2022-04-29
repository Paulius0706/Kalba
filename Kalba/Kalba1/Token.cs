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

        public List<Token> connections;

        public List<Token> inputConnections;
        public List<Token> outputConnections;

        public Token(int row, int column, TokenType type, object value)
        {
            connections = new List<Token>();
            this.row = row;
            this.column = column;
            this.type = type;
            this.value = value;
            inputConnections = new List<Token>();
            outputConnections = new List<Token>();
        }
        public Token() { }

        public override string ToString()
        {
            return ToString(0);
        }


        public string ToString(int pad)
        {
            string tab = "";
            for (int i = 0; i < pad; i++) tab += "    ";
            string output = tab + "[" + type + ":" + value + ":";
            if(this.type == TokenType.If 
                || this.type == TokenType.For 
                || this.type == TokenType.While
                || this.type == TokenType.Ass)
            {
                output += "\n" + tab + "    input:\n";
                foreach (Token token in inputConnections) output += token.ToString(pad + 2) + "\n";   
            }
            if (this.type == TokenType.Ass)
            {
                output += "\n" + tab + "    output:\n";
                foreach (Token token in outputConnections) output += token.ToString(pad + 2) + "\n";
            }
            if (this.type == TokenType.If 
                || this.type == TokenType.For 
                || this.type == TokenType.While
                || this.type == TokenType.Abs
                || this.type == TokenType.Print
                || this.type == TokenType.PrintLine
                || this.type == TokenType.Sqrt
                || this.type == TokenType.Add
                || this.type == TokenType.Sub
                || this.type == TokenType.Mul
                || this.type == TokenType.Div
                || this.type == TokenType.Less
                || this.type == TokenType.More
                || this.type == TokenType.LessOrEqual
                || this.type == TokenType.MoreOrEqual
                || this.type == TokenType.Equal
                || this.type == TokenType.NotEqual
                || this.type == TokenType.And
                || this.type == TokenType.Or)
            {
                output += "\n" + tab + "    connections:\n";
                foreach (Token token in connections) output += token.ToString(pad + 2) + "\n";
            }

            if (this.type == TokenType.Add || this.type == TokenType.Mul || this.type == TokenType.Sub || this.type == TokenType.Div)
            {
                for (int i = 0; i < this.connections.Count; i++)
                {
                    if (i == this.connections.Count)
                    {
                        output += this.connections[i];
                    }
                }
            }

            if (type == TokenType.String
                || type == TokenType.Bool
                || type == TokenType.Int
                || type == TokenType.Double) tab = "";
            output += tab + "]";
            return output;
        }

        public static List<Token> TXTtoTokens(string[] lines)
        {
            List<Token> tokens = new List<Token>();
            for (int y = 0; y < lines.Length; y++)
            {
                string line = lines[y];
                int i = 0;
                while (i < line.Length)
                {
                    string newToken = "";

                    if (line[i] == '"')
                    {
                        int ox = i;
                        i++;
                        while (i < line.Length && line[i] != '"') { newToken += line[i++]; }
                        i++;
                        tokens.Add(new Token(y, ox, TokenType.String, newToken));
                    }
                    else if (Char.IsDigit(line[i]))
                    {
                        int ox = i;
                        while (i < line.Length && (Char.IsDigit(line[i]) || line[i] == '.')) { newToken += line[i++]; }
                        tokens.Add(new Token(y, ox, TokenType.Number, newToken));
                    }
                    else if (Char.IsLetter(line[i]))
                    {
                        int ox = i;
                        //newToken += line[i];
                        while (i < line.Length && Char.IsLetterOrDigit(line[i])) { newToken += line[i++]; }
                        if (newToken == "bool") { tokens.Add(new Token(y, ox, TokenType.BoolDeclare, null)); }
                        else if (newToken == "int") { tokens.Add(new Token(y, ox, TokenType.IntDeclare, null)); }
                        else if (newToken == "double") { tokens.Add(new Token(y, ox, TokenType.DoubleDeclare, null)); }
                        else if (newToken == "string") { tokens.Add(new Token(y, ox, TokenType.StringDeclare, null)); }
                        else if (newToken == "for") { tokens.Add(new Token(y, ox, TokenType.For, null)); }
                        else if (newToken == "while") { tokens.Add(new Token(y, ox, TokenType.While, null)); }
                        else if (newToken == "if") { tokens.Add(new Token(y, ox, TokenType.If, null)); }
                        else if (newToken == "else") { tokens.Add(new Token(y, ox, TokenType.Else, null)); }
                        else if (newToken == "elsif") { tokens.Add(new Token(y, ox, TokenType.Elsif, null)); }
                        else if (newToken == "abs") { tokens.Add(new Token(y, ox, TokenType.Abs, null)); }
                        else if (newToken == "sqrt") { tokens.Add(new Token(y, ox, TokenType.Sqrt, null)); }
                        else if (newToken == "Print") { tokens.Add(new Token(y, ox, TokenType.Print, null)); }
                        else if (newToken == "PrintLine") { tokens.Add(new Token(y, ox, TokenType.PrintLine, null)); }
                        else { tokens.Add(new Token(y, ox, TokenType.Unknown, newToken)); }
                    }
                    else if (line[i] == '+') { tokens.Add(new Token(y, i, TokenType.Add, null)); i++; }
                    else if (line[i] == '-') { tokens.Add(new Token(y, i, TokenType.Sub, null)); i++; }
                    else if (line[i] == '*') { tokens.Add(new Token(y, i, TokenType.Mul, null)); i++; }
                    else if (line[i] == '/')
                    {
                        if (line[i + 1] == '/') { tokens.Add(new Token(y, i, TokenType.Comment, null)); i += 2; }
                        else { tokens.Add(new Token(y, i, TokenType.Div, null)); i++; }
                    }
                    else if (line[i] == '.') { tokens.Add(new Token(y, i, TokenType.Dot, null)); i++; }
                    else if (line[i] == ',') { tokens.Add(new Token(y, i, TokenType.Comma, null)); i++; }
                    else if (line[i] == ';') { tokens.Add(new Token(y, i, TokenType.SemiComma, null)); i++; }
                    else if (line[i] == '(') { tokens.Add(new Token(y, i, TokenType.BracketL, null)); i++; }
                    else if (line[i] == ')') { tokens.Add(new Token(y, i, TokenType.BracketR, null)); i++; }
                    else if (line[i] == '[') { tokens.Add(new Token(y, i, TokenType.BoxL, null)); i++; }
                    else if (line[i] == ']') { tokens.Add(new Token(y, i, TokenType.BoxR, null)); i++; }
                    else if (line[i] == '{') { tokens.Add(new Token(y, i, TokenType.CurlyL, null)); i++; }
                    else if (line[i] == '}') { tokens.Add(new Token(y, i, TokenType.CurlyR, null)); i++; }
                    else if (line[i] == '&')
                    {
                        if (line[i + 1] == '&') { tokens.Add(new Token(y, i, TokenType.And, null)); i += 2; }
                    }
                    else if (line[i] == '|')
                    {
                        if (line[i + 1] == '|') { tokens.Add(new Token(y, i, TokenType.Or, null)); i += 2; }
                    }
                    else if (line[i] == '=')
                    {
                        if (line[i + 1] == '=') { tokens.Add(new Token(y, i, TokenType.Equal, null)); i += 2; }
                        else { tokens.Add(new Token(y, i, TokenType.Ass, null)); i++; }
                    }
                    else if (line[i] == '!')
                    {
                        if (line[i + 1] == '=') { tokens.Add(new Token(y, i, TokenType.NotEqual, null)); i += 2; }
                    }
                    else if (line[i] == '<')
                    {
                        if (line[i + 1] == '=') { tokens.Add(new Token(y, i, TokenType.LessOrEqual, null)); i += 2; }
                        else { tokens.Add(new Token(y, i, TokenType.Less, null)); i++; }
                    }
                    else if (line[i] == '>')
                    {
                        if (line[i + 1] == '=') { tokens.Add(new Token(y, i, TokenType.MoreOrEqual, null)); i += 2; }
                        else { tokens.Add(new Token(y, i, TokenType.Less, null)); i++; }
                    }
                    else { i++; }

                }
            }
            return tokens;
        }
        
    }
}
