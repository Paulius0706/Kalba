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

            if (this.type == TokenType.If || this.type == TokenType.For || this.type == TokenType.While)
            {
                output += "\n" + tab + "    input:\n";
                foreach (Token token in inputConnections)
                {
                    output += token.ToString(pad + 2) + "\n";
                }

                output += "\n" + tab + "    connections:\n";
                foreach (Token token in connections)
                {

                    output += token.ToString(pad + 2) + "\n";
                }
            }
            else
            {
                foreach (Token token in connections)
                {
                    output += token;
                }
            }

            if (this.type == TokenType.Add || this.type == TokenType.Mul || this.type == TokenType.Sub || this.type == TokenType.Div)
            {
                bool tmp = false;
                for (int i = 0; i < this.connections.Count; i++)
                {
                    if (i == this.connections.Count)
                    {
                        output += this.connections[i];
                    }
                }
            }


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
                    else if (line[i] == '=') { tokens.Add(new Token(y, i, TokenType.Ass, null)); i++; }
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

        public static List<Token> Compress1(List<Token> tokens)
        {

            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].type == TokenType.If || tokens[i].type == TokenType.Elsif || tokens[i].type == TokenType.While)
                {
                    List<Token> conn = new List<Token>();
                    int lbracketsCount = 0;
                    int rbracketsCount = 0;
                    int nextindex = i + 1;
                    if (tokens[nextindex].type == TokenType.BracketL)
                    {
                        lbracketsCount++;
                        tokens.RemoveAt(nextindex);
                        while (rbracketsCount != lbracketsCount)
                        {
                            if (tokens[nextindex].type != TokenType.BracketR) rbracketsCount++;
                            if (tokens[nextindex].type != TokenType.BracketL) lbracketsCount++;
                            if (rbracketsCount != lbracketsCount) tokens[i].inputConnections.Add(tokens[nextindex]);
                            tokens.RemoveAt(nextindex);
                        }
                    }
                }
                if(tokens[i].type == TokenType.For)
                {

                }
                if (tokens[i].type == TokenType.PrintLine || tokens[i].type == TokenType.Print)
                {
                    
                }
                if (tokens[i].type == TokenType.Unknown)
                {

                }
                if (tokens[i].type == TokenType.Ass)
                {

                }
                if (tokens[i].type == TokenType.Add || tokens[i].type == TokenType.Sub || tokens[i].type == TokenType.Mul || tokens[i].type == TokenType.Div)
                {
                    
                }
                if (tokens[i].type == TokenType.Abs || tokens[i].type == TokenType.Sqrt)
                {
                    
                }
            }
            return tokens;
        }

        public static List<Token> Compress(List<Token> tokens)
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].type == TokenType.If || tokens[i].type == TokenType.Elsif || tokens[i].type == TokenType.For || tokens[i].type == TokenType.While)
                {
                    tokens[i].CompressFunction(i, tokens);
                }


                if (tokens[i].type == TokenType.PrintLine || tokens[i].type == TokenType.Print)
                {
                    tokens[i].CompressPrint(i, tokens);
                }

                if (tokens[i].type == TokenType.Unknown)
                {
                    tokens[i].type = TokenType.Value;
                }

                if (tokens[i].type == TokenType.Add || tokens[i].type == TokenType.Sub || tokens[i].type == TokenType.Mul || tokens[i].type == TokenType.Div)
                {
                    tokens[i].CompressAritmetic(tokens, i, 0);
                }

                if (tokens[i].type == TokenType.Abs || tokens[i].type == TokenType.Sqrt)
                {
                    tokens[i].CompressMethods(i, tokens);
                }
            }
            return tokens;
        }

        public void CompressFunction(int i, List<Token> tokens)
        {
            if (tokens[i + 1].type == TokenType.BracketL)
            {
                if (tokens[i + 2].type == TokenType.Abs || tokens[i + 2].type == TokenType.Sqrt)
                {
                    tokens[i].CompressMethods(i + 2, tokens);
                }
                tokens.RemoveAt(i + 1);
                int tIndex = i + 1;
                while (tokens[tIndex].type != TokenType.BracketR)
                {
                    tokens[i].inputConnections.Add(tokens[tIndex]);
                    tokens.RemoveAt(tIndex);
                }

                tokens.RemoveAt(tIndex);
                int index = tIndex;
                tokens[i].CompressIf(tIndex, tokens);

                for (int j = 0; j < tokens[i].connections.Count; j++)
                {
                    if (tokens[i].connections[j].type == TokenType.PrintLine || tokens[i].connections[j].type == TokenType.Print)
                    {
                        tokens[i].CompressPrint(j, tokens[i].connections);
                    }
                    if (tokens[i].connections[j].type == TokenType.Unknown)
                    {
                        tokens[i].connections[j].type = TokenType.Value;
                    }

                    if (tokens[i].connections[j].type == TokenType.Add || tokens[i].connections[j].type == TokenType.Sub || tokens[i].connections[j].type == TokenType.Mul || tokens[i].connections[j].type == TokenType.Div)
                    {
                        tokens[i].CompressAritmetic(tokens[i].connections, j, 0);
                    }
                }
            }
        }
        public void CompressIf(int index, List<Token> tokens)
        {
            if (tokens[index].type != TokenType.SemiComma)
            {
                connections.Add(tokens[index]);
                tokens.RemoveAt(index);
                CompressIf(index, tokens);
            }
            if (tokens[index].type == TokenType.Unknown)
            {
                tokens[index].type = TokenType.Value;
            }
            if (tokens[index].type == TokenType.SemiComma)
            {
                connections.Add(tokens[index]);
                tokens.RemoveAt(index);
                return;
            }
        }

        public void CompressPrint(int i, List<Token> tokens)
        {
            int tmp = i + 1;
            if (tokens[tmp].type == TokenType.BracketL)
            {
                tokens.RemoveAt(tmp);
                while (tokens[tmp].type != TokenType.BracketR)
                {
                    tokens[i].connections.Add(tokens[tmp]);
                    tokens.RemoveAt(tmp);
                }
                tokens.RemoveAt(tmp);
            }
        }
        public void CompressAritmetic(List<Token> tokens, int index, int count)
        {
            Token tmp = tokens[index - 1];
            tokens[index - 1] = tokens[index];
            tokens[index] = tmp;

            int newIndex = index - 1;
            while (count < 2)
            {
                if (tokens[index].type == TokenType.Unknown)
                {
                    tokens[index].type = TokenType.Value;
                }
                tokens[newIndex].connections.Add(tokens[index]);
                tokens.RemoveAt(index);
                count++;
            }
        }
        /*public void CompressAritmetic(List<Token> tokens)
        {
            for(int index = 0; index < tokens.Count-1; index++)
            {
                if (tokens[index].type == TokenType.Unknown)
                {
                    tokens[index].type = TokenType.Value;
                }
                if (tokens[index].type == TokenType.Add || tokens[index].type == TokenType.Sub || tokens[index].type == TokenType.Mul || tokens[index].type == TokenType.Div)
                {
                    int tmpindex = index - 1;
                    Token tmp = tokens[tmpindex];
                    tokens[tmpindex] = tokens[index];
                    tokens[index] = tmp;
                    if (tokens[index].type == TokenType.Unknown)
                    {
                        tokens[index].type = TokenType.Value;
                    }
                    tokens[index].connections.Add(tokens[index + 1]);
                    tokens.RemoveAt(index);
                    tokens[index - 1].connections.Add(tokens[index]);
                    tokens.RemoveAt(index);
                    //tokens[index].connections.Add(tokens[index + 1]);
                    //tokens.RemoveAt(index);
                }
            }
        }*/

        public void CompressMethods(int index, List<Token> tokens)
        {
            int lbracketsCount = 0;
            int rbracketsCount = 0;
            if (tokens[index + 1].type == TokenType.BracketL)
            {
                lbracketsCount++;
                int nextindex = index + 1;
                tokens.RemoveAt(index + 1);

                while (rbracketsCount != lbracketsCount)
                {
                    if (tokens[nextindex].type != TokenType.BracketR) rbracketsCount++;
                    if (tokens[nextindex].type != TokenType.BracketL) lbracketsCount++;
                    if (rbracketsCount != lbracketsCount) tokens[index].inputConnections.Add(tokens[nextindex]);
                    tokens.RemoveAt(nextindex);
                }
            }

        }
    }
}
