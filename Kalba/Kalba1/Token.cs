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
<<<<<<< Updated upstream
        
=======
        public List<Token> inputConnections;
        public List<Token> outputConnections;
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
            string output = "[" + type + ":" + value + " :";
            foreach (Token token in connections)
            {
                output += token;
=======
            string output = "[" + type + ":" + value + ":";

            if(this.type == TokenType.If || this.type == TokenType.For)
            {
                output += "input:";
                foreach (Token token in inputConnections)
                {
                    output += token;
                }

                output += "connections";
                foreach(Token token in connections)
                {
                    output += token;
                }
            }

            if(this.type == TokenType.Add || this.type == TokenType.Mul || this.type == TokenType.Sub || this.type == TokenType.Div)
            {
                bool tmp = false;
                foreach(Token token in connections)
                {
                    if(!tmp)
                    {
                        output += token;
                    }
                    tmp = true;
                    output += "," + token;
                    
                }
>>>>>>> Stashed changes
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
                        while (i < line.Length && Char.IsLetterOrDigit(line[i])) { newToken += line[i++]; }
                        if (newToken == "bool") { tokens.Add(new Token(y, ox, TokenType.Bool, null)); }
                        else if (newToken == "int") { tokens.Add(new Token(y, ox, TokenType.Int, null)); }
                        else if (newToken == "double") { tokens.Add(new Token(y, ox, TokenType.Double, null)); }
                        else if (newToken == "string") { tokens.Add(new Token(y, ox, TokenType.String, null)); }
<<<<<<< Updated upstream
                        else { tokens.Add(new Token(y, ox, TokenType.Unknown, newToken)); }
=======
                        else if (newToken == "for") { tokens.Add(new Token(y, ox, TokenType.For, null)); }
                        else if (newToken == "while") { tokens.Add(new Token(y, ox, TokenType.While, null)); }
                        else if (newToken == "if") { tokens.Add(new Token(y, ox, TokenType.If, null)); }
                        else if (newToken == "else") { tokens.Add(new Token(y, ox, TokenType.Else, null)); }
                        else if (newToken == "elsif") { tokens.Add(new Token(y, ox, TokenType.Elsif, null)); }
                        else if (newToken == "abs") { tokens.Add(new Token(y, ox, TokenType.Abs, null)); }
                        else if (newToken == "sqrt") { tokens.Add(new Token(y, ox, TokenType.Sqrt, null));  }
                        else if (newToken == "Print") { tokens.Add(new Token(y, ox, TokenType.Print, null)); }
                        else if (newToken == "PrintLine") { tokens.Add(new Token(y, ox, TokenType.PrintLine, null)); }
                        else { tokens.Add(new Token(y, ox, TokenType.Unknow, newToken)); }
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
        /// <summary>
        /// Method that compresses tokens
        /// </summary>
        /// <param name="tokens">Given list of tokens</param>
        /// <returns>Compressed token list, f.e [8][add][7] -> [add:8,7]</returns>
        public static List<Token> Compress(List<Token> tokens) {

            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].type == TokenType.Unknown)
=======
        public static List<Token> Compress(List<Token> tokens)
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].type == TokenType.If || tokens[i].type == TokenType.Elsif || tokens[i].type == TokenType.For)
>>>>>>> Stashed changes
                {
                    if (tokens[i + 1].type == TokenType.BracketL)
                    {
                        tokens.RemoveAt(i + 1);
                        int tIndex = i + 1;
                        while (tokens[tIndex].type != TokenType.BracketR)
                        {
                            tokens[i].inputConnections.Add(tokens[tIndex]);
                            tokens.RemoveAt(tIndex);
                        }

                        tokens.RemoveAt(tIndex);
                        tokens[i].CompressIf(tIndex, tokens);
                        tokens[i].CompressAritmetic(tokens[i].connections);
                    }
                }
                if (tokens[i].type == TokenType.Add || tokens[i].type == TokenType.Sub || tokens[i].type == TokenType.Mul || tokens[i].type == TokenType.Div)
                {
                    int tmpindex = i - 1;
                    Token tmp = tokens[tmpindex];
                    tokens[tmpindex] = tokens[i];
                    tokens[i] = tmp;
                    /*tokens[tmpindex].connections.Add(tokens[i]);
                    tokens.RemoveAt(i);
                    tokens[tmpindex].connections.Add(tokens[i]);
                    tokens.RemoveAt(i);*/

                }
            }
            return tokens;
        }
        
        public void CompressIf(int index, List<Token> tokens)
        {
            if (tokens[index].type != TokenType.SemiComma)
            {
                connections.Add(tokens[index]);
                tokens.RemoveAt(index);
                CompressIf(index, tokens);
            }

            if(tokens[index].type == TokenType.SemiComma)
            {
                tokens.RemoveAt(index);
                return;
            }
        }

        public void CompressAritmetic(List<Token> tokens)
        {
            for(int index = 0; index < tokens.Count-1; index++)
            {
                if (tokens[index].type == TokenType.Add || tokens[index].type == TokenType.Sub || tokens[index].type == TokenType.Mul || tokens[index].type == TokenType.Div)
                {
                    int tmpindex = index - 1;
                    Token tmp = tokens[tmpindex];
                    tokens[tmpindex] = tokens[index];
                    tokens[index] = tmp;
                    tokens[index].connections.Add(tokens[index + 1]);
                    tokens.RemoveAt(index);
                    tokens[index - 1].connections.Add(tokens[index]);
                    tokens.RemoveAt(index);
                    //tokens[index].connections.Add(tokens[index + 1]);
                    //tokens.RemoveAt(index);
                }
            }
        }
    }
}
