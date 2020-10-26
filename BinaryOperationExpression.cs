using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class BinaryOperationExpression : Expression
    {
        public string Operator { get; set; }
        public Expression Operand1 { get; set; }
        public Expression Operand2 { get; set; }

        public override string ToString()
        {
            return "(" + Operator + " " + Operand1 + " " + Operand2 + ")";
        }

        public override void Parse(TokensStack sTokens)
        {

            if (sTokens.Peek() is Parentheses && ((Parentheses)sTokens.Peek()).Name.Equals('('))
            {
                Token t1 = sTokens.Pop();
            }
            else
            {
                throw new SyntaxErrorException("missing ( ", sTokens.Peek());
            }

            if (sTokens.Peek() is Operator)
            {
                Operator = "" + ((Operator)sTokens.Pop()).Name;
            }
            else
            {
                throw new SyntaxErrorException("no operator", sTokens.Peek());
            }
            Operand1 = Expression.Create(sTokens);
            if (Operand1 != null)
            {
                Operand1.Parse(sTokens);
            }
            else
            {
                throw new SyntaxErrorException("Bad Ope1", new Token());
            }
            Operand2 = Expression.Create(sTokens);
            if (Operand2 != null)
            {
                Operand2.Parse(sTokens);
            }
            else
            {
                throw new SyntaxErrorException("Bad Ope2", new Token());
            }

            if (sTokens.Peek() is Parentheses && ((Parentheses)sTokens.Peek()).Name.Equals(')'))
            {
                Token t2 = sTokens.Pop();
            }
            else
            {
                throw new SyntaxErrorException("misssing (", sTokens.Pop());
            }
        }
    }
}
