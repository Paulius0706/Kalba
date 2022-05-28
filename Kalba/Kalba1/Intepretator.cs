using System;
using System.Collections.Generic;
using System.Text;

namespace Kalba1
{
    class Intepretator
    {

        public List<Token> tokens;
        public static Dictionary<string, Token> values = new Dictionary<string, Token>();
        public static Dictionary<string, Token> methods = new Dictionary<string, Token>();
        public Dictionary<string, Token> methodValues;
        public bool isMethod { get; set; }
        public bool isIfForWhile { get; set; }

        public Intepretator(List<Token> tokens, bool isMethod, bool ifForWhile) {
            if (isMethod) methodValues = new Dictionary<string, Token>();
            this.tokens = tokens;
            this.isMethod = isMethod;
            this.isIfForWhile = ifForWhile;
        }

        public void Run() {
            for (int i = 0; i < tokens.Count; i++) {
                object obj = executeCommand(tokens[i]);
            }
        }

        public List<Token> executeCommand(Token token) {
            List<Token> tokenList = new List<Token>();
            if (token.type == TokenType.Double || token.type == TokenType.Int || token.type == TokenType.Bool || token.type == TokenType.String || token.type == TokenType.Number) {
                tokenList.Add(token);
                return tokenList;
            }
            if (token.type == TokenType.Add) {
                Token a = executeCommand(token.connections[0])[0];
                Token b = executeCommand(token.connections[1])[0];
                if (a.type == TokenType.String || b.type == TokenType.String)
                {
                    string a1 = Convert.ToString(a.value);
                    string b1 = Convert.ToString(b.value);
                    tokenList.Add(new Token(token.row, token.column, TokenType.String, a1 + b1));
                    return tokenList;
                }
                if ((a.type == TokenType.Number ||a.type == TokenType.Double || a.type == TokenType.Int) && (b.type == TokenType.Number || b.type == TokenType.Double || b.type == TokenType.Int))
                {
                    double a1 = Convert.ToDouble(a.value);
                    double b1 = Convert.ToDouble(b.value);
                    double c = a1 + b1;

                    if (c % 1 == 0) tokenList.Add(new Token(token.row, token.column, TokenType.Int, c));
                    else            tokenList.Add(new Token(token.row, token.column, TokenType.Double, c));
                    return tokenList;
                }
                else {

                }
            }
            if (token.type == TokenType.Sub)
            {
                Token a = executeCommand(token.connections[0])[0];
                Token b = executeCommand(token.connections[1])[0];
                if (a.type == TokenType.Double || b.type == TokenType.Double)
                {
                    double a1 = Convert.ToDouble(a);
                    double b1 = Convert.ToDouble(b);
                    tokenList.Add(new Token(token.row, token.column, TokenType.Double, a1 - b1));
                    return tokenList;
                }
                if (a.type == TokenType.Int && b.type == TokenType.Int)
                {
                    int a1 = Convert.ToInt32(a);
                    int b1 = Convert.ToInt32(b);
                    tokenList.Add(new Token(token.row, token.column, TokenType.Int, a1 - b1));
                    return tokenList;
                }
            }
            if (token.type == TokenType.Mul)
            {
                Token a = executeCommand(token.connections[0])[0];
                Token b = executeCommand(token.connections[1])[0];
                if ((a.type == TokenType.Number || a.type == TokenType.Double || a.type == TokenType.Int) && (b.type == TokenType.Number || b.type == TokenType.Double || b.type == TokenType.Int))
                {
                    double a1 = Convert.ToDouble(a.value);
                    double b1 = Convert.ToDouble(b.value);
                    double c = a1 * b1;
                    if (c % 1 == 0) tokenList.Add(new Token(token.row, token.column, TokenType.Int, c));
                    else tokenList.Add(new Token(token.row, token.column, TokenType.Double, c));
                    return tokenList;
                }
                else
                {
                    Console.WriteLine(token);
                }
            }
            if (token.type == TokenType.Div)
            {
                Token a = executeCommand(token.connections[0])[0];
                Token b = executeCommand(token.connections[1])[0];
                if ((a.type == TokenType.Double && a.type == TokenType.Int) || (b.type == TokenType.Double && b.type == TokenType.Int))
                {
                    double a1 = Convert.ToDouble(a);
                    double b1 = Convert.ToDouble(b);
                    double c = a1 / b1;

                    if (c % 1 == 0) tokenList.Add(new Token(token.row, token.column, TokenType.Int, c));
                    else tokenList.Add(new Token(token.row, token.column, TokenType.Double, c));
                    return tokenList;
                }
            }
            if (token.type == TokenType.Ass)
            {
                List<Token> b = executeCommand(token.inputConnections[0]);
                // pataisyti
                int c = token.outputConnections.Count > b.Count ? b.Count : token.outputConnections.Count;
                for (int i = 0; i < c; i++)
                {
                    string variableName = Convert.ToString(token.outputConnections[i].value);
                    if (isMethod) methodValues[variableName].value = b[i].value;
                    else values[variableName].value = b[i].value;
                }
                return tokenList;
            }
            if (token.type == TokenType.Value)
            {
                string a1 = Convert.ToString(token.value);
                if (isMethod) tokenList.Add(new Token(token.row, token.column, methodValues[a1].type, methodValues[a1].value));
                else          tokenList.Add(new Token(token.row, token.column, values[a1].type, values[a1].value));

                return tokenList;
            }

            if (token.type == TokenType.Method)
            {
                string a1 = Convert.ToString(token.value);
                Intepretator intepretator = new Intepretator(methods[a1].connections, true, false);
                intepretator.methodValues = new Dictionary<string, Token>();
                // input to method input variables
                for (int i = 0; i < methods[a1].inputConnections.Count; i++)
                {
                    string variableName = Convert.ToString(methods[a1].inputConnections[i].value);
                    intepretator.methodValues.Add(
                        variableName , 
                        new Token(
                            methods[a1].row, 
                            methods[a1].column,
                            methods[a1].inputConnections[i].assignType,
                            executeCommand(token.inputConnections[i])[0].value
                        ));
                }
                // input to method output variables
                for (int i = 0; i < methods[a1].outputConnections.Count; i++)
                {
                    string variableName = Convert.ToString(methods[a1].outputConnections[i].value);
                    intepretator.methodValues.Add(
                        variableName,
                        new Token(
                            methods[a1].row,
                            methods[a1].column,
                            methods[a1].outputConnections[i].type,
                            null
                            ));
                }
                // Execute 
                intepretator.Run();
                // Extract output from method
                for (int i = 0; i < methods[a1].outputConnections.Count; i++) {
                    string variableName = Convert.ToString(methods[a1].outputConnections[i].value);
                    tokenList.Add(intepretator.methodValues[variableName]);
                }
                return tokenList;
            }

            if (token.type == TokenType.If) {
                if (Convert.ToBoolean(executeCommand(token.inputConnections[0])[0].value)) {
                    Intepretator intepretator = new Intepretator(token.connections, isMethod, true);
                    if (isMethod) intepretator.methodValues = this.methodValues;
                    intepretator.Run();
                    if (isMethod) this.methodValues = intepretator.methodValues;
                } 
                else
                {
                    if(token.outputConnections.Count > 0)
                    {
                        Token outputToken = token.outputConnections[0];
                        while(outputToken != null)
                        {
                            if(outputToken.type == TokenType.Elsif)
                            {
                                if (Convert.ToBoolean(executeCommand(outputToken.inputConnections[0])[0].value))
                                {
                                    Intepretator intepretator = new Intepretator(outputToken.connections, isMethod, true);
                                    if (isMethod) intepretator.methodValues = this.methodValues;
                                    intepretator.Run();
                                    if (isMethod) this.methodValues = intepretator.methodValues;
                                    break;
                                }
                                else
                                {
                                    if(outputToken.outputConnections.Count > 0)
                                    {
                                        outputToken = outputToken.outputConnections[0];
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                Intepretator intepretator = new Intepretator(outputToken.connections, isMethod, true);
                                if (isMethod) intepretator.methodValues = this.methodValues;
                                intepretator.Run();
                                if (isMethod) this.methodValues = intepretator.methodValues;
                                break;
                            }
                        }
                    }
                }
     
            }


            if (token.type == TokenType.For) {
                // type name = value ; bool ; exp
                
                int index = 0;
                List<Token> conn = token.connections;
                List<Token> inputs1 = new List<Token>();
                while(token.inputConnections[index].type != TokenType.SemiComma) inputs1.Add(token.inputConnections[index++]);
                index++;
                List<Token> inputs2 = new List<Token>();
                while (token.inputConnections[index].type != TokenType.SemiComma) inputs2.Add(token.inputConnections[index++]);
                index++;
                List<Token> inputs3 = new List<Token>();
                while (token.inputConnections.Count > index) inputs3.Add(token.inputConnections[index++]);

                Intepretator intepretator1 = new Intepretator(inputs1, isMethod, true);
                if (isMethod) intepretator1.methodValues = this.methodValues;
                intepretator1.Run();
                if (isMethod) this.methodValues = intepretator1.methodValues;

                Intepretator intepretator3 = new Intepretator(inputs3, isMethod, true);
                Intepretator intepretator = new Intepretator(conn, isMethod, true);

                while (Convert.ToBoolean(executeCommand(inputs2[0])[0].value)){
                    if (isMethod) intepretator.methodValues = this.methodValues;
                    intepretator.Run();
                    if (isMethod) this.methodValues = intepretator.methodValues;

                    if (isMethod) intepretator3.methodValues = this.methodValues;
                    intepretator3.Run();
                    if (isMethod) this.methodValues = intepretator3.methodValues;
                }
            }
            if (token.type == TokenType.While) {
                List<Token> conn = token.connections;
                List<Token> inputs1 = token.inputConnections;
                Intepretator intepretator = new Intepretator(conn, isMethod, true);

                while (Convert.ToBoolean(executeCommand(inputs1[0])[0].value))
                {
                    if (isMethod) intepretator.methodValues = this.methodValues;
                    intepretator.Run();
                    if (isMethod) this.methodValues = intepretator.methodValues;
                }

            }
            if (token.type == TokenType.Abs) {
                Token value = executeCommand(token.connections[0])[0];
                if (value.type == TokenType.Int)
                    if (Convert.ToInt32(value.value) < 0) value.value = -Convert.ToInt32(value.value);
                if (value.type == TokenType.Double)
                    if (Convert.ToInt32(value.value) < 0) value.value = -Convert.ToDouble(value.value);
                tokenList.Add(value);
                return tokenList;
            }
            if (token.type == TokenType.Sqrt) {
                Token value = executeCommand(token.connections[0])[0];
                if (value.type == TokenType.Int)
                    if (Convert.ToInt32(value.value) > 0) { value.value = Math.Sqrt(Convert.ToInt32(value.value)); value.type = TokenType.Double; }
                    else Console.WriteLine("Error sqrt from negative number: " + value.row + ":" + value.column);
                if (value.type == TokenType.Double)
                    if (Convert.ToInt32(value.value) > 0) { value.value = Math.Sqrt(Convert.ToDouble(value.value)); }
                    else Console.WriteLine("Error sqrt from negative number: " + value.row + ":" + value.column);
                tokenList.Add(value);
                return tokenList;
            }
            if (token.type == TokenType.Print) {
                Token value = executeCommand(token.connections[0])[0];
                if (value.type == TokenType.Int)
                    Console.Write(Convert.ToString((Convert.ToInt32(value.value)),10));
                if (value.type == TokenType.Double)
                    Console.Write(Convert.ToString(Convert.ToDouble(value.value)));
                if (value.type == TokenType.String)
                    Console.Write(Convert.ToString(value.value));
                if (value.type == TokenType.Bool)
                    Console.Write(Convert.ToString(Convert.ToBoolean(value.value)));
                return tokenList;
            }
            if (token.type == TokenType.PrintLine) {
                Token value = executeCommand(token.connections[0])[0];
                if (value.type == TokenType.Int)
                    Console.WriteLine(Convert.ToString((Convert.ToInt32(value.value)), 10));
                if (value.type == TokenType.Double)
                    Console.WriteLine(Convert.ToString(Convert.ToDouble(value.value)));
                if (value.type == TokenType.String)
                    Console.WriteLine(Convert.ToString(value.value));
                if (value.type == TokenType.Bool)
                    Console.WriteLine(Convert.ToString(Convert.ToBoolean(value.value)));
                return tokenList;
            }
            if(token.type == TokenType.Equal)
            {
                Token lt = executeCommand(token.connections[0])[0];
                Token rt = executeCommand(token.connections[1])[0];
                
                if((lt.type == TokenType.Number || lt.type == TokenType.Int || lt.type == TokenType.Double || lt.type == TokenType.Bool) && (rt.type == TokenType.Number || rt.type == TokenType.Int || rt.type == TokenType.Double || rt.type == TokenType.Bool))
                {
                    tokenList.Add(new Token(token.row, token.column, TokenType.Bool, Convert.ToDouble(lt.value) == Convert.ToDouble(rt.value)));
                }
                if (lt.type == TokenType.String && rt.type == TokenType.String) {
                    tokenList.Add(new Token(token.row, token.column, TokenType.Bool, Convert.ToString(lt.value) == Convert.ToString(rt.value)));
                }
                return tokenList;
            }
            if(token.type == TokenType.NotEqual)
            {
                Token lt = executeCommand(token.connections[0])[0];
                Token rt = executeCommand(token.connections[1])[0];

                if ((lt.type == TokenType.Number || lt.type == TokenType.Int || lt.type == TokenType.Double || lt.type == TokenType.Bool) && (rt.type == TokenType.Number || rt.type == TokenType.Int || rt.type == TokenType.Double || rt.type == TokenType.Bool))
                {
                    tokenList.Add(new Token(token.row, token.column, TokenType.Bool, Convert.ToDouble(lt.value) != Convert.ToDouble(rt.value)));
                }
                if (lt.type == TokenType.String && rt.type == TokenType.String)
                {
                    tokenList.Add(new Token(token.row, token.column, TokenType.Bool, Convert.ToString(lt.value) != Convert.ToString(rt.value)));
                }
                return tokenList;
            }
            if (token.type == TokenType.More)
            {
                Token lt = executeCommand(token.connections[0])[0];
                Token rt = executeCommand(token.connections[1])[0];

                if ((lt.type == TokenType.Number || lt.type == TokenType.Int || lt.type == TokenType.Double || lt.type == TokenType.Bool) && (rt.type == TokenType.Number || rt.type == TokenType.Int || rt.type == TokenType.Double || rt.type == TokenType.Bool))
                {
                    tokenList.Add(new Token(token.row, token.column, TokenType.Bool, Convert.ToDouble(lt.value) > Convert.ToDouble(rt.value)));
                }
                return tokenList;
            }
            if (token.type == TokenType.Less)
            {
                Token lt = executeCommand(token.connections[0])[0];
                Token rt = executeCommand(token.connections[1])[0];

                if ((lt.type == TokenType.Number || lt.type == TokenType.Int || lt.type == TokenType.Double || lt.type == TokenType.Bool) && (rt.type == TokenType.Number || rt.type == TokenType.Int || rt.type == TokenType.Double || rt.type == TokenType.Bool))
                {
                    tokenList.Add(new Token(token.row, token.column, TokenType.Bool, Convert.ToDouble(lt.value) < Convert.ToDouble(rt.value)));
                }
                return tokenList;
            }
            if (token.type == TokenType.LessOrEqual)
            {
                Token lt = executeCommand(token.connections[0])[0];
                Token rt = executeCommand(token.connections[1])[0];

                if ((lt.type == TokenType.Number || lt.type == TokenType.Int || lt.type == TokenType.Double || lt.type == TokenType.Bool) && (rt.type == TokenType.Number || rt.type == TokenType.Int || rt.type == TokenType.Double || rt.type == TokenType.Bool))
                {
                    tokenList.Add(new Token(token.row, token.column, TokenType.Bool, Convert.ToDouble(lt.value) <= Convert.ToDouble(rt.value)));
                }
                return tokenList;
            }
            if (token.type == TokenType.MoreOrEqual)
            {
                Token lt = executeCommand(token.connections[0])[0];
                Token rt = executeCommand(token.connections[1])[0];

                if ((lt.type == TokenType.Number || lt.type == TokenType.Int || lt.type == TokenType.Double || lt.type == TokenType.Bool) && (rt.type == TokenType.Number || rt.type == TokenType.Int || rt.type == TokenType.Double || rt.type == TokenType.Bool))
                {
                    tokenList.Add(new Token(token.row, token.column, TokenType.Bool, Convert.ToDouble(lt.value) >= Convert.ToDouble(rt.value)));
                }
                return tokenList;
            }

            if (token.type == TokenType.AddMethod)
            {
                
                string name = Convert.ToString(token.value);
                methods.Add(name, token);
            }
            if (token.type == TokenType.BoolDeclare)
            {
                if (isMethod)
                {
                    methodValues.Add(Convert.ToString(token.value), new Token(0, 0, TokenType.Bool, false));
                    methodValues[Convert.ToString(token.value)].assignType = TokenType.Bool;
                }
                else {
                    values.Add(Convert.ToString(token.value), new Token(0, 0, TokenType.Bool, false));
                    values[Convert.ToString(token.value)].assignType = TokenType.Bool;
                }
                return tokenList;
            }
            if (token.type == TokenType.DoubleDeclare)
            {
                if (isMethod)
                {
                    methodValues.Add(Convert.ToString(token.value), new Token(0, 0, TokenType.Double, 0));
                    methodValues[Convert.ToString(token.value)].assignType = TokenType.Double;
                }
                else {
                    values.Add(Convert.ToString(token.value), new Token(0, 0, TokenType.Double, 0));
                    values[Convert.ToString(token.value)].assignType = TokenType.Double;
                }
                return tokenList;
            }
            if (token.type == TokenType.IntDeclare)
            {
                if (isMethod)
                {
                    methodValues.Add(Convert.ToString(token.value), new Token(0, 0, TokenType.Int, 0));
                    methodValues[Convert.ToString(token.value)].assignType = TokenType.Int;
                }
                else {
                    values.Add(Convert.ToString(token.value), new Token(0, 0, TokenType.Int, 0));
                    values[Convert.ToString(token.value)].assignType = TokenType.Int;
                }
                return tokenList;
            }
            if (token.type == TokenType.StringDeclare)
            {
                if (isMethod) methodValues.Add(Convert.ToString(token.value), new Token(0, 0, TokenType.String, ""));
                else values.Add(Convert.ToString(token.value), new Token(0, 0, TokenType.String, ""));
                return tokenList;
            }
            return tokenList;
        }

    }
}
