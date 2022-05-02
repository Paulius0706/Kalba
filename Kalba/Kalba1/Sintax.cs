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
                    tokens = FunctionConstructor(i, tokens);
                }
                if(tokens[i].type == TokenType.Method)
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

            tokens[i].inputConnections = AritmeticConstructor(inputConnections);

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
            //temp = AssignConstructor(temp);
            foreach (Token t in temp) inputConnections.Add(t);
            // ; bool ;
            temp = new List<Token>();
            while (inputConnections1.Count > 0 && inputConnections1[0].type != TokenType.SemiComma)
            {
                temp.Add(inputConnections1[0]);
                inputConnections1.RemoveAt(0);
            }
            temp = AritmeticConstructor(temp);
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
            temp = Compress1(temp);
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
        private List<Token> AssignConstructor(List<Token> tokens)
        {
            // output list is not correct
            Token assign;
            List<Token> outputs = new List<Token>();
            List<Token> inputs = new List<Token>();
            int nextindex = 0;
            while (nextindex < tokens.Count && tokens[nextindex].type != TokenType.Ass)
            {
                outputs.Add(tokens[nextindex]);
                tokens.RemoveAt(nextindex);
            }
            assign = tokens[nextindex];
            tokens.RemoveAt(nextindex);
            while (nextindex < tokens.Count)
            {
                inputs.Add(tokens[nextindex]);
                tokens.RemoveAt(nextindex);
            }
            assign.inputConnections = AritmeticConstructor(inputs);
            assign.outputConnections = ListCompress(outputs);
            tokens.Add(assign);
            return tokens;
        }
        private List<Token> AritmeticConstructor(List<Token> tokens)
        {
            
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

            // logic stuff if < <= >= > == != && ||
            bool haveAndOr = false;
            for (int i = 0; i < tokens.Count; i++)
                if (tokens[i].type == TokenType.And 
                    || tokens[i].type == TokenType.Or) haveAndOr = true;
            if (haveAndOr) return LogicAndOrAritmetics(tokens);
            bool haveLessMoreEqual = false;
            for (int i = 0; i < tokens.Count; i++)
                if (tokens[i].type == TokenType.Less 
                    || tokens[i].type == TokenType.More
                    || tokens[i].type == TokenType.MoreOrEqual
                    || tokens[i].type == TokenType.LessOrEqual
                    || tokens[i].type == TokenType.Equal
                    || tokens[i].type == TokenType.NotEqual) haveLessMoreEqual = true;
            if (haveLessMoreEqual) return LogicAritmetics(tokens);

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
        private List<Token> LogicAndOrAritmetics (List<Token> tokens)
        {
            List<Token> logic = new List<Token>();
            while (tokens.Count > 0)
            {
                List<Token> expr = new List<Token>();
                while(tokens.Count > 0 && (tokens[0].type != TokenType.And && tokens[0].type != TokenType.Or))
                {
                    expr.Add(tokens[0]);
                    tokens.RemoveAt(0);
                }
                expr = AritmeticConstructor(expr);
                foreach (Token t in expr) logic.Add(t);
                if(tokens.Count > 0 && (tokens[0].type == TokenType.And || tokens[0].type == TokenType.Or))
                {
                    logic.Add(tokens[0]);
                    tokens.RemoveAt(0);
                }
            }
            while(logic.Count > 1)
            {
                if (logic.Count > 2)
                {
                    if((logic[1].type == TokenType.And || logic[1].type == TokenType.Or) && logic[1].connections.Count == 0)
                    {
                        Token temp = logic[1];
                        temp.connections.Add(logic[0]);
                        temp.connections.Add(logic[2]);
                        logic.RemoveAt(1);
                        logic.RemoveAt(1);
                        logic[0] = temp;
                    }
                }
                else break;
            }
            return logic;
        }
        private List<Token> LogicAritmetics(List<Token> tokens)
        {
            List<Token> right = new List<Token>();
            List<Token> left = new List<Token>();
            Token comparison;
            while (tokens.Count > 0 && (
                tokens[0].type != TokenType.More 
                && tokens[0].type != TokenType.Less
                && tokens[0].type != TokenType.MoreOrEqual
                && tokens[0].type != TokenType.LessOrEqual
                && tokens[0].type != TokenType.NotEqual
                && tokens[0].type != TokenType.Equal))
            {
                right.Add(tokens[0]);
                tokens.RemoveAt(0);
            }
            right = AritmeticConstructor(right);

            while (tokens.Count > 1 && (
                tokens[1].type != TokenType.More
                && tokens[1].type != TokenType.Less
                && tokens[1].type != TokenType.MoreOrEqual
                && tokens[1].type != TokenType.LessOrEqual
                && tokens[1].type != TokenType.NotEqual
                && tokens[1].type != TokenType.Equal))
            {
                left.Add(tokens[1]);
                tokens.RemoveAt(1);
            }
            left = AritmeticConstructor(left);
            tokens[0].connections.Add(right[0]);
            tokens[0].connections.Add(left[0]);
            return tokens;
        }
        private List<Token> AddMethodConstructor(int i, List<Token> tokens)
        {
            List<Token> inputConnections = new List<Token>();
            List<Token> connections = new List<Token>();
            List<Token> outputConnections = new List<Token>();
            int lBoxCount = 0;
            int rBoxCount = 0;
            int nextIndex = i + 1;

            // (output connections)
            if (tokens[nextIndex].type == TokenType.BoxL)
            {
                lBoxCount++;
                tokens.RemoveAt(nextIndex);
                while (lBoxCount != rBoxCount)
                {
                    if (tokens[nextIndex].type == TokenType.DoubleDeclare)
                    {
                        tokens.RemoveAt(nextIndex);
                        tokens[nextIndex].assignType = TokenType.Double;
                    }

                    if (tokens[nextIndex].type == TokenType.IntDeclare)
                    {
                        tokens.RemoveAt(nextIndex);
                        tokens[nextIndex].assignType = TokenType.Int;
                    }

                    if (tokens[nextIndex].type == TokenType.StringDeclare)
                    {
                        tokens.RemoveAt(nextIndex);
                        tokens[nextIndex].assignType = TokenType.String;
                    }

                    if (tokens[nextIndex].type == TokenType.BoxL)
                    {
                        lBoxCount++;
                    }
                    if (tokens[nextIndex].type == TokenType.BoxR)
                    {
                        rBoxCount++;
                    }

                    if (lBoxCount != rBoxCount)
                    {
                        if (tokens[nextIndex].type != TokenType.Comma)
                        {
                            outputConnections.Add(tokens[nextIndex]);
                        }
                    }

                    tokens.RemoveAt(nextIndex);
                }
            }

            tokens[i].outputConnections = Compress1(outputConnections);


            // (input connections)
            lBoxCount = 0;
            rBoxCount = 0;

            // Check if method name is declared
            if (tokens[nextIndex].type == TokenType.Method)
            {
                tokens[i].value = tokens[nextIndex].value;
                tokens.RemoveAt(nextIndex);
            }

            // Compress brackets
            if (tokens[nextIndex].type == TokenType.BracketL)
            {
                lBoxCount++;
                tokens.RemoveAt(nextIndex);
                while (lBoxCount != rBoxCount)
                {
                    if (tokens[nextIndex].type == TokenType.DoubleDeclare)
                    {
                        tokens.RemoveAt(nextIndex);
                        tokens[nextIndex].assignType = TokenType.Double;
                    }

                    if (tokens[nextIndex].type == TokenType.IntDeclare)
                    {
                        tokens.RemoveAt(nextIndex);
                        tokens[nextIndex].assignType = TokenType.Int;
                    }

                    if (tokens[nextIndex].type == TokenType.StringDeclare)
                    {
                        tokens.RemoveAt(nextIndex);
                        tokens[nextIndex].assignType = TokenType.String;
                    }
                    if (tokens[nextIndex].type == TokenType.BracketL)
                        lBoxCount++;
                    if (tokens[nextIndex].type == TokenType.BracketR)
                        rBoxCount++;
                    if (lBoxCount != rBoxCount)
                        inputConnections.Add(tokens[nextIndex]);
                    tokens.RemoveAt(nextIndex);
                }
            }


            // Compress connections recursively
            tokens[i].inputConnections = Compress1(inputConnections);

            lBoxCount = 1;
            rBoxCount = 0;

            // connections
            while (lBoxCount != rBoxCount)
            {
                if (tokens[nextIndex].type == TokenType.If
                    || tokens[nextIndex].type == TokenType.Elsif
                    || tokens[nextIndex].type == TokenType.While
                    || tokens[nextIndex].type == TokenType.Else)
                    lBoxCount++;
                if (tokens[nextIndex].type == TokenType.For) lBoxCount += 3;
                if (tokens[nextIndex].type == TokenType.SemiComma) rBoxCount++;
                if (rBoxCount != lBoxCount) connections.Add(tokens[nextIndex]);
                tokens.RemoveAt(nextIndex);
            }

            tokens[i].connections = Compress1(connections);


            return tokens;
        }
    }
}

