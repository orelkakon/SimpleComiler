using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class IfStatement : StatetmentBase
    {
        public Expression Term { get; private set; }
        public List<StatetmentBase> DoIfTrue { get; private set; }
        public List<StatetmentBase> DoIfFalse { get; private set; }

        public override void Parse(TokensStack sTokens)
        {

            DoIfTrue = new List<StatetmentBase>();
            DoIfFalse = new List<StatetmentBase>();
            if (sTokens.Count <= 0)
            {
                throw new SyntaxErrorException("Missing if", new Token());
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
                    throw new SyntaxErrorException("Missing (", sTokens.Pop());
                }
                Term = Expression.Create(sTokens);
                if (Term != null)
                {
                    Term.Parse(sTokens);
                }
                else
                {
                    throw new SyntaxErrorException("Bad Exp", sTokens.Pop());
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
                while (sTokens.Count > 0 && ((!(sTokens.Peek() is Parentheses))))
                {
                    StatetmentBase st = StatetmentBase.Create(sTokens.Peek()); //maybe need fix 
                    if (st != null)
                    {
                        st.Parse(sTokens);
                        DoIfTrue.Add(st);
                    }
                    else
                    {
                        throw new SyntaxErrorException("illegal exp", sTokens.Pop());
                    }
                }

                if (sTokens.Peek() is Parentheses && ((Parentheses)sTokens.Peek()).Name.Equals('}'))
                {
                    Token t3 = sTokens.Pop();
                }
                else
                {
                    throw new SyntaxErrorException("Missing }", sTokens.Pop());
                }

                if (sTokens.Peek() is Statement && ((Statement)sTokens.Peek()).Name.Equals("else"))
                {
                    Token t4 = sTokens.Pop();

                    if (sTokens.Peek() is Parentheses && ((Parentheses)sTokens.Peek()).Name.Equals('{'))
                    {
                        Token t5 = sTokens.Pop();
                    }
                    else
                    {
                        throw new SyntaxErrorException("Missing (", sTokens.Pop());
                    }
                    while (/*sTokens.Count > 0 && (((sTokens.Peek() is Parentheses) && ((Parentheses)sTokens.Peek()).Name.Equals('}')) || (!(sTokens.Peek() is Parentheses)))*/sTokens.Count > 0 && ((!(sTokens.Peek() is Parentheses))))///
                    {
                        StatetmentBase st2 = StatetmentBase.Create(sTokens.Peek());
                        if (st2 != null)
                        {
                            st2.Parse(sTokens);
                            DoIfFalse.Add(st2);
                        }
                        else
                        {
                            throw new SyntaxErrorException("illegal exp", sTokens.Pop());
                        }
                    }

                    if (sTokens.Peek() is Parentheses && ((Parentheses)sTokens.Peek()).Name.Equals('}'))
                    {
                        Token t6 = sTokens.Pop();
                    }
                    else
                    {
                        throw new SyntaxErrorException("Missing }", sTokens.Pop());
                    }
                }
                else
                {
                
                }
            }
        }
    
        
        public override string ToString()
        {
            string sIf = "if(" + Term + "){\n";
            foreach (StatetmentBase s in DoIfTrue)
                sIf += "\t\t\t" + s + "\n";
            sIf += "\t\t}";
            if (DoIfFalse.Count > 0)
            {
                sIf += "else{";
                foreach (StatetmentBase s in DoIfFalse)
                    sIf += "\t\t\t" + s + "\n";
                sIf += "\t\t}";
            }
            return sIf;
        }

    }
}
