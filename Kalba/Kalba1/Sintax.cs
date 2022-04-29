using System;
using System.Collections.Generic;
using System.Text;

namespace Kalba1
{
    class Sintax
    {
        private List<Token> unknowToSmth(List<Token> tokens)
        {
            for (int i = 0; i < tokens.Count; i++)
                if (tokens[i].type == TokenType.Unknown){
                    tokens[i].type = TokenType.Value;
                    if (tokens.Count > i + 1 && tokens[i + 1].type == TokenType.BracketL)
                    {
                        tokens[i].type = TokenType.Method;
                    }
                }
            return tokens;
        }
        public List<Token> Compress(List<Token> tokens)
        {
            tokens = unknowToSmth(tokens);
            tokens = Compress1(tokens);
            return tokens;
        }
        private List<Token> Compress1(List<Token> tokens)
        {
            
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].type == TokenType.If || tokens[i].type == TokenType.Elsif || tokens[i].type == TokenType.While)
                {
                    tokens = IfWhileElifConstructor(i, tokens);
                }
                if (tokens[i].type == TokenType.For)
                {
                    tokens = ForConstructor(i, tokens);
                }
                if (tokens[i].type == TokenType.PrintLine 
                    || tokens[i].type == TokenType.Print 
                    || tokens[i].type == TokenType.Abs 
                    || tokens[i].type == TokenType.Sqrt)
                {
                    FunctionConstructor(i, tokens);
                }
                if(tokens[i].type == TokenType.Unknown)
                {
                    tokens = FunctionConstructor(i, tokens);
                }
                if (tokens[i].type == TokenType.Ass)
                {
                    int row = tokens[i].row;
                    while (i >= 0 && row == tokens[i].row) i--;
                    i++;
                    tokens = AssignConstructor(i, tokens, row);
                    if (tokens[i].type != TokenType.Ass) i++;
                }
                if(tokens[i].type == TokenType.BoolDeclare 
                    || tokens[i].type == TokenType.DoubleDeclare 
                    || tokens[i].type == TokenType.IntDeclare 
                    || tokens[i].type == TokenType.StringDeclare)
                {
                    if(tokens.Count > i + 1)
                    {
                        tokens[i].value = tokens[i + 1].value;
                    }
                }
            }
            return tokens;
        }
        private List<Token> IfWhileElifConstructor(int i, List<Token> tokens)
        {
            List<Token> inputConnections = new List<Token>();
            List<Token> connections = new List<Token>();

            // (input)
            int lbracketsCount = 0;
            int rbracketsCount = 0;
            int nextindex = i + 1;
            if (tokens[nextindex].type == TokenType.BracketL)
            {
                lbracketsCount++;
                tokens.RemoveAt(nextindex);
                while (rbracketsCount != lbracketsCount && tokens.Count > nextindex)
                {
                    if (tokens[nextindex].type == TokenType.BracketR) rbracketsCount++;
                    if (tokens[nextindex].type == TokenType.BracketL) lbracketsCount++;
                    if (rbracketsCount != lbracketsCount) inputConnections.Add(tokens[nextindex]);
                    tokens.RemoveAt(nextindex);
                }
            }
            tokens[i].inputConnections = Compress1(inputConnections);

            // (connections)
            lbracketsCount = 1;
            rbracketsCount = 0;
            while (rbracketsCount != lbracketsCount && tokens.Count > nextindex)
            {
                if (tokens[nextindex].type == TokenType.If
                    || tokens[nextindex].type == TokenType.Elsif
                    || tokens[nextindex].type == TokenType.While
                    || tokens[nextindex].type == TokenType.Else)
                    lbracketsCount++;
                if (tokens[nextindex].type == TokenType.For) lbracketsCount += 3;
                if (tokens[nextindex].type == TokenType.SemiComma) rbracketsCount++;
                if (rbracketsCount != lbracketsCount) connections.Add(tokens[nextindex]);
                tokens.RemoveAt(nextindex);
            }
            tokens[i].connections = Compress1(connections);
            return tokens;
        }
        private List<Token> ElseConstructor(int i, List<Token> tokens)
        {
            // do stuff
            return tokens;
        }
        private List<Token> ForConstructor(int i, List<Token> tokens) {
            List<Token> inputConnections1 = new List<Token>();
            List<Token> inputConnections = new List<Token>();
            List<Token> connections = new List<Token>();

            // input ddd ; bool ; end
            int lbracketsCount = 0;
            int rbracketsCount = 0;
            int nextindex = i + 1;
            if (tokens[nextindex].type == TokenType.BracketL)
            {
                lbracketsCount++;
                tokens.RemoveAt(nextindex);
                while (rbracketsCount != lbracketsCount && tokens.Count > nextindex)
                {
                    if (tokens[nextindex].type == TokenType.BracketR) rbracketsCount++;
                    if (tokens[nextindex].type == TokenType.BracketL) lbracketsCount++;
                    if (rbracketsCount != lbracketsCount) inputConnections1.Add(tokens[nextindex]);
                    tokens.RemoveAt(nextindex);
                }
            }

            // ddd ;
            List<Token> temp = new List<Token>();
            while ( inputConnections1.Count > 0 && inputConnections1[0].type != TokenType.SemiComma)
            {
                temp.Add(inputConnections1[0]);
                inputConnections1.RemoveAt(0);
            }
            temp = Compress1(temp);
            temp.Add(inputConnections1[0]);
            inputConnections1.RemoveAt(0);
            foreach (Token t in temp) inputConnections.Add(t);
            // ; bool ;
            temp = new List<Token>();
            while (inputConnections1.Count > 0 && inputConnections1[0].type != TokenType.SemiComma)
            {
                temp.Add(inputConnections1[0]);
                inputConnections1.RemoveAt(0);
            }
            temp = Compress1(temp);
            temp.Add(inputConnections1[0]);
            inputConnections1.RemoveAt(0);
            foreach (Token t in temp) inputConnections.Add(t);
            // ; end
            temp = new List<Token>();
            while (inputConnections1.Count > 0)
            {
                temp.Add(inputConnections1[0]);
                inputConnections1.RemoveAt(0);
            }
            temp = AssignConstructor(0,temp, temp[0].row);
            foreach (Token t in temp) inputConnections.Add(t);
            // input ddd ; bool ; end  ====> FIN
            tokens[i].inputConnections = inputConnections;
            // input ddd ; bool ; end  ====> FIN

            lbracketsCount = 1;
            rbracketsCount = 0;
            while (rbracketsCount != lbracketsCount && tokens.Count > nextindex)
            {
                if (tokens[nextindex].type == TokenType.If
                    || tokens[nextindex].type == TokenType.Elsif
                    || tokens[nextindex].type == TokenType.While
                    || tokens[nextindex].type == TokenType.Else)
                    lbracketsCount++;
                if (tokens[nextindex].type == TokenType.For) lbracketsCount += 3;
                if (tokens[nextindex].type == TokenType.SemiComma) rbracketsCount++;
                if (rbracketsCount != lbracketsCount) connections.Add(tokens[nextindex]);
                tokens.RemoveAt(nextindex);
            }
            tokens[i].connections = Compress1(connections);
            return tokens;
        }
        private List<Token> FunctionConstructor(int i, List<Token> tokens)
        {
            List<Token> connections = new List<Token>();

            int lbracketsCount = 0;
            int rbracketsCount = 0;
            int nextindex = i + 1;
            if (tokens.Count > nextindex && tokens[nextindex].type == TokenType.BracketL)
            {
                lbracketsCount++;
                tokens.RemoveAt(nextindex);
                while (rbracketsCount != lbracketsCount && tokens.Count > nextindex)
                {
                    if (tokens[nextindex].type == TokenType.BracketR) rbracketsCount++;
                    if (tokens[nextindex].type == TokenType.BracketL) lbracketsCount++;
                    if (rbracketsCount != lbracketsCount) connections.Add(tokens[nextindex]);
                    tokens.RemoveAt(nextindex);
                }
            }
            if (tokens[i].type == TokenType.Sqrt)
            {
                foreach (Token token in connections) Console.WriteLine(token);
                Console.WriteLine();
            }
            tokens[i].connections = ListCompress(connections);
            return tokens;
        }
        private List<Token> ListCompress(List<Token> tokens)
        {
            List<Token> list = new List<Token>();
            while(tokens.Count > 0)
            {
                List<Token> part = new List<Token>();
                while (tokens.Count > 0 && tokens[0].type != TokenType.Comma)
                {
                    if (tokens[0].type != TokenType.Comma) part.Add(tokens[0]);
                    tokens.RemoveAt(0);
                }
                part = AritmeticConstructor(part);
                foreach (Token p in part) list.Add(p);
            }
            return list;
        }
        private List<Token> AssignConstructor(int i, List<Token> tokens, int row)
        {
            
            Token assign;
            List<Token> outputs = new List<Token>();
            List<Token> inputs = new List<Token>();
            if(tokens[i].type != TokenType.BoolDeclare
                    && tokens[i].type != TokenType.DoubleDeclare
                    && tokens[i].type != TokenType.IntDeclare
                    && tokens[i].type != TokenType.StringDeclare)
                outputs.Add(tokens[i]);
            int nextindex = i + 1;
            while (nextindex<tokens.Count && tokens[nextindex].type != TokenType.Ass)
            {
                outputs.Add(tokens[nextindex]);
                tokens.RemoveAt(nextindex);
            }
            assign = tokens[nextindex];
            tokens.RemoveAt(nextindex);
            while (nextindex < tokens.Count && tokens[nextindex].row == row)
            {
                inputs.Add(tokens[nextindex]);
                tokens.RemoveAt(nextindex);
            }
            assign.inputConnections = AritmeticConstructor(inputs);
            assign.outputConnections = ListCompress(outputs);
            if (tokens[i].type != TokenType.BoolDeclare
                    && tokens[i].type != TokenType.DoubleDeclare
                    && tokens[i].type != TokenType.IntDeclare
                    && tokens[i].type != TokenType.StringDeclare)
                tokens[i] = assign;
            else tokens.Insert(nextindex, assign);
            return tokens;
        }
        private List<Token> AritmeticConstructor(List<Token> tokens)
        {
            // logic stuff if < <= >= > == != && ||
            // do stuff

            // methods simplification
            for(int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].type == TokenType.Method 
                    || tokens[i].type == TokenType.Sqrt
                    || tokens[i].type == TokenType.Abs)
                {
                    tokens = FunctionConstructor(i, tokens);
                }
            }

            // () simplification
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].type == TokenType.BracketL)
                {
                    tokens = AritmeticBracketConstructor(i, tokens);
                }
            }

            // negative number
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens.Count > i+1)
                {
                    if (tokens[i].type == TokenType.Sub && tokens[i].connections.Count == 0 && i == 0)
                    {
                        tokens[i].connections.Add(new Token(tokens[i].row, tokens[i].column, TokenType.Int, 0));
                        tokens[i].connections.Add(tokens[i + 1]);
                        tokens.RemoveAt(i + 1);
                    }
                }
            }

            // * / simplification
            for (int i = 0; i < tokens.Count; i++)
            {
                int nextIndex = i + 1;
                if(tokens.Count > nextIndex+1)
                {
                    if ((tokens[nextIndex].type == TokenType.Mul || tokens[nextIndex].type == TokenType.Div) && tokens[nextIndex].connections.Count==0) {
                        Token temp = tokens[nextIndex];
                        temp.connections.Add(tokens[i]);
                        temp.connections.Add(tokens[nextIndex+1]);
                        tokens.RemoveAt(nextIndex);
                        tokens.RemoveAt(nextIndex);
                        tokens[i] = temp;
                    }
                }
            }
            
            // + - simplification
            for (int i = 0; i < tokens.Count; i++)
            {
                int nextIndex = i + 1;
                if (tokens.Count > nextIndex + 1)
                {
                    if ((tokens[nextIndex].type == TokenType.Sub || tokens[nextIndex].type == TokenType.Add) && tokens[nextIndex].connections.Count == 0)
                    {
                        Token temp = tokens[nextIndex];
                        temp.connections.Add(tokens[i]);
                        temp.connections.Add(tokens[nextIndex + 1]);
                        tokens.RemoveAt(nextIndex);
                        tokens.RemoveAt(nextIndex);
                        tokens[i] = temp;
                    }
                }
            }

            return tokens;
        }
        private List<Token> AritmeticBracketConstructor(int i, List<Token> tokens)
        {
            int nextIndex = i + 1;
            List<Token> tokens1 = new List<Token>();
            int lbracketsCount = 1;
            int rbracketsCount = 0;
            while (tokens.Count > nextIndex && lbracketsCount != rbracketsCount)
            {
                if (tokens[nextIndex].type == TokenType.BracketR) rbracketsCount++;
                if (tokens[nextIndex].type == TokenType.BracketL) lbracketsCount++;
                if (rbracketsCount != lbracketsCount) tokens1.Add(tokens[nextIndex]);
                tokens.RemoveAt(nextIndex);
            }
            tokens[i] = AritmeticConstructor(tokens1)[0];
            return tokens;
        }
        private List<Token> LogicAndOrAritmetics (int i, List<Token> tokens)
        {

            return tokens;
        }
        private List<Token> LogicAritmetics(int i, List<Token> tokens)
        {

            return tokens;
        }
    }
}
