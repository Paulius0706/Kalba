using System;
using System.Collections.Generic;
using System.Text;

namespace Kalba1
{
    class Sintax
    {
        public List<Token> Compress(List<Token> tokens)
        {

            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].type == TokenType.If || tokens[i].type == TokenType.Elsif || tokens[i].type == TokenType.While)
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
                        while (rbracketsCount != lbracketsCount)
                        {
                            if (tokens.Count <= nextindex) break;
                            if (tokens[nextindex].type != TokenType.BracketR) rbracketsCount++;
                            if (tokens[nextindex].type != TokenType.BracketL) lbracketsCount++;
                            if (rbracketsCount != lbracketsCount) inputConnections.Add(tokens[nextindex]);
                            tokens.RemoveAt(nextindex);
                        }
                    }
                    tokens[i].inputConnections = Compress(inputConnections);

                    // (connections)
                    lbracketsCount = 1;
                    rbracketsCount = 0;
                    while (rbracketsCount != lbracketsCount)
                    {
                        if (tokens.Count <= nextindex) break;
                        if (tokens[nextindex].type == TokenType.If 
                            || tokens[nextindex].type == TokenType.Elsif 
                            || tokens[nextindex].type == TokenType.While 
                            || tokens[nextindex].type == TokenType.For
                            || tokens[nextindex].type == TokenType.Else)
                            lbracketsCount++;
                        if (tokens[nextindex].type == TokenType.SemiComma) rbracketsCount++;
                        if (rbracketsCount != lbracketsCount) connections.Add(tokens[nextindex]);
                        tokens.RemoveAt(nextindex);
                    }
                    tokens[i].connections = Compress(connections);
                }
                if (tokens[i].type == TokenType.For)
                {
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
                        while (rbracketsCount != lbracketsCount)
                        {
                            if (tokens.Count <= nextindex) break;
                            if (tokens[nextindex].type != TokenType.BracketR) rbracketsCount++;
                            if (tokens[nextindex].type != TokenType.BracketL) lbracketsCount++;
                            if (rbracketsCount != lbracketsCount) inputConnections1.Add(tokens[nextindex]);
                            tokens.RemoveAt(nextindex);
                        }
                    }
                    // ddd ;
                    List<Token> temp = new List<Token>();
                    while (inputConnections1[0].type != TokenType.SemiComma){
                        temp.Add(inputConnections1[0]);
                        inputConnections1.RemoveAt(0);
                    }
                    temp = Compress(temp);
                    temp.Add(inputConnections1[0]);
                    inputConnections1.RemoveAt(0);
                    foreach (Token t in temp) inputConnections.Add(t);
                    // ; bool ;
                    temp = new List<Token>();
                    while (inputConnections1[0].type != TokenType.SemiComma){
                        temp.Add(inputConnections1[0]);
                        inputConnections1.RemoveAt(0);
                    }
                    temp = Compress(temp);
                    temp.Add(inputConnections1[0]);
                    inputConnections1.RemoveAt(0);
                    foreach (Token t in temp) inputConnections.Add(t);
                    // ; end
                    temp = new List<Token>();
                    while (inputConnections1.Count > 0 )
                    {
                        temp.Add(inputConnections1[0]);
                        inputConnections1.RemoveAt(0);
                    }
                    temp = Compress(temp);
                    foreach (Token t in temp) inputConnections.Add(t);
                    // input ddd ; bool ; end  ====> FIN
                    tokens[i].inputConnections = inputConnections;
                    // input ddd ; bool ; end  ====> FIN

                    lbracketsCount = 1;
                    rbracketsCount = 0;
                    while (rbracketsCount != lbracketsCount)
                    {
                        if (tokens.Count <= nextindex) break;
                        if (tokens[nextindex].type == TokenType.If
                            || tokens[nextindex].type == TokenType.Elsif
                            || tokens[nextindex].type == TokenType.While
                            || tokens[nextindex].type == TokenType.For
                            || tokens[nextindex].type == TokenType.Else)
                            lbracketsCount++;
                        if (tokens[nextindex].type == TokenType.SemiComma) rbracketsCount++;
                        if (rbracketsCount != lbracketsCount) connections.Add(tokens[nextindex]);
                        tokens.RemoveAt(nextindex);
                    }
                    tokens[i].connections = Compress(connections);
                }
                if (tokens[i].type == TokenType.PrintLine || tokens[i].type == TokenType.Print)
                {
                    List<Token> connections = new List<Token>();
                    
                    int lbracketsCount = 0;
                    int rbracketsCount = 0;
                    int nextindex = i + 1;
                    if (tokens[nextindex].type == TokenType.BracketL)
                    {
                        lbracketsCount++;
                        tokens.RemoveAt(nextindex);
                        while (rbracketsCount != lbracketsCount)
                        {
                            if (tokens.Count <= nextindex) break;
                            if (tokens[nextindex].type != TokenType.BracketR) rbracketsCount++;
                            if (tokens[nextindex].type != TokenType.BracketL) lbracketsCount++;
                            if (rbracketsCount != lbracketsCount) connections.Add(tokens[nextindex]);
                            tokens.RemoveAt(nextindex);
                        }
                    }
                    tokens[i].connections = Compress(connections);
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
    }
}
