using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class LetStatement : StatetmentBase
    {
        public string Variable { get; set; }
        public Expression Value { get; set; }

        public override string ToString()
        {
            return "let " + Variable + " = " + Value + ";";
        }

        public override void Parse(TokensStack sTokens)
        {
            if (sTokens.Count <= 0)
            {
                throw new SyntaxErrorException("Expected let Statement", new Token());
            }
            else
            {
                Token t1 = sTokens.Pop();
                if ((sTokens.Peek() is Identifier))
                {
                    Variable = ((Identifier)sTokens.Pop()).Name;
                }
                else
                {
                    throw new SyntaxErrorException("missing identifier type", sTokens.Pop());
                }

                if (sTokens.Peek() is Operator &&((Operator)sTokens.Peek()).Name.Equals('='))
                {
                    Token t2 = sTokens.Pop();
                }
                else
                {
                    throw new SyntaxErrorException("missing =", sTokens.Pop());
                }
                Value = Expression.Create(sTokens);
                if (Value != null)
                {
                    Value.Parse(sTokens);
                }
                else
                {
                    throw new SyntaxErrorException("Bad exp", sTokens.Pop());
                }
                Token t3 = sTokens.Pop();//;
            }
        }
    }
}
