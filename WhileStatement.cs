using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class WhileStatement : StatetmentBase
    {
        public Expression Term { get; private set; }
        public List<StatetmentBase> Body { get; private set; }

        public override void Parse(TokensStack sTokens)
        {
            Body = new List<StatetmentBase>();
            if (sTokens.Count <= 0)
            {
                throw new SyntaxErrorException("Missing while", new Token());
            }
            else
            {
                Token t1 = sTokens.Pop();

                if (sTokens.Peek() is Parentheses && ((Parentheses)sTokens.Peek()).Name.Equals('('))
                {
                    Token t2 = sTokens.Pop();
                }
                else
                {
                    throw new  SyntaxErrorException("Missing (", sTokens.Pop());
                }
                Term = Expression.Create(sTokens);
                if (Term != null)
                {
                    Term.Parse(sTokens);
                }
                else
                {
                    throw new SyntaxErrorException("Bad exp", new Token());
                }

                if (sTokens.Peek() is Parentheses && ((Parentheses)sTokens.Peek()).Name.Equals(')'))
                {
                    Token t3 = sTokens.Pop();
                }
                else
                {
                    throw new SyntaxErrorException("Missing )", sTokens.Pop());
                }

                if (sTokens.Peek() is Parentheses && ((Parentheses)sTokens.Peek()).Name.Equals('{'))
                {
                    Token t4 = sTokens.Pop();
                }
                else
                {
                    throw new SyntaxErrorException("Missing {", sTokens.Pop());
                }
                while (/*sTokens.Count > 0 && (((sTokens.Peek() is Parentheses) && !(((Parentheses)sTokens.Peek()).Name.Equals('}'))) || (!(sTokens.Peek() is Parentheses)))*/sTokens.Count > 0 && ((!(sTokens.Peek() is Parentheses))))
                {
                    StatetmentBase st = StatetmentBase.Create(sTokens.Peek());
                    if (st != null)
                    {
                        st.Parse(sTokens);
                        Body.Add(st);
                    }
                    else
                    {
                        throw new SyntaxErrorException("illegal statement", new Token());
                    }
                }
                if (sTokens.Count > 0 && ((sTokens.Peek() is Parentheses)))
                {
                    Token t5 = sTokens.Pop();
                }
                else
                {
                    throw new SyntaxErrorException("Missing }", sTokens.Pop());
                }
            }
        }

        public override string ToString()
        {
            string sWhile = "while(" + Term + "){\n";
            foreach (StatetmentBase s in Body)
                sWhile += "\t\t\t" + s + "\n";
            sWhile += "\t\t}";
            return sWhile;
        }

    }
}
