using System;
using System.Collections.Generic;

namespace SimpleCompiler
{
    public class FunctionCallExpression : Expression
    {
        public string FunctionName { get; private set; }
        public List<Expression> Args { get; private set; }

        public override void Parse(TokensStack sTokens)
        {
            Args = new List<Expression>();
            if (sTokens.Count <= 0)
            {
                throw new SyntaxErrorException("Missing function", new Token());
            }
            else
            {
                if (!(sTokens.Peek() is Identifier))
                {
                    throw new SyntaxErrorException("Missing name of func", sTokens.Pop());
                }
                else
                {
                    FunctionName = ((Identifier)sTokens.Pop()).Name;
                }

                if (sTokens.Peek() is Parentheses && ((Parentheses)sTokens.Peek()).Name.Equals('('))
                {
                    Token t1 = sTokens.Pop();
                }
                else
                {
                    throw new SyntaxErrorException("Missing (", sTokens.Pop());
                }
                while (sTokens.Count > 0 && (((!(sTokens.Peek() is Parentheses)) || ((sTokens.Peek() is Parentheses) && ((Parentheses)sTokens.Peek()).Name.Equals('('))))) //maybe need check casting
                {
                    Expression E1 = Expression.Create(sTokens);
                    if (E1 != null)
                    {
                        E1.Parse(sTokens);
                        Args.Add(E1);
                    }
                    else
                    {
                        throw new SyntaxErrorException("Bad Exp", new Token());
                    }

                    if (sTokens.Peek() is Separator && ((Separator)sTokens.Peek()).Name.Equals(','))
                    {
                        sTokens.Pop();
                    }

                }


                sTokens.Pop();
                ToString();
            }
        }

        public override string ToString()
        {
            string sFunction = FunctionName + "(";
            for (int i = 0; i < Args.Count - 1; i++)
                sFunction += Args[i] + ",";
            if (Args.Count > 0)
                sFunction += Args[Args.Count - 1];
            sFunction += ")";
            return sFunction;
        }
    }
}